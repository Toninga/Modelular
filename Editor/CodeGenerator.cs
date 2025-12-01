

using Modelular.Modifiers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;

namespace Modelular.Editor
{
    public static class CodeGenerator
    {
        public static string GeneratedModelPath = "Assets/Packages/Modelular/Core/Interface/Modifiers";
        public static string ModelListPath = "Assets/Packages/Modelular/Core/Interface";


        [MenuItem("Modelular/Refresh assembly")]
        public static void RegenerateModels()
        {

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Dictionary<int, List<Type>> modifiers = new ();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(Modifier)))
                    {
                        Attribute[] attrs = Attribute.GetCustomAttributes(type);
                        foreach (Attribute attr in attrs)
                        {
                            if (attr is ModelularInterfaceAttribute modAttr)
                            {
                                string model = GenerateModifierInterface(type);
                                TrySaveToFile(GeneratedModelPath, type.Name + "Model.cs", model);
                                if (!modifiers.ContainsKey(modAttr.Priority))
                                    modifiers[modAttr.Priority] = new ();
                                modifiers[modAttr.Priority].Add(type);
                                break;
                            }
                        }
                    }
                }
            }

            // Sort all the types by their priority
            List<Type> types = new();
            List<int> priorities = new();
            foreach (int priority in modifiers.Keys)
                priorities.Add(priority);
            priorities.Sort();
            foreach (int priority in  priorities)
                types.AddRange(modifiers[priority]);

            SetupModelList(ModelListPath, "ModifierModelList.cs", types);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log("[Modelular] : Complementary modifier files were successfully generated.");
        }
        public static string GetPackagePath()
        {
            var info = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(CodeGenerator).Assembly);
            return info.resolvedPath;
        }

        private static void SetupModelList(string path, string name, List<Type> modifiers)
        {
            string content = GetModelListBoilerplate();

            content = content.Replace("//[EnumElement]", "AddNewModifier" + ",\n        //[EnumElement]");
            foreach (var modifier in modifiers)
            {
                if (!content.Contains(modifier.Name))
                {
                    content = content.Replace("//[EnumElement]", modifier.Name + ",\n        //[EnumElement]");
                    content = content.Replace("//[TypeMatching]", "_typeMatching.Add(EModellularModifier." + modifier.Name + ",                         typeof(" + modifier.Name + "Model));\r\n            //[TypeMatching]");
                }
            }
            
            
            TrySaveToFile(path, name, content);
        }

        private static void TrySaveToFile(string path, string name, string content)
        {
            if (AssetDatabase.AssetPathExists(path))
            {
                if (true || AssetDatabase.AssetPathExists(path + "/" + name))
                {
                    using (var text = File.CreateText(path + "/" + name))
                    {
                        text.WriteLine(content);
                    }
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("[Modellular] : Couldn't refresh the modifier database because the package was either moved or modified. Expected path : " + path);
            }
        }
        public static string GenerateModifierInterface(Type modifier)
        {
            string boilerplate = GetModelBoilerplate();

            boilerplate = boilerplate.Replace("CLASSNAMEMODEL", modifier.Name + "Model");
            boilerplate = boilerplate.Replace("CLASSNAMEWITHSPACES", InsertSpaces(modifier.Name));
            boilerplate = boilerplate.Replace("CLASSNAMEWITHNAMESPACE", modifier.Namespace + "." + modifier.Name);
            boilerplate = boilerplate.Replace("CLASSNAME", modifier.Name);



            PropertyInfo[] properties = modifier.GetProperties();
            foreach (var ppt in properties)
            {
                boilerplate = boilerplate.Replace("//[Field]", FieldFromProperty(ppt));
                boilerplate = boilerplate.Replace("//[ReplicatedField]", ReplicatedFieldFromProperty(ppt));
                boilerplate = boilerplate.Replace("//[SetProperty]", SetTargetProperty(ppt));
                boilerplate = boilerplate.Replace("//[ChangeCheck]", ChangeCheck(ppt));
                boilerplate = boilerplate.Replace("//[ReplicatedFieldReset]", ReplicatedFieldReset(ppt));
            }
            return boilerplate;
        }

        private static string FieldFromProperty(PropertyInfo propertyInfo)
        {
            string result = "public " + MatchPropertyType(propertyInfo.PropertyType.Name) + " " + propertyInfo.Name;

            if (propertyInfo.HasAttribute<ModelularDefaultValueAttribute>())
            {
                    result += " = " + propertyInfo.GetAttribute<ModelularDefaultValueAttribute>().DefaultValue;
            }
            result += ";\n        //[Field]";
            return result;

        }
        private static string ReplicatedFieldFromProperty(PropertyInfo propertyInfo) => "private " + MatchPropertyType(propertyInfo.PropertyType.Name) + " _" + propertyInfo.Name.FirstCharacterToLower() + ";\n        //[ReplicatedField]";
        private static string SetTargetProperty(PropertyInfo propertyInfo) => "target." + propertyInfo.Name + " = " + propertyInfo.Name + ";\n            //[SetProperty]";
        private static string ChangeCheck(PropertyInfo propertyInfo) => " _" + propertyInfo.Name.FirstCharacterToLower() + " != " + propertyInfo.Name + " ||\n                //[ChangeCheck]";
        private static string ReplicatedFieldReset(PropertyInfo propertyInfo) => " _" + propertyInfo.Name.FirstCharacterToLower() + " = " + propertyInfo.Name + ";\n            //[ReplicatedFieldReset]";
        private static string MatchPropertyType(string type)
        {
            switch (type)
            {
                case "String": return "string";
                case "Single": return "float";
                case "Int32": return "int";
                case "Boolean": return "bool";

                default: return type;
            }
        }
        private static string InsertSpaces(string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && text[i - 1] == text.ToLower()[i - 1] && text[i] == text.ToUpper()[i])
                    result += " ";
                result += text[i];
            }
            return result;
        }

        private static string GetModelBoilerplate()
        {
            return "using UnityEngine;\r\nusing Modellular.Modifiers.Primitives;\r\nusing Modellular.Modifiers;\r\nusing Modellular.Selection;\r\n\r\n\r\nnamespace Modellular.Editor.Modifiers\r\n{\r\n\r\n    //[CreateAssetMenu(fileName = \"CLASSNAME\", menuName = \"Modellular/CLASSNAMEWITHSPACES\")]\r\n    public class CLASSNAMEMODEL : ModifierModel\r\n    {\r\n        #region Fields\r\n\r\n        //[Field]\r\n\r\n        // Replicated fields for change detection\r\n        private bool _enabled;\r\n        //[ReplicatedField]\r\n\r\n        #endregion\r\n\r\n        public CLASSNAMEMODEL()\r\n        {\r\n            underlyingModifier = new CLASSNAMEWITHNAMESPACE();\r\n        }\r\n\r\n        public override void ApplyParameters()\r\n        {\r\n            var target = (underlyingModifier as CLASSNAMEWITHNAMESPACE);\r\n            //[SetProperty]\r\n        }\r\n\r\n        public override bool DetectChanges()\r\n        {\r\n            // Insert here the comparison for all properties that should be change-checked\r\n            if \r\n            (\r\n                enabled != _enabled ||\r\n                //[ChangeCheck]\r\n                false\r\n            )\r\n            {\r\n                hasChanged = true;\r\n            }\r\n\r\n            // Reset the mirrored fields\r\n            _enabled = enabled;\r\n            //[ReplicatedFieldReset]\r\n\r\n            return hasChanged;\r\n        }\r\n    }\r\n\r\n}";
        }

        private static string GetModelListBoilerplate()
        {
            return "using Modellular.Editor.Modifiers;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;\r\n\r\nnamespace Modellular.Editor\r\n{\r\n    public enum EModellularModifier\r\n    {\r\n        //[EnumElement]\r\n    }\r\n\r\n    public static class ModifierModelList\r\n    {\r\n        private static Dictionary<EModellularModifier, System.Type> _typeMatching = new();\r\n        static ModifierModelList()\r\n        {\r\n            //[TypeMatching]\r\n        }\r\n        public static ModifierModel GetNewModifier(EModellularModifier modifier)\r\n        {\r\n\r\n            if (_typeMatching.TryGetValue(modifier, out System.Type typeToCreate))\r\n            {\r\n                var instance = ScriptableObject.CreateInstance(typeToCreate) as ModifierModel;\r\n                return instance;\r\n            }\r\n            return null;\r\n        }\r\n    }\r\n}";
        }
    }

}
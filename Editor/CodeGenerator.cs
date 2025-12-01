

using Modelular.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Codice.Client.BaseCommands.Import.Commit;

namespace Modelular.Editor
{
    public static class CodeGenerator
    {
        public static string PackagePath => GetPackagePath();
        private static string DefaultPackagePath = "Assets/Packages/Modelular";
        private static string BoilerplatePath = "Editor/Boilerplate";
        private static string GeneratedModels = "Runtime/Generated/Modifiers";
        //private static string GeneratedMeshes = "Runtime/Generated/Meshes";


        [MenuItem("Modelular/Refresh assembly")]
        public static void RegenerateModels()
        {
            GetPackagePath();

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
                                string model = GenerateModifierModel(type);
                                TrySaveToFile(Path.Combine(PackagePath, GeneratedModels), type.Name + "Model.cs", model);
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
            var sortedTypes = SortTypesByPriority(modifiers);

            var modelList = GenerateModelList(PackagePath, "ModifierModelList.cs", sortedTypes);
            TrySaveToFile(Path.Combine(PackagePath, GeneratedModels), "ModifierModelList.cs", modelList);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SetupIcons();
        }
        public static string GetPackagePath()
        {
            string result = DefaultPackagePath;
            var info = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(CodeGenerator).Assembly);
            if (info != null)
                result = info.resolvedPath;
            UnityEngine.Debug.Log("Package path is " + result);
            return result;
        }

        private static string GenerateModelList(string path, string name, List<Type> modifiers)
        {
            string content = GetModelListBoilerplate();

            content = content.Replace("//[EnumElement]", "AddNewModifier" + ",\n        //[EnumElement]");
            foreach (var modifier in modifiers)
            {
                if (!content.Contains(modifier.Name))
                {
                    content = content.Replace("//[PathMatching]", "ModifierPaths[\"CLASSNAME\"] = typeof(CLASSNAMEMODEL);\r\n            //[PathMatching]");
                    content = content.Replace("CLASSNAMEMODEL", modifier.Name + "Model");
                    if (modifier.HasAttribute(typeof(ModelularInterfaceAttribute)))
                    {
                        string itemName = modifier.GetAttribute<ModelularInterfaceAttribute>().ItemName;
                        if (!string.IsNullOrEmpty(itemName))
                            content = content.Replace("CLASSNAME", itemName);
                        else
                            content = content.Replace("CLASSNAME", modifier.Name);
                    }
                    else
                        content = content.Replace("CLASSNAME", modifier.Name);

                }
            }


            return content;
        }

        private static void SetupIcons()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(ModifierModel)))
                    {
                        Debug.Log("[Modelular] : Modifier model found : " + type.Name);
                        var decoy = ScriptableObject.CreateInstance(type);
                        if (EditorGUIUtility.GetIconForObject(decoy) != null)
                        {
                            UnityEngine.Debug.Log("[Modelular] : Icon found on modifier model : " + type.Name);

                        }
                        else
                        {
                            UnityEngine.Debug.Log("[Modelular] : Icon NOT found on modifier model : " + type.Name);
                            if (AssetDatabase.AssetPathExists(Path.Combine(PackagePath, "Editor/Icons", "CustomModifier.png")))
                            {
                                Debug.Log("[Modelular] : Custom modifier icon found");
                                Texture2D icon = AssetDatabase.LoadAssetAtPath(Path.Combine(PackagePath, "Editor/Icons", "CustomModifier.png"), typeof(Texture2D)) as Texture2D;
                                if (icon != null)
                                {
                                    Debug.Log("[Modelular] : Custom modifier icon loaded successfully - Trying to assign it");
                                    EditorGUIUtility.SetIconForObject(decoy, icon);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool TrySaveToFile(string path, string name, string content)
        {
            if (AssetDatabase.AssetPathExists(path))
            {
                using (var text = File.CreateText(Path.Combine(path, name)))
                {
                    text.WriteLine(content);
                }
                return true;
            }
            else
            {
                UnityEngine.Debug.LogWarning("[Modellular] : Couldn't refresh the modifier database because the package was either moved or modified. Expected path : " + path);
                return false;
            }
        }
        public static string GenerateModifierModel(Type modifier)
        {
            string boilerplate = GetModelBoilerplate();

            boilerplate = boilerplate.Replace("CLASSNAMEMODEL", modifier.Name + "Model");
            boilerplate = boilerplate.Replace("CLASSNAMEWITHSPACES", InsertSpacesBetweenWords(modifier.Name));
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
        private static string InsertSpacesBetweenWords(string text)
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
            string path = Path.Combine(GetPackagePath(), BoilerplatePath, "ModifierModelBoilerplate.txt");
            string content = File.ReadAllText(path);
            return content;
        }

        private static string GetModelListBoilerplate()
        {
            string path = Path.Combine(GetPackagePath(), BoilerplatePath, "ModifierModelListBoilerplate.txt");
            string content = File.ReadAllText(path);
            return content;
        }

        private static List<Type> SortTypesByPriority(Dictionary<int, List<Type>> modifiers)
        {
            List<Type> types = new();
            List<int> priorities = new();
            foreach (int priority in modifiers.Keys)
                priorities.Add(priority);
            priorities.Sort();
            foreach (int priority in priorities)
                types.AddRange(modifiers[priority]);
            return types;
        }
    }

}
using Modelular.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Modelular.Editor
{
    public static class CodeGenerator
    {
        public static string PackagePath => Hierarchy.PackagePath;
        private static string BoilerplatePath => Hierarchy.BoilerplatePath;
        private static string GeneratedModelsPath => Hierarchy.GeneratedModelsPath;
        private static string EditorPath => Hierarchy.EditorPath;

        private static string ModifierModelBoilerplate = "ModifierModelBoilerplate.txt";
        private static string ModifierModelListBoilerplate = "ModifierModelListBoilerplate.txt";
        private static string PrimitiveMenuItemsBoilerplate = "PrimitiveMenuItemsBoilerplate.txt";


        /// <summary>
        /// Regenerate scripts such as ModifierModels, the model list used for the "add modifier" button of the modular mesh component, or for the primitive menu items
        /// </summary>
        [MenuItem("CONTEXT/ModularMesh/Refresh editor scripts")]
        public static void RefreshModelularScripts()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Dictionary<int, List<Type>> modifiers = new ();
            List<Type> primitiveModels = new();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    TryGenerateModelForModifier(modifiers, type);
                    DetectPrimitive(type, primitiveModels);
                }
            }

            // Sort all the types by their priority
            var sortedTypes = SortTypesByPriority(modifiers);

            var modelList = GenerateModelList(sortedTypes);
            TrySaveToFile(GeneratedModelsPath, "ModifierModelList.cs", modelList);

            var primitiveMenuItems = GenerateMenuItemsForPrimitives(primitiveModels);
            TrySaveToFile(EditorPath, "PrimitiveMenuItems.cs", primitiveMenuItems);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SetupIcons();
        }
        /// <summary>
        /// Check if the type is a ModifierModel whose modifier is a primitive. If so, appends it to the list
        /// </summary>
        /// <param name="type"></param>
        /// <param name="primitives"></param>
        private static void DetectPrimitive(Type type, List<Type> primitives)
        {
            if (!type.IsSubclassOf(typeof(ModifierModel)))
            {
                return;
            }

            var decoy = ScriptableObject.CreateInstance(type) as ModifierModel;
            if (decoy == null)
                return;

            if (typeof(IPrimitiveModifier).IsAssignableFrom(decoy.underlyingModifier.GetType()))
            {
                primitives.Add(type);
            }
        }
        /// <summary>
        /// Generate the text content for the script enabling menu items for all the modifiers implementing IPrimitiveModifier
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private static string GenerateMenuItemsForPrimitives(List<Type> types)
        {
            string content = GetBoilerplate(PrimitiveMenuItemsBoilerplate);

            foreach (var type in types)
            {
                var decoy = ScriptableObject.CreateInstance(type) as ModifierModel;
                if (decoy == null)
                    continue;
                content = content.Replace("//[MenuItem]", "[MenuItem(\"GameObject/3D Object/Modelular/CLASSNAMEWITHSPACES\", priority = 1)]\r\n        public static void NewCLASSNAME(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(CLASSNAMEMODEL));\r\n        //[MenuItem]");
                content = content.Replace("CLASSNAMEMODEL", type.Name);
                content = content.Replace("CLASSNAMEWITHSPACES", InsertSpacesBetweenWords( decoy.underlyingModifier.GetType().Name));
                content = content.Replace("CLASSNAME", decoy.underlyingModifier.GetType().Name);
            }
            return content;
        }
        /// <summary>
        /// Generate the text content for the model list
        /// </summary>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        private static string GenerateModelList(List<Type> modifiers)
        {
            string content = GetBoilerplate(ModifierModelListBoilerplate);

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
        /// <summary>
        /// Generate the text content of the model script based on a modifier type
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static string GenerateModifierModel(Type modifier)
        {
            string boilerplate = GetBoilerplate(ModifierModelBoilerplate);

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
        /// <summary>
        /// Check if the type is a modifier that needs a model (scriptable object). If so, it generates it's content with GenerateModifierModel and saves it
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="type"></param>
        private static void TryGenerateModelForModifier(Dictionary<int, List<Type>> modifiers, Type type)
        {
            if (type.IsSubclassOf(typeof(Modifier)))
            {
                Attribute[] attrs = Attribute.GetCustomAttributes(type);
                foreach (Attribute attr in attrs)
                {
                    if (attr is ModelularInterfaceAttribute modAttr)
                    {
                        string model = GenerateModifierModel(type);
                        TrySaveToFile(GeneratedModelsPath, type.Name + "Model.cs", model);

                        if (!modifiers.ContainsKey(modAttr.Priority))
                            modifiers[modAttr.Priority] = new();
                        modifiers[modAttr.Priority].Add(type);

                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Setup the modelular icons on the scriptable objects to be shown in the inspector
        /// </summary>
        private static void SetupIcons()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(ModifierModel)))
                    {
                        var decoy = ScriptableObject.CreateInstance(type);
                        if (EditorGUIUtility.GetIconForObject(decoy) == null)
                        {
                            if (AssetDatabase.AssetPathExists(Path.Combine(PackagePath, "Editor/Icons", "CustomModifier.png")))
                            {
                                Texture2D icon = AssetDatabase.LoadAssetAtPath(Path.Combine(PackagePath, "Editor/Icons", "CustomModifier.png"), typeof(Texture2D)) as Texture2D;
                                if (icon != null)
                                {
                                    EditorGUIUtility.SetIconForObject(decoy, icon);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Saves the content at the specified path + name
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
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
        // Formatting
        private static string FieldFromProperty(PropertyInfo propertyInfo)
        {
            string result = "public " + MatchPropertyType(propertyInfo.PropertyType.Name) + " " + propertyInfo.Name;

            if (propertyInfo.HasAttribute<ModelularDefaultValueAttribute>())
            {
                    result += " = " + propertyInfo.GetAttribute<ModelularDefaultValueAttribute>().DefaultValue;
            }
            result += ";\n        //[Field]";
            result = AddAttributesToField(result, propertyInfo);
            return result;

        }
        private static string AddAttributesToField(string field, PropertyInfo propertyInfo)
        {
            string result = field;
            // Add attributes by reconstructing them individually
            foreach (var attr in propertyInfo.CustomAttributes)
            {
                if (attr.AttributeType == typeof(RangeAttribute))
                {
                    var a = propertyInfo.GetAttribute<RangeAttribute>();
                    result = $"[Range({a.min.ToString().Replace(",", ".")}f, {a.max.ToString().Replace(",", ".")}f)]\n        " + result;
                }
                else if (attr.AttributeType == typeof(MinAttribute))
                {
                    var a = propertyInfo.GetAttribute<MinAttribute>();
                    result = $"[Min({a.min.ToString().Replace(",", ".")}f)]\n        " + result;
                }
                else if (attr.AttributeType == typeof(HideInInspector))
                {
                    var a = propertyInfo.GetAttribute<HideInInspector>();
                    result = $"[HideInInspector]\n        " + result;
                }
            }
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
        /// <summary>
        /// Returns the same text but with spaces inserted before words : HelloWorld -> Hello World
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns the content of the specified boilerplate text file. Boilerplate code should be stored in Editor/Boilerplate
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetBoilerplate(string name)
        {
            string content = null;
            string path = Path.Combine(BoilerplatePath, name);
            if (AssetDatabase.AssetPathExists(path))
                content = File.ReadAllText(path);
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
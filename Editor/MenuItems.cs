using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Modelular.Runtime;

namespace Modelular.Editor
{

    public class MenuItems
    {
        [MenuItem("GameObject/Modellular/Empty", priority =20)]
        public static void CreateEmptyModellularMesh(MenuCommand menuCommand)
        {
            GameObject obj = new GameObject("Modellular mesh", typeof(ModularMesh));

            SetupHierarchy(obj, menuCommand);
        }

        [MenuItem("Assets/Create/Modellular/New modifier script", false, 80)]
        public static void CreateModifierScript()
        {
            string path = GetSelectedPathOrFallback();
            string fileName = "NewModifier.cs";
            string fullPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, fileName));

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<CreateModifierScriptAction>(),
                fullPath,
                null,
                null // You can pass template metadata, not needed here
            );
        }

        private static string GetSelectedPathOrFallback()
        {
            var obj = UnityEditor.Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path))
                return "Assets";

            if (Path.HasExtension(path))
                return Path.GetDirectoryName(path);

            return path;
        }

        private static string MakeModifierBoilerplate()
        {
            var boilerplate = Resources.Load("Assets/Packages/Modelular/Core/Resources/_ModifierBoilerplate.txt");
            string content = ((TextAsset)boilerplate).text;
            return content;
        }
        /// <summary>
        /// Source : 
        /// https://github.com/Maraakis/ChristinaCreatesGames/blob/main/Addin%20Menu%20Items%20to%20the%20right%20click%20menu/CreateFromHierarchyMenu_AdvancedButton.cs
        /// </summary>
        /// <param name="menuCommand"></param>
        private static void SetupHierarchy(GameObject instance, MenuCommand menuCommand)
            {
                GameObject parent = UnityEditor.Selection.activeGameObject;

                if (menuCommand.context as GameObject != null)
                    parent = menuCommand.context as GameObject;


                if (parent != null)
                    GameObjectUtility.SetParentAndAlign(instance, parent);

                UnityEditor.Selection.activeGameObject = instance;

            }

        // I used ChatGPT for this, sorry father for I have sinned
        public class CreateModifierScriptAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // Read your template
                string template = MakeModifierBoilerplate();

                string className = Path.GetFileNameWithoutExtension(pathName);
                string content = template.Replace("#SCRIPTNAME#", className);

                // Create file
                File.WriteAllText(pathName, content);

                AssetDatabase.Refresh();

                // Ping the created asset
                var asset = AssetDatabase.LoadAssetAtPath<Object>(pathName);
                ProjectWindowUtil.ShowCreatedAsset(asset);
            }
        }
    }

}
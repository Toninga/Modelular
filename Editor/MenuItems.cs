using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Modelular.Runtime;

namespace Modelular.Editor
{

    public class MenuItems
    {
        

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
            string content = File.ReadAllText(Path.Combine(Hierarchy.BoilerplatePath, "ModifierBoilerplate.txt"));
            return content;
        }

        // I used ChatGPT for this, sorry father for I have sinned
        public class CreateModifierScriptAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // Read your template
                string template = MakeModifierBoilerplate();
                if (template == null)
                    return;

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
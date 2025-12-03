using Modelular.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modelular.Editor
{
    public class ModelularMeshGenerator
    {
        public static ModularMesh New(MenuCommand menuCommand, params System.Type[] modifiers)
        {
            ModularMesh go = new GameObject("New modellular mesh", typeof(ModularMesh)).GetComponent<ModularMesh>();
            var currentScene = SceneManager.GetActiveScene();
            if (currentScene != null)
            {
                var selected = Selection.activeObject as GameObject;
                if (selected != null)
                {
                    go.transform.parent = selected.transform;
                }
            }

            foreach (var modifier in modifiers)
            {
                if (modifier.IsSubclassOf(typeof(ModifierModel)))
                {
                    go.AddModifier(modifier);
                }
            }
            Selection.activeObject = go;
            return go;
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
    }
}
using UnityEditor;
using UnityEngine;

namespace Modelular.Runtime
{
    public class SaveFile : Modifier
    {
        #region Properties

        #endregion

        #region Fields
        private StackElement _lastBakingResult;
        private string _lastPath;
        #endregion

        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            _lastBakingResult = previousResult;
            SaveBakedMesh();
            return previousResult;
        }

        public bool SaveBakedMesh()
        {
            if (_lastBakingResult == null)
                return false;
            if (_lastBakingResult.Mesh == null)
            {
                new BakeMesh().Bake(_lastBakingResult);
            }
            return SaveMesh(_lastBakingResult.Mesh, "NewMesh");
        }

        /// <summary>
        /// Source : 
        /// https://github.com/GameDevBox/CombineMeshes-Unity/blob/main/InstanceCombiner.cs
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="name"></param>
        /// <param name="makeNewInstance"></param>
        /// <param name="optimizeMesh"></param>
        public bool SaveMesh(Mesh mesh, string name)
        {
            Debug.Log("StackTrace");
            // Find the best target directory : The user's chosen path first, otherwise Modellular/Generated, otherwise the project root
            string finalDirectory = "";
            string defaultDir = "Assets/Packages/Modellular/Generated";
            string root = "Assets/";
            if (_lastPath != null || _lastPath != "")
            {
                if (AssetDatabase.AssetPathExists(_lastPath))
                    finalDirectory = _lastPath;
            }
            if (finalDirectory == "" && AssetDatabase.AssetPathExists(defaultDir))
                finalDirectory = defaultDir;
            if (finalDirectory == "")
                finalDirectory = root;


            string path = EditorUtility.SaveFilePanel("Save Mesh Asset", finalDirectory, name, "asset");
            if (string.IsNullOrEmpty(path)) return false;


            path = FileUtil.GetProjectRelativePath(path);
            _lastPath = path;

            /*
            Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

            if (optimizeMesh)
                MeshUtility.Optimize(mesh);
            */

            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
            return true;
        }

        #endregion
    }
}
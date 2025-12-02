using System.IO;

namespace Modelular.Runtime
{
    public class Hierarchy
    {
        public static string PackagePath => GetPackagePath();
        public static string DefaultPackagePath = "Assets/Packages/Modelular";
        public static string BoilerplatePath => Path.Combine(PackagePath, "Editor/Boilerplate");
        public static string GeneratedModelsPath => Path.Combine(PackagePath, "Runtime/Generated/Modifiers");
        public static string GeneratedMeshesPath => Path.Combine(PackagePath, "Runtime/Generated/Meshes");
        public static string EditorPath => Path.Combine(PackagePath, "Editor");
        public static string TexturesPath => Path.Combine(PackagePath, "Runtime/Core/Graphics/Textures");


        /// <summary>
        /// Returns the local path for the modelular package
        /// </summary>
        /// <returns></returns>
        private static string GetPackagePath()
        {
            string result = DefaultPackagePath;
            var info = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(Hierarchy).Assembly);
            if (info != null)
                result = info.resolvedPath;
            return result;
        }
    }
}
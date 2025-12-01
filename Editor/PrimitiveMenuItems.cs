using Modelular.Runtime;
using UnityEditor;
using UnityEngine;

namespace Modelular.Editor
{
    public class PrimitiveMenuItems
    {
        [MenuItem("GameObject/3D Object/Modelular/Empty", priority = 1)]
        public static void CreateEmptyModellularMesh(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand);
        [MenuItem("GameObject/3D Object/Modelular/Cube", priority = 1)]
        public static void NewCube(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(CubeModel));
        [MenuItem("GameObject/3D Object/Modelular/UV Sphere", priority = 1)]
        public static void NewUVSphere(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(UVSphereModel));
    }
}
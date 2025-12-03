using Modelular.Runtime;
using UnityEditor;

namespace Modelular.Editor
{
    public class PrimitiveMenuItems
    {
        [MenuItem("GameObject/3D Object/Modelular/Cube", priority = 1)]
        public static void NewCube(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(CubeModel));
        [MenuItem("GameObject/3D Object/Modelular/Cylinder", priority = 1)]
        public static void NewCylinder(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(CylinderModel));
        [MenuItem("GameObject/3D Object/Modelular/Quad", priority = 1)]
        public static void NewQuad(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(QuadModel));
        [MenuItem("GameObject/3D Object/Modelular/Torus", priority = 1)]
        public static void NewTorus(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(TorusModel));
        [MenuItem("GameObject/3D Object/Modelular/UV Sphere", priority = 1)]
        public static void NewUVSphere(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(UVSphereModel));
        //[MenuItem]
    }
}

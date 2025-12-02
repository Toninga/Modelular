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
        [MenuItem("GameObject/3D Object/Modelular/Ground", priority = 1)]
        public static void NewGround(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(GroundModel));
        [MenuItem("GameObject/3D Object/Modelular/Quad", priority = 1)]
        public static void NewQuad(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(QuadModel));
        [MenuItem("GameObject/3D Object/Modelular/UVSphere", priority = 1)]
        public static void NewUVSphere(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, typeof(UVSphereModel));
        //[MenuItem]
    }
}

using Modelular.Runtime;
using UnityEditor;

namespace Modelular.Editor
{
    public class PrimitiveMenuItems
    {
        [MenuItem("GameObject/3D Object/Modelular/Cone", priority = 1)]
        public static void NewCone(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, "New Cone", typeof(ConeModel));
        [MenuItem("GameObject/3D Object/Modelular/Cube", priority = 1)]
        public static void NewCube(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, "New Cube", typeof(CubeModel));
        [MenuItem("GameObject/3D Object/Modelular/Cylinder", priority = 1)]
        public static void NewCylinder(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, "New Cylinder", typeof(CylinderModel));
        [MenuItem("GameObject/3D Object/Modelular/Disk", priority = 1)]
        public static void NewDisk(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, "New Disk", typeof(DiskModel));
        [MenuItem("GameObject/3D Object/Modelular/Quad", priority = 1)]
        public static void NewQuad(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, "New Quad", typeof(QuadModel));
        [MenuItem("GameObject/3D Object/Modelular/Torus", priority = 1)]
        public static void NewTorus(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, "New Torus", typeof(TorusModel));
        [MenuItem("GameObject/3D Object/Modelular/UV Sphere", priority = 1)]
        public static void NewUVSphere(MenuCommand menuCommand) => ModelularMeshGenerator.New(menuCommand, "New UVSphere", typeof(UVSphereModel));
        //[MenuItem]
    }
}

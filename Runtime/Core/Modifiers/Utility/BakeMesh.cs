using UnityEngine;

namespace Modelular.Runtime
{
    public class BakeMesh : Modifier
    {

        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.MeshData = DataProcessor.PolygonsToMeshData(previousResult.Polygons);
            Mesh result = DataProcessor.MeshDataToMesh(previousResult.MeshData);
            previousResult.Mesh = result;
            return previousResult;
        }
    }

}
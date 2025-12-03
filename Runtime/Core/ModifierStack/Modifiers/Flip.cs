

namespace Modelular.Runtime
{
    [ModelularInterface("Normals/Flip", 20)]
    public class Flip : Modifier, IModifier
    {
        #region Parameters
        public string TargetSelectionGroup { get; set; }
        [ModelularDefaultValue("true")]
        public bool FlipFaces { get; set; } = true;
        [ModelularDefaultValue("true")]
        public bool FlipNormals { get; set; } = true;

        #endregion
        public override StackElement Bake(StackElement previousResult)
        {
            if (FlipFaces) previousResult.ReplacePolygons((p) => FlipFace(p), TargetSelectionGroup);
            if (FlipNormals) previousResult.ReplaceVertices((vert) => vert.Flipped(), TargetSelectionGroup);
            return previousResult;
        }

        private Polygon FlipFace(Polygon polygon)
        {
            var result = new Polygon(polygon);
            for (int i = 0; i < polygon.triangles.Count / 3; i++)
            {
                int temp = polygon.triangles[i * 3 + 1];
                result.triangles[i * 3 + 1] = polygon.triangles[i * 3 + 2];
                result.triangles[i * 3 + 2] = temp;
            }
            return result;
        }
    }
}
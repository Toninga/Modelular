using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("UV/Tiling and offset", 80)]
    public class UVTilingOffset : Modifier
    {
        [ModelularDefaultValue("Vector2.one")]
        public Vector2 Tiling {  get; set; }
        public Vector2 Offset { get; set; }
        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.ReplaceVertices((v) => new Vertex(
                v.position,
                v.normal,
                v.color,
                v.submesh,
                v.UV0 * Tiling + Offset,
                v.UV1,
                v.UV2,
                v.UV3
                ));
            return previousResult;
        }
    }
}
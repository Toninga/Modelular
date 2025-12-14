using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("UV/Tiling and offset", 80)]
    public class UVTilingOffset : Modifier, IModifier
    {
        #region Parameters
        public string TargetSelectionGroup { get; set; }
        [ModelularDefaultValue("Vector2.one")]
        public Vector2 Tiling {  get; set; }
        public Vector2 Offset { get; set; }
        #endregion

        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.ReplaceVertices((v) => new Vertex(v, overrideUV0:v.UV0 * Tiling + Offset), TargetSelectionGroup);
            return previousResult;
        }
        #endregion
    }
}
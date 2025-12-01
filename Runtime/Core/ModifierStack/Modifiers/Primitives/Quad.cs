using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Quad", 0)]
    public class Quad : Modifier, IPrimitiveModifier
    {
        #region Parameters
        [ModelularDefaultValue("Color.white")]
        public Color Color { get; set; }
        public string OutputSelectionGroup { get; set; }
        [ModelularDefaultValue("Vector2.one")]
        public Vector2 Size { get; set; } = Vector2.one;
        #endregion



        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            var quad = Make(Size);
            previousResult.AddPolygon(quad, OutputSelectionGroup);
            return previousResult;
        }

        protected Polygon Make(Vector2 size)
        {
            return Polygon.Quad(size);
        }
        #endregion
    }

}
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface()]
    public class ColorGradient : Modifier
    {
        #region Properties

        public Color ColorA { get; set; }
        [ModelularDefaultValue("Color.white")]
        public Color ColorB { get; set; } = Color.white;

        #endregion

        
        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            Bounds bounds = previousResult.GetBoundingBox();
            previousResult.ReplaceVertices((v) => new Vertex(v, overrideColor: NewColor(v, bounds)));
            return previousResult;
        }

        private Color NewColor(Vertex v, Bounds bounds)
        {
            float t = Mathf.InverseLerp(bounds.min.y, bounds.max.y, v.y);
            t = Mathf.Clamp01(t);
            return Color.Lerp(ColorA, ColorB, t);
        }

        #endregion
    }
}
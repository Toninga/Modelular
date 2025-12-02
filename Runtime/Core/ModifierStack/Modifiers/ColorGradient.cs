using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Color/Color gradient")]
    public class ColorGradient : Modifier
    {
        #region Properties
        [ModelularDefaultValue("Vector3.up")]
        public Vector3 Direction { get; set; } = Vector3.up;
        public Color ColorA { get; set; }
        [ModelularDefaultValue("Color.white")]
        public Color ColorB { get; set; } = Color.white;

        #endregion

        
        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            FindEndVertices(previousResult, out Vertex l, out Vertex r);
            previousResult.ReplaceVertices((v) => new Vertex(v, overrideColor: NewColor(v, l, r, Direction.normalized)));
            return previousResult;
        }

        private Color NewColor(Vertex v, Vertex left, Vertex right, Vector3 dir)
        {
            float dl = Vector3.Dot(left.position, dir);
            float dr = Vector3.Dot(right.position, dir);
            float d = Vector3.Dot(v.position, dir);
            float t = Mathf.InverseLerp(dl, dr, d);
            return Color.Lerp(ColorA, ColorB, t);
        }
        /// <summary>
        /// Finds the leftmost and rightmost vertices of the mesh along the given direction,
        /// ensuring that the gradient starts and ends precisely so that both colors are represented by at least one vertex each
        /// </summary>
        /// <param name="previousResult"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private void FindEndVertices(StackElement previousResult, out Vertex left,  out Vertex right)
        {
            Vector3 dir = Direction.normalized;
            float min = float.MaxValue;
            float max = float.MinValue;
            left = new Vertex();
            right = new Vertex();
            foreach (Vertex v in previousResult.Vertices)
            {
                float d = Vector3.Dot(v.position, dir);
                if (d < min)
                {
                    left = v;
                    min = d;
                }
                if (d > max)
                {
                    right = v;
                    max = d;
                }
            }
        }

        #endregion
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Torus")]
    public class Torus : Modifier, IPrimitive
    {
        #region Properties
        public DefaultPrimitiveProperties DefaultParameters { get; set; }

        [ModelularDefaultValue("1f")]
        [Min(0f)]
        public float Radius { get; set; } = 1f;
        [ModelularDefaultValue("0.1f")]
        [Min(0f)]
        public float Thickness { get; set; } = 0.1f;
        [ModelularDefaultValue("3")]
        [Min(3)]
        public int RadialSubdiv { get; set; } = 3;
        [ModelularDefaultValue("3")]
        [Min(3)]
        public int ThicknessSubdiv { get; set; } = 3;
        [ModelularDefaultValue("1f")]
        [Range(0f, 1f)]
        public float Arc { get; set; } = 1f;
        public EAxis Axis { get; set; }


        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            List <Polygon> polygons = Make();

            polygons = (this as IPrimitive).ApplyDefaultParameters(polygons);

            previousResult.AddPolygons(polygons);
            return previousResult;
        }

        private List<Polygon> Make()
        {
            RadialSubdiv = Mathf.Max(RadialSubdiv, 3);
            ThicknessSubdiv = Mathf.Max(ThicknessSubdiv, 3);

            List<Polygon> result = new();

            Vertex[] vertices = new Vertex[ExpectedVertexCount()];

            for (int r = 0; r < RadialSubdiv; r++)
            {
                for (int t = 0; t < ThicknessSubdiv; t++)
                {
                    Vertex v = Vertex.VertexFromPolarCoord(Thickness, 0, (t * 2f * Mathf.PI) / ThicknessSubdiv);
                    v = Quaternion.Euler(90f, 0,0) * v;
                    v.x += Radius;
                    v = Quaternion.Euler(0, (r * 360f) / RadialSubdiv * Arc, 0) * v;
                    vertices[r * ThicknessSubdiv + t] = v;
                }
            }

            int skipLastRing = 0;
            if (Arc != 1)
                skipLastRing = 1;
            for (int r = 0; r < RadialSubdiv - skipLastRing; r++)
            {
                for (int t = 0; t < ThicknessSubdiv; t++)
                {
                    int rs = RadialSubdiv;
                    int ts = ThicknessSubdiv;
                    int curr = r * ThicknessSubdiv + t;
                    int a = curr;
                    int b = t == ts - 1 ? curr + 1 - ts : curr + 1;
                    int c = r == rs - 1 ? t : curr + ts;
                    int d = curr + ts + 1;
                    if (r == rs - 1)
                        if (t == ts - 1)
                            d = 0;
                        else
                            d = t + 1;
                    else if (t == ts - 1)
                        d = curr + 1;
                        Polygon p = new();
                    try
                    {
                        p = new Polygon(vertices[a], vertices[b], vertices[d], vertices[c]);
                    }
                    catch
                    {
                        Debug.LogWarning("" + a + "  " + b + "  " + c + "  " + d + "  total is " + ExpectedVertexCount());
                    }
                    result.Add(p);
                }
            }


            return result;

        }

        private int ExpectedVertexCount()
        {
            return RadialSubdiv * ThicknessSubdiv;
        }
        

        #endregion
    }
}
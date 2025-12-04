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
        [ModelularDefaultValue("true")]
        public bool Caps { get; set; } = true;


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

            Vector3 up = AxisUtility.GetAxisDirection(Axis);
            Vector3 rotationAxis = AxisUtility.SwitchOnAxis(
                Axis,
                Vector3.zero,
                Vector3.zero,
                Vector3.right,
                Vector3.right,
                Vector3.zero,
                Vector3.zero
                );
            Vector3 right = AxisUtility.SwitchOnAxis
                (
                Axis,
                Vector3.forward,
                Vector3.back,
                Vector3.right,
                Vector3.left,
                Vector3.left,
                Vector3.right
                );
            int lastRing = 0;
            if (Arc != 1)
                lastRing = 1;

            List<Polygon> result = new();

            Vertex[] vertices = new Vertex[ExpectedVertexCount()];

            // Generate the vertices
            for (int r = 0; r < RadialSubdiv + lastRing; r++)
            {
                for (int t = 0; t < ThicknessSubdiv; t++)
                {
                    Vertex v = Vertex.VertexFromPolarCoord(Thickness, 0, (t * 2f * Mathf.PI) / ThicknessSubdiv);

                    v = Quaternion.Euler(rotationAxis * 90f) * v;
                    v = v + right * Radius;
                    
                    float a = (r * 360f) / (RadialSubdiv) * Arc;
                    v = Quaternion.Euler(up * a) * v;
                    vertices[r * ThicknessSubdiv + t] = v;
                }
            }

            // Generate the donut's polygons
            for (int r = 0; r < RadialSubdiv; r++)
            {
                for (int t = 0; t < ThicknessSubdiv; t++)
                {
                    int rs = RadialSubdiv + lastRing;
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

            // Generate the caps
            if (Caps && Arc < 1)
            {
                List<Vertex> verts0 = new();

                for (int i = 0; i < ThicknessSubdiv; i++)
                    verts0.Add(new Vertex(vertices[ThicknessSubdiv - 1 - i], overrideNormal:Vector3.up));
                Polygon cap0 = new Polygon(verts0);

                List<Vertex> verts1 = new();
                for (int i = 0; i < ThicknessSubdiv; i++)
                    verts1.Add(new Vertex(vertices[vertices.Length - ThicknessSubdiv + i], overrideNormal:Vector3.up));
                Polygon cap1 = new Polygon(verts1);

                result.Add(cap0);
                result.Add(cap1);
            }

            return result;

        }

        private int ExpectedVertexCount()
        {
            int lastRing = 0;
            if (Arc != 1)
                lastRing = 1;
            return (RadialSubdiv + lastRing) * ThicknessSubdiv;
        }
        

        #endregion
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Cone")]
    public class Cone : Modifier, IPrimitive
    {
        #region Parameters
        public DefaultPrimitiveProperties DefaultParameters { get; set; }
        [ModelularDefaultValue("0.5f")]
        [Min(0f)]
        public float BaseRadius { get; set; } = 0.5f;
        [ModelularDefaultValue("1f")]
        [Min(0f)]
        public float Height { get; set; } = 1;
        [ModelularDefaultValue("16")]
        public int FaceCount { get; set; } = 16;
        [ModelularDefaultValue("1f")]
        [Range(0f, 1f)]
        public float Fill { get; set; } = 1f;
        [ModelularDefaultValue("1f")]
        [Range(0f, 1f)]
        public float Arc { get; set; } = 1f;
        [ModelularDefaultValue("true")]
        public bool Caps {  get; set; } = true;
        [ModelularDefaultValue("true")]
        public bool SectionCaps {  get; set; } = true;

        #endregion

        #region Methods

        public override StackElement Bake(StackElement previousResult)
        {
            List<Polygon> result = Make();
            result = (this as IPrimitive).ApplyDefaultParameters(result);

            previousResult.AddPolygons(result, DefaultParameters.OutputSelectionGroup);
            return previousResult;
        }

        private List<Polygon> Make()
        {
            // Add your polygons to this list
            List<Polygon> result = new();

            int arc = 0;
            if (Arc < 1)
                arc = 1;
            Vertex[] bot = new Vertex[FaceCount + arc * 2];
            Vertex[] top = new Vertex[FaceCount + arc * 2];
            // Create your primitive (vertices + polygons) here
            for (int i = 0; i < FaceCount + arc; i++)
            {
                int curr = i + arc;
                var v = Vertex.VertexFromPolarCoord(BaseRadius, 0f, (i * 2f * Mathf.PI * Arc) / FaceCount);
                bot[curr] = v;
                bot[curr].normal = Vector3.down;
                if (BaseRadius != 0)
                    bot[curr].UV0 = new Vector2(v.x / 2 / BaseRadius + 0.5f, v.z / 2 / BaseRadius + 0.5f);
                else
                    bot[curr].UV0 = new Vector2(0.5f,0.5f);


                    //curr = i + FaceCount + arc * 2;
                    v = Vertex.VertexFromPolarCoord(Mathf.Lerp(BaseRadius, 0, Fill), 0f, (i * 2f * Mathf.PI * Arc) / FaceCount);
                top[FaceCount - i + arc * 2 - 1] = v;
                top[FaceCount - i + arc * 2 - 1].y += Height * Fill;
                top[FaceCount - i + arc * 2 - 1].normal = Vector3.up;
                if (BaseRadius != 0)
                    top[FaceCount - i + arc - 1].UV0 = new Vector2(1 - v.x / 2 / BaseRadius + 0.5f, v.z / 2 / BaseRadius + 0.5f);
                else
                    top[FaceCount - i + arc - 1].UV0 = new Vector2(0.5f, 0.5f);
            }

            if (Arc < 1)
            {
                bot[0] = new Vertex(Vector3.zero, Vector3.down, UV0: Vector2.one / 2);
                top[0] = new Vertex(Vector3.up * Height * Fill, Vector3.up, UV0: Vector2.one / 2);

            }
            if (Caps)
            {
                result.Add(new Polygon(bot));
                if (Fill < 1)
                    result.Add(new Polygon(top));
            }

            // Make the shell polygons
            for (int i = 0; i < FaceCount; i++)
            {
                // Retrieve the corresponding vertices
                int a = i + arc;
                int b = (i + 1 + arc) % (FaceCount + arc*2);
                int c = (FaceCount - i + arc) % (FaceCount + arc*2);
                int d = (FaceCount - i-1 + arc) % (FaceCount + arc*2);

                //Debug.Log(a + ":" + bot.Length + " ; " + b + ":" + bot.Length + " ; " + c + ":" + top.Length + " ; " + d + ":" + top.Length);

                Vertex A = new Vertex(bot[a]);
                Vertex B = new Vertex(top[c]);
                Vertex C = new Vertex(top[d]);
                Vertex D = new Vertex(bot[b]);

                // Fix their normals
                Vector3 O2R; // Origin to Radius
                Vector3 R2T; // Radius to Top
                Vector3 rotAxis; // Rotation axis (cross product of O2R and R2T)
                Vector3 normal; // Resultant normal
                
                // First normal
                O2R = A.position.normalized;
                R2T = (B.position - A.position).normalized;
                rotAxis = Vector3.Cross(O2R, R2T);
                rotAxis.Normalize();
                normal = Quaternion.AngleAxis(-90, rotAxis) * R2T.normalized;
                A.normal = normal.normalized;
                B.normal = normal.normalized;

                // Second normal
                O2R = D.position.normalized;
                R2T = (C.position - D.position).normalized;
                rotAxis = Vector3.Cross(O2R, R2T);
                rotAxis.Normalize();
                normal = Quaternion.AngleAxis(-90, rotAxis) * R2T.normalized;
                D.normal = normal.normalized;
                C.normal = normal.normalized;
                
                Polygon p = new Polygon(A, B, C, D);
                result.Add(p);
            }

            if (SectionCaps)
            {
                // Make the caps inside the cone when the arc isn't complete
                if (Arc < 1)
                {
                    result.Add(new Polygon(
                        new Vertex(bot[1], overrideNormal:Vector3.back),
                        new Vertex(bot[0], overrideNormal: Vector3.back),
                        new Vertex(top[0], overrideNormal:Vector3.back),
                        new Vertex(top[FaceCount + arc], overrideNormal:Vector3.back)
                        ));
                    Vector3 normal = Quaternion.Euler(0f, Mathf.Lerp(360f, 0f, Arc), 0f) * Vector3.forward;
                    result.Add(new Polygon(
                        new Vertex(bot[0], overrideNormal: normal),
                        new Vertex(bot[FaceCount + arc], overrideNormal: normal),
                        new Vertex(top[1], overrideNormal:normal),
                        new Vertex(top[0], overrideNormal:normal)
                        ));

                }
            }

            return result;
        }

        private int ExpectedVertexCount()
        {
            if (Arc < 1)
                return FaceCount * 2 + 2;
            return FaceCount * 2;
        }

        #endregion
    }
}
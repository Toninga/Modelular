using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/UV Sphere", 0)]
    public class UVSphere : Modifier, IPrimitiveModifier
    {
        #region Parameters
        [ModelularDefaultValue("Color.white")]
        public Color Color { get; set; } = Color.white;
        public string OutputSelectionGroup { get; set; }
        [ModelularDefaultValue("0.5f")]
        public float Radius { get; set; } = 0.5f;
        [ModelularDefaultValue("8")]
        public int HorizontalSubdivisions { get; set; } = 8;
        [ModelularDefaultValue("16")]
        public int VerticalSubdivisions { get; set; } = 16;
        public string TargetSelectionSet { get; set; }
        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            var sphere = MakeUVSphere(Radius, HorizontalSubdivisions, VerticalSubdivisions);
            previousResult.AddPolygons(sphere, OutputSelectionGroup);
            return previousResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r">Radius</param>
        /// <param name="hs">Horizontal subdivisions - set to 1 if inferior to 1</param>
        /// <param name="vs">Vertical subdivisions - set to 3 if inferior to 3</param>
        /// <returns></returns>
        private List<Polygon> MakeUVSphere(float r, int hs, int vs)
        {
            // Ensure a minimum set of subdivisions are used
            hs = Mathf.Max(hs, 1);
            vs = Mathf.Max(vs, 3);

            var result = new List<Polygon>();

            Vertex[] vertices = new Vertex[hs * vs + 2];
            int[] triangles = new int[(hs+1) * vs * 3];

            // MAKE VERTICES
            // Bottom and top vertices
            vertices[0] = new Vertex(new Vector3(0,-r,0), Vector3.down, UV0:new Vector2(0.5f, 0f));
            vertices[1] = new Vertex(new Vector3(0, r, 0), Vector3.up, UV0: new Vector2(0.5f, 1f));

            // Horizontal subdivisions, from bottom to top
            for (int h = 0; h<hs; h++)
            {
                // Vertical subdivisions, in trigonometric circle order
                for (int v = 0; v<vs ; v++)
                {
                    float lat = (h - (h+1) * 1/(hs+1)) / (float)hs * Mathf.PI - Mathf.PI/2;
                    float lon = (float)v / (float)vs * Mathf.PI * 2;
                    var vert = Vertex.VertexFromPolarCoord(r, lat, lon);
                    vert = new Vertex(vert, overrideUV0: new Vector2(v/(vs-1f), (h+0.5f)/hs));
                    vertices[h * vs + v + 2] = vert;
                }
            }

            // MAKE POLYGONS

            // Make the bottom and top caps, without their last triangles, which are edge cases
            for (int i = 0; i < vs - 1; i++)
            {
                result.Add(new Polygon(vertices[0], vertices[i + 2], vertices[i + 2 + 1]));
                result.Add(new Polygon(vertices[1], vertices[vertices.Length - i - 1], vertices[vertices.Length - i - 2]));
            }
            // Make these edge cases
            result.Add(new Polygon(vertices[0], vertices[vs+1], vertices[2]));
            result.Add(new Polygon(vertices[1], vertices[vertices.Length - vs], vertices[vertices.Length - 1]));

            for (int h = 0; h < hs-1; h++)
            {
                for (int v = 0; v < vs;v++)
                {
                    int curr = h * vs + v + 2;
                    int offset = 0;
                    if (v == vs-1)
                        offset = vs;
                    Polygon p = new Polygon(
                        vertices[curr],
                        vertices[curr + 1 - offset],
                        vertices[curr + vs],
                        vertices[curr + vs + 1 - offset]
                        );
                    p.triangles = new List<int> {0,2,1,3,1,2};
                    result.Add(p);
                }
            }


            return result;
        }

        


        #endregion
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Cylinder", 0)]
    public class Cylinder : Modifier, IPrimitive
    {
        #region Parameters
        [ModelularDefaultValue("DefaultPrimitiveProperties.Default()")]
        public DefaultPrimitiveProperties DefaultParameters { get; set; } = DefaultPrimitiveProperties.Default();
        [ModelularDefaultValue("1f")]
        public float Height { get; set; } = 1f;
        [ModelularDefaultValue("0.5f")]
        public float Radius { get; set; } = 0.5f;
        public int HorizontalSubdivisions { get; set; } = 0;
        [ModelularDefaultValue("16")]
        public int VerticalSubdivisions { get; set; } = 16;
        [ModelularDefaultValue("true")]
        public bool GenerateCaps { get; set; } = true;
        [ModelularDefaultValue("true")]
        public bool GenerateShell { get; set; } = true;
        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            StackElement obj = new StackElement();
            obj.AddPolygons(MakeCylinder(Height, Radius, HorizontalSubdivisions, VerticalSubdivisions), DefaultParameters.OutputSelectionGroup);
            (this as IPrimitive).ApplyDefaultParameters(obj);


            previousResult.Merge(obj);
            return previousResult;
        }

        private List<Polygon> MakeCylinder(float height, float rad, int hs, int vs)
        {
            // Ensure a minimum set of subdivisions are used
            hs = Mathf.Max(hs, 0);
            vs = Mathf.Max(vs, 3);
            hs += 2;

            var result = new List<Polygon>();

            if (!GenerateCaps && !GenerateShell)
                return result;

            bool cb = GenerateCaps;
            bool sb = GenerateShell;
            int cv = cb ? 2 + vs * 2 : 0; // Cap vertex count
            int cv2 = cb ? 2 + vs : 0; // First cap vertex count
            int sv = sb ? vs * (hs+2) : 0; // Shell vertex count
            int ct = cb ? vs * 2 : 0; // Cap triangle count
            int st = sb ? vs * (hs + 1) : 0; // Shell triangle count

            Vertex[] vertices = new Vertex[cv + sv];
            int[] triangles = new int[ct * 3 + st * 3];
            
            // MAKE VERTICES
            if (cb)
            {
                // Bottom and top vertices
                vertices[0] = new Vertex(new Vector3(0, -height / 2, 0), Vector3.down, UV0:Vector2.one/2f);
                vertices[1] = new Vertex(new Vector3(0, height / 2, 0), Vector3.up, UV0: Vector2.one / 2f);
            }

            // Horizontal subdivisions, from bottom to top
            if (cb) MakeVertexRing(-height/2, rad, vs, Vector3.down).CopyTo(vertices, 2);
            
            if (sb)
            {
                for (int h = 0; h <= hs; h++)
                {
                    float altitude = ((h - (h + 1) * 1 / (hs + 1)) / (hs - 1f) - 0.5f) * height;
                    var verts = MakeVertexRing(altitude, rad, vs);
                    for (int i = 0; i < verts.Length; i++)
                    {
                        verts[i].UV0 = new Vector2((i%vs)/(vs-1f), h/(hs-1f));
                    }
                    verts.CopyTo(vertices, vs * h + cv2);
                }
            }
            
            if (cb) MakeVertexRing(height / 2, rad, vs, Vector3.up).CopyTo(vertices, vs + 2 + sv);

            // MAKE POLYGONS

            if (cb)
            {
                // Make the bottom and top caps, without their last triangles, which are edge cases
                for (int i = 0; i < vs - 1; i++)
                {
                    result.Add(new Polygon(vertices[0], vertices[i + 2], vertices[i + 2 + 1]));
                    result.Add(new Polygon(vertices[1], vertices[vertices.Length - i - 1], vertices[vertices.Length - i - 2]));
                }
                // Make these edge cases
                result.Add(new Polygon(vertices[0], vertices[vs + 1], vertices[2]));
                result.Add(new Polygon(vertices[1], vertices[vertices.Length - vs], vertices[vertices.Length - 1]));
            }

            
            if (sb)
            {
                for (int h = 0; h < hs - 1; h++)
                {
                    for (int v = 0; v < vs; v++)
                    {
                        int curr = h * vs + v + cv2;
                        int offset = 0;
                        if (v == vs - 1)
                            offset = vs;
                        Polygon p = new Polygon(
                            vertices[curr],
                            vertices[curr + 1 - offset],
                            vertices[curr + vs],
                            vertices[curr + vs + 1 - offset]
                            );
                        p.triangles = new List<int> { 0, 2, 1, 3, 1, 2 };
                        result.Add(p);
                    }
                }
            }

            return result;
        }

        private Vertex[] MakeVertexRing(float height, float r, int count, Vector3 normal=default)
        {
            Vertex[] result = new Vertex[count];
            for (int i = 0; i < count; i++)
            {
                var v = Vertex.VertexFromPolarCoord(r, 0f, i/(float)count * 2 * Mathf.PI - Mathf.PI);
                v.position = new Vector3(v.x, height, v.z);
                if (normal != default)
                {
                    v.normal = normal;
                    if (v.normal == Vector3.up)
                        v.UV0 = new Vector2(v.x/r/2 + 0.5f, v.z/r/2+0.5f);
                    else
                        v.UV0 = new Vector2(1 - (v.x/r/2 + 0.5f), v.z/r/2+0.5f);
                }
                result[i] = v;
            }
            return result;
        }

        #endregion
    }
}
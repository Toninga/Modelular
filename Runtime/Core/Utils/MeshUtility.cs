using System.Linq;
using UnityEngine;

namespace Modelular.Runtime
{
    public static class MeshUtility
    {
        /// <summary>
        /// Takes in  a normal and right vector, computes the corresponding up vector, and recalculates the right vector to be perpendicular to the normal and up vectors.
        /// The normal stays untouched.
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="right"></param>
        /// <param name="newRight"></param>
        /// <param name="newUp"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void GetRightAndUpVectorsFromNormalAndRightVectors(Vector3 normal, Vector3 right, out Vector3 newRight, out Vector3 newUp)
        {
            if (right == default) right = (Vector3.Cross(normal, Vector3.right).magnitude == 0 ? Vector3.forward : Vector3.right);
            if (normal == right)
                throw new System.ArgumentException("Invalid arguments : The normal vector and the right vector cannot be equal - '" + normal + "'");
            newUp = Vector3.Cross(right, normal).normalized;
            newRight = Vector3.Cross(normal, newUp).normalized;
        }

        public enum NormalMode { AlignedWithPolygonNormal, SpreadingOutwards}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="subdiv"></param>
        /// <param name="normal"></param>
        /// <param name="right"></param>
        /// <param name="normalMode">
        /// The normal mode defines how the vertex normals are set.
        /// SpreadingOutwards is convenient for cylinder slices, while AlignedWithPolygonNormal is better for disks.</param>
        /// <returns></returns>
        public static Vertex[] MakeCircle(float radius, int subdiv, Vector3 normal, Vector3 right, NormalMode normalMode=NormalMode.SpreadingOutwards)
        {
            GetRightAndUpVectorsFromNormalAndRightVectors(normal, right, out right, out var up);
            Vertex[] vertices = new Vertex[subdiv];
            float r = radius;

            for (int i = 0; i < subdiv; i++)
            {
                float t = i / (float)subdiv;
                float arc = t * 6.2832f;
                vertices[i] = new Vertex();
                vertices[i].position = right * Mathf.Cos(arc) * r + up * Mathf.Sin(arc) * r;

                if (normalMode == NormalMode.SpreadingOutwards) 
                    vertices[i].normal = vertices[i].position.normalized;
                else if (normalMode == NormalMode.AlignedWithPolygonNormal)
                    vertices[i].normal = normal.normalized;
            }

            return vertices;
        }

        public static Polygon MakeDisc(float radius, int subdiv, Vector3 normal, Vector3 right=default)
        {
            Polygon polygon = new Polygon();
            polygon.vertices = MakeCircle(radius, subdiv, normal, right, NormalMode.AlignedWithPolygonNormal).ToList();
            polygon.triangles = new ();

            for (int i = 0; i < polygon.vertices.Count - 2; i++)
            {
                polygon.triangles.Add(0);
                polygon.triangles.Add(i+1);
                polygon.triangles.Add((i+2) % polygon.vertices.Count);
            }
            return polygon;
        }
        /// <summary>
        /// Outputs a quad polygon. By default, the right vector is Vector3.right.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="normal"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Polygon MakeQuad(Vector2 size, Vector3 normal, Vector3 right=default)
        {
            Polygon result;

            float x, y;
            x = size.x / 2;
            y = size.y / 2;

            GetRightAndUpVectorsFromNormalAndRightVectors(normal, right, out right, out var up);

            // Vertices
            Vertex v1, v2, v3, v4;
            v1 = new Vertex(right * x  + up *  y, normal, UV0: new Vector2(1, 1));
            v2 = new Vertex(right * x  + up * -y, normal, UV0: new Vector2(1, 0));
            v3 = new Vertex(right * -x + up * -y, normal, UV0: new Vector2(0, 0));
            v4 = new Vertex(right * -x + up *  y, normal, UV0: new Vector2(0, 1));

            result = new Polygon(new Vertex[4] { v1, v2, v3, v4 }, 0, 1, 2, 0, 2, 3);

            return result;
        }
        public static Polygon MakeGrid(Vector2 size, Vector2Int subdiv, Vector3 normal, Vector3 right=default)
        {
            Polygon result;

            subdiv = new Vector2Int(Mathf.Max(0, subdiv.x), Mathf.Max(0, subdiv.y));
            int col = subdiv.x + 2, row = subdiv.y + 2;
            Vertex[] verts = new Vertex[col * row];
            // There is one less quad-row/col than there are vertex-rows/col, then 2 triangles per quad, and 3 vertices per triangle
            int[] tris = new int[(col - 1) * (row - 1) * 2 * 3];

            GetRightAndUpVectorsFromNormalAndRightVectors(normal, right, out right, out var up);

            for (int iy = 0; iy < row; iy++)
            {
                for (int ix = 0; ix < col; ix++)
                {
                    verts[iy * col + ix] = new Vertex();

                    verts[iy * col + ix].position = 
                        right * (size.x * ix / (col-1)) + // right offset
                        up * (size.y * iy / (row - 1)) - // up offset
                        right * size.x / 2 - // center on the right axis
                        up * size.y / 2; // center on the up axis

                    verts[iy * col + ix].UV0 = new Vector2(ix / (col - 1f), iy / (row - 1f));

                    verts[iy * col + ix].normal = normal.normalized;
                }
            }
            for (int iy = 0; iy < (row - 1); iy++)
            {
                for (int ix = 0; ix < (col - 1); ix++)
                {
                    int vert = iy * col + ix;
                    // Anti-clockwise (flip 1-2 and 4-5 to make it clockwise)
                    tris[(iy * (col - 1) + ix) * 6 + 0] = vert;
                    tris[(iy * (col - 1) + ix) * 6 + 1] = vert + 1 + col;
                    tris[(iy * (col - 1) + ix) * 6 + 2] = vert + 1;
                    tris[(iy * (col - 1) + ix) * 6 + 3] = vert;
                    tris[(iy * (col - 1) + ix) * 6 + 4] = vert + col;
                    tris[(iy * (col - 1) + ix) * 6 + 5] = vert + 1 + col;
                }
            }
            
            result = new Polygon(verts, tris);
            
            return result;
        }
    }
}

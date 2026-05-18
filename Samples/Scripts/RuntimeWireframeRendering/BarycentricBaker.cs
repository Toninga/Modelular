// BarycentricBaker.cs
using UnityEngine;

public static class BarycentricBaker
{
    // Call once per mesh (cache the result — don't call every frame).
    public static Mesh BakeBarycentric(Mesh src)
    {
        if (src == null) return null;
        // Triangulate: each triangle needs its own 3 vertices so we can
        // assign a unique barycentric coord to each corner.
        int[] srcTris = src.triangles;
        Vector3[] srcVerts = src.vertices;
        Vector3[] srcNorms = src.normals;
        Vector2[] srcUV0 = src.uv;

        int triCount = srcTris.Length;
        var verts = new Vector3[triCount];
        var norms = new Vector3[triCount];
        var uv0 = new Vector2[triCount];
        var uv1 = new Vector3[triCount]; // barycentric
        var tris = new int[triCount];

        Vector3[] bary = { Vector3.right, Vector3.up, Vector3.forward }; // (1,0,0) (0,1,0) (0,0,1)

        for (int i = 0; i < triCount; i++)
        {
            int src_i = srcTris[i];
            verts[i] = srcVerts[src_i];
            norms[i] = srcNorms.Length > src_i ? srcNorms[src_i] : Vector3.up;
            uv0[i] = srcUV0.Length > src_i ? srcUV0[src_i] : Vector2.zero;
            uv1[i] = bary[i % 3];
            tris[i] = i;
        }

        var mesh = new Mesh { name = src.name + "_bary" };
        mesh.indexFormat = triCount > 65535
            ? UnityEngine.Rendering.IndexFormat.UInt32
            : UnityEngine.Rendering.IndexFormat.UInt16;
        mesh.vertices = verts;
        mesh.normals = norms;
        mesh.uv = uv0;
        mesh.SetUVs(1, uv1);   // UV channel 1 = TEXCOORD1 in the shader
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        return mesh;
    }
}
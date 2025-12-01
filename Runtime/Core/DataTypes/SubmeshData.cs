using UnityEngine;

namespace Modelular.Runtime
{

    public struct SubmeshData
    {
        public Vector3[] positions;
        public Vector3[] normals;
        public Color[] colors;
        public Vector2[] UV0;
        public Vector2[] UV1;
        public Vector2[] UV2;
        public Vector2[] UV3;
        public int[] triangles;
        public short ID;
        public EVertexFlags vertexFlags;

        public SubmeshData(
            Vector3[] positions,
            Vector3[] normals=default,
            Color[] colors=default,
            Vector2[] UV0=default,
            Vector2[] UV1=default,
            Vector2[] UV2=default,
            Vector2[] UV3=default,
            int[] triangles=default,
            short submesh = default,
            EVertexFlags vertexFlags=default
            )
        {
            this.positions = positions;
            this.normals = normals;
            this.colors = colors;
            this.UV0 = UV0;
            this.UV1 = UV1;
            this.UV2 = UV2;
            this.UV3 = UV3;
            this.triangles = triangles;
            this.ID = submesh;
            this.vertexFlags = vertexFlags;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modelular.Runtime
{

    public static class DataProcessor
    {

        #region Methods

        public static Mesh PolygonToMesh(Polygon polygon) => MeshDataToMesh(PolygonToMeshData(polygon));
        public static Mesh PolygonsToMesh(IEnumerable<Polygon> polygons) => MeshDataToMesh(PolygonsToMeshData(polygons));
        public static Mesh MeshDataToMesh(MeshData data)
        {
            Dictionary<int, Mesh> submeshes = new ();

            foreach (var sm in data.Submeshes)
            {
                if (!submeshes.ContainsKey(sm.ID))
                    submeshes[sm.ID] = new();

                if (sm.vertexFlags.HasFlag(EVertexFlags.Position) && sm.positions.Length >= UInt16.MaxValue)
                    submeshes[sm.ID].indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

                if (sm.vertexFlags.HasFlag(EVertexFlags.Position))
                    submeshes[sm.ID].SetVertices(sm.positions);
                else
                {
                    Debug.LogWarning("[Modellular] : Vertices weren't generated properly, mesh baking was aborted.");
                    return null;
                }
                if (sm.vertexFlags.HasFlag(EVertexFlags.Normal)) submeshes[sm.ID].SetNormals(sm.normals);
                if (sm.vertexFlags.HasFlag(EVertexFlags.Color)) submeshes[sm.ID].SetColors(sm.colors);
                if (sm.vertexFlags.HasFlag(EVertexFlags.UV0)) submeshes[sm.ID].SetUVs(0, sm.UV0);
                if (sm.vertexFlags.HasFlag(EVertexFlags.UV1)) submeshes[sm.ID].SetUVs(1, sm.UV0);
                if (sm.vertexFlags.HasFlag(EVertexFlags.UV2)) submeshes[sm.ID].SetUVs(2, sm.UV0);
                if (sm.vertexFlags.HasFlag(EVertexFlags.UV3)) submeshes[sm.ID].SetUVs(3, sm.UV0);

                submeshes[sm.ID].SetTriangles(sm.triangles, 0);
                submeshes[sm.ID].RecalculateBounds();
            }
            CombineInstance[] ci = new CombineInstance[submeshes.Count];
            int i = 0;
            foreach (var kvp in submeshes)
            {
                ci[i] = new CombineInstance { mesh = kvp.Value, transform=Matrix4x4.identity };
                i++;
            }
            Mesh result = new Mesh();
            result.CombineMeshes(ci,false);
            return result;
        }

        public static void PolygonToMeshData(Polygon polygon, out MeshData meshData) => PolygonsToMeshData(new List<Polygon> { polygon }, out meshData);
        public static MeshData PolygonToMeshData(Polygon p)
        {
            PolygonToMeshData(p, out MeshData data);
            return data;
        }
        public static MeshData PolygonsToMeshData(IEnumerable<Polygon> polygons)
        {
            PolygonsToMeshData(polygons, out MeshData data);
            return data;
        }
        public static void PolygonsToMeshData
            (
            IEnumerable<Polygon> polygons,
            out MeshData meshData
            )
        {
            Dictionary<short, int> verticesPerSubMesh = new Dictionary<short, int>();
            Dictionary<short, int> trisPerSubMesh = new Dictionary<short, int>();

            // Sort vertices by submesh
            int totalVertCount = 0;
            int totalTriCount = 0;
            foreach (var polygon in polygons)
            {
                foreach(var vert in polygon.vertices)
                {
                    if (!verticesPerSubMesh.ContainsKey(vert.submesh))
                        verticesPerSubMesh[vert.submesh] = 0;
                    verticesPerSubMesh[vert.submesh] ++;

                }
                foreach (int tri in polygon.triangles)
                {
                    short submesh = polygon.vertices[tri].submesh;
                    if (!trisPerSubMesh.ContainsKey(submesh))
                        trisPerSubMesh[submesh] = 0;
                    trisPerSubMesh[submesh]++;
                }
                totalVertCount += polygon.vertices.Count;
                totalTriCount += polygon.triangles.Count;
            }

            //Sort the submeshes
            var submeshes = verticesPerSubMesh.Keys.ToList();
            submeshes.Sort();

            // Iterate over each submesh and assign it's vertices to a SubmeshData
            SubmeshData[] submeshDatas = new SubmeshData[verticesPerSubMesh.Count];
            int md = 0;
            foreach (short submeshIndex in submeshes)
            {
                SubmeshData submeshData = new SubmeshData();
                short currentSubmesh = submeshIndex;
                int vertCount = verticesPerSubMesh[currentSubmesh]; // How many vertices there are in this submesh
                int triCount = trisPerSubMesh[currentSubmesh]; // How many triangle indices there are in this submesh

                submeshData.ID = currentSubmesh;
                submeshData.triangles = new int[triCount];

                // Assign vertex values in the arrays
                int v = 0; // How many vertices have been stored for this submesh
                int t = 0; // How many triangles have been stored for this submesh
                foreach (var polygon in polygons)
                {
                    Dictionary<int, int> newVertexIndex = new ();
                    int vp = 0; // How many vertices have been stored for this polygon
                    int tp = 0; // How many triangles have been stored for this polygon
                    foreach (var vertex in polygon.vertices)
                    {
                        if (vertex.submesh != currentSubmesh)
                            continue;

                        foreach (var tri in polygon.triangles)
                        {
                            if (tri == vp)
                            {
                                newVertexIndex[tri] = v;
                                break;
                            }
                        }



                        // Set flags
                        submeshData.vertexFlags = submeshData.vertexFlags | DetectFlags(vertex);

                        if (submeshData.vertexFlags.HasFlag(EVertexFlags.Position))
                        {
                            if (submeshData.positions == null)
                                submeshData.positions = new Vector3[vertCount];
                            submeshData.positions[v] = vertex.position;
                        }

                        if (submeshData.vertexFlags.HasFlag(EVertexFlags.Normal))
                        {
                            if (submeshData.normals == null)
                                submeshData.normals = new Vector3[vertCount];
                            submeshData.normals[v] = vertex.normal;
                        }

                        if (submeshData.vertexFlags.HasFlag(EVertexFlags.Color))
                        {
                            if (submeshData.colors == null)
                                submeshData.colors = new Color[vertCount];
                            submeshData.colors[v] = vertex.color;
                        }

                        if (submeshData.vertexFlags.HasFlag(EVertexFlags.UV0))
                        {
                            if (submeshData.UV0 == null)
                                submeshData.UV0 = new Vector2[vertCount];
                            submeshData.UV0[v] = vertex.UV0;
                        }

                        if (submeshData.vertexFlags.HasFlag(EVertexFlags.UV1))
                        {
                            if (submeshData.UV1 == null)
                                submeshData.UV1 = new Vector2[vertCount];
                            submeshData.UV1[v] = vertex.UV1;
                        }

                        if (submeshData.vertexFlags.HasFlag(EVertexFlags.UV2))
                        {
                            if (submeshData.UV2 == null)
                                submeshData.UV2 = new Vector2[vertCount];
                            submeshData.UV2[v] = vertex.UV2;
                        }

                        if (submeshData.vertexFlags.HasFlag(EVertexFlags.UV3))
                        {
                            if (submeshData.UV3 == null)
                                submeshData.UV3 = new Vector2[vertCount];
                            submeshData.UV3[v] = vertex.UV3;
                        }



                        v++;
                        vp++;
                    }
                    foreach(int tri in polygon.triangles)
                    {
                        int triangle = submeshData.triangles[tri];
                        if (polygon.vertices[triangle].submesh != currentSubmesh)
                            continue;
                        submeshData.triangles[t] = newVertexIndex[tri];
                        t++;
                        tp++;
                    }

                }


                submeshDatas[md] = submeshData;
                md++;



            }

            meshData = new(submeshDatas);

        }

        public static EVertexFlags DetectFlags(Vertex vert)
        {
            EVertexFlags flags = EVertexFlags.None;
            Vertex defaultVertex = new Vertex(Vector3.zero);
            if (vert.position != defaultVertex.position) flags = flags | EVertexFlags.Position;
            if (vert.normal != defaultVertex.normal) flags = flags | EVertexFlags.Normal;
            if (vert.color != defaultVertex.color) flags = flags | EVertexFlags.Color;
            if (vert.UV0 != defaultVertex.UV0) flags = flags | EVertexFlags.UV0;
            if (vert.UV1 != defaultVertex.UV1) flags = flags | EVertexFlags.UV1;
            if (vert.UV2 != defaultVertex.UV2) flags = flags | EVertexFlags.UV2;
            if (vert.UV3 != defaultVertex.UV3) flags = flags | EVertexFlags.UV3;

            return flags;
        }
        public static Vertex OverrideVertexProperties(Vertex v, Vertex overrides, bool replaceOnlyNonDefaultProperties = true)
        {
            if (!replaceOnlyNonDefaultProperties)
                return overrides;
            return new Vertex(
                overrides.position == default ? v.position : overrides.position,
                overrides.normal == default ? v.normal : overrides.normal,
                overrides.color == default ? v.color : overrides.color,
                overrides.submesh == default ? v.submesh : overrides.submesh,
                overrides.UV0 == default ? v.UV0 : overrides.UV0,
                overrides.UV1 == default ? v.UV1 : overrides.UV1,
                overrides.UV2 == default ? v.UV2 : overrides.UV2,
                overrides.UV3 == default ? v.UV3 : overrides.UV3
                );
        }

        #endregion
    }

}
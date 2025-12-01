

using Modelular.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Modelular.Runtime
{
    public static class Visualization
    {
        public static void DrawNormals(List<Vertex> vertices, Vector3 position = default, float length = 1f, EColorCoding colorCoding = EColorCoding.VertexColor, Color color=default)
        {
            if (vertices == null || vertices.Count == 0) return;
            if (color == default)
                color = Color.green;

            foreach (var v in vertices)
            {
                DrawNormal(v, position, length, colorCoding, color);
            }
        }

        public static void DrawNormal(Vertex vertex, Vector3 position = default, float length = 1f, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (color == default)
                color = Color.green;

            switch (colorCoding)
            {
                case EColorCoding.Custom:
                    Gizmos.color = color;
                    break;
                case EColorCoding.VertexColor:
                    Gizmos.color = vertex.color;
                    break;
                case EColorCoding.Selection:
                    Gizmos.color = color;
                    break;
            }
            if (vertex.normal != Vector3.zero)
            {
                Vector3 p = vertex.position + position;
                Gizmos.DrawLine(p, p + vertex.normal.normalized * length);
            }
        }

        public static void DrawFaces(List<Polygon> faces, Vector3 position = default, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (faces == null || faces.Count == 0) return;
            if (color == default)
                color = Color.orange;

            foreach (Polygon p in faces)
            {
                DrawFace(p, position, colorCoding, color);
            }
        }

        public static void DrawFace(Polygon face, Vector3 position = default, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (color == default)
                color = Color.orange;

            switch (colorCoding)
            {
                case EColorCoding.Custom:
                    Gizmos.color = color;
                    break;
                case EColorCoding.VertexColor:
                    Gizmos.color = face.vertices[0].color;
                    break;
                case EColorCoding.Selection:
                    Gizmos.color = color;
                    break;
            }
            Gizmos.DrawMesh(DataProcessor.PolygonToMesh(face), position);
        }
        public static void DrawVertices(List<Vertex> vertices, Vector3 position = default, float size = 0.05f, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (vertices == null || vertices.Count == 0) return;
            if (color == default)
                color = Color.white;

            foreach (Vertex v in vertices)
            {
                DrawVertex(v, position, size, colorCoding, color);
            }
        }
        public static void DrawVertex(Vertex vertex, Vector3 position = default, float size = 0.05f, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (color == default)
                color = Color.white;

            Camera editorCam = Camera.current;
            float s = size;
            if (editorCam != null)
                s = s * Vector3.Distance(vertex.position, editorCam.transform.position) / 5;
            switch (colorCoding)
            {
                case EColorCoding.Custom:
                    Gizmos.color = color;
                    break;
                case EColorCoding.VertexColor:
                    Gizmos.color = vertex.color;
                    break;
                case EColorCoding.Selection:
                    Gizmos.color = color;
                    break;
            }
            var p = vertex.position + position;
            Gizmos.DrawCube(p, new Vector3(s, s, s));
        }

        public static void DrawVertexNumbers(List<Vertex> vertices, Vector3 position = default, float distance = 0.1f)
        {
            if (vertices == null || vertices.Count == 0) return;
            Dictionary<Vertex, int> vertCount = new();
            int i = 0;
            foreach (Vertex v in vertices)
            {
                if (!vertCount.ContainsKey(v))
                    vertCount[v] = 0;

                DrawTextOnVertex(v, i.ToString(), position, distance * (vertCount[v] + 1));
                vertCount[v]++;

                i++;
            }
        }
        public static void DrawTextOnVertex(Vertex vertex, string text, Vector3 position = default, float distance = 0.1f)
        {
            var p = vertex.position + position;
            Handles.Label(p + vertex.normal * distance, text);
        }

    }


}
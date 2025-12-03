

using Modelular.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Modelular.Runtime
{
    public static class Visualization
    {
        public static void DrawNormals(List<Vertex> vertices, UnityEngine.Transform transform = default, float length = 1f, EColorCoding colorCoding = EColorCoding.VertexColor, Color color=default)
        {
            if (vertices == null || vertices.Count == 0) return;
            if (color == default)
                color = Color.green;

            foreach (var v in vertices)
            {
                DrawNormal(v, transform, length, colorCoding, color);
            }
        }

        public static void DrawNormal(Vertex vertex, UnityEngine.Transform transform = default, float length = 1f, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
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
                Vector3 p = (Vector3)(transform.localToWorldMatrix * new Vector4(vertex.x, vertex.y, vertex.z, 1)) + transform.position;
                Vector4 n = vertex.normal.normalized;
                Gizmos.DrawLine(p, p + (Vector3)(transform.rotation * n * length));
            }
        }

        public static void DrawFaces(List<Polygon> faces, UnityEngine.Transform transform = default, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (faces == null || faces.Count == 0) return;
            if (color == default)
                color = new Color(1f, 0.6470588f, 0f, 1f);

            foreach (Polygon p in faces)
            {
                DrawFace(p, transform, colorCoding, color);
            }
        }

        public static void DrawFace(Polygon face, UnityEngine.Transform transform = default, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (color == default)
                color = new Color(1f, 0.6470588f, 0f, 1f); ;

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
            
            Gizmos.DrawMesh(DataProcessor.PolygonToMesh(face), transform.position, transform.rotation, transform.localScale);
        }
        public static void DrawVertices(List<Vertex> vertices, UnityEngine.Transform transform = default, float size = 0.05f, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (vertices == null || vertices.Count == 0) return;
            if (color == default)
                color = Color.white;

            foreach (Vertex v in vertices)
            {
                DrawVertex(v, transform, size, colorCoding, color);
            }
        }
        public static void DrawVertex(Vertex vertex, UnityEngine.Transform transform = default, float size = 0.05f, EColorCoding colorCoding = EColorCoding.VertexColor, Color color = default)
        {
            if (color == default)
                color = Color.white;

            Vector3 p = (Vector3)(transform.localToWorldMatrix * new Vector4(vertex.x, vertex.y, vertex.z, 1)) + transform.position;
            Camera editorCam = Camera.current;
            float s = size;
            if (editorCam != null)
                s = s * Vector3.Distance(p, editorCam.transform.position) / 5;
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
            Gizmos.DrawCube(p, new Vector3(s, s, s));
        }

        public static void DrawVertexNumbers(List<Vertex> vertices, UnityEngine.Transform transform = default, float distance = 0.1f)
        {
            if (vertices == null || vertices.Count == 0) return;
            Dictionary<Vertex, int> vertCount = new();
            int i = 0;
            foreach (Vertex vertex in vertices)
            {
                if (!vertCount.ContainsKey(vertex))
                    vertCount[vertex] = 0;

                DrawTextOnVertex(vertex, i.ToString(), transform, distance * (vertCount[vertex] + 1));
                vertCount[vertex]++;

                i++;
            }
        }
        public static void DrawTextOnVertex(Vertex vertex, string text, UnityEngine.Transform transform = default, float distance = 0.1f)
        {
            Vector3 p = transform.localToWorldMatrix * new Vector4(vertex.x, vertex.y, vertex.z, 1);
            Handles.Label(p + vertex.normal * distance, text);
        }

    }


}
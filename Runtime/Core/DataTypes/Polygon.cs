using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modelular.Data
{
    public struct Polygon
    {
        public List<Vertex> vertices;
        public List<int> triangles;
        public string SelectionGroup;

        public Polygon UnitQuad
        {
            get => Quad(Vector2.one);
        }

        public void ReplaceVertices(Func<Vertex, Vertex> operation)
        {
            for (int i = 0; i < vertices.Count; i++)
                vertices[i] = operation(vertices[i]);
        }
        public void Rotate(Quaternion rotator)
        {
            List<Vertex> newVertices = new List<Vertex>();
            foreach (var vertex in vertices)
            {
                newVertices.Add(rotator * vertex);
            }
            vertices = newVertices;
        }

        public static void Rotate(Polygon polygon, Quaternion rotator)
        {
            List<Vertex> newVertices = new List<Vertex>();
            foreach (var vertex in polygon.vertices)
            {
                newVertices.Add(rotator * vertex);
            }
            polygon.vertices = newVertices;
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex v = new(vertices[i]);
                v.color = color;
                vertices[i] = v;
            }
        }

        public static void SetColor(Polygon polygon, Color color)
        {
            for (int i = 0; i < polygon.vertices.Count; i++)
            {
                Vertex v = new(polygon.vertices[i]);
                v.color = color;
                polygon.vertices[i] = v;
            }
        }

        public static Polygon Quad(Vector2 size)
        {
            Polygon result;

            float x, y;
            x = size.x / 2;
            y = size.y / 2;


            // Vertices
            Vertex v1, v2, v3, v4;
            v1 = new Vertex(new Vector3(x, 0, y), Vector3.up,   UV0:new Vector2(1,1));
            v2 = new Vertex(new Vector3(x, 0, -y), Vector3.up,  UV0:new Vector2(1,0));
            v3 = new Vertex(new Vector3(-x, 0, -y), Vector3.up, UV0:new Vector2(0,0));
            v4 = new Vertex(new Vector3(-x, 0, y), Vector3.up,  UV0:new Vector2(0,1));

            result = new Polygon(new Vertex[4] { v1, v2, v3, v4 }, 0, 1, 2, 0, 2, 3);

            return result;
        }
        public Polygon(bool init=true)
        {
            this.vertices = new();
            this.triangles = new();
            this.SelectionGroup = "";
        }

        public Polygon(Polygon p, string overrideSelectionGroup = default)
        {
            this.vertices = new();
            this.triangles = new();
            this.SelectionGroup = string.IsNullOrEmpty(overrideSelectionGroup) ? p.SelectionGroup : overrideSelectionGroup;
            foreach (Vertex v in p.vertices)
                this.vertices.Add(new Vertex(v, overrideSelectionGroup:SelectionGroup));
            foreach(int i in p.triangles)
                this.triangles.Add(i);
        }

        public Polygon(List<Vertex> vertices)
        {
            // Set vertices
            if (vertices.Count < 3)
                throw new ArgumentOutOfRangeException("Only " + vertices.Count + " vertices were specified to make a polygon. Please put at least 3");
            this.vertices = vertices;

            // Auto-generate triangles
            this.triangles = new List<int>();
            int triCount = vertices.Count - 2;
            for (int tri = 0 ; tri < triCount; tri++)
            {
                this.triangles.Add(0);
                this.triangles.Add(tri + 1);
                this.triangles.Add(tri + 2);
            }
            this.SelectionGroup = "";
        }

        public Polygon(List<Vertex> vertices, List<int> triangles)
        {
            this.vertices = vertices;
            this.triangles = triangles;
            this.SelectionGroup = "";
        }

        public Polygon(Vertex[] vertices, params int[] triangles)
        {
            this.vertices = new List<Vertex>(vertices);
            this.triangles = new List<int>(triangles);
            this.SelectionGroup = "";
        }

        public Polygon(params Vertex[] vertices)
        {
            this.vertices = new List<Vertex>(vertices);

            // Auto-generate triangles
            this.triangles = new List<int>();
            int triCount = vertices.Length - 2;
            for (int tri = 0; tri < triCount; tri++)
            {
                this.triangles.Add(0);
                this.triangles.Add(tri + 1);
                this.triangles.Add(tri + 2);
            }

            this.SelectionGroup = "";
        }

        public static Polygon operator *(Matrix4x4 m, Polygon p)
        {
            Polygon polygon = new(true);
            foreach (Vertex v in p.vertices)
                polygon.vertices.Add(m * v);
            polygon.triangles = p.triangles;
            return polygon;
        }

        public static Polygon operator *(Quaternion q,  Polygon p)
        {
            Polygon polygon = new(true);
            foreach(Vertex v in p.vertices)
                polygon.vertices.Add(q * v);
            polygon.triangles = p.triangles;
            return polygon;
        }

        public static Polygon operator +(Polygon p, Vector3 o)
        {
            Polygon polygon = new(true);
            foreach(Vertex v in p.vertices)
                polygon.vertices.Add(v + o);
            polygon.triangles = p.triangles;
            return polygon;
        }
        public static Polygon operator +(Vector3 o, Polygon p) => p + o;
        public static Polygon operator -(Polygon p,  Vector3 o) => p + (-o);
        public static Polygon operator -(Vector3 o, Polygon p) => p - o;


    }
}
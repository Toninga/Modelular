using Modelular.Data;
using Modelular.Selection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Modifiers
{

    public class StackElement
    {
        public MeshData MeshData { get; set; }
        public List<Material> Materials { get; set; } = new();
        public List<Polygon> Polygons { get;} = new();
        public List<Vertex> Vertices { get; } = new();
        public Mesh Mesh { get; set; }
        public Dictionary<string, SelectionStack> SelectionStacksByName { get; set; } = new();

        #region Methods

        public void AddPolygons(List<Polygon> polygons, string targetSelectionGroup = default)
        {
            EnforceSelectionStack(targetSelectionGroup);
            if (string.IsNullOrEmpty(targetSelectionGroup))
            {
                foreach (var p in polygons)
                    Polygons.Add(p);
            }
            else
            {
                foreach (var p in polygons)
                    Polygons.Add(new Polygon(p, targetSelectionGroup));
            }

                DetectVertices();
        }
        public void AddPolygon(Polygon polygon, string targetSelectionGroup = default)
        {
            EnforceSelectionStack(targetSelectionGroup);
            Polygons.Add(new Polygon(polygon, targetSelectionGroup));
            Vertices.AddRange(polygon.vertices);
        }
        public void AddSelection(string selectionName, Selector selector)
        {
            EnforceSelectionStack(selectionName)?.AddSelector(selector);
        }
        public void ClearSelection(string selectionName)
        {
            if (SelectionStacksByName.ContainsKey (selectionName))
                SelectionStacksByName.Remove (selectionName);
        }
        public void ClearSelections() => SelectionStacksByName = new();
        public void ReplaceVertices(Func<Vertex, Vertex> operation, string selectionName = default)
        {
            EnforceSelectionStack(selectionName);
            if (!string.IsNullOrEmpty(selectionName))
            {
                if (!SelectionStacksByName.ContainsKey(selectionName))
                    return;
                for (int p = 0; p < Polygons.Count; p++)
                {
                    for (int v = 0; v < Polygons[p].vertices.Count; v++)
                    {
                        if (SelectionStacksByName[selectionName].Contains(Polygons[p].vertices[v]))
                            Polygons[p].vertices[v] = operation(Polygons[p].vertices[v]);
                    }
                }
            }
            else
                foreach (var polygon in Polygons)
                    polygon.ReplaceVertices(operation);

            DetectVertices();
        }
        public void ReplacePolygons(Func<Polygon, Polygon> operation, string selectionName = default)
        {
            EnforceSelectionStack(selectionName);
            if (!string.IsNullOrEmpty(selectionName))
            {
                if (!SelectionStacksByName.ContainsKey(selectionName))
                    return;
                for (int p = 0; p < Polygons.Count; p++)
                {
                    if (SelectionStacksByName[selectionName].Contains(Polygons[p]))
                        Polygons[p] = operation(Polygons[p]);
                }
            }
            else
                for (int i = 0; i < Polygons.Count; i++)
                    Polygons[i] = operation(Polygons[i]);
            DetectVertices();
        }
        public void ReplacePolygons(List<Polygon> newPolygons)
        {
            Polygons.Clear();
            foreach(var poly in newPolygons)
                Polygons.Add(poly);
            DetectVertices();
        }
        public List<Vertex> DetectVertices()
        {
            List<Vertex> result = new();
            foreach (var polygon in Polygons)
            {
                result.AddRange(polygon.vertices);
            }
            Vertices.Clear();
            Vertices.AddRange(result);
            return result;
        }
        public List<int> GetSubmeshesID()
        {
            List<int> result = new();
            foreach(Vertex vert in Vertices)
            {
                if (!result.Contains(vert.submesh))
                    result.Add(vert.submesh);
            }
            return result;
        }

        public SelectionStack EnforceSelectionStack(string selectionName)
        {
            if (!string.IsNullOrEmpty(selectionName))
            {
                if (!SelectionStacksByName.ContainsKey(selectionName))
                {
                    SelectionStacksByName[selectionName] = new();
                    SelectionStacksByName[selectionName].AddSelector(new Selector((Polygon p) => p.SelectionGroup == selectionName));
                    SelectionStacksByName[selectionName].AddSelector(new Selector((Vertex v) => v.SelectionGroup == selectionName));
                }
                return SelectionStacksByName[selectionName];
            }
            else
                return null;
            
        }

        public Bounds GetBoundingBox()
        {
            Bounds result = new Bounds();
            foreach (var v in Vertices)
            {
                if (v.x > result.max.x) result.max = new Vector3(v.x, result.max.y, result.max.z);
                if (v.x < result.min.x) result.min = new Vector3(v.x, result.min.y, result.min.z);
                if (v.y > result.max.y) result.max = new Vector3(result.max.x, v.y, result.max.z);
                if (v.y < result.min.y) result.min = new Vector3(result.max.x, v.y, result.min.z);
                if (v.z > result.max.z) result.max = new Vector3(result.max.x, result.max.y, v.z);
                if (v.z < result.min.z) result.min = new Vector3(result.max.x, result.min.y, v.z);
            }
            return result;
        }
        #endregion
    }

}
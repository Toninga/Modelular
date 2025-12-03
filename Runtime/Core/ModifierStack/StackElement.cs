using Codice.Client.Common.WebApi.Responses;
using Modelular.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{

    public class StackElement
    {
        public MeshData MeshData { get; set; }
        public List<Material> Materials { get; set; } = new();
        public List<Polygon> Polygons { get;} = new();
        public List<Vertex> Vertices { get; } = new();
        public Mesh Mesh { get; set; }
        public ModifierStack Stack { get; set; }
        public bool HasStackOwner => Stack != null;
        public Dictionary<string, SelectionStack> SelectionStacksByName { get; set; } = new();

        #region Methods
        public StackElement() { }
        public StackElement(ModifierStack owner)
        {
            Stack = owner;
        }
        public void RemovePolygons(string selectionName = "")
        {
            List<int> result = new();
            if (!string.IsNullOrEmpty(selectionName))
            {
                if (!SelectionStacksByName.ContainsKey(selectionName))
                {
                    Polygons.Clear();
                    return;
                }
                for (int p = 0; p < Polygons.Count; p++)
                {
                    if (SelectionStacksByName[selectionName].Contains(Polygons[p]))
                        result.Add(p);
                }
                result.Reverse();
                foreach (int i in result)
                    Polygons.RemoveAt(i);
            }
            else
            {
                Polygons.Clear();
                return;
            }
        }
        /// <summary>
        /// Returns a copy of the polygons that belong to this selection. Modifications made to this list will not be reflected in the StackElement
        /// </summary>
        /// <param name="selectionName"></param>
        /// <returns></returns>
        public List<Polygon> GetPolygons(string selectionName="")
        {
            List<Polygon> result = new();
            if (!string.IsNullOrEmpty(selectionName))
            {
                if (!SelectionStacksByName.ContainsKey(selectionName))
                    return Polygons;
                for (int p = 0; p < Polygons.Count; p++)
                {
                    if (SelectionStacksByName[selectionName].Contains(Polygons[p]))
                        result.Add(Polygons[p]);
                    
                }
                return result;
            }
            else
                return Polygons;
        }
        public List<Vertex> GetVertices(string targetSelectionGroup)
        {
            List<Vertex> result = new();
            if (!string.IsNullOrEmpty(targetSelectionGroup))
            {
                if (!SelectionStacksByName.ContainsKey(targetSelectionGroup))
                    return Vertices;
                for (int p = 0; p < Polygons.Count; p++)
                {
                    for (int v = 0; v < Polygons[p].vertices.Count; v++)
                    {
                        if (SelectionStacksByName[targetSelectionGroup].Contains(Polygons[p].vertices[v]))
                            result.Add(Vertices[v]);
                    }
                }
                return result;
            }
            else
                return Vertices;
        }
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
        public void ReplaceVertices(Func<Vertex, Vertex> operation, string targetSelectionGroup = default)
        {
            EnforceSelectionStack(targetSelectionGroup);
            if (!string.IsNullOrEmpty(targetSelectionGroup))
            {
                if (!SelectionStacksByName.ContainsKey(targetSelectionGroup))
                    return;
                for (int p = 0; p < Polygons.Count; p++)
                {
                    for (int v = 0; v < Polygons[p].vertices.Count; v++)
                    {
                        if (SelectionStacksByName[targetSelectionGroup].Contains(Polygons[p].vertices[v]))
                            Polygons[p].vertices[v] = operation(Polygons[p].vertices[v]);
                    }
                }
            }
            else
                foreach (var polygon in Polygons)
                    polygon.ReplaceVertices(operation);

            DetectVertices();
        }
        public void ReplacePolygons(Func<Polygon, Polygon> operation, string targetSelectionGroup = default)
        {
            EnforceSelectionStack(targetSelectionGroup);
            if (!string.IsNullOrEmpty(targetSelectionGroup))
            {
                if (!SelectionStacksByName.ContainsKey(targetSelectionGroup))
                    return;
                for (int p = 0; p < Polygons.Count; p++)
                {
                    if (SelectionStacksByName[targetSelectionGroup].Contains(Polygons[p]))
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
        public void Merge(StackElement other)
        {
            if (other == null) return;

            MeshData += other.MeshData;

            if (other.Materials != null)
                Materials.AddRange(other.Materials);

            if (other.Polygons != null)
                Polygons.AddRange(other.Polygons);

            if (other.Vertices != null)
                Vertices.AddRange(other.Vertices);

            // The mesh property isn't merged, it will be regenerated on the next rebake instead

            if (other.SelectionStacksByName != null)
                foreach (var kvp in other.SelectionStacksByName)
                {
                    if (!SelectionStacksByName.TryAdd(kvp.Key, kvp.Value) && SelectionStacksByName[kvp.Key] != null)
                        SelectionStacksByName[kvp.Key].Merge(other.SelectionStacksByName[kvp.Key]);
                }
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
        public List<Material> FetchMaterials()
        {
            List<Material> result = new List<Material>();
            if (HasStackOwner)
            {
                if (Stack.HasOwner)
                {
                    if (Stack.Owner.MeshRenderer != null)
                    {
                        foreach (Material m in Stack.Owner.MeshRenderer.sharedMaterials)
                            result.Add(m);
                    }
                }
            }
            return result;
        }
        public void UpdateMaterials() => Materials = FetchMaterials();
        #endregion
    }

}
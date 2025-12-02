using System.Collections.Generic;
using UnityEngine;
using Modelular.Runtime;
using System;

namespace Modelular.Runtime
{

    public class ModifierStack
    {
        #region Properties
        public ModularMesh Owner { get; set; }
        public bool HasOwner => Owner != null;
        public MeshData MeshData {  get; private set; }
        public List<Modifier> Modifiers => modifiers;
        protected List<Modifier> modifiers = new();
        public StackElement Output { get; private set; }
        public List<Polygon> Polygons => GetBakedPolygons();
        public List<Vertex> Vertices => GetBakedVertices();

        #endregion

        #region Events
        public Action OnCompilation { get; set; }
        #endregion

        #region Methods
        public ModifierStack() { }
        public ModifierStack(ModularMesh owner)
        {
            Owner = owner;
        }
        public StackElement CompileStack()
        {
            StackElement current = new StackElement(this);
            foreach (var modifier in modifiers)
            {
                modifier.Bake(current);
                if (current.Vertices.Count >= GlobalSettings.ErrorVertexCount && !(HasOwner && Owner.IgnoreMaximumAllowedVertexCount))
                    GlobalSettings.MaximumVertexCountExceededException(current.Vertices.Count);
            }

            if (current.Vertices.Count >= GlobalSettings.WarningVertexCount && !(HasOwner && Owner.IgnoreMaximumAllowedVertexCount))
            {
                GlobalSettings.MaximumVertexCountExceededWarning(current.Vertices.Count);
            }

            Output = current;

            OnCompilation?.Invoke();

            return current;
        }

        public void AddModifier(Modifier modifier) => modifiers.Add(modifier);
        public void AddModifiers(IEnumerable<Modifier> modifiers) => this.modifiers.AddRange(modifiers);
        public void ClearModifiers() => this.modifiers.Clear();
        public bool HasModifier(Modifier modifier) => modifiers.Contains(modifier);
        public List<Polygon> GetBakedPolygons()
        {
            if (Output == null)
                return new();
            return Output.Polygons;
        }
        public List<Vertex> GetBakedVertices()
        {
            if (Output == null)
                return new();
            return Output.Vertices;
        }
        #endregion
    }

}
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

        public int WarningVertexCount { get; set; } = 50000;
        public int ErrorVertexCount { get; set; } = 300000;
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
                if (current.Vertices.Count >= ErrorVertexCount)
                    throw new Exception("[Modellular] Compiling the modular mesh stack was aborted : Too much vertices were generated : " + current.Vertices.Count +
                        ". You can override this error by modifying ModifierStack.ErrorVertexCount if you want to push the limits higher.");
            }

            if (current.Vertices.Count >= WarningVertexCount)
            {
                Debug.LogWarning("[Modellular] Warning when compiling the modular mesh stack : The vertex count exceeds the recommended threshold (" + WarningVertexCount + ") : " +
                    current.Vertices.Count + " vertices generated.");
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
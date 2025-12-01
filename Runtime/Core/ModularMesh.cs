using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Modelular.Runtime
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [AddComponentMenu("Modellular/Modular Mesh")]
    public class ModularMesh : MonoBehaviour
    {
        #region Properties

        public ModifierStack Stack => stack;
        public List<ModifierModel> Modifiers = new();

        // Visualization
        public bool ShowVertices { get; set; }
            public float VertexDisplaySize { get; set; } = 0.05f;
            public EColorCoding VertexDisplayMode { get; set; }
            public Color VertexColor { get; set; } = Color.white;
            public bool ShowVertexNumber { get; set; }
            public float VertexNumberDistance { get; set; } = 0.02f;
            
        public bool ShowNormals { get; set; }
            public float NormalDisplaySize { get; set; } = 0.1f;
            public EColorCoding NormalDisplayMode { get; set; }
            public Color NormalColor { get; set; } = Color.green;
        public bool ShowFaces { get; set; }
            public EColorCoding FaceDisplayMode { get; set; }
            public Color FaceColor { get; set; } = new Color(1f, 0.65f, 0f, 0.5f);

        
        #endregion
        #region Fields
        [Header("Parameters")]

        [Tooltip("Specifies if the mesh should be updated each time a value is modified")]
        public EUpdateMode updateMode = EUpdateMode.OnChange;

        

        public double LastBakingTime;

        // Private fields
        MeshRenderer mRenderer;
        MeshFilter mFilter;
        private ModifierStack stack = new();
        private List<Modifier> underlyingModifiers = new List<Modifier>();
        private SaveFile _saveFile = new();

        private int _lastKnownModifierCount;
        #endregion

        #region Methods

        private void OnEnable()
        {
            EditorApplication.update += TryRebake;
        }

        private void OnDisable()
        {
            EditorApplication.update -= TryRebake;
        }



        public void ApplyModifierStack()
        {
            mRenderer = GetComponent<MeshRenderer>();
            mFilter = GetComponent<MeshFilter>();

            // Retrieve the actual modifiers from the models
            underlyingModifiers.Clear();
            foreach(ModifierModel model in Modifiers)
            {
                if (model == null || !model.enabled)
                    continue;
                model.ApplyParameters();
                model.SetDirty(false);
                underlyingModifiers.Add(model.underlyingModifier);
            }

            // Compute the result from all the models
            stack.ClearModifiers();
            stack.AddModifiers(underlyingModifiers);

            // Ensure the mesh is baked in the end
            EnforceModifier(new BakeMesh());

            // Ensure there are enough materials on the mesh
            EnforceModifier(new AutoMaterial(mRenderer));

            Mesh mesh = stack.CompileStack().Mesh;
            if (mesh != null)
            {
                mesh.name = "Generated mesh";
                mFilter.sharedMesh = mesh;

                mRenderer.sharedMaterials = stack.Output.Materials.ToArray();
            }
            

            LastBakingTime = EditorApplication.timeSinceStartup;
            _lastKnownModifierCount = Modifiers.Count;

        }

        /// <summary>
        /// Adds this modifier to the stack only if no modifier of this type is contained in the stack
        /// <return>Whether or not the modifier was added - if false, the modifier was already there</return>
        /// </summary>
        private bool EnforceModifier<T>(T newModifier) where T : Modifier
        {
            bool requireModifier = true;
            foreach (Modifier modifier in stack.Modifiers)
                if (modifier is T)
                    requireModifier = false;
            if (requireModifier)
                stack.AddModifier(newModifier);
            return requireModifier;
        }

        private void TryRebake()
        {
            if (updateMode == EUpdateMode.Manual) return;
            if (updateMode == EUpdateMode.Always)
            {
                ApplyModifierStack();
                return;
            }
            if (updateMode == EUpdateMode.OnChange)
            {
                bool rebake = (stack.Output == null);

                if (!rebake)
                {
                    if (Modifiers.Count != _lastKnownModifierCount)
                    {
                        rebake = true;
                    }
                }

                if (!rebake)
                { 
                    foreach(ModifierModel model in Modifiers)
                    {
                        if (model == null)
                            continue;
                        model.DetectChanges();
                        if ((model.enabled && model.hasChanged) || (stack.HasModifier(model.underlyingModifier) && !model.enabled))
                        {
                            rebake = true;
                        }
                    }
                }

                if (rebake)
                {

                    ApplyModifierStack();
                }
                return;
            }
        }

        public bool Save()
        {
            if (stack.Output.Mesh == null)
                return false;
            return _saveFile.SaveMesh(stack.Output.Mesh, "New generated mesh");
        }
        public void AddModifier<T>() where T : ModifierModel
        {
            Modifiers.Add(ScriptableObject.CreateInstance(typeof(T)) as ModifierModel);
        }
        public bool AddModifier(System.Type modifierModel)
        {
            if (modifierModel.IsSubclassOf(typeof(ModifierModel)))
            {
                Modifiers.Add(ScriptableObject.CreateInstance(modifierModel) as ModifierModel);
                return true;
            }
            return false;
        }

        public void ClearModifierStack()
        {
            Modifiers.Clear();
        }

        #region Visualization

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (ShowVertices)
                Visualization.DrawVertices(stack.Output.Vertices, transform.position, VertexDisplaySize, VertexDisplayMode, VertexColor);
            if (ShowVertexNumber)
                Visualization.DrawVertexNumbers(stack.Output.Vertices, transform.position, VertexNumberDistance);
            if (ShowFaces)
                Visualization.DrawFaces(stack.Output.Polygons, transform.position, FaceDisplayMode, FaceColor);
            if (ShowNormals)
                Visualization.DrawNormals(stack.Output.Vertices, transform.position, NormalDisplaySize, NormalDisplayMode, NormalColor);
        }
#endif

#endregion

#endregion

        public enum EUpdateMode
        {
            Manual = 0,
            OnChange,
            Always
        }

    }


}
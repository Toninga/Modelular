

namespace Modelular.Runtime
{
    [ModelularInterface("Utility/Attach modifier stack", 60)]
    public class AttachModifierStack : Modifier, IModifier, ISelector
    {
        #region Properties
        [ModelularDefaultValue("STransform.Default()")]
        public STransform Transform { get; set; } = STransform.Default();
        public ModularMesh LinkedMesh { get; set; }
        public string TargetSelectionGroup { get; set; }
        public SelectorParameters OutputParameters {get; set; }


        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            if (LinkedMesh != null)
            {
                if (previousResult.Stack.HasOwner && !HasCyclicDependency(previousResult.Stack.Owner, LinkedMesh))
                    LinkedMesh.ApplyModifierStack();
                else
                    throw new System.Exception("[Modelular] : Circular dependencies are not supported. Do not attach another mesh if it or it's linked meshes already contain a reference to this modifier stack");
                StackElement current = new();
                current.AddPolygons(LinkedMesh.Stack.Output.GetPolygons(TargetSelectionGroup), OutputParameters.OutputSelectionGroup);
                var mod = new Transform();
                mod.Setup(Transform);
                mod.Bake(current);

                previousResult.Merge(current);
            }

            return previousResult;
        }

        private bool HasCyclicDependency(ModularMesh current, ModularMesh other)
        {
            if (other == null) return false;

            foreach(var mod in other.Stack.Modifiers)
            {
                if (mod is AttachModifierStack link)
                {
                    if (link.LinkedMesh == current)
                        return true;
                    if (link.HasCyclicDependency(current, link.LinkedMesh))
                        return true;
                }
            }


            return false;
        }


        #endregion
    }
}
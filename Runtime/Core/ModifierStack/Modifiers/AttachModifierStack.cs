

namespace Modelular.Runtime
{
    [ModelularInterface("Utility/Attach modifier stack", 60)]
    public class AttachModifierStack : Modifier, IModifier
    {
        #region Properties
        [ModelularDefaultValue("STransform.Default()")]
        public STransform Transform { get; set; } = STransform.Default();
        public ModularMesh LinkedMesh { get; set; }
        public string TargetSelectionGroup { get; set; }

        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            if (LinkedMesh != null)
            {
                LinkedMesh.Stack.CompileStack();
                StackElement current = new();
                current.AddPolygons(LinkedMesh.Stack.Output.GetPolygons(TargetSelectionGroup));
                var mod = new Transform();
                mod.Setup(Transform);
                mod.Bake(current);

                previousResult.Merge(current);
            }

            return previousResult;
        }



        #endregion
    }
}
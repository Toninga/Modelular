using Modelular.Editor;

namespace Modelular.Modifiers
{
    [ModelularInterface(60)]
    public class AttachModifierStack : Modifier
    {
        #region Properties


        public ModularMesh LinkedMesh { get; set; }

        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            if (LinkedMesh != null)
                previousResult.AddPolygons(LinkedMesh.Stack.Polygons);

            return previousResult;
        }



        #endregion
    }
}
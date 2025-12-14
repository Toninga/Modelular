using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Utility/AttachMesh")]
    public class AttachMesh : Modifier
    {
        #region Properties
        [ModelularDefaultValue("STransform.Default()")]
        public STransform Transform { get; set; } = STransform.Default();
        public Mesh Mesh { get; set; }
        public SelectorParameters OutputParameters { get; set; }



        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            var l = DataProcessor.MeshToPolygons(Mesh);
            l = Modelular.Runtime.Transform.Apply(l, Transform);

            previousResult.AddPolygons(l, OutputParameters.OutputSelectionGroup);
            return previousResult;
        }

        

        #endregion
    }
}
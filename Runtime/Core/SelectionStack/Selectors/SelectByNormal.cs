using UnityEngine;


namespace Modelular.Runtime
{

    public class SelectByNormal : Modifier, ISelector
    {
        #region Parameters
        public string OutputSelectionGroup { get; set; } = "SelectByNormal";
        public ESelectionOperand SelectionOperand { get; set; } = ESelectionOperand.Union;
        public Vector3 Normal { get; set; } = Vector3.up;
        public float Tolerance01 { get; set; } = 0.5f;
        #endregion
        public override StackElement Bake(StackElement previousResult)
        {
            var selector = new Selector(Select);
            previousResult.AddSelection(OutputSelectionGroup, selector);
            return previousResult;
        }

        private bool Select(Vertex v)
        {
            return (Vector3.Dot(v.normal, Normal) / 2 + 0.5f) >= 1 - Tolerance01;
        }
    }

}

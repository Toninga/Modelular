using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Utility/Set Submesh")]
    public class SetSubmesh : Modifier, IModifier
    {
        #region Properties
        public string TargetSelectionGroup { get; set; }
        [Min(0)]
        public int SubmeshID { get; set; }


        #endregion

        
        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            Debug.Log("Submesh set to " + SubmeshID + " for selection group " + TargetSelectionGroup);
            previousResult.ReplaceVertices((v) => (new Vertex(v, overrideSubmesh:(short)SubmeshID)), TargetSelectionGroup);
            return previousResult;
        }

        

        #endregion
    }
}
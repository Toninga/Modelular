using Modelular.Runtime;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Select/Select by color")]
    public class SelectByColor : Modifier, ISelector
    {
        #region Properties
        public SelectorParameters OutputParameters { get; set; }
        [ModelularDefaultValue("Color.white")]
        public Color Color {get;set;} = Color.white;
        public float Tolerance01 { get;set;}

        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.AddSelection(OutputParameters.OutputSelectionGroup, new Selector(Select));
            return previousResult;
        }

        private bool Select(Vertex v) => (new Vector3(v.color.r, v.color.g, v.color.b) - new Vector3(Color.r, Color.g, Color.b)).magnitude / 1.7321f <= Tolerance01;
        

        #endregion
    }
}
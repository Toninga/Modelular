using Modelular.Data;
using Modelular.Editor;
using Modelular.Selection;
using UnityEngine;

namespace Modelular.Modifiers
{
    [ModelularInterface()]
    public class SelectByColor : Modifier, ISelector
    {
        #region Properties
        public string OutputSelectionGroup {get;set;}
        public ESelectionOperand SelectionOperand {get;set;}
        [ModelularDefaultValue("Color.white")]
        public Color Color {get;set;} = Color.white;
        public float Tolerance01 { get;set;}

        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.AddSelection(OutputSelectionGroup, new Selector(Select));
            return previousResult;
        }

        private bool Select(Vertex v) => (new Vector3(v.color.r, v.color.g, v.color.b) - new Vector3(Color.r, Color.g, Color.b)).magnitude / 1.7321f <= Tolerance01;
        

        #endregion
    }
}
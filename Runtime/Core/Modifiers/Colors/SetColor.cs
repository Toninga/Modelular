using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Color/Set color", 20)]
    public class SetColor : Modifier, IModifier
    {
        #region Properties
        public string TargetSelectionGroup { get; set; }
        [ModelularDefaultValue("Color.white")]
        public Color Color { get; set; } = Color.white;

        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.ReplaceVertices((v) => new Vertex(v, overrideColor:Color), TargetSelectionGroup);
            return previousResult;
        }



        #endregion
    }
}
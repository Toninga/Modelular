using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Quad", 0)]
    public class Quad : Modifier, IPrimitive
    {
        #region Parameters
        [ModelularDefaultValue("DefaultPrimitiveProperties.Default()")]
        public DefaultPrimitiveProperties DefaultParameters { get; set; } = DefaultPrimitiveProperties.Default();
        [ModelularDefaultValue("Vector2.one")]
        public Vector2 Size { get; set; } = Vector2.one;
        #endregion



        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            StackElement obj = new StackElement();
            obj.AddPolygon(Make(Size), DefaultParameters.OutputSelectionGroup);
            (this as IPrimitive).ApplyDefaultParameters(obj);


            previousResult.Merge(obj);
            return previousResult;
        }

        protected Polygon Make(Vector2 size)
        {
            return Polygon.Quad(size);
        }
        #endregion
    }

}
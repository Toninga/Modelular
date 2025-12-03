

using UnityEngine;

namespace Modelular.Runtime
{
    public interface IPrimitive
    {
        public DefaultPrimitiveProperties DefaultParameters {get; set;}

        public void ApplyDefaultParameters(StackElement other)
        {
            other.ReplacePolygons((p) => new Polygon(p, DefaultParameters.OutputSelectionGroup));

            var trans = new Transform();
            trans.Setup(DefaultParameters.Transform);
            trans.Bake(other);

            var col = new SetColor();
            col.Color = DefaultParameters.Color;
            col.Bake(other);
        }
    }
}
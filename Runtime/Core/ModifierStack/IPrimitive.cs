
using System.Collections.Generic;

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

        public List<Polygon> ApplyDefaultParameters(List<Polygon> other)
        {
            StackElement elm = new StackElement();
            elm.AddPolygons(other);
            ApplyDefaultParameters(elm);
            return elm.Polygons;
        }
    }
}
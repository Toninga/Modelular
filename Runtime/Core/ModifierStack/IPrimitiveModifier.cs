

using UnityEngine;

namespace Modelular.Runtime
{
    public interface IPrimitiveModifier
    {
        public DefaultPrimitiveProperties DefaultParameters {get; set;}

        public void ApplyDefaultParameters(StackElement other)
        {
            other.ReplacePolygons((p) => new Polygon(p, DefaultParameters.OutputSelectionGroup));

            var trans = new Transform();
            trans.Position = DefaultParameters.Transform.Position;
            trans.Rotation = DefaultParameters.Transform.Rotation;
            trans.Scale = DefaultParameters.Transform.Scale;
            trans.Bake(other);

            var col = new SetColor();
            col.Color = DefaultParameters.Color;
            col.Bake(other);
        }
    }
}
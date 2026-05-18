
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    public interface IPrimitive
    {
        public DefaultPrimitiveProperties DefaultParameters {get; set;}

        public Vector3 Position
        {
            get => DefaultParameters.Transform.Position;
            set
            {
                DefaultParameters = new(
                    new STransform(value, DefaultParameters.Transform.Rotation, DefaultParameters.Transform.Scale),
                    DefaultParameters.Color,
                    DefaultParameters.OutputSelectionGroup);
            }
        }
        public Vector3 Rotation
        {
            get => DefaultParameters.Transform.Rotation;
            set
            {
                DefaultParameters = new(
                    new STransform(DefaultParameters.Transform.Position, value, DefaultParameters.Transform.Scale),
                    DefaultParameters.Color,
                    DefaultParameters.OutputSelectionGroup);
            }
        }
        public Vector3 Scale
        {
            get => DefaultParameters.Transform.Scale;
            set
            {
                DefaultParameters = new(
                    new STransform(DefaultParameters.Transform.Position, DefaultParameters.Transform.Rotation, value),
                    DefaultParameters.Color,
                    DefaultParameters.OutputSelectionGroup);
            }
        }
        public Color Color
        {
            get => DefaultParameters.Color;
            set
            {
                DefaultParameters = new(
                    DefaultParameters.Transform,
                    value,
                    DefaultParameters.OutputSelectionGroup);
            }
        }
        public string OutputSelectionGroup
        {
            get => DefaultParameters.OutputSelectionGroup;
            set
            {
                DefaultParameters = new(
                    DefaultParameters.Transform,
                    DefaultParameters.Color,
                    value);
            }
        }

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
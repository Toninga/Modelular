using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Modelular.Runtime
{
    [System.Serializable]
    public struct DefaultPrimitiveProperties
    {
        public STransform Transform;
        public UnityEngine.Color Color;
        public string OutputSelectionGroup;

        public static DefaultPrimitiveProperties Default() => new DefaultPrimitiveProperties(STransform.Default(), UnityEngine.Color.white, "");


        public DefaultPrimitiveProperties(STransform transform,  UnityEngine.Color color, string outputSelectionGroup)
        {
            Transform = transform;
            Color = color;
            OutputSelectionGroup = outputSelectionGroup;
        }

        public static bool operator ==(DefaultPrimitiveProperties left, DefaultPrimitiveProperties right)
        {
            return left.Transform == right.Transform && left.Color == right.Color && left.OutputSelectionGroup == right.OutputSelectionGroup;
        }
        public static bool operator != (DefaultPrimitiveProperties left, DefaultPrimitiveProperties right) { return !left.Equals(right); }
        public override bool Equals(object obj)
        {
            return obj is DefaultPrimitiveProperties properties &&
                   EqualityComparer<STransform>.Default.Equals(Transform, properties.Transform) &&
                   Color.Equals(properties.Color) &&
                   OutputSelectionGroup == properties.OutputSelectionGroup;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Transform, Color, OutputSelectionGroup);
        }
        public static implicit operator string(DefaultPrimitiveProperties o)
        {
            return "DefaultPrimitiveProperties :\n\tTransform :\n\t\tPosition : " + o.Transform.Position + "\n\t\tRotation : " + o.Transform.Rotation +
                "\n\t\tScale : " + o.Transform.Scale + 
                "\n\tColor : " + o.Color + "\n\tOutputSelectionGroup : " + o.OutputSelectionGroup;
        }
        public override string ToString()
        {
            return (string)this;
        }
    }
}


using System;
using UnityEngine;

namespace Modelular.Runtime
{
    [System.Serializable]
    public struct STransform
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;

        public static STransform Default() => new STransform(Vector3.zero, Vector3.zero, Vector3.one);


        public STransform(Vector3 position)
        {
            Position = position;
            Rotation = Vector3.zero;
            Scale = Vector3.one;
        }
        public STransform(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
            Scale = Vector3.one;
        }
        public STransform(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation.eulerAngles ;
            Scale = Vector3.one;
        }
        public STransform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
        public STransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation.eulerAngles;
            Scale = scale;
        }

        public static bool operator ==(STransform left, STransform right) => (left.Position == right.Position) && (left.Rotation == right.Rotation) && (left.Scale == right.Scale);
        public static bool operator !=(STransform left, STransform right) { return !(left == right); }
        public override bool Equals(object obj)
        {
            return obj is STransform transform &&
                   Position.Equals(transform.Position) &&
                   Rotation.Equals(transform.Rotation) &&
                   Scale.Equals(transform.Scale);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Rotation, Scale);
        }
        public static implicit operator string(STransform transform)
        {
            return "Transform :\n\tPosition : " + transform.Position + "\n\tRotation : " + transform.Rotation + "\n\tScale : " + transform.Scale;
        }
        public override string ToString()
        {
            return (string)this;
        }
    }
}

using UnityEngine;

namespace Modelular.Runtime
{
    public enum Axis
    {
        X,
        XMinus,
        Y,
        YMinus,
        Z,
        ZMinus
    }

    public static class AxisUtility
    {
        public static T SwitchOnAxis<T>(Axis axis, T xPlus, T xMinus, T yPlus, T yMinus, T zPlus, T zMinus)
        {
            switch (axis)
            {
                case Axis.X:
                    return xPlus;
                case Axis.Y:
                    return yPlus;
                case Axis.Z:
                    return zPlus;
                case Axis.XMinus:
                    return xMinus;
                case Axis.YMinus:
                    return yMinus;
                case Axis.ZMinus:
                    return zMinus;
                default : 
                    return default;
            }
        }

        public static Vector3 ForwardFromAxis(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return Vector3.right;
                case Axis.Y:
                    return Vector3.up;
                case Axis.Z:
                    return Vector3.forward;
                case Axis.XMinus:
                    return Vector3.left;
                case Axis.YMinus:
                    return Vector3.down;
                case Axis.ZMinus:
                    return Vector3.back;
                default :
                    return default;
            }
        }
    }
}
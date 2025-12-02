
using UnityEngine;

namespace Modelular.Runtime
{
    public enum EAxis
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
        public static T SwitchOnAxis<T>(EAxis axis, T xPlus, T xMinus, T yPlus, T yMinus, T zPlus, T zMinus)
        {
            switch (axis)
            {
                case EAxis.X:
                    return xPlus;
                case EAxis.Y:
                    return yPlus;
                case EAxis.Z:
                    return zPlus;
                case EAxis.XMinus:
                    return xMinus;
                case EAxis.YMinus:
                    return yMinus;
                case EAxis.ZMinus:
                    return zMinus;
                default : 
                    return default;
            }
        }

        public static Vector3 GetAxisDirection(EAxis axis)
        {
            switch (axis)
            {
                case EAxis.X:
                    return Vector3.right;
                case EAxis.Y:
                    return Vector3.up;
                case EAxis.Z:
                    return Vector3.forward;
                case EAxis.XMinus:
                    return Vector3.left;
                case EAxis.YMinus:
                    return Vector3.down;
                case EAxis.ZMinus:
                    return Vector3.back;
                default :
                    return default;
            }
        }
    }
}
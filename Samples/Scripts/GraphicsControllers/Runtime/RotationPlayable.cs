using UnityEngine;

public class RotationPlayable : PlayableGraphicsController
{
    [SerializeField] RotatorDatatype _rotationInputMode;
    [SerializeField] Vector3 _startRotation;
    [SerializeField] Vector3 _endRotation = new(0,360,0);

    protected override void Apply(float t)
    {        
        transform.rotation = GetInterpolatedQuaternion(T);
    }
    
    Quaternion GetInterpolatedQuaternion(float t)
    {
        switch (_rotationInputMode)
        {
            case RotatorDatatype.Quaternion:
                return Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_endRotation), t);

            default:
            case RotatorDatatype.EulerAngles:
                return Quaternion.Euler(Vector3.Lerp(_startRotation, _endRotation, t));
        }
    }
    
    public enum RotatorDatatype
    {
        EulerAngles,
        Quaternion
    }
}

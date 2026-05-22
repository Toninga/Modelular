using UnityEngine;

public class HoverPlayable : PlayableGraphicsController
{
    [Header("Hover options")]
    [SerializeField] private float range = 0.5f;

    private Vector3 _defaultPosition;
    private Vector3 _lastPosition;

    private Vector3 OffsetVector => Vector3.up * _currentOffset * EffectMultiplier;
    private float _currentOffset;

    protected override void Apply(float t)
    {
        if (transform.position != _lastPosition)
            _defaultPosition = transform.position - OffsetVector;
        ApplyCurrentOffset();
    }

    private void ApplyCurrentOffset()
    {
        ComputeCurrentOffset();
        transform.position = _defaultPosition + OffsetVector;
        _lastPosition = transform.position;
    }

    private void ComputeCurrentOffset()
    {       
        _currentOffset = T * range - range/2;
    }
}

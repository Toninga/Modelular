using Unity.VisualScripting;
using UnityEngine;

public class ScalePlayable : PlayableGraphicsController
{
    [SerializeField] Vector3 _startScale;
    [SerializeField] Vector3 _endScale;
    Vector3 _defaultScale;

    protected override void Start()
    {
        base.Start();
        _defaultScale = transform.localScale;
    }

    protected override void Apply(float t)
    {
        transform.localScale = Vector3.Lerp(_startScale, _endScale, T);
    }
}

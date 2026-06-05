using System;
using UnityEngine;

public class MatchTransformPlayable : PlayableGraphicsController
{
    public Transform target;

    Vector3 startPos;
    Quaternion startRot;
    Vector3 startScale;

    private void Awake()
    {
        OnStart += CacheStartTransform;
    }

    private void CacheStartTransform()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        startScale = transform.localScale;
    }

    protected override void Apply(float t)
    {
        transform.position = Vector3.Lerp(startPos, target.position, t);
        transform.rotation = Quaternion.Lerp(startRot, target.rotation, t);
        transform.localScale = Vector3.Lerp(startScale, target.localScale, t);
    }
}

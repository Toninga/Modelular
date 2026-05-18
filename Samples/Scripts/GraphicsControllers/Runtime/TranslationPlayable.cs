using UnityEngine;

public class TranslationPlayable : PlayableGraphicsController
{
    [Header("Translation options")]
    [SerializeField] Transform _startPosition;
    [SerializeField] Transform _endPosition;

    protected override void Update()
    {
        base.Update();
        if (!IsPlaying || DelayTimeElapsed < Delay)
            return;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(_startPosition.position, _endPosition.position, T);
    }
}



using UnityEngine;

public class TranslationPlayable : PlayableGraphicsController
{
    [Header("Translation options")]
    [SerializeField] Transform _startPosition;
    [SerializeField] Transform _endPosition;

    protected override void Apply(float t)
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(_startPosition.position, _endPosition.position, T);
    }
}



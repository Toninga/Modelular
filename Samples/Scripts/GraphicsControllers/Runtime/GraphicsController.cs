using System;
using System.Collections;
using UnityEngine;

public abstract class GraphicsController : MonoBehaviour
{
    public Action OnEnable;
    public Action OnDisable;

    protected float transitionDurationWhenDisabledOrEnabled = 0.1f;
    protected Coroutine CurrentCoroutine;
    protected float EffectMultiplier = 1;

    public void Enable()
    {
        OnEnable?.Invoke();
        if (CurrentCoroutine != null)
            StopCoroutine(CurrentCoroutine);
        CurrentCoroutine = StartCoroutine(Lerp(EffectMultiplier, 1, transitionDurationWhenDisabledOrEnabled));
    }

    public void Disable()
    {
        if (CurrentCoroutine != null)
            StopCoroutine(CurrentCoroutine);
        CurrentCoroutine = StartCoroutine(Lerp(EffectMultiplier, 0, transitionDurationWhenDisabledOrEnabled, OnDisable));
    }

    protected IEnumerator Lerp(float start, float end, float duration, Action onEnd = null)
    {
        var curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
        {
            float t = Mathf.Clamp01(curve.Evaluate(elapsed / duration));
            EffectMultiplier = Mathf.Lerp(start, end, t);
            yield return null;
        }
        onEnd?.Invoke();
    }
}

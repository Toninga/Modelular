using System.Collections.Generic;
using UnityEngine;

public class MultiTranslationPlayable : PlayableGraphicsController
{
    [System.Serializable]
    public struct TargetObject
    {
        public Transform TargetTransform;
        public float WaitDuration;
    }

    [Header("Translation options")]
    [SerializeField] List<TargetObject> targets;
    int _currentTarget;
    float _secondaryDelay;

    protected override void Apply(float t)
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (targets[_currentTarget].TargetTransform == null)
        {
            Debug.LogWarning("[MultiTranslationPlayable] : TargetTransform at index " + _currentTarget + " was null");
            IsPlaying = false;
            return;
        }
        if (_currentTarget >= targets.Count - 1)
        {
            transform.position = targets[^1].TargetTransform.position;
            return;
        }

        if (targets[_currentTarget + 1].TargetTransform == null)
        {
            Debug.LogWarning("[MultiTranslationPlayable] : TargetTransform at index " + (_currentTarget + 1) + " was null");
            IsPlaying = false;
            return;
        }
        transform.position = Vector3.Lerp(targets[_currentTarget].TargetTransform.position, targets[_currentTarget + 1].TargetTransform.position, T);
    }

    protected override void ProcessElapsedTime()
    {
        if (!IsPlaying)
            return;

        if (DelayTimeElapsed < Delay)
        {
            DelayTimeElapsed += Time.deltaTime;
            return;
        }
        if (_secondaryDelay < targets[_currentTarget].WaitDuration)
        {
            _secondaryDelay += Time.deltaTime;
            return;
        }

        switch (AnimationMode)
        {
            case EAnimationMode.PlayOnce:
                TimeElapsed += Dt;
                if (TimeElapsed >= Duration)
                {
                    _currentTarget++;
                    _secondaryDelay = 0;
                    TimeElapsed = 0;
                }
                if (_currentTarget >= targets.Count)
                {
                    IsPlaying = false;
                }
                break;

            case EAnimationMode.Loop:
                TimeElapsed += Dt;
                if (TimeElapsed >= Duration)
                {
                    _currentTarget++;
                    _secondaryDelay = 0;
                    TimeElapsed = 0;
                }
                if (_currentTarget >= targets.Count)
                {
                    _currentTarget = 0;
                }
                break;

            case EAnimationMode.PingPongOnce:
                TimeElapsed += Dt;
                if (TimeElapsed >= Duration)
                {
                    _currentTarget++;
                    _secondaryDelay = 0;
                    TimeElapsed = 0;
                }
                if (_currentTarget >= targets.Count)
                {
                    IsReversed = !IsReversed;
                    _currentTarget = Mathf.Clamp(_currentTarget, 0, targets.Count - 1);
                }
                if (_currentTarget < 0)
                    IsPlaying = false;
                break;

            case EAnimationMode.PingPongLoop:
                TimeElapsed += Dt;
                if (TimeElapsed >= Duration)
                {
                    _currentTarget++;
                    _secondaryDelay = 0;
                    TimeElapsed = 0;
                }
                if (_currentTarget >= targets.Count || _currentTarget < 0)
                {
                    IsReversed = !IsReversed;
                    _currentTarget = Mathf.Clamp(_currentTarget, 0, targets.Count - 1);
                }
                break;
        }
    }
}



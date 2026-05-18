using System;
using UnityEngine;

public abstract class PlayableGraphicsController : GraphicsController
{
    [Header("Playable options")]
    public EAnimationMode AnimationMode;
    public float Duration = 1f;
    public float Delay = 0f;
    [SerializeField] protected bool PlayOnAwake = true;
    public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);

    [HideInInspector] public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            if (_isPlaying == value) return;
            _isPlaying = value;
            if (value) OnStart?.Invoke();
            else OnStop?.Invoke();
        }
    }
    private bool _isPlaying;
    public Action OnStart;
    public Action OnStop;
    public Action OnDelayOver;

    protected float TimeElapsed;
    protected float DelayTimeElapsed;
    protected bool IsReversed;

    protected float Dt => Time.deltaTime * (IsReversed ? -1 : 1);
    protected float T => Curve.Evaluate(TimeElapsed / Duration);

    protected virtual void Start()
    {
        if (PlayOnAwake)
            Play();
    }

    protected virtual void Update()
    {
        ProcessElapsedTime();
    }

    public virtual void Play(bool skipDelay=false)
    {
        IsReversed = false;
        IsPlaying = true;
        DelayTimeElapsed = 0;
        if (skipDelay)
            DelayTimeElapsed = Delay;
    }
    public virtual void PlayOnce(bool skipDelay = false)
    {
        AnimationMode = EAnimationMode.PlayOnce;
        Play(skipDelay);
    }
    public virtual void PlayOnLoop(bool skipDelay = false)
    {
        AnimationMode = EAnimationMode.Loop;
        Play(skipDelay);
    }
    public virtual void PlayPingPong(bool skipDelay = false)
    {
        AnimationMode = EAnimationMode.PingPongLoop;
        Play(skipDelay);
    }

    protected virtual void ProcessElapsedTime()
    {
        if (!IsPlaying)
            return;

        if (DelayTimeElapsed < Delay)
        {
            DelayTimeElapsed += Time.deltaTime;
            if (DelayTimeElapsed >= Delay)
            {
                OnDelayOver?.Invoke();
            }
            return;
        }

        switch (AnimationMode)
        {
            case EAnimationMode.PlayOnce:
                TimeElapsed += Dt;
                if (TimeElapsed >= Duration)
                {
                    IsPlaying = false;
                    TimeElapsed = Duration;
                }
                break;

            case EAnimationMode.Loop:
                TimeElapsed += Dt;
                TimeElapsed %= Duration;
                break;

            case EAnimationMode.PingPongOnce:
                TimeElapsed += Dt;
                if (TimeElapsed >= Duration)
                    IsReversed = !IsReversed;
                if (TimeElapsed <= 0)
                {
                    IsPlaying = false;
                    TimeElapsed = 0;
                }
                break;

            case EAnimationMode.PingPongLoop:
                TimeElapsed += Dt;
                if (TimeElapsed >= Duration || TimeElapsed <= 0)
                {
                    IsReversed = !IsReversed;
                    TimeElapsed = Mathf.Clamp(TimeElapsed, 0, Duration);
                }
                break;
        }
    }

    public enum EAnimationMode
    {
        PlayOnce,
        Loop,
        PingPongOnce,
        PingPongLoop
    }
}

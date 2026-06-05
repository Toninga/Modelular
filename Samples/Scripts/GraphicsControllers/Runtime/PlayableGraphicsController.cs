using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayableGraphicsController : GraphicsController
{
    [Header("Playable options")]
    public EAnimationMode AnimationMode;
    [Tooltip("If the duration is inferior to a frame's time budget, the animation won't play. For example, if the duration is 0.01 (10ms), and a frame takes 16ms, the animation will be ignored.")]
    public float Duration = 1f;
    public float Delay = 0f;
    [SerializeField] protected bool PlayOnAwake = true;
    public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);
    public PlayableCallbacks Callbacks = new PlayableCallbacks();

    public bool IsPlaying
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

    public bool IsWaiting
    {
        get => _isWaiting;
        set
        {
            if (_isWaiting == value) return;
            _isWaiting = value;
            if (value) OnDelayStart?.Invoke();
            else OnDelayStop?.Invoke();
        }
    }
    private bool _isWaiting;
    public Action OnDelayStart;
    public Action OnDelayStop;

    protected float TimeElapsed;
    protected float DelayTimeElapsed;
    protected bool IsReversed;

    protected float Dt => Time.deltaTime * (IsReversed ? -1 : 1);
    /// <summary>
    /// Animation progression, between 0 and 1
    /// </summary>
    protected float T => Curve.Evaluate(TimeElapsed / Duration);

    protected virtual void Start()
    {
        if (PlayOnAwake)
            Play();

        OnDelayStart += Callbacks.OnDelayStart.Invoke;
        OnDelayStop += Callbacks.OnDelayStop.Invoke;
        OnStart += Callbacks.OnAnimStart.Invoke;
        OnStop += Callbacks.OnAnimStop.Invoke;
    }

    protected void Update()
    {
        ProcessElapsedTime();
        if (IsPlaying)
            Apply(T);
    }

    /// <summary>
    /// Put the animation logic here.
    /// </summary>
    /// <param name="t">Progression of the animation between 0 and 1.</param>
    protected virtual void Apply(float t)
    {

    }
    public void StartDelay()
    {
        DelayTimeElapsed = 0;
        IsWaiting = true;
    }
    public void Play() => Play(false);
    public virtual void Play(bool skipDelay=false)
    {
        IsReversed = false;
        if (skipDelay)
            IsPlaying = true;
        else
            StartDelay();
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
        if (!IsPlaying && !IsWaiting)
            return;

        if (IsWaiting && DelayTimeElapsed < Delay)
        {
            DelayTimeElapsed += Time.deltaTime;
            return;
        }
        else
        {
            IsWaiting = false;
            IsPlaying = true;
        }

            switch (AnimationMode)
            {
                case EAnimationMode.PlayOnce:
                    TimeElapsed += Dt;
                    if (TimeElapsed >= Duration)
                    {
                        IsPlaying = false;
                        TimeElapsed = Duration;
                        Apply(1);
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
                        Apply(0);
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

    [System.Serializable]
    public struct PlayableCallbacks
    {
        public UnityEvent OnDelayStart;
        public UnityEvent OnDelayStop;
        public UnityEvent OnAnimStart;
        public UnityEvent OnAnimStop;
    }
}

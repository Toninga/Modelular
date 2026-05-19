using UnityEngine;

public class SetActivePlayable : PlayableGraphicsController
{
    public GameObject Target;
    public bool TargetState;

    private bool _defaultState;

    protected override void Start()
    {
        base.Start();
        OnStart += () => _defaultState = Target.activeSelf;
    }
    protected override void Update()
    {
        base.Update();
        if (!IsPlaying)
            return;

        if (Target == null)
            return;

        Target.SetActive(T >= 0.5 ? TargetState : _defaultState);
    }
}

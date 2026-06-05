using System.Collections.Generic;
using UnityEngine;

public class PlayableSequence : PlayableGraphicsController
{
    [Tooltip("The duration parameter is not taken into account")]
    public List<PlayableGraphicsController> PlayableTargets;

    protected override void Start()
    {
        base.Start();

        if (PlayableTargets.Count > 0)
        {
            if (Delay > 0)
            {
                OnDelayStop += () => PlayableTargets[0].Play();
            }
            else
            {
                Debug.Log("Playing first anim (" + PlayableTargets[0].name + ")");
                PlayableTargets[0].Play();
            }
        }

        if (PlayableTargets.Count < 2)
            return;
        for (int i = 0; i < PlayableTargets.Count - 1; i++)
        {
            int index = i; // Used to prevent the lambda expression to capture a reference to i, and pass it a value instead;
            if (PlayableTargets[i] == null || PlayableTargets[i + 1] == null || !PlayableTargets[i].enabled)
                continue;
            //Debug.Log("Bind playable anim '" + i + "' (" + PlayableTargets[i].name + ") with '" + (i + 1) + "' (" + PlayableTargets[i+1].name + ")");
            PlayableTargets[i].OnStop += () => PlayableTargets[index + 1].Play();
            //PlayableTargets[i].OnStop += () => Debug.Log("Playable anim '" + i + "' (" + PlayableTargets[index].name + ") stopped");
        }
    }
}

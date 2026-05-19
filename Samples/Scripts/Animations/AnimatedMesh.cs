using Modelular.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AnimatedMesh : MonoBehaviour
{
    public bool PlayOnAwake = true;

    /// <summary>
    /// The main modifier stack, taking <see cref="modifiers"/> as it's modifier list. It will be rebaked every frame if an animation is playing (<see cref="isPlaying"/> == true).
    /// </summary>
    protected ModifierStack stack;
    /// <summary>
    /// The list of modifiers that are to be added to <see cref="stack"/>.
    /// </summary>
    protected Dictionary<string, Modifier> modifiers = new();
    protected List<(Action<Modifier, float> anim, Modifier target, float duration)> animations = new();
    protected bool isPlaying;
    protected MeshFilter meshFilter;

    protected void Awake()
    {
        Init();
    }
    protected virtual void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        SetupStack();
        if (PlayOnAwake)
            Play();
    }

    /// <summary>
    /// Start the animation
    /// </summary>
    public virtual void Play()
    {
        isPlaying = true;
        Action combinedAnimations = OnAnimationEnd;
        for (int i = animations.Count - 1; i >= 0 ; i--)
        {
            // The following assignments are here to make local copies of the variables, to make sure they are passed by value rather than by reference.
            int index = i;
            Action next = combinedAnimations;

            combinedAnimations = () => StartCoroutine(Animate(animations[index].anim, animations[index].target, animations[index].duration, next));
        }
        combinedAnimations();
    }

    protected virtual void OnAnimationEnd()
    {
        isPlaying = false;
    }

    /// <summary>
    /// Retrieves the first modifier from <see cref="modifiers"/> that matches the given type. Can be null.
    /// </summary>
    /// <typeparam name="T">The requested type</typeparam>
    /// <returns></returns>
    public virtual T Fetch<T>() where T : Modifier => modifiers.Values.Where(modifier => modifier is T).FirstOrDefault() as T;
    /// <summary>
    /// Retrieves the modifier from <see cref="modifiers"/> assigned to the given string ID. Will be null if the key doesn't exist.
    /// </summary>
    /// <param name="ID">ID for the requested modifier</param>
    /// <returns></returns>
    public virtual Modifier Fetch(string ID)
    {
        if (!modifiers.ContainsKey(ID))
            return null;
        return modifiers[ID];
    }
    public virtual T Add<T>() where T : Modifier, new()
    {
        T modifier = new T();
        modifiers.Add(modifiers.Count.ToString(), modifier);
        return modifier;
    }
    public virtual T Add<T>(Action<T, float> anim, float duration, AnimationCurve curve) where T : Modifier, new()
    {
        T modifier = new T();
        modifiers.Add(modifiers.Count.ToString(), modifier);
        animations.Add((new Action<Modifier, float>((target, t) => anim?.Invoke((T)target, curve.Evaluate(t))), modifier, duration));
        return modifier;
    }
    public virtual T Add<T>(Action<T, float> anim, float duration=1f) where T : Modifier, new()
    {
        T modifier = new T();
        modifiers.Add(modifiers.Count.ToString(), modifier);
        animations.Add((new Action<Modifier, float>((target, t) => anim?.Invoke((T)target, t)), modifier, duration));
        return modifier;
    }
    public virtual void Add<T>(Action<T, float> anim, T target, float duration=1f) where T : Modifier, new()
    {
        animations.Add((new Action<Modifier, float>((mod, t) => anim?.Invoke((T)mod, t)), target, duration));
    }

    /// <summary>
    /// Init() is called on Awake. All the setup that does not rely on other objects being initialized should be done here.
    /// </summary>
    protected virtual void Init()
    {

    }
    /// <summary>
    /// Initializes the modifier stack and assigns all the modifiers to it. Shouldn't be called before <see cref="modifiers"/> is initialized.
    /// </summary>
    protected virtual void SetupStack()
    {
        stack = new ModifierStack();
        stack.AddModifiers(modifiers.Values);
    }
    /// <summary>
    /// Runs a method, taking a modifier and the animation's normalized elapsed time as parameters, for a given amount of time.
    /// </summary>
    /// <param name="animation">A method that will animate <param name="target"/> accross time, base on the float parameter (0-1 range)</param>
    /// <param name="target">The target modifier to animate</param>
    /// <param name="duration">The total duration of the animation</param>
    /// <param name="OnEnd">Will be invoked as soon as the animation is over</param>
    /// <returns></returns>
    protected IEnumerator Animate(Action<Modifier, float> animation, Modifier target, float duration = 1f, Action OnEnd=null)
    {
        if (duration > 0f)
        {
            for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
            {
                animation?.Invoke(target, elapsed/duration);
                Apply();
                /*
                Debug.Log("Compiled mesh is '" + result.Mesh + "'."
                    + " The stack has " + stack.Modifiers.Count + " modifier(s)."
                    + " Result's MeshData is '" + result.MeshData + "'."
                    + " MeshData.Submeshes is '" + result.MeshData.Submeshes + "'."
                    ); */
                yield return null;
            }
        }
        animation?.Invoke(target, 1);
        Apply();

        OnEnd?.Invoke();
    }
    
    public virtual void Apply()
    {
        meshFilter.sharedMesh = stack.CompileStack().Mesh;
    }

}

using Modelular.Runtime;
using UnityEngine;

public class FractalPattern : AnimatedMesh
{

    protected override void Init()
    {
        base.Init();
        modifiers = new()
        {
            {"Cube01",              new Modelular.Runtime.Cube() },
            {"CopyToRadius01",      new Modelular.Runtime.CopyToRadius()},
            {"ColorGradient01",     new Modelular.Runtime.ColorGradient()},
            {"SelectByColor01",     new Modelular.Runtime.SelectByColor()},
            {"SelectByColor02",     new Modelular.Runtime.SelectByColor()},
            {"SelectByColor03",     new Modelular.Runtime.SelectByColor()},
            {"Transform01",         new Modelular.Runtime.Transform()},
            {"Transform02",         new Modelular.Runtime.Transform()},
            {"Transform03",         new Modelular.Runtime.Transform()},
            {"Cone01",              new Modelular.Runtime.Cone()},
            {"Flip01",              new Modelular.Runtime.Flip()}
            /*
            */
        };

        animations = new()
        {
            (null, null, 1f),
            (Anim_Cone, Fetch<Cone>(), 0f),
            (Anim_Flip, Fetch<Flip>(), 0f),
            (Anim_Cone, Fetch<Cone>(), 2f),
            (Anim_GrowingCube, Fetch<Cube>(), 2f),
            (Anim_CircularLayout, Fetch<CopyToRadius>(), 4f),
            (Anim_ColorGradient, Fetch<ColorGradient>(), 1f),
            (Anim_Selection01, Fetch("SelectByColor01"), 0f),
            (Anim_Selection02, Fetch("SelectByColor02"), 0f),
            (Anim_Selection03, Fetch("SelectByColor03"), 0f),
            (Anim_Transform01, Fetch("Transform01"), 1f),
            (Anim_Transform02, Fetch("Transform02"), 1f),
            (Anim_Transform03, Fetch("Transform03"), 1f),
        };

        SetupDefaultStates();
    }

    void SetupDefaultStates()
    {
        foreach(var a in animations)
        {
            if (a.target == null)
                continue;
            a.target.Enabled = false;
        }
    }
    void Anim_GrowingCube(Modifier targetModifier, float t)
    {
        // Parameters
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float T = curve.Evaluate(t);
        float targetLength = 50f;
        Vector3 axis = Vector3.forward;
        int subdivisions = 20;

        // Type enforcement
        if (targetModifier is not Cube target) return;

        // Animation
        target.Enabled = true;
        target.Size = Vector3.Max( axis * T * targetLength, Vector3.one);
        (target as IPrimitive).Position = axis * T * targetLength * 0.5f;
        target.Subdivisions = new Vector3Int((int)axis.x, (int)axis.y, (int)axis.z) * (int)(T * subdivisions);
    }
    void Anim_CircularLayout(Modifier targetModifier, float t)
    {
        // Parameters
        int count = 24;
        float radius = 7f;
        Axis axis = Axis.Z;
        float translationPortion = 0.25f;

        // Type enforcement
        if (targetModifier is not CopyToRadius target) return;

        // Animation
        target.Enabled = true;
        target.Axis = axis;
        if (t <= translationPortion)
        {
            target.Count = 1;
            target.Radius = radius * t / translationPortion;
        }
        else
        {
            target.Arc01 = (t - translationPortion) / (1 - translationPortion);
            target.Count = count;
            target.Radius = radius;
        }
            
    }
    void Anim_ColorGradient(Modifier targetModifier, float t)
    {
        // Parameters
        Vector3 direction = Vector3.forward;
        Color colorA = Color.black;
        Color colorB = Color.white;

        // Type enforcement
        if (targetModifier is not ColorGradient target) return;

        // Animation
        target.Enabled = true;
        target.Direction = direction;
        target.ColorA = Color.Lerp(Color.white, colorA, t);
        target.ColorB = Color.Lerp(Color.white, colorB, t);
            
    }
    void Anim_Selection01(Modifier targetModifier, float t)
    {
        // Parameters
        string targetGroup = "01";
        Color color = new Color(0.25f, 0.25f, 0.25f);
        float tolerance = 0.15f;

        // Type enforcement
        if (targetModifier is not SelectByColor target) return;

        // Animation
        target.Enabled = true;
        target.OutputParameters = new(targetGroup, ESelectionOperand.Union);
        target.Color = color;
        target.Tolerance01 = tolerance;
            
    }
    void Anim_Selection02(Modifier targetModifier, float t)
    {
        // Parameters
        string targetGroup = "02";
        Color color = new Color(0.75f, 0.75f, 0.75f);
        float tolerance = 0.15f;

        // Type enforcement
        if (targetModifier is not SelectByColor target) return;

        // Animation
        target.Enabled = true;
        target.OutputParameters = new(targetGroup, ESelectionOperand.Union);
        target.Color = color;
        target.Tolerance01 = tolerance;
            
    }
    void Anim_Selection03(Modifier targetModifier, float t)
    {
        // Parameters
        string targetGroup = "03";
        Color color = Color.white;
        float tolerance = 0.05f;

        // Type enforcement
        if (targetModifier is not SelectByColor target) return;

        // Animation
        target.Enabled = true;
        target.OutputParameters = new(targetGroup, ESelectionOperand.Union);
        target.Color = color;
        target.Tolerance01 = tolerance;
            
    }
    void Anim_Transform01(Modifier targetModifier, float t)
    {
        // Parameters
        string targetGroup = "01";
        Vector3 newPos = Vector3.right;
        Vector3 newRot = Vector3.zero;
        Vector3 newScale = Vector3.one;

        // Type enforcement
        if (targetModifier is not Modelular.Runtime.Transform target) return;

        // Animation
        target.Enabled = true;
        target.TargetSelectionGroup = targetGroup;
        target.Position = newPos * t;
        target.Rotation = newRot * t;
        target.Scale = Vector3.Lerp(Vector3.one, newScale, t);
            
    }
    void Anim_Transform02(Modifier targetModifier, float t)
    {
        // Parameters
        string targetGroup = "02";
        Vector3 newPos = Vector3.left;
        Vector3 newRot = Vector3.zero;
        Vector3 newScale = Vector3.one;

        // Type enforcement
        if (targetModifier is not Modelular.Runtime.Transform target) return;

        // Animation
        target.Enabled = true;
        target.TargetSelectionGroup = targetGroup;
        target.Position = newPos * t;
        target.Rotation = newRot * t;
        target.Scale = Vector3.Lerp(Vector3.one, newScale, t);
            
    }
    void Anim_Transform03(Modifier targetModifier, float t)
    {
        // Parameters
        string targetGroup = "03";
        Vector3 newPos = Vector3.zero;
        Vector3 newRot = Vector3.forward * 30;
        Vector3 newScale = Vector3.one;

        // Type enforcement
        if (targetModifier is not Modelular.Runtime.Transform target) return;

        // Animation
        target.Enabled = true;
        target.TargetSelectionGroup = targetGroup;
        target.Position = newPos * t;
        target.Rotation = newRot * t;
        target.Scale = Vector3.Lerp(Vector3.one, newScale, t);
            
    }
    void Anim_Cone(Modifier targetModifier, float t)
    {
        // Parameters

        // Type enforcement
        if (targetModifier is not Modelular.Runtime.Cone target) return;

        // Animation
        target.Enabled = true;
        (target as IPrimitive).Rotation = Vector3.right * 90;
        (target as IPrimitive).OutputSelectionGroup = "Cone";
        target.BaseRadius = 25;
        target.Height = 100;
        target.FaceCount = Mathf.Max(1, (int)(t * 64));
        target.Arc = t;
        target.SectionCaps = false;
    }
    void Anim_Flip(Modifier targetModifier, float t)
    {
        // Parameters

        // Type enforcement
        if (targetModifier is not Modelular.Runtime.Flip target) return;

        // Animation
        target.Enabled = true;
        target.TargetSelectionGroup = "Cone";
    }
}

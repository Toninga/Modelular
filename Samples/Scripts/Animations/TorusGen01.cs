using DG.Tweening;
using Modelular.Runtime;
using UnityEngine;

public class TorusGen01 : AnimatedMesh
{
    MeshRenderer _mr;
    Material _mat;
    protected override void Init()
    {
        base.Init();
        _mr = GetComponent<MeshRenderer>();
        _mat = new Material(Shader.Find("Modelular/SH_Previz"));
        _mr.sharedMaterial = _mat;

        // ANIM #1 - Arc growth
        var target = Add<Torus>();
        var submesh = Add<SetSubmesh>();
        submesh.SubmeshID = -1;
        target.Arc = 0;
        target.Radius = 1;
        target.RadialSubdiv = 64;
        target.ThicknessSubdiv = 64;
        target.Caps = false;
        Add((target, t) => target.Arc = t * (1f), target, 4f);


        // ANIM #2 - Opacity fade in
        // Empty because the animation function doesn't need a modifier to work on, but the system requires one to be passed in.
        // The animation function will just ignore the modifier and use the _mat field instead.
        Modelular.Runtime.Transform empty = null;
        Add((a, b) => { }, empty, 3f);
        Add((mod, t) => _mat.SetFloat("_Opacity", 1-t*0.5f), empty, 2f);
    }
}

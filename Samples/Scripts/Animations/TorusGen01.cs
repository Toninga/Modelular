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

        var target = Add<Torus>();
        target.Arc = 0;
        target.Radius = 1;
        target.RadialSubdiv = 64;
        target.ThicknessSubdiv = 64;
        target.Caps = false;
        Add((target, t) => target.Arc = t * (1f), target, 4f);


    }
}

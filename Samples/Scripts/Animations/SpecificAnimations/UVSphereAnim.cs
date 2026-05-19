using Modelular.Runtime;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ModularMesh))]
public class UVSphereAnim : PlayableGraphicsController
{
    ModularMesh _mm;
    UVSphereModel _model;
    protected override void Start()
    {
        base.Start();
        _mm = GetComponent<ModularMesh>();
        _model = _mm.Modifiers.Where(model => model is UVSphereModel).FirstOrDefault() as UVSphereModel;
    }
    protected override void Update()
    {
        base.Update();
        if (!IsPlaying) return;
        
        _model.Radius = Mathf.Round(Mathf.SmoothStep(0.75f, 0.5f, Mathf.Abs((T - 1 / 6f) * 6)) * 100)/100;

        _model.HorizontalSubdivisions = (int)Mathf.SmoothStep(24, 8, Mathf.Abs((T - 4 / 6f) * 6));
        _model.VerticalSubdivisions = (int)Mathf.SmoothStep(35, 16, Mathf.Abs((T - 5 / 6f) * 6));
    }
}

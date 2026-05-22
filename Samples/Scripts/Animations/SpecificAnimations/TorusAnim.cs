using Modelular.Runtime;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ModularMesh))]
public class TorusAnim : PlayableGraphicsController
{
    ModularMesh _mm;
    TorusModel _model;
    protected override void Start()
    {
        base.Start();
        _mm = GetComponent<ModularMesh>();
        _model = _mm.Modifiers.Where(model => model is TorusModel).FirstOrDefault() as TorusModel;
    }
    protected override void Apply(float t)
    {
        _model.Radius = Mathf.Round(Mathf.SmoothStep(0.4f, 0.7f, Mathf.Abs((T - 1 / 8f) * 8))*100) / 100;
        _model.Thickness = Mathf.Round(Mathf.SmoothStep(0.3f, 0.2f, Mathf.Abs((T - 2 / 8f) * 8))*100) / 100;

        _model.RadialSubdiv = (int)Mathf.SmoothStep(6, 32, Mathf.Abs((T - 4 / 8f) * 8));
        _model.ThicknessSubdiv = (int)Mathf.SmoothStep(6, 24, Mathf.Abs((T - 5 / 8f) * 8));

        _model.Arc = Mathf.Round(Mathf.SmoothStep(0.3f, 1.01f, Mathf.Abs((T - 7 / 8f) * 8))*100) / 100;
    }
}

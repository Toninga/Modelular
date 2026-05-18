using Modelular.Runtime;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ModularMesh))]
public class CylinderAnim : PlayableGraphicsController
{
    ModularMesh _mm;
    CylinderModel _model;
    protected override void Start()
    {
        base.Start();
        _mm = GetComponent<ModularMesh>();
        _model = _mm.Modifiers.Where(model => model is CylinderModel).FirstOrDefault() as CylinderModel;
    }
    protected override void Update()
    {
        base.Update();
        if (!IsPlaying) return;

        _model.Height = Mathf.Round(Mathf.SmoothStep(1, 0.6f, (T - 0 / 6f) * 6)*100) / 100;
        _model.Radius = Mathf.Round(Mathf.SmoothStep(0.5f, 0.65f, (T - 1 / 6f) * 6)*100) / 100;
        _model.HorizontalSubdivisions = (int)Mathf.SmoothStep(0, 15, (T - 2 / 6f) * 6);
        _model.VerticalSubdivisions = (int)Mathf.SmoothStep(3, 20, Mathf.Abs((T - 3 / 6f) * 6));
        _model.GenerateCaps = (T - 5 / 6f) * 6 < 0 || T >= 0.97;
        _model.GenerateShell = (T - 11 / 12f) * 6 < 0 || T >= 0.97;

    }
}
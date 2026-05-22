using Modelular.Runtime;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ModularMesh))]
public class DiskAnim : PlayableGraphicsController
{
    ModularMesh _mm;
    DiskModel _model;
    protected override void Start()
    {
        base.Start();
        _mm = GetComponent<ModularMesh>();
        _model = _mm.Modifiers.Where(model => model is DiskModel).FirstOrDefault() as DiskModel;
    }
    protected override void Apply(float t)
    {
        _model.Radius = Mathf.Round(Mathf.SmoothStep(0.6f, 0.5f, Mathf.Abs((T - 1 / 6f) * 6))*100) / 100;
        _model.Subdivisions = (int)Mathf.SmoothStep(64, 6, Mathf.Abs((T - 3 / 6f) * 6));
        _model.Arc = Mathf.Round(Mathf.SmoothStep(0.1f, 1.01f, Mathf.Abs((T - 5 / 6f) * 6))*100) / 100;

    }
}
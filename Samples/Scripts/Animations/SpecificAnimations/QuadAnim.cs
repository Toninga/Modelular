using Modelular.Runtime;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ModularMesh))]
public class QuadAnim : PlayableGraphicsController
{
    ModularMesh _mm;
    QuadModel _model;
    protected override void Start()
    {
        base.Start();
        _mm = GetComponent<ModularMesh>();
        _model = _mm.Modifiers.Where(model => model is QuadModel).FirstOrDefault() as QuadModel;
    }
    protected override void Apply(float t)
    {
        _model.Size.x = Mathf.Round(Mathf.SmoothStep(2.5f, 1, Mathf.Abs((T - 1 / 6f) * 6))*100) / 100;
        _model.Size.y = Mathf.Round(Mathf.SmoothStep(0.3f, 1, Mathf.Abs((T - 2 / 6f) * 6))*100) / 100;

        _model.Subdivisions.x = (int)Mathf.SmoothStep(10, 0, Mathf.Abs((T - 4 / 6f) * 6));
        _model.Subdivisions.y = (int)Mathf.SmoothStep(10, 0, Mathf.Abs((T - 9 / 12f) * 6));
    }
}

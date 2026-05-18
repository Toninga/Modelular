using Modelular.Runtime;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ModularMesh))]
public class ConeAnim : PlayableGraphicsController
{
    ModularMesh _mm;
    ConeModel _model;

    override protected void Start()
    {
        base.Start();
        _mm = GetComponent<ModularMesh>();
        _model = _mm.Modifiers.Where((model) => model is ConeModel).FirstOrDefault() as ConeModel;
    }
    override protected void Update()
    {
        base.Update();
        _model.Fill = Mathf.Round(Mathf.SmoothStep(0.1f, 1f, Mathf.Abs((T - 1 / 3f) * 3))*100) / 100;
        _model.Arc = Mathf.Round(Mathf.SmoothStep(0.3f, 1f, Mathf.Abs((T - 2 / 3f) * 3))*100) / 100;
    }
}

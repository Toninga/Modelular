using Modelular.Runtime;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ModularMesh))]
public class CubeAnim : PlayableGraphicsController
{
    public int MaxSubdiv = 10;
    public float MinSize = 1f;
    public float MaxSize = 2f;

    ModularMesh _mm;
    CubeModel _model;
    protected override void Start()
    {
        base.Start();
        _mm = GetComponent<ModularMesh>();
        _model = _mm.Modifiers.Where(model => model is CubeModel).FirstOrDefault() as CubeModel; 
    }
    protected override void Update()
    {
        base.Update();
        if (!IsPlaying) return;
        
        _model.Subdivisions.x = (int)(Mathf.SmoothStep(0, 1, T * 6) * MaxSubdiv);
        _model.Subdivisions.y = (int)(Mathf.SmoothStep(0, 1, (T - 1 / 6f) * 6) * MaxSubdiv);
        _model.Subdivisions.z = (int)(Mathf.SmoothStep(0, 1, (T - 2 / 6f) * 6) * MaxSubdiv);

        _model.Size.x = Mathf.Round((Mathf.SmoothStep(0, 1, (T - 3 / 6f) * 6) * (MaxSize - MinSize) + MinSize) *100)/100;
        _model.Size.y = Mathf.Round((Mathf.SmoothStep(0, 1, (T - 4 / 6f) * 6) * (MaxSize - MinSize) + MinSize) *100)/100;
        _model.Size.z = Mathf.Round((Mathf.SmoothStep(0, 1, (T - 5 / 6f) * 6) * (MaxSize - MinSize) + MinSize) *100)/100;
    }
}

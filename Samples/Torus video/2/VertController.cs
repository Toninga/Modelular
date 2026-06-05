using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteAlways]
public class VertController : MonoBehaviour
{
    public float OutlineSize;
    public Color Color = new Color(210f/255f, 150/255f, 0f, 1f);

    MeshRenderer _mr;

    private void OnEnable()
    {
        _mr = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        Apply();   
    }
    void Apply()
    {
        _mr = GetComponent<MeshRenderer>();
        if (_mr == null) return;

        var mpb = new MaterialPropertyBlock();
        _mr.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", Color);
        _mr.SetPropertyBlock(mpb);

        if (_mr.sharedMaterials.Length > 1)
        {
            Debug.Log("Modifying outline");
            var mat = _mr.sharedMaterials[1];
            mat.SetFloat("_Inflation", OutlineSize);
            _mr.SetMaterials(new System.Collections.Generic.List<Material> {_mr.sharedMaterials[0], mat });
        }
    }
    private void Reset()
    {
        _mr = GetComponent<MeshRenderer>();
    }
    private void OnValidate() => Apply();
    private void OnAnimatorMove() => Apply();
    void OnDidApplyAnimationProperties() => Apply();

}

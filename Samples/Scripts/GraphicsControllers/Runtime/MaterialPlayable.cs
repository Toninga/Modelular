using UnityEngine;

public class MaterialPlayable : PlayableGraphicsController
{
    [Header("Translation options")]
    [SerializeField] public ETargetMaterialScope TargetMaterialScope;
    [SerializeField] Material _sharedMaterial;
    [SerializeField] Renderer _targetRenderer;

    [SerializeField] public ETargetMaterialProperty TargetMaterialProperty;
    [SerializeField] public string _targetMaterialPropertyName = "_YourPropertyName";
    [SerializeField] public Vector2 _floatRange = new(0,1);
    [SerializeField] public Vector4 _startVector = Vector4.zero;
    [SerializeField] public Vector4 _endVector = Vector4.one;
    [SerializeField] public Color _startColor = Color.black;
    [SerializeField] public Color _endColor = Color.white;


    protected override void Apply(float t)
    {
        Apply();
    }
    void Apply()
    {
        if (TargetMaterialScope == ETargetMaterialScope.SharedMaterial)
        {
            if (_sharedMaterial == null)
                return;
            ApplyProperty_SharedMaterial(_sharedMaterial);
        }

        else if (TargetMaterialScope == ETargetMaterialScope.SpecificRendererOnly)
        {
            if (_targetRenderer == null)
                return;

            var mpb = ApplyProperty_MaterialPropertyBlock();
            _targetRenderer.SetPropertyBlock(mpb);
        }
    }

    MaterialPropertyBlock ApplyProperty_MaterialPropertyBlock()
    {
        MaterialPropertyBlock mpb = new();

        switch (TargetMaterialProperty)
        {
            case ETargetMaterialProperty.Float:
                mpb.SetFloat(_targetMaterialPropertyName, T * (_floatRange.y - _floatRange.x) + _floatRange.x);
                break;

            case ETargetMaterialProperty.Vector:
                mpb.SetVector(_targetMaterialPropertyName, Vector4.Lerp(_startVector, _endVector, T));
                break;

            case ETargetMaterialProperty.Color:
                mpb.SetColor(_targetMaterialPropertyName, Color.Lerp(_startColor, _endColor, T));
                break;

        }

        return mpb;
    }
    void ApplyProperty_SharedMaterial(Material sharedMaterial)
    {
        switch (TargetMaterialProperty)
        {
            case ETargetMaterialProperty.Float:
                sharedMaterial.SetFloat(_targetMaterialPropertyName, T * (_floatRange.y - _floatRange.x) + _floatRange.x);
                break;

            case ETargetMaterialProperty.Vector:
                sharedMaterial.SetVector(_targetMaterialPropertyName, Vector4.Lerp(_startVector, _endVector, T));
                break;

            case ETargetMaterialProperty.Color:
                sharedMaterial.SetColor(_targetMaterialPropertyName, Color.Lerp(_startColor, _endColor, T));
                break;

        }
    }

    public enum ETargetMaterialScope
    {
        SharedMaterial,
        SpecificRendererOnly
    }
    public enum ETargetMaterialProperty
    {
        Float,
        Vector,
        Color
    }
}

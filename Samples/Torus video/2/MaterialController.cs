using UnityEngine;

[ExecuteAlways]
public class MaterialController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Opacity;

    // public Color Color; // Uncomment to control Color

    private void Update()
    {
        Apply();
    }

    void Apply()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out MeshRenderer mr))
            {
                var mpb = new MaterialPropertyBlock();
                mr.GetPropertyBlock(mpb);
                mpb.SetFloat("_Opacity", Opacity);
                // mpb.SetColor("_Color", Color); // Uncomment to control Color
                mr.SetPropertyBlock(mpb);
            }
        }
    }


    private void OnValidate() => Apply();
    void OnDidApplyAnimationProperties() => Apply();
}

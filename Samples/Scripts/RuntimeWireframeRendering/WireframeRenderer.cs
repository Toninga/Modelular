using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WireframeRenderer : MonoBehaviour
{
    [Header("Wire appearance")]
    public Color wireColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    public float wireWidth = 1.5f;
    public bool refreshEveryFrame = true;

    // Internal
    [System.NonSerialized] public Mesh BaryMesh;
    [System.NonSerialized] public Material WireMat;
    [System.NonSerialized] public MeshFilter Filter;

    void Start()
    {
        Filter = GetComponent<MeshFilter>();

        // Bake barycentric coords (cached on the component)
        if (BaryMesh == null)
            UpdateMesh();

        // Clone the wireframe material so each mesh can have independent color/width
        UpdateMat();

        WireframeManager.Instance.Register(this);
    }

    private void Update()
    {
        if (refreshEveryFrame)
        {
            UpdateMesh();
            UpdateMat();
        }
    }

    private void UpdateMesh() => BaryMesh = BarycentricBaker.BakeBarycentric(Filter.sharedMesh);
    private void UpdateMat()
    {
        if (WireMat == null)
        {
            var src = Resources.Load<Material>("M_Wireframe"); // put your mat in Resources/
            WireMat = new Material(src);
        }
        WireMat.color = wireColor;
        WireMat.SetFloat("_WireWidth", wireWidth);
    }

    void OnDisable() => WireframeManager.Instance?.Unregister(this);

    void OnValidate()
    {
        // Live-update color in editor
        if (WireMat != null)
        {
            WireMat.color = wireColor;
            WireMat.SetFloat("_WireWidth", wireWidth);
        }
    }
}

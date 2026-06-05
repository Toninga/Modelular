using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
[ExecuteAlways]
public class QuadVertsDisplayController : MonoBehaviour
{
    MeshRenderer _mr;
    MeshFilter _mf;

    [Tooltip("The mesh to instantiate")]
    public Mesh VertexMesh;
    Mesh _vertexMesh;
    public float UniformScale = 1f;
    float _uniformScale = 1f;
    public Color Color = Color.orange;
    Color _color = Color.orange;
    public Material VertexMaterial;
    Material _vertexMaterial;
    public Material OutlineMaterial;
    Material _outlineMaterial;
    [Range(0f,1f)]
    public float VertexOpacity = 1f;

    Action OnValueChange;
    private void Reset()
    {
        _mr = GetComponent<MeshRenderer>();
        _mf = GetComponent<MeshFilter>();
    }
    private void Update()
    {
        CheckForValueChanges();
    }
    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += CheckForValueChanges;
#endif
        OnValueChange += Regenerate;
        _mr = GetComponent<MeshRenderer>();
        _mf = GetComponent<MeshFilter>();
    }
    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update += CheckForValueChanges;
#endif
        OnValueChange -= Regenerate;
    }

    void CheckForValueChanges()
    {
        bool hasChanged = false;

        if (VertexMesh != _vertexMesh)
        {
            _vertexMesh = VertexMesh;
            hasChanged = true;
        }
        if (UniformScale != _uniformScale)
        {
            _uniformScale = UniformScale;
            hasChanged = true;
        }
        if (Color != _color)
        {
            _color = Color;
            hasChanged = true;
        }
        if (VertexMaterial != _vertexMaterial)
        {
            _vertexMaterial = VertexMaterial;
            hasChanged = true;
        }
        if (OutlineMaterial != _outlineMaterial)
        {
            _outlineMaterial = OutlineMaterial;
            hasChanged = true;
        }

        if (hasChanged)
            OnValueChange?.Invoke();
    }
    public void Regenerate()
    {
        Clear();
        InstantiateVertexMeshOnMeshVertices(VertexMesh, _mf.sharedMesh);
    }

    public void Clear()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void InstantiateVertexMeshOnMeshVertices(Mesh meshToInstantiate, Mesh targetMesh)
    {
        VertexMaterial.SetColor("_Color", Color);
        for (int i = 0; i < targetMesh.vertices.Length; i++)
        {
            Vector3 vertPos = targetMesh.vertices[i];

            
            MakeVertex(meshToInstantiate, vertPos, UniformScale, "Vert no. " + i, new List<Material> { VertexMaterial, new Material(OutlineMaterial) });
        }
    }

    private void MakeVertex(Mesh meshToInstantiate, Vector3 localPos, float scale, string name, List<Material> materials)
    {
        var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
        go.GetComponent<MeshFilter>().mesh = meshToInstantiate;
        var r = go.GetComponent<MeshRenderer>();
     
        if (materials != null)
        {
            r.SetMaterials(materials);
        }


        go.transform.parent = transform;
        go.transform.localPosition = localPos;
        go.transform.localScale = Vector3.one * scale;

        // Add vert controller
        var ctrl = go.AddComponent<VertController>();
    }

    void Apply()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<MeshRenderer>(out var mr))
            {
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                mpb.SetFloat("_Opacity", VertexOpacity);
                mr.SetPropertyBlock(mpb);
            }
        }
    }
    private void OnValidate() => Apply();
    void OnDidApplyAnimationProperties() => Apply();
}

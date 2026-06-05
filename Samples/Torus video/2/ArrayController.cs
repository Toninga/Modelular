using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class ArrayController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Opacity = 1f;
    [Header("Layout")]
    [Range(0f, 1f)]
    public float Width = 0.5f;
    [Range(0f, 1f)]
    public float PosX = 0.5f;
    [Range(0f, 1f)]
    public float PosY = 0.5f;
    public EAlignment Alignment = EAlignment.Center;

    [Header("Content")]
    [Range(0, 20)]
    public int Count;
    int _count;
    [Range(0f, 1f)]
    public float CursorPos;
    [Range(0f, 1f)]
    public float CursorOpacity;
    public Mesh Content;
    public Material ContentMaterial;
    public float ContentScale = 1f;

    [Header("Fixed parameters")]
    public Transform StartBracket;
    public Transform EndBracket;
    public Transform Container;
    public Transform Cursor;
    public Material BracketMaterial;
    public float DistanceFromCamera = 0.35f;
    public enum EAlignment {  Left, Center, Right }

    Action OnValueChange;
    private void Update()
    {
        CheckForValueChanges();
    }

    private void CheckForValueChanges()
    {
        if (Count != _count)
        {
            _count = Count;
            OnValueChange?.Invoke();
        }
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += CheckForValueChanges;
#endif
        OnValueChange += EnforceContent;
    }
    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update += CheckForValueChanges;
#endif
        OnValueChange -= EnforceContent;
    }

    Vector2 GetHalfScreenSize()
    {
        float FOV = Camera.main.fieldOfView;
        float d = DistanceFromCamera;
        float slope = Mathf.Atan((float)9 / 16);
        float tan = Mathf.Tan(FOV / 180 * Mathf.PI);

        return new Vector2(tan * Mathf.Cos(slope) * d, tan * Mathf.Sin(slope) * d);
    }

    void Apply()
    {
        if (StartBracket == null || EndBracket == null) return;

        // Place brackets correctly
        StartBracket.transform.localPosition = new Vector3(GetLeftX(), GetY(), DistanceFromCamera);
        EndBracket.transform.localPosition = new Vector3(GetRightX(), GetY(), DistanceFromCamera);

        // Apply bracket opacity
        if (BracketMaterial != null)
        {
            Color c = BracketMaterial.color;
            BracketMaterial.color = new Color(c.r, c.g, c.b, Opacity);
        }

        // Apply cursor opacity
        if (Cursor.TryGetComponent(out MeshRenderer mr))
        {
            var mpb = new MaterialPropertyBlock();
            mr.GetPropertyBlock(mpb);
            mpb.SetFloat("_Opacity", CursorOpacity * Opacity);
            mr.SetPropertyBlock(mpb);
        }

        // Apply content opacity
        for (int i = 0; i < Container.childCount; i++)
        {
            if (Container.GetChild(i).TryGetComponent(out MeshRenderer mr2))
            {
                var mpb = new MaterialPropertyBlock();
                mr2.GetPropertyBlock(mpb);
                mpb.SetFloat("_Opacity", Opacity);
                mr2.SetPropertyBlock(mpb);
            }
        }

        // Apply content scale
        for (int i = 0; i < Container.childCount; i++)
        {
            Container.GetChild(i).localScale = Vector3.one * ContentScale;
        }

        // Place cursor
        if (Count > 0)
        {
            if (Count == 1)
                Cursor.localPosition = new Vector3(GetPosX(0.5f), GetY(), DistanceFromCamera);
            else
            {
                float delta = 1f / (Count + 1);
                Cursor.localPosition = new Vector3(GetPosX(CursorPos / (Count + 1) * (Count-1) + delta), GetY(), DistanceFromCamera);
            }
        }
    }
    float GetCenterX() => (PosX * 2 - 1) * GetHalfScreenSize().x;
    float GetLeftX()
    {
        Vector2 wh = GetHalfScreenSize();
        var offset = Alignment switch { EAlignment.Left => 0, EAlignment.Center => 1, EAlignment.Right => 2 };
        return GetCenterX() - 0.77f * Width * wh.x * offset;
    }
    float GetRightX()
    {
        Vector2 wh = GetHalfScreenSize();
        var offset = Alignment switch { EAlignment.Left => 2, EAlignment.Center => 1, EAlignment.Right => 1 };
        return GetCenterX() + 0.77f * Width * wh.x * offset;
    }
    float GetPosX(float t) => (GetRightX() - GetLeftX()) * t + GetLeftX();
    float GetY() => (PosY * 2 - 1) * GetHalfScreenSize().y * 0.6f;
    public void EnforceContent()
    {
        if (Count == Container.childCount) return;
        if (Count < Container.childCount)
        {
            for (int i = 0; i < Container.childCount - Count; i++)
            {
                DestroyImmediate(Container.GetChild(Container.childCount - 1).gameObject);
            }
        }
        if (Count > Container.childCount)
        {
            for (int i = 0; i < Count - Container.childCount; i++)
            {
                Vector3 pos = Vector3.zero;
                var mat = new Material(Shader.Find("Modelular/SH_Previz"));
                mat.SetFloat("_Outline", 0);
                var go = MakeVertex(
                    Content,
                    pos, 
                    ContentScale, 
                    "Element n° " + Container.childCount, 
                    new List<Material> { ContentMaterial, 
                        mat
                        });
                go.transform.SetParent(Container, false);
            }
        }
        float Y = GetY();
        for (int i = 0;i < Container.childCount;i++)
        {
            Container.GetChild(i).localPosition = new Vector3(GetPosX((i + 1f) / (Container.childCount+1)), Y, DistanceFromCamera);
            if (Container.GetChild(i).TryGetComponent(out MeshRenderer mr3))
            {
                var mpb = new MaterialPropertyBlock();
                mr3.GetPropertyBlock(mpb);
                mpb.SetFloat("_Thickness", 0f);
                mr3.SetPropertyBlock(mpb);
            }
        }
    }
    public void Clear()
    {
        int childCount = Container.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(Container.GetChild(0).gameObject);
        }
    }
    private GameObject MakeVertex(Mesh meshToInstantiate, Vector3 localPos, float scale, string name, List<Material> materials)
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

        return go;
    }
    private void OnValidate() => Apply();
    private void OnAnimatorMove() => Apply();
    void OnDidApplyAnimationProperties() => Apply();
}

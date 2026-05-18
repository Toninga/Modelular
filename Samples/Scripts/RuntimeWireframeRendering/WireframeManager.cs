using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WireframeManager
{
    public static WireframeManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new WireframeManager();
            return _instance;
        }
    }
    private static WireframeManager _instance;

    public Camera TargetCamera
    {
        get => _cam;
        set => _cam = value;
    }
    readonly List<WireframeRenderer> _renderers = new();
    public List<WireframeRenderer> Renderers => _renderers;
    CommandBuffer _buf;
    Camera _cam;
    public bool ThrowExceptionIfBufferIsNullOnRebuild = false;

    public bool Enabled => _enabled;
    bool _enabled;

    void Enable()
    {
        if (_cam == null) _cam = Camera.main;

        _buf = new CommandBuffer { name = "Wireframe Overlay" };
        // Inject AFTER the opaque + skybox passes so the overlay sits on top
        _cam.AddCommandBuffer(CameraEvent.AfterForwardOpaque, _buf);
        _enabled = true;
    }

    void Disable()
    {
        if (_cam != null) _cam.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, _buf);
        _buf?.Release();
        _enabled = false;
    }

    public void Register(WireframeRenderer r)
    {
        if (!_renderers.Contains(r)) _renderers.Add(r);
        if (!_enabled)
            Enable();
        RebuildBuffer();
    }

    public void Unregister(WireframeRenderer r)
    {
        _renderers.Remove(r);
        if (_enabled && _renderers.Count == 0)
            Disable();
        RebuildBuffer();
    }

    // Call this whenever the list changes (or from Update if meshes are dynamic)
    public void RebuildBuffer()
    {
        if (_buf == null)
        {
            if (ThrowExceptionIfBufferIsNullOnRebuild)
                throw new System.NullReferenceException("[WireframeManager] CommandBuffer was null on rebuild. Rebuild was canceled");
            return;
        }
        try
        {
            _buf.Clear();
            foreach (var r in _renderers)
            {
                if (r == null || r.BaryMesh == null || r.WireMat == null) continue;
                // DrawMesh respects the object's transform automatically
                _buf.DrawMesh(r.BaryMesh, r.transform.localToWorldMatrix, r.WireMat);
            }
        }
        catch
        {
            if (ThrowExceptionIfBufferIsNullOnRebuild)
                throw new System.NullReferenceException("[WireframeManager] CommandBuffer was null on rebuild. Rebuild was canceled");
            return;
        }
        
    }
}
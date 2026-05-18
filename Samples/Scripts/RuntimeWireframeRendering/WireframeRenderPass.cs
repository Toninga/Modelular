using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class WireframeRenderPass : ScriptableRenderPass
{
    const string k_PassName = "Wireframe Overlay";

    public WireframeRenderPass()
    {
        renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // ── RenderGraph path (Unity 6 / URP 17+) ──────────────────────────────
    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        var renderers = WireframeManager.Instance?.Renderers;
        if (renderers == null || renderers.Count == 0) return;

        var resourceData = frameData.Get<UniversalResourceData>();

        using var builder = renderGraph.AddUnsafePass<PassData>(k_PassName, out var data);

        // We only write to the already-active color + depth targets
        builder.UseTexture(resourceData.activeColorTexture, AccessFlags.Write);
        builder.UseTexture(resourceData.activeDepthTexture, AccessFlags.Read);

        // Snapshot the list so the lambda captures stable data
        data.Entries = new List<(Mesh mesh, Matrix4x4 matrix, Material mat)>(renderers.Count);
        foreach (var r in renderers)
        {
            if (r == null || r.BaryMesh == null || r.WireMat == null) continue;
            data.Entries.Add((r.BaryMesh, r.transform.localToWorldMatrix, r.WireMat));
        }

        builder.SetRenderFunc(static (PassData d, UnsafeGraphContext ctx) =>
        {
            var cmd = ctx.cmd;
            foreach (var (mesh, matrix, mat) in d.Entries)
                cmd.DrawMesh(mesh, matrix, mat);
        });
    }

    class PassData
    {
        public List<(Mesh mesh, Matrix4x4 matrix, Material mat)> Entries;
    }
}
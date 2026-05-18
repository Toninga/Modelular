using UnityEngine.Rendering.Universal;

public class WireframeRendererFeature : ScriptableRendererFeature
{
    WireframeRenderPass _pass;

    public override void Create()
    {
        _pass = new WireframeRenderPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_pass);
    }
}
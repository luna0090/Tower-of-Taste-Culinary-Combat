using UnityEngine.Rendering.Universal;

public class PixelateRenderFeature : ScriptableRendererFeature
{
    private PixelatePass pixelatePass;

    public int pixelWidth = 320;
    public int pixelHeight = 180;

    public override void Create()
    {
        pixelatePass?.Dispose();

        pixelatePass = new PixelatePass(pixelWidth, pixelHeight);
        pixelatePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            pixelatePass?.Dispose();
            pixelatePass = null;
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pixelatePass);
    }
}

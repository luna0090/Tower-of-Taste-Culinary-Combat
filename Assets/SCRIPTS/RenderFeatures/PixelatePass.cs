using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelatePass : ScriptableRenderPass
{
    private RTHandle tempRT;
    private int pixelWidth;
    private int pixelHeight;

    public PixelatePass(int width, int height)
    {
        pixelWidth = width;
        pixelHeight = height;
    }

    [System.Obsolete]
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cameraData = renderingData.cameraData;

        if (cameraData.isPreviewCamera || cameraData.isSceneViewCamera || cameraData.cameraType != CameraType.Game)
            return;

        if (cameraData.renderType != CameraRenderType.Base)
            return;

        var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

        var desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.width = pixelWidth;
        desc.height = pixelHeight;
        desc.msaaSamples = 1;
        desc.depthBufferBits = 0;

        if (tempRT == null || tempRT.rt.width != pixelWidth || tempRT.rt.height != pixelHeight)
        {
            tempRT?.Release();
            tempRT = RTHandles.Alloc(
                desc.width,
                desc.height,
                dimension: TextureDimension.Tex2D,
                colorFormat: desc.graphicsFormat,
                name: "_PixelateRT"
            );
        }

        CommandBuffer cmd = CommandBufferPool.Get("PixelatePass");

        Blit(cmd, source, tempRT);
        Blit(cmd, tempRT, source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        // Optional cleanup
    }
}

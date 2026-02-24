namespace GLibTest;

public sealed class SkiaVulkanControl : SkiaGpuControlBase
{
    protected override ISkiaGpuRenderer CreateRenderer(uint width, uint height)
    {
        return new SkiaVulkanRenderer(
            Handle,
            width,
            height,
            () => new Size(Math.Max(1, Width), Math.Max(1, Height)));
    }
}

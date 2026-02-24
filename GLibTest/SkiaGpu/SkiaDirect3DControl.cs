namespace GLibTest;

public sealed class SkiaDirect3DControl : SkiaGpuControlBase
{
    protected override ISkiaGpuRenderer CreateRenderer(uint width, uint height)
    {
        if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 10240))
        {
            throw new PlatformNotSupportedException("Skia Direct3D control requires Windows 10 (10.0.10240) or later.");
        }

        return new SkiaDirect3DRenderer(Handle, width, height, VSync);
    }
}

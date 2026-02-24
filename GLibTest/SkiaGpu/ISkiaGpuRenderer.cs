using SkiaSharp;

namespace GLibTest;

public interface ISkiaGpuRenderer : IDisposable
{
    uint Width { get; }
    uint Height { get; }
    SKCanvas Canvas { get; }
    void Resize(uint width, uint height);
    void BeginFrame();
    void EndFrame();
}

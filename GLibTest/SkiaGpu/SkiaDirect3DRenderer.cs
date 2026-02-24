using SkiaSharp;
using Silk.NET.Direct3D12;
using Silk.NET.DXGI;
using Silk.NET.Core.Native;

namespace GLibTest;

internal sealed class SkiaDirect3DRenderer : ISkiaGpuRenderer
{
    private readonly nint _windowHandle;
    private readonly bool _vSync;
    private readonly SkiaDirect3DContext _d3dContext;
    private GRContext? _skiaContext;
    private ComPtr<IDXGISwapChain3> _swapChain;
    private SKSurface[]? _surfaces;
    private ComPtr<ID3D12Resource>[]? _renderTargets;
    private bool _disposed;

    private uint _width;
    private uint _height;
    private int _currentFrameIndex;

    private const int FrameCount = 3;

    public uint Width => _width;
    public uint Height => _height;
    public SKCanvas Canvas => _surfaces![_currentFrameIndex].Canvas;

    public SkiaDirect3DRenderer(nint windowHandle, uint width, uint height, bool vSync)
    {
        _windowHandle = windowHandle;
        _width = width;
        _height = height;
        _vSync = vSync;

        _d3dContext = new SkiaDirect3DContext();
        InitializeSwapChain();
        InitializeSkia();
    }

    private unsafe void InitializeSwapChain()
    {
        if (_windowHandle == 0)
        {
            throw new InvalidOperationException("Invalid window handle for Direct3D swapchain.");
        }

        SwapChainDesc1 swapChainDesc = new()
        {
            Width = _width,
            Height = _height,
            Format = Format.FormatR8G8B8A8Unorm,
            Stereo = false,
            SampleDesc = new SampleDesc(1, 0),
            BufferUsage = DXGI.UsageRenderTargetOutput,
            BufferCount = FrameCount,
            Scaling = Scaling.None,
            SwapEffect = SwapEffect.FlipDiscard,
            AlphaMode = AlphaMode.Ignore,
            Flags = _vSync ? 0u : (uint)SwapChainFlag.AllowTearing
        };

        IDXGISwapChain1* tempSwapChainPtr = null;
        int result = _d3dContext.Factory.Get().CreateSwapChainForHwnd((IUnknown*)_d3dContext.Queue.Handle, _windowHandle, &swapChainDesc, null, null, &tempSwapChainPtr);
        if (result != 0)
        {
            throw new InvalidOperationException($"Failed to create Direct3D swapchain: {result:X}");
        }

        using ComPtr<IDXGISwapChain1> tempSwapChain = new(tempSwapChainPtr);
        _swapChain = tempSwapChain.QueryInterface<IDXGISwapChain3>();

        CreateRenderTargets();
    }

    private void CreateRenderTargets()
    {
        _renderTargets = new ComPtr<ID3D12Resource>[FrameCount];
        for (int i = 0; i < FrameCount; i++)
        {
            _renderTargets[i] = _swapChain.GetBuffer<ID3D12Resource>((uint)i);
        }

        _currentFrameIndex = (int)_swapChain.Get().GetCurrentBackBufferIndex();
    }

    private void InitializeSkia()
    {
        GRD3DBackendContext backendContext = _d3dContext.CreateBackendContext();
        _skiaContext = GRContext.CreateDirect3D(backendContext)
            ?? throw new InvalidOperationException("Failed to create Skia Direct3D context.");

        CreateSurfaces();
    }

    private unsafe void CreateSurfaces()
    {
        _surfaces = new SKSurface[FrameCount];

        for (int i = 0; i < FrameCount; i++)
        {
            using GRD3DTextureResourceInfo renderTargetInfo = new()
            {
                Resource = (nint)_renderTargets![i].Handle,
                ResourceState = (uint)ResourceStates.RenderTarget,
                Format = (uint)Format.FormatR8G8B8A8Unorm,
                SampleCount = 1,
                LevelCount = 1
            };

            using GRBackendRenderTarget backendRenderTarget = new((int)_width, (int)_height, renderTargetInfo);
            _surfaces[i] = SKSurface.Create(_skiaContext!, backendRenderTarget, GRSurfaceOrigin.TopLeft, SKColorType.Rgba8888)
                ?? throw new InvalidOperationException($"Failed to create Direct3D Skia surface for backbuffer {i}.");
        }
    }

    public void BeginFrame()
    {
        _currentFrameIndex = (int)_swapChain.Get().GetCurrentBackBufferIndex();
    }

    public void EndFrame()
    {
        _skiaContext?.Flush();

        uint presentFlags = _vSync ? 0u : DXGI.PresentAllowTearing;
        _swapChain.Get().Present(_vSync ? 1u : 0u, presentFlags);
    }

    public void Resize(uint width, uint height)
    {
        if (width == _width && height == _height)
        {
            return;
        }

        _width = width;
        _height = height;

        _skiaContext?.Flush();
        _skiaContext?.Submit(true);

        CleanupRenderTargets();

        int result = _swapChain.Get().ResizeBuffers(FrameCount, width, height, Format.FormatR8G8B8A8Unorm, _vSync ? 0u : (uint)SwapChainFlag.AllowTearing);
        if (result != 0)
        {
            throw new InvalidOperationException($"Failed to resize Direct3D swapchain buffers: {result:X}");
        }

        CreateRenderTargets();
        CreateSurfaces();
    }

    private void CleanupRenderTargets()
    {
        if (_surfaces != null)
        {
            foreach (SKSurface surface in _surfaces)
            {
                surface.Dispose();
            }

            _surfaces = null;
        }

        if (_renderTargets != null)
        {
            foreach (ComPtr<ID3D12Resource> renderTarget in _renderTargets)
            {
                renderTarget.Dispose();
            }

            _renderTargets = null;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        CleanupRenderTargets();
        _swapChain.Dispose();
        _skiaContext?.Dispose();
        _d3dContext.Dispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

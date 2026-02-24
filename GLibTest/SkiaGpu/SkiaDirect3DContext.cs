using Silk.NET.Direct3D12;
using Silk.NET.DXGI;
using Silk.NET.Core.Native;
using SkiaSharp;

namespace GLibTest;

internal sealed class SkiaDirect3DContext : IDisposable
{
    private readonly ComPtr<IDXGIFactory4> _factory;
    private readonly ComPtr<ID3D12Device2> _device;
    private readonly ComPtr<IDXGIAdapter1> _adapter;
    private ComPtr<ID3D12CommandQueue> _queue;
    private bool _disposed;

    public unsafe SkiaDirect3DContext()
    {
        DXGI dxgi = DXGI.GetApi(null, false);
        D3D12 d3d12 = D3D12.GetApi();

        int factoryResult = dxgi.CreateDXGIFactory2(0, out _factory);
        if (factoryResult != 0)
        {
            throw new InvalidOperationException($"Failed to create DXGI factory: {factoryResult:X}");
        }

        ComPtr<ID3D12Device2> device = default;
        ComPtr<IDXGIAdapter1> adapter = default;

        for (uint i = 0; ; i++)
        {
            int result = _factory.Get().EnumAdapters1(i, ref adapter);
            if (result != 0)
            {
                break;
            }

            int createResult = d3d12.CreateDevice(adapter, D3DFeatureLevel.Level111, out device);
            if (createResult == 0)
            {
                break;
            }

            adapter.Dispose();
            adapter = default;
        }

        if (device.Handle == null)
        {
            throw new PlatformNotSupportedException("Failed to create Direct3D12 device.");
        }

        _device = device;
        _adapter = adapter;

        CommandQueueDesc queueDesc = new()
        {
            Flags = CommandQueueFlags.None,
            Type = CommandListType.Direct
        };

        int queueResult = _device.Get().CreateCommandQueue(&queueDesc, out _queue);
        if (queueResult != 0)
        {
            throw new InvalidOperationException($"Failed to create command queue: {queueResult:X}");
        }
    }

    public ComPtr<IDXGIFactory4> Factory => _factory;
    public ComPtr<ID3D12Device2> Device => _device;
    public ComPtr<IDXGIAdapter1> Adapter => _adapter;
    public ComPtr<ID3D12CommandQueue> Queue => _queue;

    public unsafe GRD3DBackendContext CreateBackendContext()
    {
        return new GRD3DBackendContext
        {
            Adapter = (nint)_adapter.Handle,
            Device = (nint)_device.Handle,
            Queue = (nint)_queue.Handle,
        };
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _queue.Dispose();
        _device.Dispose();
        _adapter.Dispose();
        _factory.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

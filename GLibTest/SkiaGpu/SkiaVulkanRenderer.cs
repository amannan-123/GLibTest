using SkiaSharp;
using Silk.NET.Vulkan;

namespace GLibTest;

internal sealed class SkiaVulkanRenderer : ISkiaGpuRenderer
{
    private readonly SkiaVulkanContext _vulkanContext;
    private GRContext? _skiaContext;
    private SKSurface[]? _surfaces;
    private bool _disposed;

    private int _currentSurfaceIndex;

    public uint Width { get; private set; }
    public uint Height { get; private set; }
    public SKCanvas Canvas => _surfaces![_currentSurfaceIndex].Canvas;

    public SkiaVulkanRenderer(nint windowHandle, uint width, uint height, Func<Size> getControlSize)
    {
        Width = width;
        Height = height;

        _vulkanContext = new SkiaVulkanContext(windowHandle, getControlSize);
        InitializeSkia();
    }

    private void InitializeSkia()
    {
        (nint instanceHandle, nint physicalDeviceHandle, nint deviceHandle, nint queueHandle) = _vulkanContext.GetVulkanHandles();

        GRVkBackendContext backendContext = new()
        {
            VkInstance = instanceHandle,
            VkPhysicalDevice = physicalDeviceHandle,
            VkDevice = deviceHandle,
            VkQueue = queueHandle,
            GraphicsQueueIndex = _vulkanContext.GetGraphicsQueueFamilyIndex(),
            GetProcedureAddress = GetVulkanProcDelegate()
        };

        _skiaContext = GRContext.CreateVulkan(backendContext, new GRContextOptions())
            ?? throw new InvalidOperationException("Failed to create Skia Vulkan context.");

        CreateSurfaces();
    }

    private void CreateSurfaces()
    {
        int swapchainImageCount = _vulkanContext.SwapchainImageCount;
        _surfaces = new SKSurface[swapchainImageCount];

        for (int i = 0; i < swapchainImageCount; i++)
        {
            SKColorType colorType = _vulkanContext.SwapchainImageFormat switch
            {
                Format.B8G8R8A8Unorm => SKColorType.Bgra8888,
                Format.R8G8B8A8Unorm => SKColorType.Rgba8888,
                _ => SKColorType.Rgba8888
            };

            GRBackendRenderTarget renderTarget = _vulkanContext.CreateVulkanRenderTarget((int)Width, (int)Height, i);

            _surfaces[i] = SKSurface.Create(_skiaContext, renderTarget, GRSurfaceOrigin.TopLeft, colorType)
                ?? throw new InvalidOperationException($"Failed to create Vulkan Skia surface for swapchain image {i}.");
        }
    }

    public void BeginFrame()
    {
        uint imageIndex = _vulkanContext.AcquireNextImage();
        _currentSurfaceIndex = (int)imageIndex;
    }

    public void EndFrame()
    {
        if (_skiaContext == null || _surfaces == null)
        {
            return;
        }

        _skiaContext.Flush();
        _vulkanContext.SignalRenderFinished();
        _vulkanContext.PresentImage();
    }

    public void Resize(uint width, uint height)
    {
        if (_surfaces == null || (Width == width && Height == height))
        {
            return;
        }

        Width = width;
        Height = height;

        foreach (SKSurface surface in _surfaces)
        {
            surface.Dispose();
        }

        _vulkanContext.RecreateSwapchain();
        CreateSurfaces();
    }

    private static GRVkGetProcedureAddressDelegate GetVulkanProcDelegate()
    {
        return (name, instance, device) =>
        {
            Vk vk = Vk.GetApi();
            IntPtr result = IntPtr.Zero;

            if (device != IntPtr.Zero)
            {
                result = (IntPtr)vk.GetDeviceProcAddr(new Device(device), name);
                if (result != IntPtr.Zero)
                {
                    return result;
                }
            }

            if (instance != IntPtr.Zero)
            {
                result = (IntPtr)vk.GetInstanceProcAddr(new Instance(instance), name);
                if (result != IntPtr.Zero)
                {
                    return result;
                }
            }

            return (IntPtr)vk.GetInstanceProcAddr(new Instance(0), name);
        };
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_surfaces != null)
        {
            foreach (SKSurface surface in _surfaces)
            {
                surface.Dispose();
            }
        }

        _skiaContext?.Dispose();
        _vulkanContext.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

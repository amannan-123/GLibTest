using Silk.NET.Vulkan;

namespace GLibTest;

internal sealed class SkiaVulkanFrameResources : IDisposable
{
    private readonly Vk _vk;
    private readonly Device _device;
    private bool _disposed;

    public Silk.NET.Vulkan.Semaphore ImageAvailableSemaphore { get; private set; }
    public Silk.NET.Vulkan.Semaphore RenderFinishedSemaphore { get; private set; }
    public Fence InFlightFence { get; private set; }

    public SkiaVulkanFrameResources(Vk vk, Device device)
    {
        _vk = vk;
        _device = device;
        CreateSyncObjects();
    }

    private unsafe void CreateSyncObjects()
    {
        SemaphoreCreateInfo semaphoreInfo = new()
        {
            SType = StructureType.SemaphoreCreateInfo
        };

        _vk.CreateSemaphore(_device, &semaphoreInfo, null, out Silk.NET.Vulkan.Semaphore imageAvailableSem).ThrowOnError();
        _vk.CreateSemaphore(_device, &semaphoreInfo, null, out Silk.NET.Vulkan.Semaphore renderFinishedSem).ThrowOnError();

        ImageAvailableSemaphore = imageAvailableSem;
        RenderFinishedSemaphore = renderFinishedSem;

        FenceCreateInfo fenceInfo = new()
        {
            SType = StructureType.FenceCreateInfo,
            Flags = FenceCreateFlags.SignaledBit
        };

        _vk.CreateFence(_device, &fenceInfo, null, out Fence fence).ThrowOnError();
        InFlightFence = fence;
    }

    public unsafe void WaitAndResetFence()
    {
        Fence fence = InFlightFence;
        _vk.WaitForFences(_device, 1, &fence, true, ulong.MaxValue).ThrowOnError();
        _vk.ResetFences(_device, 1, &fence).ThrowOnError();
    }

    public unsafe void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_device.Handle != 0)
        {
            _vk.DeviceWaitIdle(_device);
            _vk.DestroySemaphore(_device, ImageAvailableSemaphore, null);
            _vk.DestroySemaphore(_device, RenderFinishedSemaphore, null);
            _vk.DestroyFence(_device, InFlightFence, null);
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

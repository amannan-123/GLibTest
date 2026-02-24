using System.Diagnostics;
using System.Runtime.InteropServices;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;
using SkiaSharp;
using Image = Silk.NET.Vulkan.Image;
using VkSemaphore = Silk.NET.Vulkan.Semaphore;
using VkImageLayout = Silk.NET.Vulkan.ImageLayout;

namespace GLibTest;

internal sealed class SkiaVulkanContext : IDisposable
{
    private bool _disposed = false;
    // Core Vulkan objects
    private readonly Vk vk;
    private Instance instance;
    private PhysicalDevice physicalDevice;
    private Device device;
    private Queue graphicsQueue;
    private Queue presentQueue;
    private SurfaceKHR surface;
    private SwapchainKHR swapchain;
    private Image[]? swapchainImages;
    private uint graphicsQueueIndex;
    private uint presentQueueIndex;
    private readonly nint windowHandle;
    private readonly Func<Size> getControlSize;
    private KhrSurface? khrSurface;
    private KhrSwapchain? khrSwapchain;
    private SurfaceFormatKHR swapchainFormat;
    private Extent2D swapchainExtent;
    private PresentModeKHR presentMode;

    // Frame-in-flight synchronization
    private const int MAX_FRAMES_IN_FLIGHT = 3;
    private SkiaVulkanFrameResources[]? frameResources;
    private int currentFrame = 0;

    // Public properties
    public uint CurrentImageIndex { get; private set; }
    public Format SwapchainImageFormat => swapchainFormat.Format;
    public int SwapchainImageCount => swapchainImages?.Length ?? 0;

    public SkiaVulkanContext(nint windowHandle, Func<Size> getControlSize)
    {
        this.windowHandle = windowHandle;
        this.getControlSize = getControlSize;
        vk = Vk.GetApi();

        CreateInstance();
        CreateSurface();
        SelectPhysicalDevice();
        CreateLogicalDevice();
        CreateSwapchain();
        CreateFrameResources();
    }

    private void CreateInstance()
    {
        unsafe
        {
            // Application info
            ApplicationInfo appInfo = new()
            {
                SType = StructureType.ApplicationInfo,
                PApplicationName = (byte*)SilkMarshal.StringToPtr("SkiaVulkan Demo"),
                ApplicationVersion = Vk.MakeVersion(1, 0, 0),
                PEngineName = (byte*)SilkMarshal.StringToPtr("No Engine"),
                EngineVersion = Vk.MakeVersion(1, 0, 0),
                ApiVersion = Vk.Version12
            };

            // Required extensions
            List<string> requiredExtensions =
            [
                "VK_KHR_surface",
                "VK_KHR_get_physical_device_properties2"
            ];

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                requiredExtensions.Add("VK_KHR_win32_surface");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                requiredExtensions.Add("VK_KHR_xlib_surface");
            }

            nint extensionNames = SilkMarshal.StringArrayToPtr(requiredExtensions.ToArray());

            InstanceCreateInfo createInfo = new()
            {
                SType = StructureType.InstanceCreateInfo,
                PApplicationInfo = &appInfo,
                EnabledExtensionCount = (uint)requiredExtensions.Count,
                PpEnabledExtensionNames = (byte**)extensionNames
            };

            fixed (Instance* pInstance = &instance)
            {
                vk.CreateInstance(&createInfo, null, pInstance).ThrowOnError();
            }

            // Clean up
            SilkMarshal.Free((nint)appInfo.PApplicationName);
            SilkMarshal.Free((nint)appInfo.PEngineName);
            SilkMarshal.Free(extensionNames);

            // Get KHR surface extension
            if (!vk.TryGetInstanceExtension(instance, out khrSurface))
            {
                throw new Exception("Failed to load VK_KHR_surface extension");
            }
        }
    }

    private void CreateSurface()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new PlatformNotSupportedException("Vulkan control currently supports Windows only.");
        }

        if (windowHandle == 0)
        {
            throw new InvalidOperationException("Invalid window handle for Vulkan surface creation.");
        }

        unsafe
        {
            Win32SurfaceCreateInfoKHR info = new()
            {
                SType = StructureType.Win32SurfaceCreateInfoKhr,
                Hinstance = Marshal.GetHINSTANCE(typeof(SkiaVulkanContext).Module),
                Hwnd = windowHandle
            };

            if (!vk.TryGetInstanceExtension<KhrWin32Surface>(instance, out KhrWin32Surface? win32Ext))
            {
                throw new InvalidOperationException("VK_KHR_win32_surface extension is not available.");
            }

            win32Ext.CreateWin32Surface(instance, &info, null, out surface).ThrowOnError();
        }
    }

    private void SelectPhysicalDevice()
    {
        unsafe
        {
            // Get physical device count
            uint deviceCount = 0;
            vk.EnumeratePhysicalDevices(instance, &deviceCount, null);

            if (deviceCount == 0)
                throw new Exception("No Vulkan physical devices found");

            // Get physical devices
            PhysicalDevice[] devices = new PhysicalDevice[deviceCount];
            fixed (PhysicalDevice* devicesPtr = devices)
            {
                vk.EnumeratePhysicalDevices(instance, &deviceCount, devicesPtr);
            }

            // Find a suitable device
            foreach (PhysicalDevice device in devices)
            {
                if (IsDeviceSuitable(device))
                {
                    physicalDevice = device;
                    break;
                }
            }

            if (physicalDevice.Handle == 0)
            {
                throw new Exception("Failed to find a suitable GPU");
            }
        }
    }

    private bool IsDeviceSuitable(PhysicalDevice device)
    {
        FindQueueFamilies(device, out uint? graphicsIndex, out uint? presentIndex);
        bool extensionsSupported = CheckDeviceExtensionSupport(device);

        bool swapChainAdequate = false;
        if (extensionsSupported)
        {
            QuerySwapChainSupport(device, out SwapChainSupportDetails swapChainSupport);
            swapChainAdequate = swapChainSupport.Formats.Length > 0 && swapChainSupport.PresentModes.Length > 0;
        }

        return graphicsIndex.HasValue && presentIndex.HasValue && extensionsSupported && swapChainAdequate;
    }

    private bool CheckDeviceExtensionSupport(PhysicalDevice device)
    {
        unsafe
        {
            uint extensionCount;
            vk.EnumerateDeviceExtensionProperties(device, (byte*)null, &extensionCount, null);

            if (extensionCount == 0)
                return false;

            ExtensionProperties[] availableExtensions = new ExtensionProperties[extensionCount];
            fixed (ExtensionProperties* extensionsPtr = availableExtensions)
            {
                vk.EnumerateDeviceExtensionProperties(device, (byte*)null, &extensionCount, extensionsPtr);
            }

            // Check for required extension
            string requiredExtension = "VK_KHR_swapchain";
            foreach (ExtensionProperties extension in availableExtensions)
            {
                string extName = SilkMarshal.PtrToString((nint)extension.ExtensionName) ?? "";
                if (extName == requiredExtension)
                    return true;
            }

            return false;
        }
    }

    private void FindQueueFamilies(PhysicalDevice device, out uint? graphicsIndex, out uint? presentIndex)
    {
        graphicsIndex = null;
        presentIndex = null;

        unsafe
        {
            // Get queue family properties
            uint queueFamilyCount = 0;
            vk.GetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyCount, null);

            QueueFamilyProperties[] queueFamilies = new QueueFamilyProperties[queueFamilyCount];
            fixed (QueueFamilyProperties* queueFamiliesPtr = queueFamilies)
            {
                vk.GetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyCount, queueFamiliesPtr);
            }

            // Find a queue family with graphics and present support
            for (uint i = 0; i < queueFamilies.Length; i++)
            {
                // Check for graphics support
                if ((queueFamilies[i].QueueFlags & QueueFlags.GraphicsBit) != 0)
                {
                    graphicsIndex = i;
                }

                // Check for present support
                Bool32 presentSupport = false;
                khrSurface?.GetPhysicalDeviceSurfaceSupport(device, i, surface, &presentSupport);

                if (presentSupport)
                {
                    presentIndex = i;
                }

                // If we found both, we're done
                if (graphicsIndex.HasValue && presentIndex.HasValue)
                    break;
            }
        }

        if (!graphicsIndex.HasValue || !presentIndex.HasValue)
        {
            throw new Exception("Failed to find suitable queue families for graphics and presentation");
        }
    }

    private void CreateLogicalDevice()
    {
        unsafe
        {
            // Get queue indices
            FindQueueFamilies(physicalDevice, out uint? graphicsIndex, out uint? presentIndex);
            graphicsQueueIndex = graphicsIndex!.Value;
            presentQueueIndex = presentIndex!.Value;

            // Prepare queue create infos
            HashSet<uint> uniqueQueueFamilies =
            [
                graphicsQueueIndex,
                presentQueueIndex
            ];

            DeviceQueueCreateInfo[] queueCreateInfos = new DeviceQueueCreateInfo[uniqueQueueFamilies.Count];
            int queueCreateInfoIndex = 0;

            float queuePriority = 1.0f;
            foreach (uint queueFamilyIndex in uniqueQueueFamilies)
            {
                queueCreateInfos[queueCreateInfoIndex++] = new DeviceQueueCreateInfo
                {
                    SType = StructureType.DeviceQueueCreateInfo,
                    QueueFamilyIndex = queueFamilyIndex,
                    QueueCount = 1,
                    PQueuePriorities = &queuePriority
                };
            }

            // Device features
            PhysicalDeviceFeatures deviceFeatures = new();

            // Device extensions
            string[] deviceExtensions = ["VK_KHR_swapchain"];
            nint extensionNames = SilkMarshal.StringArrayToPtr(deviceExtensions);

            // Create logical device
            fixed (DeviceQueueCreateInfo* queueCreateInfosPtr = queueCreateInfos)
            fixed (Device* pDevice = &device)
            {
                DeviceCreateInfo createInfo = new()
                {
                    SType = StructureType.DeviceCreateInfo,
                    QueueCreateInfoCount = (uint)queueCreateInfos.Length,
                    PQueueCreateInfos = queueCreateInfosPtr,
                    PEnabledFeatures = &deviceFeatures,
                    EnabledExtensionCount = (uint)deviceExtensions.Length,
                    PpEnabledExtensionNames = (byte**)extensionNames
                };

                vk.CreateDevice(physicalDevice, &createInfo, null, pDevice).ThrowOnError();
            }

            // Clean up
            SilkMarshal.Free(extensionNames);

            // Get device queues
            vk.GetDeviceQueue(device, graphicsQueueIndex, 0, out graphicsQueue);
            vk.GetDeviceQueue(device, presentQueueIndex, 0, out presentQueue);

            // Get swapchain extension
            if (!vk.TryGetDeviceExtension(instance, device, out khrSwapchain))
            {
                throw new Exception("Failed to load VK_KHR_swapchain extension");
            }
        }
    }

    private void QuerySwapChainSupport(PhysicalDevice device, out SwapChainSupportDetails details)
    {
        unsafe
        {
            // Get surface capabilities
            khrSurface!.GetPhysicalDeviceSurfaceCapabilities(device, surface, out SurfaceCapabilitiesKHR capabilities);

            // Get surface formats
            uint formatCount;
            khrSurface!.GetPhysicalDeviceSurfaceFormats(device, surface, &formatCount, null);

            SurfaceFormatKHR[] formats = new SurfaceFormatKHR[formatCount];
            fixed (SurfaceFormatKHR* formatsPtr = formats)
            {
                khrSurface!.GetPhysicalDeviceSurfaceFormats(device, surface, &formatCount, formatsPtr);
            }

            // Get present modes
            uint presentModeCount;
            khrSurface.GetPhysicalDeviceSurfacePresentModes(device, surface, &presentModeCount, null);

            PresentModeKHR[] presentModes = new PresentModeKHR[presentModeCount];
            fixed (PresentModeKHR* presentModesPtr = presentModes)
            {
                khrSurface.GetPhysicalDeviceSurfacePresentModes(device, surface, &presentModeCount, presentModesPtr);
            }

            // Set output
            details = new SwapChainSupportDetails
            {
                Capabilities = capabilities,
                Formats = formats,
                PresentModes = presentModes
            };
        }
    }

    private void CreateSwapchain()
    {
        unsafe
        {
            // Get swap chain details
            QuerySwapChainSupport(physicalDevice, out SwapChainSupportDetails swapChainSupport);

            // Choose swap surface format, present mode, and extent
            swapchainFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);
            presentMode = ChooseSwapPresentMode(swapChainSupport.PresentModes);
            swapchainExtent = ChooseSwapExtent(swapChainSupport.Capabilities);

            // Determine image count
            uint minImages = swapChainSupport.Capabilities.MinImageCount;
            uint maxImages = swapChainSupport.Capabilities.MaxImageCount > 0
                ? swapChainSupport.Capabilities.MaxImageCount
                : uint.MaxValue;
            uint imageCount = Math.Clamp(minImages + 1, minImages, maxImages);

            // Fill out create info
            SwapchainCreateInfoKHR createInfo = new()
            {
                SType = StructureType.SwapchainCreateInfoKhr,
                Surface = surface,
                MinImageCount = imageCount,
                ImageFormat = swapchainFormat.Format,
                ImageColorSpace = swapchainFormat.ColorSpace,
                ImageExtent = swapchainExtent,
                ImageArrayLayers = 1,
                ImageUsage = ImageUsageFlags.ColorAttachmentBit,
                PreTransform = swapChainSupport.Capabilities.CurrentTransform,
                CompositeAlpha = ChooseSupportedCompositeAlpha(swapChainSupport.Capabilities),
                PresentMode = presentMode,
            };

            // Set image sharing mode
            if (graphicsQueueIndex != presentQueueIndex)
            {
                uint[] queueFamilyIndices = [graphicsQueueIndex, presentQueueIndex];
                fixed (uint* queueFamilyIndicesPtr = queueFamilyIndices)
                {
                    createInfo.ImageSharingMode = SharingMode.Concurrent;
                    createInfo.QueueFamilyIndexCount = 2;
                    createInfo.PQueueFamilyIndices = queueFamilyIndicesPtr;
                }
            }
            else
            {
                createInfo.ImageSharingMode = SharingMode.Exclusive;
                createInfo.QueueFamilyIndexCount = 0;
                createInfo.PQueueFamilyIndices = null;
            }

            // Create swapchain
            fixed (SwapchainKHR* pSwapchain = &swapchain)
            {
                khrSwapchain!.CreateSwapchain(device, &createInfo, null, pSwapchain).ThrowOnError();
            }

            // Get swapchain images
            khrSwapchain.GetSwapchainImages(device, swapchain, &imageCount, null);
            swapchainImages = new Image[imageCount];
            fixed (Image* swapchainImagesPtr = swapchainImages)
            {
                khrSwapchain!.GetSwapchainImages(device, swapchain, &imageCount, swapchainImagesPtr);
            }
        }
    }

    private void CreateFrameResources()
    {
        frameResources = new SkiaVulkanFrameResources[MAX_FRAMES_IN_FLIGHT];

        for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
        {
            frameResources[i] = new SkiaVulkanFrameResources(vk, device);
        }

        Debug.WriteLine($"Created {MAX_FRAMES_IN_FLIGHT} frame resources for frame-in-flight synchronization");
    }

    public uint AcquireNextImage()
    {
        unsafe
        {
            try
            {
                // Wait for the current frame's fence to ensure the frame resources are free
                frameResources![currentFrame].WaitAndResetFence();

                // Acquire the next image using current frame's semaphore
                uint imageIndex = 0;
                Result result = khrSwapchain!.AcquireNextImage(
                    device,
                    swapchain,
                    ulong.MaxValue,
                    frameResources![currentFrame].ImageAvailableSemaphore,
                    default,
                    &imageIndex);

                // Check if we need to recreate the swapchain
                if (result == Result.ErrorOutOfDateKhr || result == Result.SuboptimalKhr)
                {
                    Debug.WriteLine($"Swapchain out of date, recreating...");
                    RecreateSwapchain();
                    return AcquireNextImage(); // Try again
                }
                else if (result != Result.Success)
                {
                    Debug.WriteLine($"Failed to acquire swap chain image: {result}");
                    throw new Exception($"Failed to acquire swap chain image: {result}");
                }

                CurrentImageIndex = imageIndex;
                return imageIndex;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AcquireNextImage: {ex.Message}");
                throw;
            }
        }
    }

    public void PresentImage()
    {
        unsafe
        {
            try
            {
                // Present the image, waiting for render to finish
                SwapchainKHR swapchainLocal = swapchain;
                uint imageIndexLocal = CurrentImageIndex;

                VkSemaphore renderFinishedSem = frameResources![currentFrame].RenderFinishedSemaphore;
                PresentInfoKHR presentInfo = new()
                {
                    SType = StructureType.PresentInfoKhr,
                    WaitSemaphoreCount = 1, // Wait for render to finish
                    PWaitSemaphores = &renderFinishedSem,
                    SwapchainCount = 1,
                    PSwapchains = &swapchainLocal,
                    PImageIndices = &imageIndexLocal
                };

                Result result = khrSwapchain!.QueuePresent(presentQueue, &presentInfo);

                if (result == Result.ErrorOutOfDateKhr || result == Result.SuboptimalKhr)
                {
                    Debug.WriteLine($"Swapchain out of date during present, recreating...");
                    RecreateSwapchain();
                }
                else if (result != Result.Success)
                {
                    Debug.WriteLine($"Failed to present swap chain image: {result}");
                    throw new Exception($"Failed to present swap chain image: {result}");
                }

                // Move to next frame
                currentFrame = (currentFrame + 1) % MAX_FRAMES_IN_FLIGHT;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in PresentImage: {ex.Message}");
                throw;
            }
        }
    }

    public void RecreateSwapchain()
    {
        unsafe
        {
            // Wait for the device to be idle
            vk.DeviceWaitIdle(device);

            // Clean up the old swapchain
            CleanupSwapchain();

            // Create a new swapchain
            CreateSwapchain();
        }
    }

    private void CleanupSwapchain()
    {
        unsafe
        {
            khrSwapchain!.DestroySwapchain(device, swapchain, null);
        }
    }

    private static SurfaceFormatKHR ChooseSwapSurfaceFormat(SurfaceFormatKHR[] availableFormats)
    {
        // Prefer SRGB and UNORM formats
        foreach (SurfaceFormatKHR format in availableFormats)
        {
            // Best option - BGRA format
            if (format.Format == Format.B8G8R8A8Unorm)
            {
                return format;
            }
        }

        // If not found, fall back to the first format
        return availableFormats[0];
    }

    private static PresentModeKHR ChooseSwapPresentMode(PresentModeKHR[] availablePresentModes)
    {
        // Preference order: Mailbox (triple buffering), Immediate (no vsync), FIFO (vsync)
        PresentModeKHR[] preferredModes =
        [
            PresentModeKHR.MailboxKhr,     // Best for smooth performance with minimal tearing
            PresentModeKHR.ImmediateKhr,   // Fastest but may have tearing
            PresentModeKHR.FifoKhr         // Guaranteed available but adds latency
        ];

        foreach (PresentModeKHR preferredMode in preferredModes)
        {
            foreach (PresentModeKHR availableMode in availablePresentModes)
            {
                if (availableMode == preferredMode)
                {
                    return preferredMode;
                }
            }
        }

        // Last resort - FIFO (vsync)
        return PresentModeKHR.FifoKhr;
    }

    private Extent2D ChooseSwapExtent(SurfaceCapabilitiesKHR capabilities)
    {
        // If the current extent is not the special value, use it
        if (capabilities.CurrentExtent.Width != uint.MaxValue)
        {
            return capabilities.CurrentExtent;
        }

        // Otherwise, set the extent to match the window size
        Extent2D actualExtent = new()
        {
            Width = (uint)Math.Max(1, getControlSize().Width),
            Height = (uint)Math.Max(1, getControlSize().Height)
        };

        // Clamp to the allowed extents
        actualExtent.Width = Math.Clamp(
            actualExtent.Width,
            capabilities.MinImageExtent.Width,
            capabilities.MaxImageExtent.Width);

        actualExtent.Height = Math.Clamp(
            actualExtent.Height,
            capabilities.MinImageExtent.Height,
            capabilities.MaxImageExtent.Height);

        return actualExtent;
    }

    private static CompositeAlphaFlagsKHR ChooseSupportedCompositeAlpha(SurfaceCapabilitiesKHR capabilities)
    {
        // Prefer PreMultipliedBitKhr if supported, otherwise fallback to OpaqueBitKhr
        if ((capabilities.SupportedCompositeAlpha & CompositeAlphaFlagsKHR.PreMultipliedBitKhr) != 0)
            return CompositeAlphaFlagsKHR.PreMultipliedBitKhr;

        return CompositeAlphaFlagsKHR.OpaqueBitKhr;
    }

    // Key method for SkiaSharp integration - creates a render target for the swapchain image
    public GRBackendRenderTarget CreateVulkanRenderTarget(int width, int height, int imageIndex)
    {
        unsafe
        {
            try
            {
                // Create the Skia Vulkan image info - no layout transitions needed per frame
                GRVkImageInfo imageInfo = new()
                {
                    Image = swapchainImages![imageIndex].Handle,
                    ImageLayout = (uint)VkImageLayout.ColorAttachmentOptimal,
                    Format = (uint)swapchainFormat.Format,
                    ImageTiling = (uint)ImageTiling.Optimal,
                    LevelCount = 1,
                    CurrentQueueFamily = graphicsQueueIndex,
                    SampleCount = 1,
                    Alloc = new GRVkAlloc
                    {
                        Memory = 0, // Handled by Vulkan swapchain
                        Offset = 0,
                        Size = 0, // Handled by Vulkan
                        Flags = 0,
                        BackendMemory = 0
                    }
                };

                Debug.WriteLine($"Creating render target for image {imageIndex} with format {swapchainFormat.Format}, " +
                                  $"handle: {swapchainImages[imageIndex].Handle}");

                return new GRBackendRenderTarget(width, height, imageInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating Vulkan render target: {ex.Message}");
                throw;
            }
        }
    }

    // Get Vulkan handles needed for SkiaSharp integration
    public (IntPtr Instance, IntPtr PhysicalDevice, IntPtr Device, IntPtr Queue) GetVulkanHandles()
    {
        return (
            instance.Handle,
            physicalDevice.Handle,
            device.Handle,
            graphicsQueue.Handle
        );
    }

    public uint GetGraphicsQueueFamilyIndex() => graphicsQueueIndex;

    public void SignalRenderFinished()
    {
        unsafe
        {
            try
            {
                SkiaVulkanFrameResources currentFrameRes = frameResources![currentFrame];

                // Skip unnecessary command buffer recording - SkiaSharp handles GPU work
                // Just submit with proper semaphore synchronization
                PipelineStageFlags waitStages = PipelineStageFlags.ColorAttachmentOutputBit;
                VkSemaphore waitSemaphore = currentFrameRes.ImageAvailableSemaphore;
                VkSemaphore signalSemaphore = currentFrameRes.RenderFinishedSemaphore;

                SubmitInfo submitInfo = new()
                {
                    SType = StructureType.SubmitInfo,
                    WaitSemaphoreCount = 1,
                    PWaitSemaphores = &waitSemaphore,
                    PWaitDstStageMask = &waitStages,
                    CommandBufferCount = 0, // No command buffers needed!
                    PCommandBuffers = null,
                    SignalSemaphoreCount = 1,
                    PSignalSemaphores = &signalSemaphore
                };

                Result submitResult = vk.QueueSubmit(graphicsQueue, 1, &submitInfo, currentFrameRes.InFlightFence);
                if (submitResult != Result.Success)
                {
                    Debug.WriteLine($"QueueSubmit failed: {submitResult}");
                    throw new Exception($"QueueSubmit failed: {submitResult}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SignalRenderFinished: {ex.Message}");
                throw;
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            unsafe
            {
                // Wait for device to be idle before cleaning up
                vk.DeviceWaitIdle(device);

                // Clean up all frame resources
                CleanupFrameResources();

                // Clean up swapchain
                CleanupSwapchain();

                // Clean up device
                vk.DestroyDevice(device, null);

                // Clean up surface
                khrSurface!.DestroySurface(instance, surface, null);

                // Clean up instance
                vk.DestroyInstance(instance, null);
            }
            _disposed = true;
        }
    }

    ~SkiaVulkanContext()
    {
        Dispose(false);
    }

    private void CleanupFrameResources()
    {
        unsafe
        {
            if (frameResources != null)
            {
                for (int i = 0; i < frameResources.Length; i++)
                {
                    frameResources[i]?.Dispose();
                }
                frameResources = null;
            }

        }
    }

    private struct SwapChainSupportDetails
    {
        public SurfaceCapabilitiesKHR Capabilities;
        public SurfaceFormatKHR[] Formats;
        public PresentModeKHR[] PresentModes;
    }
}

internal static class VulkanExtensions
{
    public static void ThrowOnError(this Result result)
    {
        if (result != Result.Success)
        {
            throw new Exception($"Vulkan error: {result}");
        }
    }
}



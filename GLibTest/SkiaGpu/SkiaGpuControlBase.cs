using SkiaSharp;
using System.ComponentModel;

namespace GLibTest;

public abstract class SkiaGpuControlBase : UserControl
{
    private bool _isInitializing;
    private bool _isDisposed;

    private bool _vSync;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool VSync
    {
        get => _vSync;
        set => _vSync = value;
    }

    protected ISkiaGpuRenderer? Renderer { get; private set; }

    public delegate void RenderHandler(object sender, SKCanvas canvas);
    public event RenderHandler? OnRendering;

    protected SkiaGpuControlBase()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        BackColor = Color.Black;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        InitializeRendererIfNeeded();
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        DisposeRenderer();
        base.OnHandleDestroyed(e);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (_isInitializing || Renderer == null || Width <= 0 || Height <= 0)
        {
            return;
        }

        try
        {
            Renderer.Resize((uint)Width, (uint)Height);
        }
        catch
        {
            // Keep control responsive if backend resize fails.
        }
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // Intentionally empty. Backends clear the framebuffer.
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (!IsHandleCreated || Width <= 0 || Height <= 0)
        {
            return;
        }

        InitializeRendererIfNeeded();
        if (Renderer == null)
        {
            return;
        }

        try
        {
            Renderer.BeginFrame();
            OnRendering?.Invoke(this, Renderer.Canvas);
            Renderer.EndFrame();
        }
        catch
        {
            // Keep benchmark UI alive even if a backend errors.
        }
    }

    private void InitializeRendererIfNeeded()
    {
        if (_isInitializing || _isDisposed || Renderer != null || !IsHandleCreated || Width <= 0 || Height <= 0)
        {
            return;
        }

        _isInitializing = true;
        try
        {
            Renderer = CreateRenderer((uint)Width, (uint)Height);
        }
        catch
        {
            DisposeRenderer();
        }
        finally
        {
            _isInitializing = false;
        }
    }

    private void DisposeRenderer()
    {
        Renderer?.Dispose();
        Renderer = null;
    }

    protected abstract ISkiaGpuRenderer CreateRenderer(uint width, uint height);

    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed && disposing)
        {
            DisposeRenderer();
            _isDisposed = true;
        }

        base.Dispose(disposing);
    }
}

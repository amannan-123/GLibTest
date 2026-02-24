namespace GLibTest;

public class CairoControl : UserControl
{
    private Cairo.ImageSurface? _imageSurface;
    private Cairo.Context? _imageContext;
    private int _surfaceWidth;
    private int _surfaceHeight;

    public delegate void RenderHandler(object sender, Cairo.Context context);
    public event RenderHandler? OnRendering;

    public CairoControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        BackColor = Color.Black;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // Intentionally empty. Cairo clears in render callback.
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (Width <= 0 || Height <= 0)
        {
            return;
        }

        EnsureCairoResources(Width, Height);
        OnRendering?.Invoke(this, _imageContext!);
        _imageSurface!.Flush();

        nint hdc = nint.Zero;
        try
        {
            hdc = e.Graphics.GetHdc();
            using Cairo.Surface targetSurface = new Cairo.Win32Surface(hdc);
            using Cairo.Context targetContext = new(targetSurface);
            targetContext.SetSourceSurface(_imageSurface, 0, 0);
            targetContext.Paint();
        }
        finally
        {
            if (hdc != nint.Zero)
            {
                e.Graphics.ReleaseHdc(hdc);
            }
        }
    }

    private void EnsureCairoResources(int width, int height)
    {
        if (_imageSurface != null && _imageContext != null &&
            _surfaceWidth == width && _surfaceHeight == height)
        {
            return;
        }

        DisposeCairoResources();

        _surfaceWidth = width;
        _surfaceHeight = height;
        _imageSurface = new Cairo.ImageSurface(Cairo.Format.Argb32, width, height);
        _imageContext = new Cairo.Context(_imageSurface)
        {
            Antialias = Cairo.Antialias.Gray
        };
    }

    private void DisposeCairoResources()
    {
        _imageContext?.Dispose();
        _imageSurface?.Dispose();
        _imageContext = null;
        _imageSurface = null;
        _surfaceWidth = 0;
        _surfaceHeight = 0;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposeCairoResources();
        }

        base.Dispose(disposing);
    }
}

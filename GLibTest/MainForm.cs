using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Numerics;
using Vortice.Direct2D1;
using System.ComponentModel;

namespace GLibTest
{
    public partial class MainForm : Form
    {
        private readonly Random _rand = new();
        private readonly List<double> _renderTimesMSec = [];
        private readonly SKPaint _skLinePaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke
        };

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Lines { get; set; } = 5000;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Runs { get; set; } = 100;
        public bool StopFlag = false;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Drawing

        private void skglControl1_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            DrawSkiaRandomLines(e.Surface.Canvas, skglPnl.Width, skglPnl.Height);
        }

        private void skControl1_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            DrawSkiaRandomLines(e.Surface.Canvas, skPnl.Width, skPnl.Height);
        }

        private void gControl1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawGdiRandomLines(g, gdiPnl.Width, gdiPnl.Height);
        }

        private void d2dPnl_OnRendering(object sender, ID2D1DeviceContext6 g)
        {
            g.AntialiasMode = AntialiasMode.PerPrimitive;
            DrawD2dRandomLines(g, d2dPnl.Width, d2dPnl.Height);
        }

        private void cairoPnl_OnRendering(object sender, Cairo.Context context)
        {
            DrawCairoRandomLines(context, CairoPnl.Width, CairoPnl.Height);
        }

        private void skiaD3dPnl_OnRendering(object sender, SKCanvas canvas)
        {
            DrawSkiaRandomLines(canvas, skiaD3dPnl.Width, skiaD3dPnl.Height);
        }

        private void skiaVulkanPnl_OnRendering(object sender, SKCanvas canvas)
        {
            DrawSkiaRandomLines(canvas, skiaVulkanPnl.Width, skiaVulkanPnl.Height);
        }

        private void DrawSkiaRandomLines(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(SKColors.Black);
            for (int i = 0; i < Lines; i++)
            {
                _skLinePaint.Color = NextRandomSkColor();
                _skLinePaint.StrokeWidth = _rand.Next(1, 10);
                canvas.DrawLine(
                    _rand.NextSingle() * width,
                    _rand.NextSingle() * height,
                    _rand.NextSingle() * width,
                    _rand.NextSingle() * height,
                    _skLinePaint);
            }
        }

        private void DrawGdiRandomLines(Graphics graphics, int width, int height)
        {
            graphics.Clear(Color.Black);
            for (int i = 0; i < Lines; i++)
            {
                using Pen pen = new(Color.FromArgb(
                    _rand.Next(255),
                    _rand.Next(255),
                    _rand.Next(255),
                    _rand.Next(255)))
                {
                    Width = _rand.Next(1, 10)
                };

                graphics.DrawLine(
                    pen,
                    _rand.NextSingle() * width,
                    _rand.NextSingle() * height,
                    _rand.NextSingle() * width,
                    _rand.NextSingle() * height);
            }
        }

        private void DrawD2dRandomLines(ID2D1DeviceContext6 context, int width, int height)
        {
            context.Clear(new Vortice.Mathematics.Color(0, 0, 0, 0));
            for (int i = 0; i < Lines; i++)
            {
                using ID2D1SolidColorBrush brush = context.CreateSolidColorBrush(
                    new Vortice.Mathematics.Color(
                        (byte)_rand.Next(255),
                        (byte)_rand.Next(255),
                        (byte)_rand.Next(255),
                        (byte)_rand.Next(255)));

                context.DrawLine(
                    new Vector2(_rand.NextSingle() * width, _rand.NextSingle() * height),
                    new Vector2(_rand.NextSingle() * width, _rand.NextSingle() * height),
                    brush,
                    _rand.Next(1, 10));
            }
        }

        private void DrawCairoRandomLines(Cairo.Context cr, int width, int height)
        {
            cr.SetSourceRGBA(0, 0, 0, 1);
            cr.Paint();

            for (int i = 0; i < Lines; i++)
            {
                cr.LineWidth = _rand.Next(1, 10);

                float x1 = _rand.NextSingle() * width;
                float x2 = _rand.NextSingle() * width;
                float y1 = _rand.NextSingle() * height;
                float y2 = _rand.NextSingle() * height;

                cr.MoveTo(x1, y1);
                cr.LineTo(x2, y2);
                cr.SetSourceRGBA(_rand.NextDouble(), _rand.NextDouble(), _rand.NextDouble(), _rand.NextDouble());
                cr.Stroke();
            }
        }

        private SKColor NextRandomSkColor()
        {
            return new SKColor(
                (byte)_rand.Next(255),
                (byte)_rand.Next(255),
                (byte)_rand.Next(255),
                (byte)_rand.Next(255));
        }

        #endregion

        #region Benchmarking

        /// <summary>
        /// Start benchmarking process.
        /// </summary>
        /// <param name="ctrl">1 SKGL, 2 SK, 3 GDI+, 4 DirectX, 5 Cairo, 6 Skia+D3D, 7 Skia+Vulkan.</param>
        void StartBenchmarking(int ctrl)
        {
            btnSettings.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            _renderTimesMSec.Clear();
            Control control;
            string msg;

            switch (ctrl)
            {
                case 1:
                    control = skglPnl;
                    msg = "Skia + OpenGL: ";
                    break;

                case 2:
                    control = skPnl;
                    msg = "Skia: ";
                    break;

                case 3:
                    control = gdiPnl;
                    msg = "GDI+: ";
                    break;

                case 4:
                    control = d2dPnl;
                    msg = "DirectX: ";
                    break;

                case 5:
                    control = CairoPnl;
                    msg = "Cairo: ";
                    break;

                case 6:
                    control = skiaD3dPnl;
                    msg = "Skia + Direct3D: ";
                    break;

                case 7:
                    control = skiaVulkanPnl;
                    msg = "Skia + Vulkan: ";
                    break;

                default:
                    MessageBox.Show("Invalid value!");
                    return;
            }

            lstResults.Items.Add(msg);

            Application.DoEvents(); //Process all messages to get better results.

            Stopwatch stopwatch = new();
            for (int i = 0; i < Runs; i++)
            {

                if (StopFlag)
                    break;

                stopwatch.Restart();
                control.Invalidate();
                Application.DoEvents();
                stopwatch.Stop();

                _renderTimesMSec.Add(1000.0 * stopwatch.ElapsedTicks / Stopwatch.Frequency);
                double mean = _renderTimesMSec.Sum() / _renderTimesMSec.Count;
                lstResults.Items.Add($"{_renderTimesMSec.Count:00}. " +
                    $"{_renderTimesMSec.Last():0.000} ms " +
                    $"(running mean: {mean:0.000} ms)");
                lstResults.SelectedIndex = lstResults.Items.Count - 1;

                //if last run, print average FPS
                if (i == Runs - 1)
                {
                    double avg = 1000 / mean;
                    lstResults.Items.Add($"Average FPS: {avg:0.00}");
                    lstResults.SelectedIndex = lstResults.Items.Count - 1;
                }
            }

            btnSettings.Enabled = true;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        #endregion

        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            skglPnl.Dispose();
            skPnl.Dispose();
            gdiPnl.Dispose();
            d2dPnl.Dispose();
            CairoPnl.Dispose();
            skiaD3dPnl.Dispose();
            skiaVulkanPnl.Dispose();
            _skLinePaint.Dispose();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsDialog st = new();
            st.nRuns.Value = Runs;
            st.nLines.Value = Lines;
            st.btnVSync.Checked = skglPnl.VSync;
            if (st.ShowDialog() != DialogResult.OK) return;
            Runs = (int)st.nRuns.Value;
            Lines = (int)st.nLines.Value;
            skglPnl.VSync = st.btnVSync.Checked;
            d2dPnl.VSync = st.btnVSync.Checked;
            skiaD3dPnl.VSync = st.btnVSync.Checked;
            skiaVulkanPnl.VSync = st.btnVSync.Checked;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int ct = 0;
            if (rbSKGL.Checked) ct = 1;
            if (rbSK.Checked) ct = 2;
            if (rbGDI.Checked) ct = 3;
            if (rbDX.Checked) ct = 4;
            if (rbCairo.Checked) ct = 5;
            if (rbSkiaD3D.Checked) ct = 6;
            if (rbSkiaVulkan.Checked) ct = 7;
            StopFlag = false;
            StartBenchmarking(ct);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopFlag = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lstResults.Items.Clear();
        }
    }
}

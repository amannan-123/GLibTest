using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Numerics;
using Vortice.Direct2D1;

namespace GLibTest
{
	public partial class MainForm : Form
	{
		private readonly Random _rand = new();
		private readonly List<double> _renderTimesMSec = [];

		public int Lines { get; set; } = 5000;
		public int Runs { get; set; } = 500;
		public bool StopFlag = false;

		public MainForm()
		{
			InitializeComponent();
		}

		#region Drawing

		private void skglControl1_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
		{
			
			SKCanvas canvas = e.Surface.Canvas;

			canvas.Clear(SKColors.Black);

			for (int i = 0; i < Lines; i++)
			{
				SKPaint paint = new()
				{
					IsAntialias = true,
					Style = SKPaintStyle.Stroke,
					Color = new SKColor(
						red: (byte)_rand.Next(255),
						green: (byte)_rand.Next(255),
						blue: (byte)_rand.Next(255),
						alpha: (byte)_rand.Next(255)),

					StrokeWidth = _rand.Next(1, 10)
				};

				float x1 = _rand.NextSingle() * skglPnl.Width;
				float x2 = _rand.NextSingle() * skglPnl.Width;
				float y1 = _rand.NextSingle() * skglPnl.Height;
				float y2 = _rand.NextSingle() * skglPnl.Height;
				canvas.DrawLine(x1, y1, x2, y2, paint);

				paint.Dispose();
			}

		}

		private void skControl1_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{

			SKCanvas canvas = e.Surface.Canvas;

			canvas.Clear(SKColors.Black);

			for (int i = 0; i < Lines; i++)
			{
				SKPaint paint = new()
				{
					IsAntialias = true,
					Style = SKPaintStyle.Stroke,
					Color = new SKColor(
						red: (byte)_rand.Next(255),
						green: (byte)_rand.Next(255),
						blue: (byte)_rand.Next(255),
						alpha: (byte)_rand.Next(255)),

					StrokeWidth = _rand.Next(1, 10)
				};

				float x1 = _rand.NextSingle() * skPnl.Width;
				float x2 = _rand.NextSingle() * skPnl.Width;
				float y1 = _rand.NextSingle() * skPnl.Height;
				float y2 = _rand.NextSingle() * skPnl.Height;

				canvas.DrawLine(x1, y1, x2, y2, paint);

				paint.Dispose();
			}

		}

		private void gControl1_Paint(object sender, PaintEventArgs e)
		{

			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			g.Clear(Color.Black);

			for (int i = 0; i < Lines; i++)
			{
				Pen paint = new(Color.FromArgb(
					(byte)_rand.Next(255),
					(byte)_rand.Next(255),
					(byte)_rand.Next(255),
					(byte)_rand.Next(255)))
				{
					Width = _rand.Next(1, 10)
				};

				float x1 = _rand.NextSingle() * gdiPnl.Width;
				float x2 = _rand.NextSingle() * gdiPnl.Width;
				float y1 = _rand.NextSingle() * gdiPnl.Height;
				float y2 = _rand.NextSingle() * gdiPnl.Height;

				g.DrawLine(paint, x1, y1, x2, y2);

				paint.Dispose();
			}

		}

		private void d2dPnl_OnRendering(object sender, ID2D1DeviceContext g)
		{

			g.AntialiasMode = AntialiasMode.PerPrimitive;

			g.Clear(new Vortice.Mathematics.Color(0, 0, 0, 0));

			for (int i = 0; i < Lines; i++)
			{
				Vortice.Mathematics.Color randCol = new(
					(byte)_rand.Next(255),
					(byte)_rand.Next(255),
					(byte)_rand.Next(255),
					(byte)_rand.Next(255));

				ID2D1SolidColorBrush brush = g.CreateSolidColorBrush(randCol);

				int penWidth = _rand.Next(1, 10);


				float x1 = _rand.NextSingle() * d2dPnl.Width;
				float x2 = _rand.NextSingle() * d2dPnl.Width;
				float y1 = _rand.NextSingle() * d2dPnl.Height;
				float y2 = _rand.NextSingle() * d2dPnl.Height;

				g.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), brush, penWidth);

				brush.Dispose();
			}
		}

		private void doubleBufferedControl2_Paint(object sender, PaintEventArgs e)
		{

			Graphics g = e.Graphics;
			Cairo.Surface surface = new Cairo.Win32Surface(g.GetHdc());
			Cairo.Context cr = new(surface)
			{
				Antialias = Cairo.Antialias.Subpixel
			};

			cr.SetSourceRGBA(0, 0, 0, 1);
			cr.Paint();

			for (int i = 0; i < Lines; i++)
			{
				cr.LineWidth = _rand.Next(1, 10);


				float x1 = _rand.NextSingle() * CairoPnl.Width;
				float x2 = _rand.NextSingle() * CairoPnl.Width;
				float y1 = _rand.NextSingle() * CairoPnl.Height;
				float y2 = _rand.NextSingle() * CairoPnl.Height;

				cr.MoveTo(x1, y1);
				cr.LineTo(x2, y2);
				cr.SetSourceRGBA(_rand.NextDouble(), _rand.NextDouble(), _rand.NextDouble(), _rand.NextDouble());
				cr.Stroke();
			}

			cr.Dispose();
			surface.Dispose();
			g.ReleaseHdc();

		}

		#endregion

		#region Benchmarking

		/// <summary>
		/// Start benchmarking process.
		/// </summary>
		/// <param name="ctrl">1 for SKGL, 2 for SK, 3 for GDI+, 4 for DirectX and 5 for Cairo.</param>
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
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			int ct = 0;
			if (rbSKGL.Checked) ct = 1;
			if (rbSK.Checked) ct = 2;
			if (rbGDI.Checked) ct = 3;
			if (rbDX.Checked) ct = 4;
			if (rbCairo.Checked) ct = 5;
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
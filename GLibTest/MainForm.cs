using SkiaSharp;
using System.Diagnostics;
using unvell.D2DLib;

namespace GLibTest
{
	public partial class MainForm : Form
	{
		readonly Random rand = new();
		readonly List<double> renderTimesMsec = new();

		public int Lines { get; set; }
		public int Runs { get; set; }
		public bool StopFlag = false;

		public MainForm()
		{
			InitializeComponent();
		}

		#region Drawing

		private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
		{

			SKCanvas canvas = e.Surface.Canvas;
			for (int i = 0; i < Lines; i++)
			{
				SKPaint paint = new()
				{
					IsAntialias = true,
					Style = SKPaintStyle.Stroke,
					Color = new SKColor(
						red: (byte)rand.Next(255),
						green: (byte)rand.Next(255),
						blue: (byte)rand.Next(255),
						alpha: (byte)rand.Next(255)),

					StrokeWidth = rand.Next(1, 10)
				};

				float x1 = (float)(rand.NextDouble() * skglPnl.Width);
				float x2 = (float)(rand.NextDouble() * skglPnl.Width);
				float y1 = (float)(rand.NextDouble() * skglPnl.Height);
				float y2 = (float)(rand.NextDouble() * skglPnl.Height);
				canvas.DrawLine(x1, y1, x2, y2, paint);

				paint.Dispose();
			}

		}

		private void skControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
		{

			SKCanvas canvas = e.Surface.Canvas;
			for (int i = 0; i < Lines; i++)
			{
				SKPaint paint = new()
				{
					IsAntialias = true,
					Style = SKPaintStyle.Stroke,
					Color = new SKColor(
						red: (byte)rand.Next(255),
						green: (byte)rand.Next(255),
						blue: (byte)rand.Next(255),
						alpha: (byte)rand.Next(255)),

					StrokeWidth = rand.Next(1, 10)
				};

				float x1 = (float)(rand.NextDouble() * skPnl.Width);
				float x2 = (float)(rand.NextDouble() * skPnl.Width);
				float y1 = (float)(rand.NextDouble() * skPnl.Height);
				float y2 = (float)(rand.NextDouble() * skPnl.Height);
				canvas.DrawLine(x1, y1, x2, y2, paint);

				paint.Dispose();
			}

		}

		private void gControl1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			for (int i = 0; i < Lines; i++)
			{
				Pen paint = new(Color.FromArgb(
					(byte)rand.Next(255),
					(byte)rand.Next(255),
					(byte)rand.Next(255),
					(byte)rand.Next(255)))
				{
					Width = rand.Next(1, 10)
				};


				float x1 = (float)(rand.NextDouble() * skPnl.Width);
				float x2 = (float)(rand.NextDouble() * skPnl.Width);
				float y1 = (float)(rand.NextDouble() * skPnl.Height);
				float y2 = (float)(rand.NextDouble() * skPnl.Height);
				g.DrawLine(paint, x1, y1, x2, y2);

				paint.Dispose();
			}

		}

		private void d2dTest1_OnRendering(Object sender, D2DGraphics g)
		{
			g.Antialias = true;

			for (int i = 0; i < Lines; i++)
			{
				D2DColor randCol = new(
					(float)rand.NextDouble(),
					(float)rand.NextDouble(),
					(float)rand.NextDouble(),
					(float)rand.NextDouble());

				int penWidth = rand.Next(1, 10);

				float x1 = (float)(rand.NextDouble() * d2dPnl.Width);
				float x2 = (float)(rand.NextDouble() * d2dPnl.Width);
				float y1 = (float)(rand.NextDouble() * d2dPnl.Height);
				float y2 = (float)(rand.NextDouble() * d2dPnl.Height);
				g.DrawLine(x1, x2, y1, y2, randCol, penWidth);
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

			for (int i = 0; i < Lines; i++)
			{
				cr.LineWidth = rand.Next(1, 10);

				float x1 = (float)(rand.NextDouble() * CairoPnl.Width);
				float x2 = (float)(rand.NextDouble() * CairoPnl.Width);
				float y1 = (float)(rand.NextDouble() * CairoPnl.Height);
				float y2 = (float)(rand.NextDouble() * CairoPnl.Height);

				cr.MoveTo(x1, y1);
				cr.LineTo(x2, y2);
				cr.SetSourceRGBA(rand.NextDouble(), rand.NextDouble(), rand.NextDouble(), rand.NextDouble());
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
		void startBenchmarking(int ctrl)
		{
			btnSettings.Enabled = false;
			btnStart.Enabled = false;
			btnStop.Enabled = true;
			renderTimesMsec.Clear();
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

				renderTimesMsec.Add(1000.0 * stopwatch.ElapsedTicks / Stopwatch.Frequency);
				double mean = renderTimesMsec.Sum() / renderTimesMsec.Count;
				lstResults.Items.Add($"{renderTimesMsec.Count:00}. " +
					$"{renderTimesMsec.Last():0.000} ms " +
					$"(running mean: {mean:0.000} ms)");
				lstResults.SelectedIndex = lstResults.Items.Count - 1;
			}

			btnSettings.Enabled = true;
			btnStart.Enabled = true;
			btnStop.Enabled = false;
		}

		#endregion

		private void Form1_Load(object sender, EventArgs e)
		{
			Lines = 1000;
			Runs = 500;

			// changing size is necessary otherwise it doesn't work correctly (idk why)
			d2dPnl.Size = Size.Empty;

		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
			if (st.ShowDialog() == DialogResult.OK)
			{
				Runs = (int)st.nRuns.Value;
				Lines = (int)st.nLines.Value;
				skglPnl.VSync = st.btnVSync.Checked;
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			int _ct = 0;
			if (rbSKGL.Checked) _ct = 1;
			if (rbSK.Checked) _ct = 2;
			if (rbGDI.Checked) _ct = 3;
			if (rbDX.Checked) _ct = 4;
			if (rbCairo.Checked) _ct = 5;
			StopFlag = false;
			startBenchmarking(_ct);
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			StopFlag = true;
		}

	}
}
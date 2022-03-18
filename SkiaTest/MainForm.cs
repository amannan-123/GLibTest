using SkiaSharp;
using System.Diagnostics;

namespace SkiaTest
{
	public partial class MainForm : Form
	{
		readonly Random rand = new();
		readonly List<double> renderTimesMsec = new();

		public int Lines { get; set; }
		public int Runs { get; set; }

		public MainForm()
		{
			InitializeComponent();
		}

		#region Drawing

		private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
		{

			SKCanvas canvas = e.Surface.Canvas;
			canvas.Clear(SKColors.Red);
			using SKPaint paint = new();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Stroke;
			for (int i = 0; i < Lines; i++)
			{

				paint.Color = new SKColor(
					red: (byte)rand.Next(255),
					green: (byte)rand.Next(255),
					blue: (byte)rand.Next(255),
					alpha: (byte)rand.Next(255));

				paint.StrokeWidth = rand.Next(1, 10);


				float x1 = (float)(rand.NextDouble() * skglControl1.Width);
				float x2 = (float)(rand.NextDouble() * skglControl1.Width);
				float y1 = (float)(rand.NextDouble() * skglControl1.Height);
				float y2 = (float)(rand.NextDouble() * skglControl1.Height);
				canvas.DrawLine(x1, y1, x2, y2, paint);
			}

		}

		private void skControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
		{

			SKCanvas canvas = e.Surface.Canvas;
			canvas.Clear(SKColors.Red);
			using SKPaint paint = new();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Stroke;
			for (int i = 0; i < Lines; i++)
			{

				paint.Color = new SKColor(
					red: (byte)rand.Next(255),
					green: (byte)rand.Next(255),
					blue: (byte)rand.Next(255),
					alpha: (byte)rand.Next(255));

				paint.StrokeWidth = rand.Next(1, 10);


				float x1 = (float)(rand.NextDouble() * skControl1.Width);
				float x2 = (float)(rand.NextDouble() * skControl1.Width);
				float y1 = (float)(rand.NextDouble() * skControl1.Height);
				float y2 = (float)(rand.NextDouble() * skControl1.Height);
				canvas.DrawLine(x1, y1, x2, y2, paint);
			}

		}

		private void gControl1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.Clear(Color.Red);
			using Pen paint = new(Color.Empty);
			for (int i = 0; i < Lines; i++)
			{

				paint.Color = Color.FromArgb(
					(byte)rand.Next(255),
					(byte)rand.Next(255),
					(byte)rand.Next(255),
					(byte)rand.Next(255));

				paint.Width = rand.Next(1, 10);


				float x1 = (float)(rand.NextDouble() * skControl1.Width);
				float x2 = (float)(rand.NextDouble() * skControl1.Width);
				float y1 = (float)(rand.NextDouble() * skControl1.Height);
				float y2 = (float)(rand.NextDouble() * skControl1.Height);
				g.DrawLine(paint, x1, y1, x2, y2);
			}

		}

		#endregion

		#region Benchmarking

		/// <summary>
		/// Start benchmarking process.
		/// </summary>
		/// <param name="ctrl">1 for SKGL, 2 for SK and 3 for GDI+.</param>
		void startBenchmarking(int ctrl)
		{
			btnSettings.Enabled = false;
			renderTimesMsec.Clear();
			Control control;
			string msg;

			switch (ctrl)
			{
				case 1:
					control = skglControl1;
					msg = "Skia + OpenGL: ";
					break;

				case 2:
					control = skControl1;
					msg = "Skia: ";
					break;

				case 3:
					control = gControl1;
					msg = "GDI+: ";
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
		}

		#endregion

		private void Form1_Load(object sender, EventArgs e)
		{
			Lines = 2000;
			Runs = 500;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			skglControl1.Dispose();
			skControl1.Dispose();
			gControl1.Dispose();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			startBenchmarking(1);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			startBenchmarking(2);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			startBenchmarking(3);
		}

		private void btnSettings_Click(object sender, EventArgs e)
		{
			SettingsDialog st = new();
			st.nRuns.Value = Runs;
			st.nLines.Value = Lines;
			st.btnVSync.Checked = skglControl1.VSync;
			if (st.ShowDialog() == DialogResult.OK)
			{
				Runs = (int)st.nRuns.Value;
				Lines = (int)st.nLines.Value;
				skglControl1.VSync = st.btnVSync.Checked;
			}
		}
	}
}
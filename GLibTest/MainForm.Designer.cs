namespace GLibTest
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.skglPnl = new SkiaSharp.Views.Desktop.SKGLControl();
			this.skPnl = new SkiaSharp.Views.Desktop.SKControl();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.d2dPnl = new GLibTest.D2DControl();
			this.gdiPnl = new GLibTest.DoubleBufferedControl();
			this.CairoPnl = new GLibTest.DoubleBufferedControl();
			this.btnSettings = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.lstResults = new System.Windows.Forms.ListBox();
			this.rbSKGL = new System.Windows.Forms.RadioButton();
			this.rbSK = new System.Windows.Forms.RadioButton();
			this.rbGDI = new System.Windows.Forms.RadioButton();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.rbDX = new System.Windows.Forms.RadioButton();
			this.rbCairo = new System.Windows.Forms.RadioButton();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// skglPnl
			// 
			this.skglPnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.skglPnl.BackColor = System.Drawing.Color.Black;
			this.skglPnl.Location = new System.Drawing.Point(4, 3);
			this.skglPnl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.skglPnl.Name = "skglPnl";
			this.skglPnl.Size = new System.Drawing.Size(446, 81);
			this.skglPnl.TabIndex = 0;
			this.skglPnl.VSync = false;
			this.skglPnl.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.skglControl1_PaintSurface);
			// 
			// skPnl
			// 
			this.skPnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.skPnl.BackColor = System.Drawing.Color.Black;
			this.skPnl.Location = new System.Drawing.Point(3, 90);
			this.skPnl.Name = "skPnl";
			this.skPnl.Size = new System.Drawing.Size(448, 81);
			this.skPnl.TabIndex = 2;
			this.skPnl.Text = "skControl1";
			this.skPnl.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.skControl1_PaintSurface);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(472, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 21);
			this.label1.TabIndex = 7;
			this.label1.Text = "Benchmarking:";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.skglPnl, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.skPnl, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.d2dPnl, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.gdiPnl, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.CairoPnl, 0, 4);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 437);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// d2dPnl
			// 
			this.d2dPnl.BackColor = System.Drawing.Color.Black;
			this.d2dPnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.d2dPnl.Location = new System.Drawing.Point(3, 264);
			this.d2dPnl.Name = "d2dPnl";
			this.d2dPnl.Size = new System.Drawing.Size(448, 81);
			this.d2dPnl.TabIndex = 7;
			this.d2dPnl.OnRendering += new GLibTest.D2DControl.Render(this.d2dTest1_OnRendering);
			// 
			// gdiPnl
			// 
			this.gdiPnl.BackColor = System.Drawing.Color.Black;
			this.gdiPnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gdiPnl.Location = new System.Drawing.Point(3, 177);
			this.gdiPnl.Name = "gdiPnl";
			this.gdiPnl.Size = new System.Drawing.Size(448, 81);
			this.gdiPnl.TabIndex = 8;
			this.gdiPnl.Paint += new System.Windows.Forms.PaintEventHandler(this.gControl1_Paint);
			// 
			// CairoPnl
			// 
			this.CairoPnl.BackColor = System.Drawing.Color.Black;
			this.CairoPnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CairoPnl.Location = new System.Drawing.Point(3, 351);
			this.CairoPnl.Name = "CairoPnl";
			this.CairoPnl.Size = new System.Drawing.Size(448, 83);
			this.CairoPnl.TabIndex = 9;
			this.CairoPnl.Paint += new System.Windows.Forms.PaintEventHandler(this.doubleBufferedControl2_Paint);
			// 
			// btnSettings
			// 
			this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSettings.Location = new System.Drawing.Point(472, 424);
			this.btnSettings.Name = "btnSettings";
			this.btnSettings.Size = new System.Drawing.Size(300, 25);
			this.btnSettings.TabIndex = 6;
			this.btnSettings.Text = "Modify Settings";
			this.btnSettings.UseVisualStyleBackColor = true;
			this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label2.Location = new System.Drawing.Point(472, 93);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 21);
			this.label2.TabIndex = 11;
			this.label2.Text = "Results:";
			// 
			// lstResults
			// 
			this.lstResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstResults.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lstResults.FormattingEnabled = true;
			this.lstResults.IntegralHeight = false;
			this.lstResults.ItemHeight = 17;
			this.lstResults.Location = new System.Drawing.Point(472, 117);
			this.lstResults.Name = "lstResults";
			this.lstResults.Size = new System.Drawing.Size(300, 301);
			this.lstResults.TabIndex = 5;
			// 
			// rbSKGL
			// 
			this.rbSKGL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbSKGL.AutoSize = true;
			this.rbSKGL.Checked = true;
			this.rbSKGL.Location = new System.Drawing.Point(489, 39);
			this.rbSKGL.Name = "rbSKGL";
			this.rbSKGL.Size = new System.Drawing.Size(103, 19);
			this.rbSKGL.TabIndex = 0;
			this.rbSKGL.TabStop = true;
			this.rbSKGL.Text = "Skia + OpenGL";
			this.rbSKGL.UseVisualStyleBackColor = true;
			// 
			// rbSK
			// 
			this.rbSK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbSK.AutoSize = true;
			this.rbSK.Location = new System.Drawing.Point(598, 39);
			this.rbSK.Name = "rbSK";
			this.rbSK.Size = new System.Drawing.Size(46, 19);
			this.rbSK.TabIndex = 1;
			this.rbSK.Text = "Skia";
			this.rbSK.UseVisualStyleBackColor = true;
			// 
			// rbGDI
			// 
			this.rbGDI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbGDI.AutoSize = true;
			this.rbGDI.Location = new System.Drawing.Point(650, 39);
			this.rbGDI.Name = "rbGDI";
			this.rbGDI.Size = new System.Drawing.Size(110, 19);
			this.rbGDI.TabIndex = 2;
			this.rbGDI.Text = "System.Drawing";
			this.rbGDI.UseVisualStyleBackColor = true;
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.Location = new System.Drawing.Point(654, 64);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(50, 23);
			this.btnStart.TabIndex = 3;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStop.Enabled = false;
			this.btnStop.Location = new System.Drawing.Point(710, 64);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(50, 23);
			this.btnStop.TabIndex = 4;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// rbDX
			// 
			this.rbDX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbDX.AutoSize = true;
			this.rbDX.Location = new System.Drawing.Point(489, 64);
			this.rbDX.Name = "rbDX";
			this.rbDX.Size = new System.Drawing.Size(63, 19);
			this.rbDX.TabIndex = 1;
			this.rbDX.Text = "DirectX";
			this.rbDX.UseVisualStyleBackColor = true;
			// 
			// rbCairo
			// 
			this.rbCairo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbCairo.AutoSize = true;
			this.rbCairo.Location = new System.Drawing.Point(558, 64);
			this.rbCairo.Name = "rbCairo";
			this.rbCairo.Size = new System.Drawing.Size(53, 19);
			this.rbCairo.TabIndex = 12;
			this.rbCairo.Text = "Cairo";
			this.rbCairo.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 461);
			this.Controls.Add(this.rbCairo);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.rbGDI);
			this.Controls.Add(this.rbDX);
			this.Controls.Add(this.rbSK);
			this.Controls.Add(this.rbSKGL);
			this.Controls.Add(this.lstResults);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnSettings);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Benchmarking App";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SkiaSharp.Views.Desktop.SKGLControl skglPnl;
		private SkiaSharp.Views.Desktop.SKControl skPnl;
		private Label label1;
		private TableLayoutPanel tableLayoutPanel1;
		private Button btnSettings;
		private Label label2;
		private ListBox lstResults;
		private RadioButton rbSKGL;
		private RadioButton rbSK;
		private RadioButton rbGDI;
		private Button btnStart;
		private Button btnStop;
		private D2DControl d2dPnl;
		private RadioButton rbDX;
		private DoubleBufferedControl gdiPnl;
		private DoubleBufferedControl CairoPnl;
		private RadioButton rbCairo;
	}
}
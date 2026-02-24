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
            skglPnl = new SkiaSharp.Views.Desktop.SKGLControl();
            skPnl = new SkiaSharp.Views.Desktop.SKControl();
            label1 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            d2dPnl = new D2DControl();
            gdiPnl = new DoubleBufferedControl();
            CairoPnl = new CairoControl();
            skiaD3dPnl = new SkiaDirect3DControl();
            skiaVulkanPnl = new SkiaVulkanControl();
            btnSettings = new Button();
            label2 = new Label();
            lstResults = new ListBox();
            rbSKGL = new RadioButton();
            rbSK = new RadioButton();
            rbGDI = new RadioButton();
            btnStart = new Button();
            btnStop = new Button();
            rbDX = new RadioButton();
            rbCairo = new RadioButton();
            rbSkiaD3D = new RadioButton();
            rbSkiaVulkan = new RadioButton();
            button1 = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // skglPnl
            // 
            skglPnl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            skglPnl.BackColor = Color.Black;
            skglPnl.Location = new Point(4, 3);
            skglPnl.Margin = new Padding(4, 3, 4, 3);
            skglPnl.Name = "skglPnl";
            skglPnl.Size = new Size(410, 56);
            skglPnl.TabIndex = 0;
            skglPnl.VSync = false;
            skglPnl.PaintSurface += skglControl1_PaintSurface;
            // 
            // skPnl
            // 
            skPnl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            skPnl.BackColor = Color.Black;
            skPnl.Location = new Point(3, 65);
            skPnl.Name = "skPnl";
            skPnl.Size = new Size(412, 56);
            skPnl.TabIndex = 2;
            skPnl.Text = "skControl1";
            skPnl.PaintSurface += skControl1_PaintSurface;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.Location = new Point(454, 11);
            label1.Name = "label1";
            label1.Size = new Size(118, 21);
            label1.TabIndex = 7;
            label1.Text = "Benchmarking:";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(skglPnl, 0, 0);
            tableLayoutPanel1.Controls.Add(skPnl, 0, 1);
            tableLayoutPanel1.Controls.Add(d2dPnl, 0, 3);
            tableLayoutPanel1.Controls.Add(gdiPnl, 0, 2);
            tableLayoutPanel1.Controls.Add(CairoPnl, 0, 4);
            tableLayoutPanel1.Controls.Add(skiaD3dPnl, 0, 5);
            tableLayoutPanel1.Controls.Add(skiaVulkanPnl, 0, 6);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.Size = new Size(418, 437);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // d2dPnl
            // 
            d2dPnl.BackColor = Color.Black;
            d2dPnl.Dock = DockStyle.Fill;
            d2dPnl.Location = new Point(3, 189);
            d2dPnl.Name = "d2dPnl";
            d2dPnl.Size = new Size(412, 56);
            d2dPnl.TabIndex = 7;
            d2dPnl.OnRendering += d2dPnl_OnRendering;
            // 
            // gdiPnl
            // 
            gdiPnl.BackColor = Color.Black;
            gdiPnl.Dock = DockStyle.Fill;
            gdiPnl.Location = new Point(3, 127);
            gdiPnl.Name = "gdiPnl";
            gdiPnl.Size = new Size(412, 56);
            gdiPnl.TabIndex = 8;
            gdiPnl.Paint += gControl1_Paint;
            // 
            // CairoPnl
            // 
            CairoPnl.BackColor = Color.Black;
            CairoPnl.Dock = DockStyle.Fill;
            CairoPnl.Location = new Point(3, 251);
            CairoPnl.Name = "CairoPnl";
            CairoPnl.Size = new Size(412, 56);
            CairoPnl.TabIndex = 9;
            CairoPnl.OnRendering += cairoPnl_OnRendering;
            // 
            // skiaD3dPnl
            // 
            skiaD3dPnl.BackColor = Color.Black;
            skiaD3dPnl.Dock = DockStyle.Fill;
            skiaD3dPnl.Location = new Point(3, 313);
            skiaD3dPnl.Name = "skiaD3dPnl";
            skiaD3dPnl.Size = new Size(412, 56);
            skiaD3dPnl.TabIndex = 10;
            skiaD3dPnl.OnRendering += skiaD3dPnl_OnRendering;
            // 
            // skiaVulkanPnl
            // 
            skiaVulkanPnl.BackColor = Color.Black;
            skiaVulkanPnl.Dock = DockStyle.Fill;
            skiaVulkanPnl.Location = new Point(3, 375);
            skiaVulkanPnl.Name = "skiaVulkanPnl";
            skiaVulkanPnl.Size = new Size(412, 59);
            skiaVulkanPnl.TabIndex = 11;
            skiaVulkanPnl.OnRendering += skiaVulkanPnl_OnRendering;
            // 
            // btnSettings
            // 
            btnSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSettings.Location = new Point(454, 426);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(318, 25);
            btnSettings.TabIndex = 6;
            btnSettings.Text = "Modify Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label2.Location = new Point(454, 98);
            label2.Name = "label2";
            label2.Size = new Size(66, 21);
            label2.TabIndex = 11;
            label2.Text = "Results:";
            // 
            // lstResults
            // 
            lstResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lstResults.Font = new Font("Segoe UI", 10F);
            lstResults.FormattingEnabled = true;
            lstResults.IntegralHeight = false;
            lstResults.Location = new Point(454, 122);
            lstResults.Name = "lstResults";
            lstResults.Size = new Size(318, 301);
            lstResults.TabIndex = 5;
            // 
            // rbSKGL
            // 
            rbSKGL.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbSKGL.AutoSize = true;
            rbSKGL.Checked = true;
            rbSKGL.Location = new Point(462, 41);
            rbSKGL.Name = "rbSKGL";
            rbSKGL.Size = new Size(103, 19);
            rbSKGL.TabIndex = 0;
            rbSKGL.TabStop = true;
            rbSKGL.Text = "Skia + OpenGL";
            rbSKGL.UseVisualStyleBackColor = true;
            // 
            // rbSK
            // 
            rbSK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbSK.AutoSize = true;
            rbSK.Location = new Point(571, 41);
            rbSK.Name = "rbSK";
            rbSK.Size = new Size(46, 19);
            rbSK.TabIndex = 1;
            rbSK.Text = "Skia";
            rbSK.UseVisualStyleBackColor = true;
            // 
            // rbGDI
            // 
            rbGDI.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbGDI.AutoSize = true;
            rbGDI.Location = new Point(623, 41);
            rbGDI.Name = "rbGDI";
            rbGDI.Size = new Size(110, 19);
            rbGDI.TabIndex = 2;
            rbGDI.Text = "System.Drawing";
            rbGDI.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            btnStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStart.Location = new Point(582, 94);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(50, 25);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStop.Enabled = false;
            btnStop.Location = new Point(638, 94);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(50, 25);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // rbDX
            // 
            rbDX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbDX.AutoSize = true;
            rbDX.Location = new Point(462, 66);
            rbDX.Name = "rbDX";
            rbDX.Size = new Size(63, 19);
            rbDX.TabIndex = 1;
            rbDX.Text = "DirectX";
            rbDX.UseVisualStyleBackColor = true;
            // 
            // rbCairo
            // 
            rbCairo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbCairo.AutoSize = true;
            rbCairo.Location = new Point(531, 66);
            rbCairo.Name = "rbCairo";
            rbCairo.Size = new Size(53, 19);
            rbCairo.TabIndex = 12;
            rbCairo.Text = "Cairo";
            rbCairo.UseVisualStyleBackColor = true;
            // 
            // rbSkiaD3D
            // 
            rbSkiaD3D.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbSkiaD3D.AutoSize = true;
            rbSkiaD3D.Location = new Point(590, 66);
            rbSkiaD3D.Name = "rbSkiaD3D";
            rbSkiaD3D.Size = new Size(82, 19);
            rbSkiaD3D.TabIndex = 14;
            rbSkiaD3D.Text = "Skia + D3D";
            rbSkiaD3D.UseVisualStyleBackColor = true;
            // 
            // rbSkiaVulkan
            // 
            rbSkiaVulkan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbSkiaVulkan.AutoSize = true;
            rbSkiaVulkan.Location = new Point(678, 66);
            rbSkiaVulkan.Name = "rbSkiaVulkan";
            rbSkiaVulkan.Size = new Size(96, 19);
            rbSkiaVulkan.TabIndex = 15;
            rbSkiaVulkan.Text = "Skia + Vulkan";
            rbSkiaVulkan.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(526, 94);
            button1.Name = "button1";
            button1.Size = new Size(50, 25);
            button1.TabIndex = 13;
            button1.Text = "Clear";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(button1);
            Controls.Add(rbSkiaVulkan);
            Controls.Add(rbSkiaD3D);
            Controls.Add(rbCairo);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(rbGDI);
            Controls.Add(rbDX);
            Controls.Add(rbSK);
            Controls.Add(rbSKGL);
            Controls.Add(lstResults);
            Controls.Add(label2);
            Controls.Add(btnSettings);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Benchmarking App";
            FormClosing += TestForm_FormClosing;
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
		private CairoControl CairoPnl;
		private RadioButton rbCairo;
		private Button button1;
		private SkiaDirect3DControl skiaD3dPnl;
		private SkiaVulkanControl skiaVulkanPnl;
		private RadioButton rbSkiaD3D;
		private RadioButton rbSkiaVulkan;
	}
}

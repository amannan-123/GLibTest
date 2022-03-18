namespace SkiaTest
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
			this.skglControl1 = new SkiaSharp.Views.Desktop.SKGLControl();
			this.button1 = new System.Windows.Forms.Button();
			this.skControl1 = new SkiaSharp.Views.Desktop.SKControl();
			this.label1 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.gControl1 = new SkiaTest.gControl();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			this.SuspendLayout();
			// 
			// skglControl1
			// 
			this.skglControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.skglControl1.BackColor = System.Drawing.Color.Black;
			this.skglControl1.Location = new System.Drawing.Point(12, 75);
			this.skglControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.skglControl1.Name = "skglControl1";
			this.skglControl1.Size = new System.Drawing.Size(776, 150);
			this.skglControl1.TabIndex = 0;
			this.skglControl1.VSync = true;
			this.skglControl1.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.skglControl1_PaintSurface);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(102, 25);
			this.button1.TabIndex = 1;
			this.button1.Text = "Skia + OpenGL";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// skControl1
			// 
			this.skControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.skControl1.BackColor = System.Drawing.Color.Black;
			this.skControl1.Location = new System.Drawing.Point(12, 231);
			this.skControl1.Name = "skControl1";
			this.skControl1.Size = new System.Drawing.Size(776, 150);
			this.skControl1.TabIndex = 2;
			this.skControl1.Text = "skControl1";
			this.skControl1.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.skControl1_PaintSurface);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(336, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(451, 56);
			this.label1.TabIndex = 4;
			this.label1.Text = "Results will display here!";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(120, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(102, 25);
			this.button2.TabIndex = 1;
			this.button2.Text = "Skia ";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(228, 12);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(102, 25);
			this.button3.TabIndex = 5;
			this.button3.Text = "System.Drawing";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// gControl1
			// 
			this.gControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gControl1.BackColor = System.Drawing.Color.Black;
			this.gControl1.Location = new System.Drawing.Point(12, 387);
			this.gControl1.Name = "gControl1";
			this.gControl1.Size = new System.Drawing.Size(776, 150);
			this.gControl1.TabIndex = 6;
			this.gControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.gControl1_Paint);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown1.Location = new System.Drawing.Point(56, 45);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(100, 23);
			this.numericUpDown1.TabIndex = 7;
			this.numericUpDown1.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 23);
			this.label2.TabIndex = 8;
			this.label2.Text = "Lines:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown2.Location = new System.Drawing.Point(228, 45);
			this.numericUpDown2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(102, 23);
			this.numericUpDown2.TabIndex = 7;
			this.numericUpDown2.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(178, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 23);
			this.label3.TabIndex = 8;
			this.label3.Text = "Runs:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 549);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.numericUpDown2);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.gControl1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.skControl1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.skglControl1);
			this.Name = "MainForm";
			this.Text = "Benchmarking App";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SkiaSharp.Views.Desktop.SKGLControl skglControl1;
		private Button button1;
		private SkiaSharp.Views.Desktop.SKControl skControl1;
		private Label label1;
		private Button button2;
		private Button button3;
		private gControl gControl1;
		private NumericUpDown numericUpDown1;
		private Label label2;
		private NumericUpDown numericUpDown2;
		private Label label3;
	}
}
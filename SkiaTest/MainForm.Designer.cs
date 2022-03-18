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
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.gControl1 = new SkiaTest.gControl();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSettings = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.lstResults = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// skglControl1
			// 
			this.skglControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.skglControl1.BackColor = System.Drawing.Color.Black;
			this.skglControl1.Location = new System.Drawing.Point(4, 3);
			this.skglControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.skglControl1.Name = "skglControl1";
			this.skglControl1.Size = new System.Drawing.Size(446, 139);
			this.skglControl1.TabIndex = 0;
			this.skglControl1.VSync = false;
			this.skglControl1.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.skglControl1_PaintSurface);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(472, 41);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(113, 25);
			this.button1.TabIndex = 0;
			this.button1.Text = "Skia + OpenGL";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// skControl1
			// 
			this.skControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.skControl1.BackColor = System.Drawing.Color.Black;
			this.skControl1.Location = new System.Drawing.Point(3, 148);
			this.skControl1.Name = "skControl1";
			this.skControl1.Size = new System.Drawing.Size(448, 139);
			this.skControl1.TabIndex = 2;
			this.skControl1.Text = "skControl1";
			this.skControl1.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.skControl1_PaintSurface);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(591, 41);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(74, 25);
			this.button2.TabIndex = 1;
			this.button2.Text = "Skia ";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(671, 41);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(101, 25);
			this.button3.TabIndex = 2;
			this.button3.Text = "System.Drawing";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// gControl1
			// 
			this.gControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gControl1.BackColor = System.Drawing.Color.Black;
			this.gControl1.Location = new System.Drawing.Point(3, 293);
			this.gControl1.Name = "gControl1";
			this.gControl1.Size = new System.Drawing.Size(448, 141);
			this.gControl1.TabIndex = 6;
			this.gControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.gControl1_Paint);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(472, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(153, 21);
			this.label1.TabIndex = 7;
			this.label1.Text = "Start Benchmarking";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.skglControl1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.skControl1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.gControl1, 0, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 437);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// btnSettings
			// 
			this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSettings.Location = new System.Drawing.Point(472, 424);
			this.btnSettings.Name = "btnSettings";
			this.btnSettings.Size = new System.Drawing.Size(300, 25);
			this.btnSettings.TabIndex = 4;
			this.btnSettings.Text = "Modify Settings";
			this.btnSettings.UseVisualStyleBackColor = true;
			this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label2.Location = new System.Drawing.Point(472, 83);
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
			this.lstResults.Location = new System.Drawing.Point(472, 107);
			this.lstResults.Name = "lstResults";
			this.lstResults.Size = new System.Drawing.Size(300, 311);
			this.lstResults.TabIndex = 3;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 461);
			this.Controls.Add(this.lstResults);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnSettings);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "Benchmarking App";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SkiaSharp.Views.Desktop.SKGLControl skglControl1;
		private Button button1;
		private SkiaSharp.Views.Desktop.SKControl skControl1;
		private Button button2;
		private Button button3;
		private gControl gControl1;
		private Label label1;
		private TableLayoutPanel tableLayoutPanel1;
		private Button btnSettings;
		private Label label2;
		private ListBox lstResults;
	}
}
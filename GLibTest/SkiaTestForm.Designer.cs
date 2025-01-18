namespace GLibTest
{
	partial class SkiaTestForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			skglPnl = new SkiaSharp.Views.Desktop.SKGLControl();
			SuspendLayout();
			// 
			// skglPnl
			// 
			skglPnl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			skglPnl.BackColor = Color.Black;
			skglPnl.Location = new Point(14, 14);
			skglPnl.Margin = new Padding(5);
			skglPnl.Name = "skglPnl";
			skglPnl.Size = new Size(650, 650);
			skglPnl.TabIndex = 1;
			skglPnl.VSync = false;
			skglPnl.PaintSurface += skglPnl_PaintSurface;
			// 
			// SkiaTestForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(678, 678);
			Controls.Add(skglPnl);
			Location = new Point(940, 150);
			Name = "SkiaTestForm";
			StartPosition = FormStartPosition.Manual;
			Text = "   ";
			Load += SkiaTestForm_Load;
			Click += SkiaTestForm_Click;
			ResumeLayout(false);
		}

		#endregion

		private SkiaSharp.Views.Desktop.SKGLControl skglPnl;
	}
}
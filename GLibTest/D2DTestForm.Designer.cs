namespace GLibTest
{
	partial class D2DTestForm
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
			d2dControl1 = new D2DControl();
			SuspendLayout();
			// 
			// d2dControl1
			// 
			d2dControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			d2dControl1.Location = new Point(14, 14);
			d2dControl1.Margin = new Padding(5);
			d2dControl1.Name = "d2dControl1";
			d2dControl1.Size = new Size(650, 650);
			d2dControl1.TabIndex = 0;
			d2dControl1.VSync = false;
			d2dControl1.OnRendering += d2dControl1_OnRendering;
			// 
			// D2DTestForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(678, 678);
			Controls.Add(d2dControl1);
			Location = new Point(230, 150);
			Name = "D2DTestForm";
			StartPosition = FormStartPosition.Manual;
			Text = "TestForm";
			Click += D2DTestForm_Click;
			ResumeLayout(false);
		}

		#endregion

		private D2DControl d2dControl1;
	}
}
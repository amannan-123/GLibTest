namespace GLibTest
{
	partial class TestForm
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
			cStrokeStyle = new ComboBox();
			label1 = new Label();
			SuspendLayout();
			// 
			// d2dControl1
			// 
			d2dControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			d2dControl1.Location = new Point(12, 38);
			d2dControl1.Name = "d2dControl1";
			d2dControl1.Size = new Size(660, 660);
			d2dControl1.TabIndex = 0;
			d2dControl1.VSync = false;
			d2dControl1.OnRendering += d2dControl1_OnRendering;
			// 
			// cStrokeStyle
			// 
			cStrokeStyle.FormattingEnabled = true;
			cStrokeStyle.Location = new Point(86, 9);
			cStrokeStyle.Name = "cStrokeStyle";
			cStrokeStyle.Size = new Size(121, 23);
			cStrokeStyle.TabIndex = 1;
			cStrokeStyle.SelectedIndexChanged += cStrokeStyle_SelectedIndexChanged;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(12, 12);
			label1.Name = "label1";
			label1.Size = new Size(68, 15);
			label1.TabIndex = 2;
			label1.Text = "Stroke Style";
			// 
			// TestForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(684, 710);
			Controls.Add(label1);
			Controls.Add(cStrokeStyle);
			Controls.Add(d2dControl1);
			Name = "TestForm";
			Text = "TestForm";
			Load += TestForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private D2DControl d2dControl1;
		private ComboBox cStrokeStyle;
		private Label label1;
	}
}
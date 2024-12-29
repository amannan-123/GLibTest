namespace GLibTest
{
	partial class SettingsDialog
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
			button1 = new Button();
			button2 = new Button();
			label1 = new Label();
			label2 = new Label();
			nRuns = new NumericUpDown();
			nLines = new NumericUpDown();
			label3 = new Label();
			label4 = new Label();
			btnVSync = new CheckBox();
			label5 = new Label();
			((System.ComponentModel.ISupportInitialize)nRuns).BeginInit();
			((System.ComponentModel.ISupportInitialize)nLines).BeginInit();
			SuspendLayout();
			// 
			// button1
			// 
			button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			button1.Location = new Point(176, 286);
			button1.Name = "button1";
			button1.Size = new Size(75, 23);
			button1.TabIndex = 1;
			button1.Text = "Cancel";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			button2.Location = new Point(257, 286);
			button2.Name = "button2";
			button2.Size = new Size(75, 23);
			button2.TabIndex = 0;
			button2.Text = "OK";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
			label1.Location = new Point(12, 9);
			label1.Name = "label1";
			label1.Size = new Size(49, 21);
			label1.TabIndex = 1;
			label1.Text = "Runs:";
			// 
			// label2
			// 
			label2.Location = new Point(12, 30);
			label2.Name = "label2";
			label2.Size = new Size(320, 30);
			label2.TabIndex = 2;
			label2.Text = "It determines how many times you want to render graphics.";
			// 
			// nRuns
			// 
			nRuns.Increment = new decimal(new int[] { 100, 0, 0, 0 });
			nRuns.Location = new Point(121, 63);
			nRuns.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			nRuns.Name = "nRuns";
			nRuns.Size = new Size(102, 23);
			nRuns.TabIndex = 2;
			nRuns.Value = new decimal(new int[] { 500, 0, 0, 0 });
			// 
			// nLines
			// 
			nLines.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
			nLines.Location = new Point(121, 153);
			nLines.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
			nLines.Name = "nLines";
			nLines.Size = new Size(102, 23);
			nLines.TabIndex = 3;
			nLines.Value = new decimal(new int[] { 2000, 0, 0, 0 });
			// 
			// label3
			// 
			label3.Location = new Point(12, 120);
			label3.Name = "label3";
			label3.Size = new Size(320, 30);
			label3.TabIndex = 12;
			label3.Text = "It determines how many lines you want to draw in a single run.";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
			label4.Location = new Point(12, 99);
			label4.Name = "label4";
			label4.Size = new Size(51, 21);
			label4.TabIndex = 11;
			label4.Text = "Lines:";
			// 
			// btnVSync
			// 
			btnVSync.AutoSize = true;
			btnVSync.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
			btnVSync.Location = new Point(12, 188);
			btnVSync.Name = "btnVSync";
			btnVSync.Size = new Size(261, 25);
			btnVSync.TabIndex = 4;
			btnVSync.Text = "VSync for OpenGL and Direct2D";
			btnVSync.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			label5.Location = new Point(12, 216);
			label5.Name = "label5";
			label5.Size = new Size(320, 50);
			label5.TabIndex = 14;
			label5.Text = "VSync aims to match the graphics processor's frames with the refresh rate of the monitor to fix any syncing issues. (Recommended when more objects are being drawn)";
			// 
			// SettingsDialog
			// 
			AcceptButton = button2;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = button1;
			ClientSize = new Size(344, 321);
			Controls.Add(label5);
			Controls.Add(btnVSync);
			Controls.Add(label3);
			Controls.Add(label4);
			Controls.Add(nRuns);
			Controls.Add(nLines);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(button2);
			Controls.Add(button1);
			Name = "SettingsDialog";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Settings";
			((System.ComponentModel.ISupportInitialize)nRuns).EndInit();
			((System.ComponentModel.ISupportInitialize)nLines).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button button1;
		private Button button2;
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label5;
		public NumericUpDown nRuns;
		public NumericUpDown nLines;
		public CheckBox btnVSync;
	}
}
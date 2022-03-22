namespace SkiaTest
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.nRuns = new System.Windows.Forms.NumericUpDown();
			this.nLines = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnVSync = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nRuns)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nLines)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(176, 286);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Cancel";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(257, 286);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 0;
			this.button2.Text = "OK";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 21);
			this.label1.TabIndex = 1;
			this.label1.Text = "Runs:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(320, 30);
			this.label2.TabIndex = 2;
			this.label2.Text = "It determines how many times you want to render graphics.";
			// 
			// nRuns
			// 
			this.nRuns.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nRuns.Location = new System.Drawing.Point(121, 63);
			this.nRuns.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nRuns.Name = "nRuns";
			this.nRuns.Size = new System.Drawing.Size(102, 23);
			this.nRuns.TabIndex = 2;
			this.nRuns.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			// 
			// nLines
			// 
			this.nLines.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nLines.Location = new System.Drawing.Point(121, 153);
			this.nLines.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
			this.nLines.Name = "nLines";
			this.nLines.Size = new System.Drawing.Size(102, 23);
			this.nLines.TabIndex = 3;
			this.nLines.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(320, 30);
			this.label3.TabIndex = 12;
			this.label3.Text = "It determines how many lines you want to draw in a single run.";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label4.Location = new System.Drawing.Point(12, 99);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(51, 21);
			this.label4.TabIndex = 11;
			this.label4.Text = "Lines:";
			// 
			// btnVSync
			// 
			this.btnVSync.AutoSize = true;
			this.btnVSync.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.btnVSync.Location = new System.Drawing.Point(12, 188);
			this.btnVSync.Name = "btnVSync";
			this.btnVSync.Size = new System.Drawing.Size(162, 25);
			this.btnVSync.TabIndex = 4;
			this.btnVSync.Text = "VSync for OpenGL";
			this.btnVSync.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 216);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(320, 50);
			this.label5.TabIndex = 14;
			this.label5.Text = "VSync aims to match the graphics processor\'s frames with the refresh rate of the " +
    "monitor to fix any syncing issues. (Recommended when more objects are being draw" +
    "n)";
			// 
			// SettingsDialog
			// 
			this.AcceptButton = this.button2;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button1;
			this.ClientSize = new System.Drawing.Size(344, 321);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnVSync);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.nRuns);
			this.Controls.Add(this.nLines);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "SettingsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			((System.ComponentModel.ISupportInitialize)(this.nRuns)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nLines)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
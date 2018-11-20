namespace WOT_Launcher
{
    partial class Processing
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
            this.BigLabel = new System.Windows.Forms.Label();
            this.MainBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // BigLabel
            // 
            this.BigLabel.AutoSize = true;
            this.BigLabel.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.BigLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BigLabel.Location = new System.Drawing.Point(25, 22);
            this.BigLabel.Name = "BigLabel";
            this.BigLabel.Size = new System.Drawing.Size(204, 61);
            this.BigLabel.TabIndex = 0;
            this.BigLabel.Text = "Applying";
            this.BigLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            // 
            // MainBar
            // 
            this.MainBar.Location = new System.Drawing.Point(37, 100);
            this.MainBar.Name = "MainBar";
            this.MainBar.Size = new System.Drawing.Size(560, 45);
            this.MainBar.TabIndex = 1;
            // 
            // Processing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(634, 185);
            this.Controls.Add(this.MainBar);
            this.Controls.Add(this.BigLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Processing";
            this.Text = "Processing";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BigLabel;
        private System.Windows.Forms.ProgressBar MainBar;
    }
}
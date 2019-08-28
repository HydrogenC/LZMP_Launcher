namespace LauncherUI
{
    partial class InstallWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallWizard));
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.ServerCheckBox = new System.Windows.Forms.CheckBox();
            this.ClientCheckBox = new System.Windows.Forms.CheckBox();
            this.InstallLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.TitleLabel.ForeColor = System.Drawing.Color.White;
            this.TitleLabel.Location = new System.Drawing.Point(23, 24);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(723, 86);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "LZMP Modpack Installer";
            this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.DescriptionLabel.ForeColor = System.Drawing.Color.White;
            this.DescriptionLabel.Location = new System.Drawing.Point(30, 122);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(860, 45);
            this.DescriptionLabel.TabIndex = 1;
            this.DescriptionLabel.Text = "Please choose the Minecraft instance(s) you wish to install: ";
            // 
            // ServerCheckBox
            // 
            this.ServerCheckBox.AutoSize = true;
            this.ServerCheckBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ServerCheckBox.ForeColor = System.Drawing.Color.White;
            this.ServerCheckBox.Location = new System.Drawing.Point(352, 183);
            this.ServerCheckBox.Name = "ServerCheckBox";
            this.ServerCheckBox.Size = new System.Drawing.Size(284, 49);
            this.ServerCheckBox.TabIndex = 21;
            this.ServerCheckBox.Text = "Minecraft Server";
            this.ServerCheckBox.UseVisualStyleBackColor = true;
            // 
            // ClientCheckBox
            // 
            this.ClientCheckBox.AutoSize = true;
            this.ClientCheckBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ClientCheckBox.ForeColor = System.Drawing.Color.White;
            this.ClientCheckBox.Location = new System.Drawing.Point(38, 183);
            this.ClientCheckBox.Name = "ClientCheckBox";
            this.ClientCheckBox.Size = new System.Drawing.Size(278, 49);
            this.ClientCheckBox.TabIndex = 20;
            this.ClientCheckBox.Text = "Minecraft Client";
            this.ClientCheckBox.UseVisualStyleBackColor = true;
            // 
            // InstallLabel
            // 
            this.InstallLabel.AutoSize = true;
            this.InstallLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.InstallLabel.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.InstallLabel.ForeColor = System.Drawing.Color.White;
            this.InstallLabel.Location = new System.Drawing.Point(711, 173);
            this.InstallLabel.Name = "InstallLabel";
            this.InstallLabel.Size = new System.Drawing.Size(135, 59);
            this.InstallLabel.TabIndex = 22;
            this.InstallLabel.Text = "Install";
            this.InstallLabel.Click += new System.EventHandler(this.InstallLabel_Click);
            // 
            // InstallWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(926, 260);
            this.Controls.Add(this.InstallLabel);
            this.Controls.Add(this.ServerCheckBox);
            this.Controls.Add(this.ClientCheckBox);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.TitleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InstallWizard";
            this.Text = "InstallWizard";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.CheckBox ServerCheckBox;
        private System.Windows.Forms.CheckBox ClientCheckBox;
        private System.Windows.Forms.Label InstallLabel;
    }
}
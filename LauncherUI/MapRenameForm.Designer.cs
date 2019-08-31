﻿namespace LauncherUI
{
    partial class MapRenameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapRenameForm));
            this.TitleLabel = new System.Windows.Forms.Label();
            this.FolderLabel = new System.Windows.Forms.Label();
            this.FolderBox = new System.Windows.Forms.TextBox();
            this.LevelBox = new System.Windows.Forms.TextBox();
            this.LevelLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.TitleLabel.ForeColor = System.Drawing.Color.White;
            this.TitleLabel.Location = new System.Drawing.Point(33, 20);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(409, 86);
            this.TitleLabel.TabIndex = 1;
            this.TitleLabel.Text = "Rename Map";
            // 
            // FolderLabel
            // 
            this.FolderLabel.AutoSize = true;
            this.FolderLabel.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.FolderLabel.ForeColor = System.Drawing.Color.White;
            this.FolderLabel.Location = new System.Drawing.Point(39, 135);
            this.FolderLabel.Name = "FolderLabel";
            this.FolderLabel.Size = new System.Drawing.Size(255, 51);
            this.FolderLabel.TabIndex = 2;
            this.FolderLabel.Text = "Folder Name: ";
            // 
            // FolderBox
            // 
            this.FolderBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.FolderBox.Location = new System.Drawing.Point(300, 136);
            this.FolderBox.Name = "FolderBox";
            this.FolderBox.Size = new System.Drawing.Size(402, 50);
            this.FolderBox.TabIndex = 3;
            // 
            // LevelBox
            // 
            this.LevelBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LevelBox.Location = new System.Drawing.Point(300, 212);
            this.LevelBox.Name = "LevelBox";
            this.LevelBox.Size = new System.Drawing.Size(402, 50);
            this.LevelBox.TabIndex = 5;
            // 
            // LevelLabel
            // 
            this.LevelLabel.AutoSize = true;
            this.LevelLabel.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.LevelLabel.ForeColor = System.Drawing.Color.White;
            this.LevelLabel.Location = new System.Drawing.Point(59, 212);
            this.LevelLabel.Name = "LevelLabel";
            this.LevelLabel.Size = new System.Drawing.Size(235, 51);
            this.LevelLabel.TabIndex = 4;
            this.LevelLabel.Text = "Level Name: ";
            // 
            // OKButton
            // 
            this.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OKButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.OKButton.ForeColor = System.Drawing.Color.White;
            this.OKButton.Location = new System.Drawing.Point(412, 292);
            this.OKButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(128, 66);
            this.OKButton.TabIndex = 10;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.CancelButton.ForeColor = System.Drawing.Color.White;
            this.CancelButton.Location = new System.Drawing.Point(574, 292);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(128, 66);
            this.CancelButton.TabIndex = 11;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // MapRenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(748, 390);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.LevelBox);
            this.Controls.Add(this.LevelLabel);
            this.Controls.Add(this.FolderBox);
            this.Controls.Add(this.FolderLabel);
            this.Controls.Add(this.TitleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MapRenameForm";
            this.Text = "MapRename";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label FolderLabel;
        private System.Windows.Forms.TextBox FolderBox;
        private System.Windows.Forms.TextBox LevelBox;
        private System.Windows.Forms.Label LevelLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
namespace LZMP_Launcher
{
    partial class SavesManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SavesManager));
            this.BigTitle = new System.Windows.Forms.Label();
            this.ExitForm = new System.Windows.Forms.Button();
            this.SavesList = new System.Windows.Forms.ListBox();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.XmlDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // BigTitle
            // 
            this.BigTitle.AutoSize = true;
            this.BigTitle.Font = new System.Drawing.Font("Segoe UI", 31.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BigTitle.ForeColor = System.Drawing.Color.White;
            this.BigTitle.Location = new System.Drawing.Point(22, 30);
            this.BigTitle.Name = "BigTitle";
            this.BigTitle.Size = new System.Drawing.Size(614, 113);
            this.BigTitle.TabIndex = 0;
            this.BigTitle.Text = "Saves Manager";
            this.BigTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SavesManager_MouseDown);
            // 
            // ExitForm
            // 
            this.ExitForm.FlatAppearance.BorderSize = 0;
            this.ExitForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitForm.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.ExitForm.ForeColor = System.Drawing.Color.White;
            this.ExitForm.Location = new System.Drawing.Point(784, 30);
            this.ExitForm.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ExitForm.Name = "ExitForm";
            this.ExitForm.Size = new System.Drawing.Size(70, 86);
            this.ExitForm.TabIndex = 8;
            this.ExitForm.Text = "X";
            this.MainToolTip.SetToolTip(this.ExitForm, "Close the current form");
            this.ExitForm.UseVisualStyleBackColor = true;
            this.ExitForm.Click += new System.EventHandler(this.ExitForm_Click);
            // 
            // SavesList
            // 
            this.SavesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.SavesList.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.SavesList.ForeColor = System.Drawing.Color.White;
            this.SavesList.FormattingEnabled = true;
            this.SavesList.ItemHeight = 45;
            this.SavesList.Location = new System.Drawing.Point(41, 186);
            this.SavesList.Name = "SavesList";
            this.SavesList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SavesList.Size = new System.Drawing.Size(405, 319);
            this.SavesList.Sorted = true;
            this.SavesList.TabIndex = 9;
            // 
            // ExportButton
            // 
            this.ExportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportButton.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.ExportButton.ForeColor = System.Drawing.Color.White;
            this.ExportButton.Location = new System.Drawing.Point(486, 186);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(368, 110);
            this.ExportButton.TabIndex = 10;
            this.ExportButton.Text = "Export Selected";
            this.MainToolTip.SetToolTip(this.ExportButton, "Export the selected map to a zip file");
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImportButton.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.ImportButton.ForeColor = System.Drawing.Color.White;
            this.ImportButton.Location = new System.Drawing.Point(486, 395);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(368, 110);
            this.ImportButton.TabIndex = 11;
            this.ImportButton.Text = "Import From Zip";
            this.MainToolTip.SetToolTip(this.ImportButton, "Import a zip-format map into the current LZMP copy");
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportDialog
            // 
            this.ExportDialog.Filter = "Zip File（*.zip）|*.zip";
            // 
            // OpenDialog
            // 
            this.OpenDialog.Filter = "Zip File（*.zip）|*.zip";
            // 
            // RefreshButton
            // 
            this.RefreshButton.FlatAppearance.BorderSize = 0;
            this.RefreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshButton.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.RefreshButton.ForeColor = System.Drawing.Color.White;
            this.RefreshButton.Location = new System.Drawing.Point(706, 30);
            this.RefreshButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(70, 86);
            this.RefreshButton.TabIndex = 12;
            this.RefreshButton.Text = "R";
            this.MainToolTip.SetToolTip(this.RefreshButton, "Refreshs the map list");
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // XmlDialog
            // 
            this.XmlDialog.Filter = "Xml File（*.xml）|*.xml";
            this.XmlDialog.Title = "Save the map\'s modset to";
            // 
            // SavesManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(886, 560);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.SavesList);
            this.Controls.Add(this.ExitForm);
            this.Controls.Add(this.BigTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SavesManager";
            this.Text = "SavesManager";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SavesManager_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BigTitle;
        private System.Windows.Forms.Button ExitForm;
        private System.Windows.Forms.ListBox SavesList;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.SaveFileDialog ExportDialog;
        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.SaveFileDialog XmlDialog;
        private System.Windows.Forms.ToolTip MainToolTip;
    }
}
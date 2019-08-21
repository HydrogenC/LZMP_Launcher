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
            this.label1 = new System.Windows.Forms.Label();
            this.ExitForm = new System.Windows.Forms.Button();
            this.SavesList = new System.Windows.Forms.ListBox();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 31.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(22, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(614, 113);
            this.label1.TabIndex = 0;
            this.label1.Text = "Saves Manager";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SavesManager_MouseDown);
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
            this.ExitForm.UseVisualStyleBackColor = true;
            this.ExitForm.Click += new System.EventHandler(this.ExitForm_Click);
            // 
            // SavesList
            // 
            this.SavesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.SavesList.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SavesList.ForeColor = System.Drawing.Color.White;
            this.SavesList.FormattingEnabled = true;
            this.SavesList.ItemHeight = 50;
            this.SavesList.Items.AddRange(new object[] {
            "Empty"});
            this.SavesList.Location = new System.Drawing.Point(41, 186);
            this.SavesList.Name = "SavesList";
            this.SavesList.Size = new System.Drawing.Size(377, 354);
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
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImportButton.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.ImportButton.ForeColor = System.Drawing.Color.White;
            this.ImportButton.Location = new System.Drawing.Point(486, 430);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(368, 110);
            this.ImportButton.TabIndex = 11;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            // 
            // SavesManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(894, 596);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.SavesList);
            this.Controls.Add(this.ExitForm);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SavesManager";
            this.Text = "SavesManager";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SavesManager_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ExitForm;
        private System.Windows.Forms.ListBox SavesList;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
    }
}
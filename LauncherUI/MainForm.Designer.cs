namespace LauncherUI
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SmallTitle = new System.Windows.Forms.Label();
            this.BigTitle = new System.Windows.Forms.Label();
            this.MainTree = new System.Windows.Forms.TreeView();
            this.LaunchClientButton = new System.Windows.Forms.Button();
            this.Apply = new System.Windows.Forms.Button();
            this.ToggleCheck = new System.Windows.Forms.Button();
            this.ReadSet = new System.Windows.Forms.Button();
            this.SaveSet = new System.Windows.Forms.Button();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ExitForm = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.LaunchServerButton = new System.Windows.Forms.Button();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.InitializeButton = new System.Windows.Forms.Button();
            this.CleanUpButton = new System.Windows.Forms.Button();
            this.OpenXmlDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveXmlDialog = new System.Windows.Forms.SaveFileDialog();
            this.ApplyLabel = new System.Windows.Forms.Label();
            this.ClientCheckBox = new System.Windows.Forms.CheckBox();
            this.ServerCheckBox = new System.Windows.Forms.CheckBox();
            this.InstanceLabel = new System.Windows.Forms.Label();
            this.ClientRadioButton = new System.Windows.Forms.RadioButton();
            this.ServerRadioButton = new System.Windows.Forms.RadioButton();
            this.SavesList = new System.Windows.Forms.ListBox();
            this.MapMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapLabel = new System.Windows.Forms.Label();
            this.ExportDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportDialog = new System.Windows.Forms.OpenFileDialog();
            this.deleteMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SmallTitle
            // 
            this.SmallTitle.AutoSize = true;
            this.SmallTitle.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.SmallTitle.ForeColor = System.Drawing.Color.White;
            this.SmallTitle.Location = new System.Drawing.Point(38, 42);
            this.SmallTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SmallTitle.Name = "SmallTitle";
            this.SmallTitle.Size = new System.Drawing.Size(283, 86);
            this.SmallTitle.TabIndex = 0;
            this.SmallTitle.Text = "ExMatics";
            this.SmallTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // BigTitle
            // 
            this.BigTitle.AutoSize = true;
            this.BigTitle.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BigTitle.ForeColor = System.Drawing.Color.White;
            this.BigTitle.Location = new System.Drawing.Point(34, 128);
            this.BigTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BigTitle.Name = "BigTitle";
            this.BigTitle.Size = new System.Drawing.Size(288, 114);
            this.BigTitle.TabIndex = 1;
            this.BigTitle.Text = "LZMP ";
            this.BigTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // MainTree
            // 
            this.MainTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.MainTree.CheckBoxes = true;
            this.MainTree.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainTree.ForeColor = System.Drawing.Color.White;
            this.MainTree.LineColor = System.Drawing.Color.Gainsboro;
            this.MainTree.Location = new System.Drawing.Point(50, 390);
            this.MainTree.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.MainTree.Name = "MainTree";
            this.MainTree.Size = new System.Drawing.Size(788, 687);
            this.MainTree.TabIndex = 3;
            this.MainTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.MainTree_AfterCheck);
            // 
            // LaunchClientButton
            // 
            this.LaunchClientButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LaunchClientButton.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LaunchClientButton.ForeColor = System.Drawing.Color.White;
            this.LaunchClientButton.Location = new System.Drawing.Point(50, 1106);
            this.LaunchClientButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.LaunchClientButton.Name = "LaunchClientButton";
            this.LaunchClientButton.Size = new System.Drawing.Size(240, 110);
            this.LaunchClientButton.TabIndex = 4;
            this.LaunchClientButton.Text = "Client";
            this.MainToolTip.SetToolTip(this.LaunchClientButton, "This will automaticly apply the current set. ");
            this.LaunchClientButton.UseVisualStyleBackColor = true;
            this.LaunchClientButton.Click += new System.EventHandler(this.LaunchClientButton_Click);
            // 
            // Apply
            // 
            this.Apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Apply.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Apply.ForeColor = System.Drawing.Color.White;
            this.Apply.Location = new System.Drawing.Point(598, 1106);
            this.Apply.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(240, 110);
            this.Apply.TabIndex = 5;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // ToggleCheck
            // 
            this.ToggleCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ToggleCheck.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.ToggleCheck.ForeColor = System.Drawing.Color.White;
            this.ToggleCheck.Location = new System.Drawing.Point(50, 258);
            this.ToggleCheck.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ToggleCheck.Name = "ToggleCheck";
            this.ToggleCheck.Size = new System.Drawing.Size(240, 110);
            this.ToggleCheck.TabIndex = 6;
            this.ToggleCheck.Text = "Check All";
            this.ToggleCheck.UseVisualStyleBackColor = true;
            this.ToggleCheck.Click += new System.EventHandler(this.ToggleCheck_Click);
            // 
            // ReadSet
            // 
            this.ReadSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReadSet.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.ReadSet.ForeColor = System.Drawing.Color.White;
            this.ReadSet.Location = new System.Drawing.Point(324, 258);
            this.ReadSet.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ReadSet.Name = "ReadSet";
            this.ReadSet.Size = new System.Drawing.Size(240, 110);
            this.ReadSet.TabIndex = 10;
            this.ReadSet.Text = "Read Set";
            this.MainToolTip.SetToolTip(this.ReadSet, "This will override the current set. ");
            this.ReadSet.UseVisualStyleBackColor = true;
            this.ReadSet.Click += new System.EventHandler(this.ReadSet_Click);
            // 
            // SaveSet
            // 
            this.SaveSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveSet.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.SaveSet.ForeColor = System.Drawing.Color.White;
            this.SaveSet.Location = new System.Drawing.Point(598, 258);
            this.SaveSet.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.SaveSet.Name = "SaveSet";
            this.SaveSet.Size = new System.Drawing.Size(240, 110);
            this.SaveSet.TabIndex = 9;
            this.SaveSet.Text = "Save Set";
            this.MainToolTip.SetToolTip(this.SaveSet, "This will automaticly apply the set. ");
            this.SaveSet.UseVisualStyleBackColor = true;
            this.SaveSet.Click += new System.EventHandler(this.SaveSet_Click);
            // 
            // ExitForm
            // 
            this.ExitForm.FlatAppearance.BorderSize = 0;
            this.ExitForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitForm.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.ExitForm.ForeColor = System.Drawing.Color.White;
            this.ExitForm.Location = new System.Drawing.Point(1216, 42);
            this.ExitForm.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ExitForm.Name = "ExitForm";
            this.ExitForm.Size = new System.Drawing.Size(70, 86);
            this.ExitForm.TabIndex = 7;
            this.ExitForm.Text = "X";
            this.MainToolTip.SetToolTip(this.ExitForm, "Closing the form won\'t apply the sets. ");
            this.ExitForm.UseVisualStyleBackColor = true;
            this.ExitForm.Click += new System.EventHandler(this.ExitForm_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImportButton.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.ImportButton.ForeColor = System.Drawing.Color.White;
            this.ImportButton.Location = new System.Drawing.Point(888, 1106);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(374, 110);
            this.ImportButton.TabIndex = 27;
            this.ImportButton.Text = "Import From Zip";
            this.MainToolTip.SetToolTip(this.ImportButton, "Import a zip-format map into the current LZMP copy");
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // LaunchServerButton
            // 
            this.LaunchServerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LaunchServerButton.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LaunchServerButton.ForeColor = System.Drawing.Color.White;
            this.LaunchServerButton.Location = new System.Drawing.Point(324, 1106);
            this.LaunchServerButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.LaunchServerButton.Name = "LaunchServerButton";
            this.LaunchServerButton.Size = new System.Drawing.Size(240, 110);
            this.LaunchServerButton.TabIndex = 29;
            this.LaunchServerButton.Text = "Server";
            this.MainToolTip.SetToolTip(this.LaunchServerButton, "This will automaticly apply the current set. ");
            this.LaunchServerButton.UseVisualStyleBackColor = true;
            this.LaunchServerButton.Click += new System.EventHandler(this.LaunchServerButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.FlatAppearance.BorderSize = 0;
            this.RefreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshButton.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.RefreshButton.ForeColor = System.Drawing.Color.White;
            this.RefreshButton.Location = new System.Drawing.Point(1060, 42);
            this.RefreshButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(70, 86);
            this.RefreshButton.TabIndex = 30;
            this.RefreshButton.Text = "R";
            this.MainToolTip.SetToolTip(this.RefreshButton, "Refresh the map list. ");
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // InitializeButton
            // 
            this.InitializeButton.FlatAppearance.BorderSize = 0;
            this.InitializeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InitializeButton.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.InitializeButton.ForeColor = System.Drawing.Color.White;
            this.InitializeButton.Location = new System.Drawing.Point(982, 42);
            this.InitializeButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.InitializeButton.Name = "InitializeButton";
            this.InitializeButton.Size = new System.Drawing.Size(70, 86);
            this.InitializeButton.TabIndex = 31;
            this.InitializeButton.Text = "I";
            this.MainToolTip.SetToolTip(this.InitializeButton, "Developer feature. ");
            this.InitializeButton.UseVisualStyleBackColor = true;
            this.InitializeButton.Click += new System.EventHandler(this.InitializeButton_Click);
            // 
            // CleanUpButton
            // 
            this.CleanUpButton.FlatAppearance.BorderSize = 0;
            this.CleanUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CleanUpButton.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.CleanUpButton.ForeColor = System.Drawing.Color.White;
            this.CleanUpButton.Location = new System.Drawing.Point(1138, 42);
            this.CleanUpButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.CleanUpButton.Name = "CleanUpButton";
            this.CleanUpButton.Size = new System.Drawing.Size(70, 86);
            this.CleanUpButton.TabIndex = 16;
            this.CleanUpButton.Text = "C";
            this.CleanUpButton.UseVisualStyleBackColor = true;
            this.CleanUpButton.Click += new System.EventHandler(this.CleanUpButton_Click);
            // 
            // OpenXmlDialog
            // 
            this.OpenXmlDialog.Filter = "Xml File（*.xml）|*.xml";
            // 
            // SaveXmlDialog
            // 
            this.SaveXmlDialog.Filter = "Xml File（*.xml）|*.xml";
            // 
            // ApplyLabel
            // 
            this.ApplyLabel.AutoSize = true;
            this.ApplyLabel.Font = new System.Drawing.Font("Segoe UI", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyLabel.ForeColor = System.Drawing.Color.White;
            this.ApplyLabel.Location = new System.Drawing.Point(878, 404);
            this.ApplyLabel.Name = "ApplyLabel";
            this.ApplyLabel.Size = new System.Drawing.Size(384, 59);
            this.ApplyLabel.TabIndex = 17;
            this.ApplyLabel.Text = "Apply Modset For: ";
            // 
            // ClientCheckBox
            // 
            this.ClientCheckBox.AutoSize = true;
            this.ClientCheckBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ClientCheckBox.ForeColor = System.Drawing.Color.White;
            this.ClientCheckBox.Location = new System.Drawing.Point(888, 466);
            this.ClientCheckBox.Name = "ClientCheckBox";
            this.ClientCheckBox.Size = new System.Drawing.Size(134, 49);
            this.ClientCheckBox.TabIndex = 18;
            this.ClientCheckBox.Text = "Client";
            this.ClientCheckBox.UseVisualStyleBackColor = true;
            // 
            // ServerCheckBox
            // 
            this.ServerCheckBox.AutoSize = true;
            this.ServerCheckBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ServerCheckBox.ForeColor = System.Drawing.Color.White;
            this.ServerCheckBox.Location = new System.Drawing.Point(1068, 465);
            this.ServerCheckBox.Name = "ServerCheckBox";
            this.ServerCheckBox.Size = new System.Drawing.Size(140, 49);
            this.ServerCheckBox.TabIndex = 19;
            this.ServerCheckBox.Text = "Server";
            this.ServerCheckBox.UseVisualStyleBackColor = true;
            // 
            // InstanceLabel
            // 
            this.InstanceLabel.AutoSize = true;
            this.InstanceLabel.Font = new System.Drawing.Font("Segoe UI", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstanceLabel.ForeColor = System.Drawing.Color.White;
            this.InstanceLabel.Location = new System.Drawing.Point(878, 257);
            this.InstanceLabel.Name = "InstanceLabel";
            this.InstanceLabel.Size = new System.Drawing.Size(408, 59);
            this.InstanceLabel.TabIndex = 20;
            this.InstanceLabel.Text = "Active MC Instance: ";
            // 
            // ClientRadioButton
            // 
            this.ClientRadioButton.AutoSize = true;
            this.ClientRadioButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ClientRadioButton.ForeColor = System.Drawing.Color.White;
            this.ClientRadioButton.Location = new System.Drawing.Point(888, 319);
            this.ClientRadioButton.Name = "ClientRadioButton";
            this.ClientRadioButton.Size = new System.Drawing.Size(133, 49);
            this.ClientRadioButton.TabIndex = 23;
            this.ClientRadioButton.TabStop = true;
            this.ClientRadioButton.Text = "Client";
            this.ClientRadioButton.UseVisualStyleBackColor = true;
            this.ClientRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // ServerRadioButton
            // 
            this.ServerRadioButton.AutoSize = true;
            this.ServerRadioButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ServerRadioButton.ForeColor = System.Drawing.Color.White;
            this.ServerRadioButton.Location = new System.Drawing.Point(1068, 319);
            this.ServerRadioButton.Name = "ServerRadioButton";
            this.ServerRadioButton.Size = new System.Drawing.Size(139, 49);
            this.ServerRadioButton.TabIndex = 24;
            this.ServerRadioButton.TabStop = true;
            this.ServerRadioButton.Text = "Server";
            this.ServerRadioButton.UseVisualStyleBackColor = true;
            this.ServerRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // SavesList
            // 
            this.SavesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.SavesList.ContextMenuStrip = this.MapMenu;
            this.SavesList.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.SavesList.ForeColor = System.Drawing.Color.White;
            this.SavesList.FormattingEnabled = true;
            this.SavesList.ItemHeight = 45;
            this.SavesList.Location = new System.Drawing.Point(888, 623);
            this.SavesList.Name = "SavesList";
            this.SavesList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SavesList.Size = new System.Drawing.Size(374, 454);
            this.SavesList.Sorted = true;
            this.SavesList.TabIndex = 25;
            // 
            // MapMenu
            // 
            this.MapMenu.BackColor = System.Drawing.Color.White;
            this.MapMenu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.MapMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MapMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.renameMapToolStripMenuItem,
            this.deleteMapToolStripMenuItem});
            this.MapMenu.Name = "MapMenu";
            this.MapMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MapMenu.Size = new System.Drawing.Size(301, 180);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(300, 44);
            this.exportToolStripMenuItem.Text = "Export Map";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItem_Click);
            // 
            // renameMapToolStripMenuItem
            // 
            this.renameMapToolStripMenuItem.Name = "renameMapToolStripMenuItem";
            this.renameMapToolStripMenuItem.Size = new System.Drawing.Size(300, 44);
            this.renameMapToolStripMenuItem.Text = "Rename Map";
            this.renameMapToolStripMenuItem.Click += new System.EventHandler(this.RenameMapToolStripMenuItem_Click);
            // 
            // MapLabel
            // 
            this.MapLabel.AutoSize = true;
            this.MapLabel.Font = new System.Drawing.Font("Segoe UI", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MapLabel.ForeColor = System.Drawing.Color.White;
            this.MapLabel.Location = new System.Drawing.Point(878, 550);
            this.MapLabel.Name = "MapLabel";
            this.MapLabel.Size = new System.Drawing.Size(207, 59);
            this.MapLabel.TabIndex = 28;
            this.MapLabel.Text = "Map List: ";
            // 
            // ExportDialog
            // 
            this.ExportDialog.Filter = "Zip File（*.zip）|*.zip";
            // 
            // ImportDialog
            // 
            this.ImportDialog.Filter = "Zip File（*.zip）|*.zip";
            // 
            // deleteMapToolStripMenuItem
            // 
            this.deleteMapToolStripMenuItem.Name = "deleteMapToolStripMenuItem";
            this.deleteMapToolStripMenuItem.Size = new System.Drawing.Size(300, 44);
            this.deleteMapToolStripMenuItem.Text = "Delete Map";
            this.deleteMapToolStripMenuItem.Click += new System.EventHandler(this.DeleteMapToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1322, 1252);
            this.Controls.Add(this.InitializeButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.LaunchServerButton);
            this.Controls.Add(this.MapLabel);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.SavesList);
            this.Controls.Add(this.ServerRadioButton);
            this.Controls.Add(this.ClientRadioButton);
            this.Controls.Add(this.InstanceLabel);
            this.Controls.Add(this.ServerCheckBox);
            this.Controls.Add(this.ClientCheckBox);
            this.Controls.Add(this.ApplyLabel);
            this.Controls.Add(this.CleanUpButton);
            this.Controls.Add(this.ReadSet);
            this.Controls.Add(this.SaveSet);
            this.Controls.Add(this.ExitForm);
            this.Controls.Add(this.ToggleCheck);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.LaunchClientButton);
            this.Controls.Add(this.MainTree);
            this.Controls.Add(this.BigTitle);
            this.Controls.Add(this.SmallTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.MapMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SmallTitle;
        private System.Windows.Forms.Label BigTitle;
        private System.Windows.Forms.TreeView MainTree;
        private System.Windows.Forms.Button LaunchClientButton;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Button ToggleCheck;
        private System.Windows.Forms.Button ReadSet;
        private System.Windows.Forms.Button SaveSet;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.OpenFileDialog OpenXmlDialog;
        private System.Windows.Forms.SaveFileDialog SaveXmlDialog;
        private System.Windows.Forms.Button CleanUpButton;
        private System.Windows.Forms.Button ExitForm;
        private System.Windows.Forms.Label ApplyLabel;
        private System.Windows.Forms.CheckBox ClientCheckBox;
        private System.Windows.Forms.CheckBox ServerCheckBox;
        private System.Windows.Forms.Label InstanceLabel;
        private System.Windows.Forms.RadioButton ClientRadioButton;
        private System.Windows.Forms.RadioButton ServerRadioButton;
        private System.Windows.Forms.ListBox SavesList;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Label MapLabel;
        private System.Windows.Forms.Button LaunchServerButton;
        private System.Windows.Forms.SaveFileDialog ExportDialog;
        private System.Windows.Forms.OpenFileDialog ImportDialog;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Button InitializeButton;
        private System.Windows.Forms.ContextMenuStrip MapMenu;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMapToolStripMenuItem;
    }
}

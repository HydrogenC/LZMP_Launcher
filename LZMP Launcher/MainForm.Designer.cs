namespace LZMP_Launcher
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
            this.LaunchClient = new System.Windows.Forms.Button();
            this.Apply = new System.Windows.Forms.Button();
            this.CheckAll = new System.Windows.Forms.Button();
            this.ExitForm = new System.Windows.Forms.Button();
            this.CancelAll = new System.Windows.Forms.Button();
            this.ReadSet = new System.Windows.Forms.Button();
            this.SaveSet = new System.Windows.Forms.Button();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LaunchServer = new System.Windows.Forms.Button();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.PlaceHolder = new System.Windows.Forms.Label();
            this.MainProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // SmallTitle
            // 
            this.SmallTitle.AutoSize = true;
            this.SmallTitle.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.SmallTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.SmallTitle.Location = new System.Drawing.Point(25, 26);
            this.SmallTitle.Name = "SmallTitle";
            this.SmallTitle.Size = new System.Drawing.Size(176, 54);
            this.SmallTitle.TabIndex = 0;
            this.SmallTitle.Text = "ExMatics";
            this.SmallTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            // 
            // BigTitle
            // 
            this.BigTitle.AutoSize = true;
            this.BigTitle.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BigTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BigTitle.Location = new System.Drawing.Point(23, 80);
            this.BigTitle.Name = "BigTitle";
            this.BigTitle.Size = new System.Drawing.Size(181, 72);
            this.BigTitle.TabIndex = 1;
            this.BigTitle.Text = "LZMP ";
            this.BigTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            // 
            // MainTree
            // 
            this.MainTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.MainTree.CheckBoxes = true;
            this.MainTree.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainTree.ForeColor = System.Drawing.Color.White;
            this.MainTree.LineColor = System.Drawing.Color.Gainsboro;
            this.MainTree.Location = new System.Drawing.Point(34, 334);
            this.MainTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MainTree.Name = "MainTree";
            this.MainTree.Size = new System.Drawing.Size(524, 431);
            this.MainTree.TabIndex = 3;
            this.MainTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.MainTree_AfterCheck);
            // 
            // LaunchClient
            // 
            this.LaunchClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LaunchClient.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LaunchClient.ForeColor = System.Drawing.Color.White;
            this.LaunchClient.Location = new System.Drawing.Point(34, 781);
            this.LaunchClient.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LaunchClient.Name = "LaunchClient";
            this.LaunchClient.Size = new System.Drawing.Size(168, 69);
            this.LaunchClient.TabIndex = 4;
            this.LaunchClient.Text = "Client";
            this.MainToolTip.SetToolTip(this.LaunchClient, "This will automaticly apply the current set. ");
            this.LaunchClient.UseVisualStyleBackColor = true;
            this.LaunchClient.Click += new System.EventHandler(this.LaunchClient_Click);
            // 
            // Apply
            // 
            this.Apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Apply.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Apply.ForeColor = System.Drawing.Color.White;
            this.Apply.Location = new System.Drawing.Point(390, 781);
            this.Apply.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(168, 69);
            this.Apply.TabIndex = 5;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // CheckAll
            // 
            this.CheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckAll.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckAll.ForeColor = System.Drawing.Color.White;
            this.CheckAll.Location = new System.Drawing.Point(34, 251);
            this.CheckAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CheckAll.Name = "CheckAll";
            this.CheckAll.Size = new System.Drawing.Size(245, 69);
            this.CheckAll.TabIndex = 6;
            this.CheckAll.Text = "Check All";
            this.CheckAll.UseVisualStyleBackColor = true;
            this.CheckAll.Click += new System.EventHandler(this.CheckAll_Click);
            // 
            // ExitForm
            // 
            this.ExitForm.FlatAppearance.BorderSize = 0;
            this.ExitForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitForm.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitForm.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ExitForm.Location = new System.Drawing.Point(510, 22);
            this.ExitForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExitForm.Name = "ExitForm";
            this.ExitForm.Size = new System.Drawing.Size(47, 54);
            this.ExitForm.TabIndex = 7;
            this.ExitForm.Text = "X";
            this.MainToolTip.SetToolTip(this.ExitForm, "Closing the form won\'t apply the sets. ");
            this.ExitForm.UseVisualStyleBackColor = true;
            this.ExitForm.Click += new System.EventHandler(this.ExitForm_Click);
            // 
            // CancelAll
            // 
            this.CancelAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelAll.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelAll.ForeColor = System.Drawing.Color.White;
            this.CancelAll.Location = new System.Drawing.Point(313, 251);
            this.CancelAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CancelAll.Name = "CancelAll";
            this.CancelAll.Size = new System.Drawing.Size(245, 69);
            this.CancelAll.TabIndex = 8;
            this.CancelAll.Text = "Cancel All";
            this.CancelAll.UseVisualStyleBackColor = true;
            this.CancelAll.Click += new System.EventHandler(this.CancelAll_Click);
            // 
            // ReadSet
            // 
            this.ReadSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReadSet.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadSet.ForeColor = System.Drawing.Color.White;
            this.ReadSet.Location = new System.Drawing.Point(313, 170);
            this.ReadSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ReadSet.Name = "ReadSet";
            this.ReadSet.Size = new System.Drawing.Size(245, 69);
            this.ReadSet.TabIndex = 10;
            this.ReadSet.Text = "Read Set";
            this.MainToolTip.SetToolTip(this.ReadSet, "This will override the current set. ");
            this.ReadSet.UseVisualStyleBackColor = true;
            this.ReadSet.Click += new System.EventHandler(this.ReadSet_Click);
            // 
            // SaveSet
            // 
            this.SaveSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveSet.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSet.ForeColor = System.Drawing.Color.White;
            this.SaveSet.Location = new System.Drawing.Point(34, 170);
            this.SaveSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SaveSet.Name = "SaveSet";
            this.SaveSet.Size = new System.Drawing.Size(245, 69);
            this.SaveSet.TabIndex = 9;
            this.SaveSet.Text = "Save Set";
            this.MainToolTip.SetToolTip(this.SaveSet, "This will automaticly apply the set. ");
            this.SaveSet.UseVisualStyleBackColor = true;
            this.SaveSet.Click += new System.EventHandler(this.SaveSet_Click);
            // 
            // LaunchServer
            // 
            this.LaunchServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LaunchServer.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LaunchServer.ForeColor = System.Drawing.Color.White;
            this.LaunchServer.Location = new System.Drawing.Point(212, 781);
            this.LaunchServer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LaunchServer.Name = "LaunchServer";
            this.LaunchServer.Size = new System.Drawing.Size(168, 69);
            this.LaunchServer.TabIndex = 14;
            this.LaunchServer.Text = "Server";
            this.MainToolTip.SetToolTip(this.LaunchServer, "This will automaticly apply the current set. ");
            this.LaunchServer.UseVisualStyleBackColor = true;
            this.LaunchServer.Click += new System.EventHandler(this.LaunchServer_Click);
            // 
            // FileDialog
            // 
            this.FileDialog.FileName = "set";
            this.FileDialog.Filter = "Set File（*.set）|*.set";
            // 
            // SaveDialog
            // 
            this.SaveDialog.Filter = "Set File（*.set）|*.set";
            // 
            // PlaceHolder
            // 
            this.PlaceHolder.AutoSize = true;
            this.PlaceHolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.PlaceHolder.Location = new System.Drawing.Point(535, 861);
            this.PlaceHolder.Name = "PlaceHolder";
            this.PlaceHolder.Size = new System.Drawing.Size(23, 15);
            this.PlaceHolder.TabIndex = 13;
            this.PlaceHolder.Text = "PH";
            // 
            // MainProgressBar
            // 
            this.MainProgressBar.Location = new System.Drawing.Point(34, 100);
            this.MainProgressBar.Maximum = 2000;
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(524, 47);
            this.MainProgressBar.TabIndex = 15;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(591, 874);
            this.Controls.Add(this.MainProgressBar);
            this.Controls.Add(this.LaunchServer);
            this.Controls.Add(this.PlaceHolder);
            this.Controls.Add(this.ReadSet);
            this.Controls.Add(this.SaveSet);
            this.Controls.Add(this.CancelAll);
            this.Controls.Add(this.ExitForm);
            this.Controls.Add(this.CheckAll);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.LaunchClient);
            this.Controls.Add(this.MainTree);
            this.Controls.Add(this.BigTitle);
            this.Controls.Add(this.SmallTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SmallTitle;
        private System.Windows.Forms.Label BigTitle;
        private System.Windows.Forms.TreeView MainTree;
        private System.Windows.Forms.Button LaunchClient;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Button CheckAll;
        private System.Windows.Forms.Button ExitForm;
        private System.Windows.Forms.Button CancelAll;
        private System.Windows.Forms.Button ReadSet;
        private System.Windows.Forms.Button SaveSet;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.Windows.Forms.SaveFileDialog SaveDialog;
        private System.Windows.Forms.Label PlaceHolder;
        private System.Windows.Forms.Button LaunchServer;
        private System.Windows.Forms.ProgressBar MainProgressBar;
    }
}


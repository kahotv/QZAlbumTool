﻿namespace QZAlbumTool
{
    partial class FormMain
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageLogin = new System.Windows.Forms.TabPage();
            this.tabPageAlbum = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDir = new System.Windows.Forms.CheckBox();
            this.cbUseSize = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSelectNot = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPageAlbum.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageLogin);
            this.tabControl1.Controls.Add(this.tabPageAlbum);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(492, 442);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControl1_Selected);
            // 
            // tabPageLogin
            // 
            this.tabPageLogin.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogin.Name = "tabPageLogin";
            this.tabPageLogin.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogin.Size = new System.Drawing.Size(484, 416);
            this.tabPageLogin.TabIndex = 0;
            this.tabPageLogin.Text = "1.登录";
            this.tabPageLogin.UseVisualStyleBackColor = true;
            // 
            // tabPageAlbum
            // 
            this.tabPageAlbum.Controls.Add(this.panel2);
            this.tabPageAlbum.Controls.Add(this.panel1);
            this.tabPageAlbum.Location = new System.Drawing.Point(4, 22);
            this.tabPageAlbum.Name = "tabPageAlbum";
            this.tabPageAlbum.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAlbum.Size = new System.Drawing.Size(484, 416);
            this.tabPageAlbum.TabIndex = 1;
            this.tabPageAlbum.Text = "2.相册";
            this.tabPageAlbum.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(478, 358);
            this.panel2.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(478, 358);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDir);
            this.panel1.Controls.Add(this.cbUseSize);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(478, 52);
            this.panel1.TabIndex = 0;
            // 
            // cbDir
            // 
            this.cbDir.AutoSize = true;
            this.cbDir.Checked = true;
            this.cbDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDir.Location = new System.Drawing.Point(7, 6);
            this.cbDir.Name = "cbDir";
            this.cbDir.Size = new System.Drawing.Size(108, 16);
            this.cbDir.TabIndex = 6;
            this.cbDir.Text = "以相册名做区分";
            this.cbDir.UseVisualStyleBackColor = true;
            // 
            // cbUseSize
            // 
            this.cbUseSize.AutoSize = true;
            this.cbUseSize.Checked = true;
            this.cbUseSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseSize.Location = new System.Drawing.Point(7, 27);
            this.cbUseSize.Name = "cbUseSize";
            this.cbUseSize.Size = new System.Drawing.Size(108, 16);
            this.cbUseSize.TabIndex = 5;
            this.cbUseSize.Text = "文件名包含尺寸";
            this.cbUseSize.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSelectAll);
            this.panel3.Controls.Add(this.btnExport);
            this.panel3.Controls.Add(this.btnSelectNot);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(172, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(306, 52);
            this.panel3.TabIndex = 3;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(27, 9);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 34);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.BtnSelectAll_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(226, 9);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 34);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "导出选中";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // btnSelectNot
            // 
            this.btnSelectNot.Location = new System.Drawing.Point(119, 9);
            this.btnSelectNot.Name = "btnSelectNot";
            this.btnSelectNot.Size = new System.Drawing.Size(75, 34);
            this.btnSelectNot.TabIndex = 1;
            this.btnSelectNot.Text = "反选";
            this.btnSelectNot.UseVisualStyleBackColor = true;
            this.btnSelectNot.Click += new System.EventHandler(this.BtnSelectNot_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(484, 416);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "3.关于";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Items.AddRange(new object[] {
            "版本 1.5",
            "时间 2025.04.07",
            "作者 Yui",
            "源码 https://github.com/kahotv/QZAlbumTool",
            "本次更新",
            "1. 修改：更新获取原图的方式。",
            "2. 修改：修复相册名没有处理特殊字符导致创建相册目录失败的bug。",
            "3. 修改：升级`CefShrp`库到134.3.9。",
            "4. 修改：升级`.net framework`到4.8。"});
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(478, 410);
            this.listBox1.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 442);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMain";
            this.Text = "QQ相册工具 1.5";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageAlbum.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageLogin;
        private System.Windows.Forms.TabPage tabPageAlbum;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnSelectNot;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox cbUseSize;
        private System.Windows.Forms.CheckBox cbDir;
    }
}


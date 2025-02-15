namespace CodeBuilder
{
    partial class frmPreBuild
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPreBuild));
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstPart = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.cboPath = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "部件名称";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 200;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(466, 476);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(552, 476);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(380, 476);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(80, 28);
            this.btnSelectPath.TabIndex = 14;
            this.btnSelectPath.Text = "浏览(&B)";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 480);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "输出目录:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 19);
            this.label1.TabIndex = 11;
            this.label1.Text = "生成部件:";
            // 
            // lstPart
            // 
            this.lstPart.AllowUpdateDataItem = false;
            this.lstPart.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstPart.BackColor = System.Drawing.SystemColors.Control;
            this.lstPart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstPart.CheckAllChecked = true;
            this.lstPart.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstPart.DataSource = null;
            this.lstPart.Footer = null;
            this.lstPart.FooterHeight = 28;
            this.lstPart.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstPart.HandCursor = false;
            this.lstPart.HeaderHeight = 28;
            this.lstPart.ItemHeight = 28;
            this.lstPart.Location = new System.Drawing.Point(13, 35);
            this.lstPart.Name = "lstPart";
            this.lstPart.NoneItemImage = null;
            this.lstPart.NoneItemText = "没有可显示的数据";
            this.lstPart.RowNumberIndex = 0;
            this.lstPart.ShowCheckAllBoxOnHeader = true;
            this.lstPart.ShowCheckBoxes = true;
            this.lstPart.ShowGridLines = false;
            this.lstPart.Size = new System.Drawing.Size(619, 428);
            this.lstPart.SortKey = null;
            this.lstPart.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstPart.TabIndex = 10;
            this.lstPart.BeforeCellEditing += new Fireasy.Windows.Forms.TreeListBeforeCellEditingEventHandler(this.lstPart_BeforeCellEditing);
            this.lstPart.BeforeCellUpdating += new Fireasy.Windows.Forms.TreeListBeforeCellUpdatingEventHandler(this.lstPart_BeforeCellUpdating);
            this.lstPart.AfterItemCheckChange += new Fireasy.Windows.Forms.TreeListItemAfterCheckedEventHandler(this.lstPart_AfterItemCheckChange);
            this.lstPart.CheckAllChanged += new Fireasy.Windows.Forms.TreeListCheckAllEventHandler(this.lstPart_CheckAllChanged);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Editable = true;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "输出文件";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 340;
            // 
            // cboPath
            // 
            this.cboPath.DropDownWidth = 500;
            this.cboPath.FormattingEnabled = true;
            this.cboPath.Location = new System.Drawing.Point(82, 477);
            this.cboPath.Name = "cboPath";
            this.cboPath.Size = new System.Drawing.Size(292, 27);
            this.cboPath.TabIndex = 15;
            this.cboPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboPath_KeyDown);
            this.cboPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboPath_KeyPress);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbReset,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(582, 10);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(49, 25);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbReset
            // 
            this.tlbReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbReset.Image = ((System.Drawing.Image)(resources.GetObject("tlbReset.Image")));
            this.tlbReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbReset.Name = "tlbReset";
            this.tlbReset.Size = new System.Drawing.Size(23, 22);
            this.tlbReset.Text = "重置";
            this.tlbReset.Click += new System.EventHandler(this.tlbReset_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "小贴士";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Info;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(138, 240);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(342, 34);
            this.panel3.TabIndex = 20;
            this.panel3.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(321, 19);
            this.label5.TabIndex = 0;
            this.label5.Text = "单击输出文件列进行编辑，可以使用变量或架构限定符";
            // 
            // frmPreBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(646, 515);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.cboPath);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstPart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPreBuild";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成配置";
            this.Load += new System.EventHandler(this.frmPreBuild_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Fireasy.Windows.Forms.TreeList lstPart;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.ComboBox cboPath;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripButton tlbReset;
    }
}
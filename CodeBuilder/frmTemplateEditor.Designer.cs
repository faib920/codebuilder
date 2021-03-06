namespace CodeBuilder
{
    partial class frmTemplateEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lstPart = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn3 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn4 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn5 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAddRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstRes = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn6 = new Fireasy.Windows.Forms.TreeListColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "标识:";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(75, 24);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(228, 21);
            this.txtId.TabIndex = 0;
            // 
            // lstPart
            // 
            this.lstPart.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstPart.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2,
            this.treeListColumn3,
            this.treeListColumn4,
            this.treeListColumn5});
            this.lstPart.ContextMenuStrip = this.contextMenuStrip1;
            this.lstPart.DataSource = null;
            this.lstPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPart.Footer = null;
            this.lstPart.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstPart.HandCursor = false;
            this.lstPart.Location = new System.Drawing.Point(3, 3);
            this.lstPart.Name = "lstPart";
            this.lstPart.NoneItemText = "右键弹出菜单点击“添加组”或“添加分部”";
            this.lstPart.RowNumberIndex = 0;
            this.lstPart.ShowPlusMinus = true;
            this.lstPart.ShowPlusMinusLines = false;
            this.lstPart.Size = new System.Drawing.Size(798, 439);
            this.lstPart.SortKey = null;
            this.lstPart.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstPart.TabIndex = 2;
            this.lstPart.BeforeCellEditing += new Fireasy.Windows.Forms.TreeListBeforeCellEditingEventHandler(this.lstPart_BeforeCellEditing);
            this.lstPart.AfterCellUpdated += new Fireasy.Windows.Forms.TreeListAfterCellUpdatedEventHandler(this.lstPart_AfterCellUpdated);
            this.lstPart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstPart_KeyDown);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Editable = true;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "名称";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 200;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Editable = true;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "文件";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 120;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.Editable = true;
            this.treeListColumn3.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.Formatter = null;
            this.treeListColumn3.Image = null;
            this.treeListColumn3.Text = "输出";
            this.treeListColumn3.Validator = null;
            this.treeListColumn3.Width = 200;
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn4.Editable = true;
            this.treeListColumn4.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn4.Formatter = null;
            this.treeListColumn4.Image = null;
            this.treeListColumn4.Text = "循环";
            this.treeListColumn4.Validator = null;
            // 
            // treeListColumn5
            // 
            this.treeListColumn5.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn5.Editable = true;
            this.treeListColumn5.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn5.Formatter = null;
            this.treeListColumn5.Image = null;
            this.treeListColumn5.Text = "语法";
            this.treeListColumn5.Validator = null;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddRoot,
            this.mnuAddGroup,
            this.mnuAdd,
            this.toolStripSeparator1,
            this.mnuDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(198, 114);
            // 
            // mnuAddRoot
            // 
            this.mnuAddRoot.Name = "mnuAddRoot";
            this.mnuAddRoot.Size = new System.Drawing.Size(197, 26);
            this.mnuAddRoot.Text = "添加顶级组";
            this.mnuAddRoot.Click += new System.EventHandler(this.mnuAddRoot_Click);
            // 
            // mnuAddGroup
            // 
            this.mnuAddGroup.Name = "mnuAddGroup";
            this.mnuAddGroup.Size = new System.Drawing.Size(197, 26);
            this.mnuAddGroup.Text = "添加组";
            this.mnuAddGroup.Click += new System.EventHandler(this.mnuAddGroup_Click);
            // 
            // mnuAdd
            // 
            this.mnuAdd.Name = "mnuAdd";
            this.mnuAdd.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.mnuAdd.Size = new System.Drawing.Size(197, 26);
            this.mnuAdd.Text = "添加分部";
            this.mnuAdd.Click += new System.EventHandler(this.mnuAdd_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(197, 26);
            this.mnuDelete.Text = "删除";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(692, 573);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(773, 573);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnLocation
            // 
            this.btnLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLocation.Location = new System.Drawing.Point(36, 573);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(119, 23);
            this.btnLocation.TabIndex = 7;
            this.btnLocation.Text = "定位到文件夹(&L)";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(75, 56);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(228, 21);
            this.txtName.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "名称:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(309, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "唯一标识，确定后不能修改";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(309, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "显示在菜单上";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(36, 93);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(812, 471);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lstPart);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(804, 445);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "分部";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstRes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(804, 445);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "资源";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstRes
            // 
            this.lstRes.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstRes.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn6});
            this.lstRes.DataSource = null;
            this.lstRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRes.Footer = null;
            this.lstRes.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstRes.HandCursor = false;
            this.lstRes.Location = new System.Drawing.Point(3, 3);
            this.lstRes.Name = "lstRes";
            this.lstRes.NoneItemText = "将资源拷贝到模板目录的下级目录\"Resources\"中";
            this.lstRes.RowNumberIndex = 0;
            this.lstRes.Size = new System.Drawing.Size(798, 439);
            this.lstRes.SortKey = null;
            this.lstRes.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstRes.TabIndex = 0;
            // 
            // treeListColumn6
            // 
            this.treeListColumn6.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn6.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn6.Formatter = null;
            this.treeListColumn6.Image = null;
            this.treeListColumn6.Text = "资源路径";
            this.treeListColumn6.Validator = null;
            this.treeListColumn6.Width = 500;
            // 
            // frmTemplateEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(867, 610);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(820, 510);
            this.Name = "frmTemplateEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模板编辑";
            this.Load += new System.EventHandler(this.frmTemplateEditor_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtId;
        private Fireasy.Windows.Forms.TreeList lstPart;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn3;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn4;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuAdd;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Fireasy.Windows.Forms.TreeList lstRes;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn6;
        private System.Windows.Forms.ToolStripMenuItem mnuAddGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuAddRoot;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
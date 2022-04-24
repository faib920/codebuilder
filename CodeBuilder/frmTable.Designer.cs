namespace CodeBuilder
{
    partial class frmTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTable));
            this.lstObject = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSelAllTable = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelInvTable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSelAllColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelInvColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstObject
            // 
            this.lstObject.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstObject.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstObject.ContextMenuStrip = this.contextMenuStrip1;
            this.lstObject.DataSource = null;
            this.lstObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstObject.Footer = null;
            this.lstObject.GroupFont = new System.Drawing.Font("宋体", 12F);
            this.lstObject.HandCursor = false;
            this.lstObject.Location = new System.Drawing.Point(2, 2);
            this.lstObject.Name = "lstObject";
            this.lstObject.NoneItemText = "请点击“数据源\"菜单，选择数据源";
            this.lstObject.RowNumberIndex = 0;
            this.lstObject.ShowCheckBoxes = true;
            this.lstObject.ShowPlusMinus = true;
            this.lstObject.ShowPlusMinusLines = false;
            this.lstObject.Size = new System.Drawing.Size(668, 405);
            this.lstObject.SortKey = null;
            this.lstObject.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstObject.TabIndex = 5;
            this.lstObject.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lstObject_ItemSelectionChanged);
            this.lstObject.AfterCellUpdated += new Fireasy.Windows.Forms.TreeListAfterCellUpdatedEventHandler(this.lstObject_AfterCellUpdated);
            this.lstObject.ItemCheckChanged += new Fireasy.Windows.Forms.TreeListItemCheckChangeEventHandler(this.lstObject_ItemCheckChanged);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
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
            this.treeListColumn2.Text = "备注";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSelAllTable,
            this.mnuSelInvTable,
            this.toolStripMenuItem1,
            this.mnuSelAllColumn,
            this.mnuSelInvColumn,
            this.toolStripMenuItem2,
            this.mnuBuild});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 146);
            // 
            // mnuSelAllTable
            // 
            this.mnuSelAllTable.Name = "mnuSelAllTable";
            this.mnuSelAllTable.Size = new System.Drawing.Size(160, 26);
            this.mnuSelAllTable.Text = "选择所有表";
            this.mnuSelAllTable.Click += new System.EventHandler(this.mnuSelAllTable_Click);
            // 
            // mnuSelInvTable
            // 
            this.mnuSelInvTable.Name = "mnuSelInvTable";
            this.mnuSelInvTable.Size = new System.Drawing.Size(160, 26);
            this.mnuSelInvTable.Text = "反向选择表";
            this.mnuSelInvTable.Click += new System.EventHandler(this.mnuSelInvTable_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuSelAllColumn
            // 
            this.mnuSelAllColumn.Name = "mnuSelAllColumn";
            this.mnuSelAllColumn.Size = new System.Drawing.Size(160, 26);
            this.mnuSelAllColumn.Text = "选择所有列";
            this.mnuSelAllColumn.Click += new System.EventHandler(this.mnuSelAllColumn_Click);
            // 
            // mnuSelInvColumn
            // 
            this.mnuSelInvColumn.Name = "mnuSelInvColumn";
            this.mnuSelInvColumn.Size = new System.Drawing.Size(160, 26);
            this.mnuSelInvColumn.Text = "反向选择列";
            this.mnuSelInvColumn.Click += new System.EventHandler(this.mnuSelInvColumn_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuBuild
            // 
            this.mnuBuild.Name = "mnuBuild";
            this.mnuBuild.Size = new System.Drawing.Size(160, 26);
            this.mnuBuild.Text = "生成预览";
            this.mnuBuild.Click += new System.EventHandler(this.mnuBuild_Click);
            // 
            // frmTable
            // 
            this.AllowEndUserDocking = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 409);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.lstObject);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTable";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "对象列表";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList lstObject;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuSelAllTable;
        private System.Windows.Forms.ToolStripMenuItem mnuSelInvTable;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuSelAllColumn;
        private System.Windows.Forms.ToolStripMenuItem mnuSelInvColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuBuild;
    }
}
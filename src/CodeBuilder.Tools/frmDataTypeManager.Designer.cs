namespace CodeBuilder.Tools
{
    partial class frmDataTypeManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataTypeManager));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.mnuDatabase = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuAddDb = new System.Windows.Forms.ToolStripButton();
            this.mnuDelDb = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSave = new System.Windows.Forms.ToolStripButton();
            this.mnuJson = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.lstDataType = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDatabase,
            this.mnuAddDb,
            this.mnuDelDb,
            this.toolStripSeparator1,
            this.mnuSave,
            this.mnuJson,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // mnuDatabase
            // 
            this.mnuDatabase.Image = ((System.Drawing.Image)(resources.GetObject("mnuDatabase.Image")));
            this.mnuDatabase.Name = "mnuDatabase";
            this.mnuDatabase.Size = new System.Drawing.Size(29, 22);
            // 
            // mnuAddDb
            // 
            this.mnuAddDb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuAddDb.Image = ((System.Drawing.Image)(resources.GetObject("mnuAddDb.Image")));
            this.mnuAddDb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuAddDb.Name = "mnuAddDb";
            this.mnuAddDb.Size = new System.Drawing.Size(23, 22);
            this.mnuAddDb.Text = "新增数据库配置";
            this.mnuAddDb.Click += new System.EventHandler(this.mnuAddDb_Click);
            // 
            // mnuDelDb
            // 
            this.mnuDelDb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuDelDb.Image = ((System.Drawing.Image)(resources.GetObject("mnuDelDb.Image")));
            this.mnuDelDb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuDelDb.Name = "mnuDelDb";
            this.mnuDelDb.Size = new System.Drawing.Size(23, 22);
            this.mnuDelDb.Text = "删除数据库配置";
            this.mnuDelDb.Click += new System.EventHandler(this.mnuDelDb_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // mnuSave
            // 
            this.mnuSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuSave.Image = ((System.Drawing.Image)(resources.GetObject("mnuSave.Image")));
            this.mnuSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(23, 22);
            this.mnuSave.Text = "保存修改";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuJson
            // 
            this.mnuJson.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuJson.Image = ((System.Drawing.Image)(resources.GetObject("mnuJson.Image")));
            this.mnuJson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuJson.Name = "mnuJson";
            this.mnuJson.Size = new System.Drawing.Size(23, 22);
            this.mnuJson.Text = "编辑配置文件";
            this.mnuJson.Click += new System.EventHandler(this.mnuJson_Click);
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
            // lstDataType
            // 
            this.lstDataType.AllowMoveNextItemOnEditing = true;
            this.lstDataType.AllowUpdateDataItem = false;
            this.lstDataType.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstDataType.BackColor = System.Drawing.SystemColors.Control;
            this.lstDataType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstDataType.CheckAllChecked = false;
            this.lstDataType.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstDataType.DataSource = null;
            this.lstDataType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDataType.Footer = null;
            this.lstDataType.FooterHeight = 28;
            this.lstDataType.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstDataType.HandCursor = false;
            this.lstDataType.HeaderHeight = 28;
            this.lstDataType.ItemHeight = 28;
            this.lstDataType.Location = new System.Drawing.Point(0, 25);
            this.lstDataType.Name = "lstDataType";
            this.lstDataType.NoneItemImage = null;
            this.lstDataType.NoneItemText = "没有可显示的数据";
            this.lstDataType.RowNumberIndex = 0;
            this.lstDataType.ShowPlusMinusLines = false;
            this.lstDataType.ShowRowNumber = true;
            this.lstDataType.Size = new System.Drawing.Size(800, 425);
            this.lstDataType.SortKey = null;
            this.lstDataType.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstDataType.TabIndex = 1;
            this.lstDataType.AfterCellUpdated += new Fireasy.Windows.Forms.TreeListAfterCellUpdatedEventHandler(this.lstDataType_AfterCellUpdated);
            this.lstDataType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstDataType_KeyUp);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Editable = true;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Sortable = false;
            this.treeListColumn1.Text = "数据类型";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 260;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Editable = true;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Sortable = false;
            this.treeListColumn2.Text = "DbType";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 150;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Info;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(224, 208);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(353, 34);
            this.panel3.TabIndex = 19;
            this.panel3.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(327, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "按【Delete】键删除当前行，按【F9】键设定/取消正则匹配";
            // 
            // frmDataTypeManager
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lstDataType);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmDataTypeManager";
            this.Text = "数据类型编辑器";
            this.Load += new System.EventHandler(this.frmDataTypeManager_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Fireasy.Windows.Forms.TreeList lstDataType;
        private System.Windows.Forms.ToolStripButton mnuAddDb;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.ToolStripButton mnuDelDb;
        private System.Windows.Forms.ToolStripDropDownButton mnuDatabase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton mnuSave;
        private System.Windows.Forms.ToolStripButton mnuJson;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
    }
}
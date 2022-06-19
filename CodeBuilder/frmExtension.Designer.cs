namespace CodeBuilder
{
    partial class frmExtension
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
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tlbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tlbHelp = new System.Windows.Forms.ToolStripButton();
            this.lstExt = new Fireasy.Windows.Forms.TreeList();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Spring = true;
            this.treeListColumn1.Text = "treeListColumn1";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 310;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbEdit,
            this.toolStripSeparator1,
            this.tlbRefresh,
            this.tlbHelp});
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(312, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbEdit
            // 
            this.tlbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbEdit.Image = global::CodeBuilder.Properties.Resources.edit;
            this.tlbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbEdit.Name = "tlbEdit";
            this.tlbEdit.Size = new System.Drawing.Size(23, 22);
            this.tlbEdit.Text = "编辑代码";
            this.tlbEdit.Click += new System.EventHandler(this.tlbEdit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tlbRefresh
            // 
            this.tlbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbRefresh.Image = global::CodeBuilder.Properties.Resources.refresh;
            this.tlbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbRefresh.Name = "tlbRefresh";
            this.tlbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tlbRefresh.Text = "刷新";
            this.tlbRefresh.Click += new System.EventHandler(this.tlbRefresh_Click);
            // 
            // tlbHelp
            // 
            this.tlbHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tlbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbHelp.Image = global::CodeBuilder.Properties.Resources.help;
            this.tlbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbHelp.Name = "tlbHelp";
            this.tlbHelp.Size = new System.Drawing.Size(23, 22);
            this.tlbHelp.Text = "帮助";
            this.tlbHelp.Click += new System.EventHandler(this.tlbHelp_Click);
            // 
            // lstExt
            // 
            this.lstExt.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstExt.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lstExt.DataSource = null;
            this.lstExt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstExt.Footer = null;
            this.lstExt.GroupFont = new System.Drawing.Font("宋体", 12F);
            this.lstExt.HandCursor = false;
            this.lstExt.Location = new System.Drawing.Point(2, 27);
            this.lstExt.Name = "lstExt";
            this.lstExt.NoneItemText = "请点击“模板\"菜单";
            this.lstExt.RowNumberIndex = 0;
            this.lstExt.ShowGridLines = false;
            this.lstExt.ShowHeader = false;
            this.lstExt.ShowPlusMinus = true;
            this.lstExt.Size = new System.Drawing.Size(312, 496);
            this.lstExt.SortKey = null;
            this.lstExt.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstExt.TabIndex = 10;
            this.lstExt.ItemDoubleClick += new Fireasy.Windows.Forms.TreeListItemDoubleClickEventHandler(this.lstExt_ItemDoubleClick);
            this.lstExt.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lstExt_ItemSelectionChanged);
            // 
            // frmExtension
            // 
            this.ClientSize = new System.Drawing.Size(316, 525);
            this.Controls.Add(this.lstExt);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmExtension";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "扩展";
            this.Load += new System.EventHandler(this.frmExtension_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlbEdit;
        private System.Windows.Forms.ToolStripButton tlbRefresh;
        private Fireasy.Windows.Forms.TreeList lstExt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tlbHelp;
    }
}
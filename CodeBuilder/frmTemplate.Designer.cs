namespace CodeBuilder
{
    partial class frmTemplate
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
            this.tlbNew = new System.Windows.Forms.ToolStripButton();
            this.tlbEditTemp = new System.Windows.Forms.ToolStripButton();
            this.tlbCopy = new System.Windows.Forms.ToolStripButton();
            this.tlbEditAsCode = new System.Windows.Forms.ToolStripButton();
            this.tlbExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tlbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tlbHelp = new System.Windows.Forms.ToolStripButton();
            this.lstPart = new Fireasy.Windows.Forms.TreeList();
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
            this.treeListColumn1.Text = "模板文件";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 274;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbEdit,
            this.toolStripSeparator1,
            this.tlbNew,
            this.tlbEditTemp,
            this.tlbCopy,
            this.tlbEditAsCode,
            this.tlbExport,
            this.toolStripSeparator2,
            this.tlbRefresh,
            this.tlbHelp});
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(276, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbEdit
            // 
            this.tlbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbEdit.Image = global::CodeBuilder.Properties.Resources.edit;
            this.tlbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbEdit.Name = "tlbEdit";
            this.tlbEdit.Size = new System.Drawing.Size(23, 22);
            this.tlbEdit.Text = "编辑模板";
            this.tlbEdit.Click += new System.EventHandler(this.tlbEdit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tlbNew
            // 
            this.tlbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbNew.Image = global::CodeBuilder.Properties.Resources.add;
            this.tlbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbNew.Name = "tlbNew";
            this.tlbNew.Size = new System.Drawing.Size(23, 22);
            this.tlbNew.Text = "新建模板";
            this.tlbNew.Click += new System.EventHandler(this.tlbNew_Click);
            // 
            // tlbEditTemp
            // 
            this.tlbEditTemp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbEditTemp.Image = global::CodeBuilder.Properties.Resources.edit1;
            this.tlbEditTemp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbEditTemp.Name = "tlbEditTemp";
            this.tlbEditTemp.Size = new System.Drawing.Size(23, 22);
            this.tlbEditTemp.Text = "编辑模板";
            this.tlbEditTemp.Click += new System.EventHandler(this.tlbEditTemp_Click);
            // 
            // tlbCopy
            // 
            this.tlbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbCopy.Image = global::CodeBuilder.Properties.Resources.copy;
            this.tlbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbCopy.Name = "tlbCopy";
            this.tlbCopy.Size = new System.Drawing.Size(23, 22);
            this.tlbCopy.Text = "复制模板";
            this.tlbCopy.Click += new System.EventHandler(this.tlbCopy_Click);
            // 
            // tlbEditAsCode
            // 
            this.tlbEditAsCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbEditAsCode.Image = global::CodeBuilder.Properties.Resources.edit04;
            this.tlbEditAsCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbEditAsCode.Name = "tlbEditAsCode";
            this.tlbEditAsCode.Size = new System.Drawing.Size(23, 22);
            this.tlbEditAsCode.Text = "编辑模板定义";
            this.tlbEditAsCode.Click += new System.EventHandler(this.tlbEditAsCode_Click);
            // 
            // tlbExport
            // 
            this.tlbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbExport.Image = global::CodeBuilder.Properties.Resources.export;
            this.tlbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbExport.Name = "tlbExport";
            this.tlbExport.Size = new System.Drawing.Size(23, 22);
            this.tlbExport.Text = "导出";
            this.tlbExport.Click += new System.EventHandler(this.tlbExport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // lstPart
            // 
            this.lstPart.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstPart.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lstPart.DataSource = null;
            this.lstPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPart.Footer = null;
            this.lstPart.GroupFont = new System.Drawing.Font("宋体", 12F);
            this.lstPart.HandCursor = false;
            this.lstPart.Location = new System.Drawing.Point(2, 27);
            this.lstPart.Name = "lstPart";
            this.lstPart.NoneItemText = "请点击“模板\"菜单";
            this.lstPart.RowNumberIndex = 0;
            this.lstPart.ShowGridLines = false;
            this.lstPart.ShowHeader = false;
            this.lstPart.ShowPlusMinus = true;
            this.lstPart.Size = new System.Drawing.Size(276, 322);
            this.lstPart.SortKey = null;
            this.lstPart.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstPart.TabIndex = 7;
            this.lstPart.ItemDoubleClick += new Fireasy.Windows.Forms.TreeListItemDoubleClickEventHandler(this.lstPart_ItemDoubleClick);
            this.lstPart.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lstPart_ItemSelectionChanged);
            // 
            // frmTemplate
            // 
            this.ClientSize = new System.Drawing.Size(280, 351);
            this.Controls.Add(this.lstPart);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmTemplate";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "模板";
            this.Load += new System.EventHandler(this.frmTemplate_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Fireasy.Windows.Forms.TreeList lstPart;
        private System.Windows.Forms.ToolStripButton tlbNew;
        private System.Windows.Forms.ToolStripButton tlbRefresh;
        private System.Windows.Forms.ToolStripButton tlbCopy;
        private System.Windows.Forms.ToolStripButton tlbEditAsCode;
        private System.Windows.Forms.ToolStripButton tlbEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tlbEditTemp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tlbHelp;
        private System.Windows.Forms.ToolStripButton tlbExport;
    }
}
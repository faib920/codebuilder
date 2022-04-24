namespace CodeBuilder.Tools
{
    partial class frmRemoter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRemoter));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbtnAdd = new System.Windows.Forms.ToolStripButton();
            this.tlbtnEdit = new System.Windows.Forms.ToolStripButton();
            this.tlbtnDelete = new System.Windows.Forms.ToolStripButton();
            this.lstItems = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbtnAdd,
            this.tlbtnEdit,
            this.tlbtnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(623, 28);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbtnAdd
            // 
            this.tlbtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("tlbtnAdd.Image")));
            this.tlbtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbtnAdd.Name = "tlbtnAdd";
            this.tlbtnAdd.Size = new System.Drawing.Size(62, 25);
            this.tlbtnAdd.Text = "添加";
            this.tlbtnAdd.Click += new System.EventHandler(this.tlbtnAdd_Click);
            // 
            // tlbtnEdit
            // 
            this.tlbtnEdit.Image = ((System.Drawing.Image)(resources.GetObject("tlbtnEdit.Image")));
            this.tlbtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbtnEdit.Name = "tlbtnEdit";
            this.tlbtnEdit.Size = new System.Drawing.Size(62, 25);
            this.tlbtnEdit.Text = "修改";
            this.tlbtnEdit.Click += new System.EventHandler(this.tlbtnEdit_Click);
            // 
            // tlbtnDelete
            // 
            this.tlbtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("tlbtnDelete.Image")));
            this.tlbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbtnDelete.Name = "tlbtnDelete";
            this.tlbtnDelete.Size = new System.Drawing.Size(62, 25);
            this.tlbtnDelete.Text = "删除";
            this.tlbtnDelete.Click += new System.EventHandler(this.tlbtnDelete_Click);
            // 
            // lstItems
            // 
            this.lstItems.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstItems.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstItems.DataSource = null;
            this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstItems.Footer = null;
            this.lstItems.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstItems.HandCursor = false;
            this.lstItems.ImageList = this.imageList1;
            this.lstItems.ItemHeight = 30;
            this.lstItems.Location = new System.Drawing.Point(0, 28);
            this.lstItems.Name = "lstItems";
            this.lstItems.NoneItemText = "没有可显示的数据";
            this.lstItems.RowNumberIndex = 0;
            this.lstItems.ShowGridLines = false;
            this.lstItems.Size = new System.Drawing.Size(623, 437);
            this.lstItems.SortKey = null;
            this.lstItems.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstItems.TabIndex = 1;
            this.lstItems.ItemDoubleClick += new Fireasy.Windows.Forms.TreeListItemDoubleClickEventHandler(this.lstItems_ItemDoubleClick);
            this.lstItems.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lstItems_ItemSelectionChanged);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "名称";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 300;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "主机";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 200;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Computer_16px_1194750_easyicon.net.png");
            // 
            // frmRemoter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 465);
            this.Controls.Add(this.lstItems);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmRemoter";
            this.Text = "远程连接";
            this.Load += new System.EventHandler(this.frmRemote_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Fireasy.Windows.Forms.TreeList lstItems;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton tlbtnAdd;
        private System.Windows.Forms.ToolStripButton tlbtnEdit;
        private System.Windows.Forms.ToolStripButton tlbtnDelete;
    }
}
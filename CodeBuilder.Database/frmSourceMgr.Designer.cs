namespace CodeBuilder.Database
{
    partial class frmSourceMgr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSourceMgr));
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbtnSelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbtnAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.tbtnEdit = new System.Windows.Forms.ToolStripButton();
            this.tbtnDelete = new System.Windows.Forms.ToolStripButton();
            this.tbtnTest = new System.Windows.Forms.ToolStripButton();
            this.lstProvider = new Fireasy.Windows.Forms.TreeList();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "名称";
            this.treeListColumn1.Width = 150;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "连接串";
            this.treeListColumn2.Width = 550;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbtnSelect,
            this.toolStripSeparator2,
            this.tbtnAdd,
            this.tbtnEdit,
            this.tbtnDelete,
            this.tbtnTest});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(796, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbtnSelect
            // 
            this.tbtnSelect.Enabled = false;
            this.tbtnSelect.Image = ((System.Drawing.Image)(resources.GetObject("tbtnSelect.Image")));
            this.tbtnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnSelect.Name = "tbtnSelect";
            this.tbtnSelect.Size = new System.Drawing.Size(52, 22);
            this.tbtnSelect.Text = "选择";
            this.tbtnSelect.Click += new System.EventHandler(this.tbtnSelect_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbtnAdd
            // 
            this.tbtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("tbtnAdd.Image")));
            this.tbtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnAdd.Name = "tbtnAdd";
            this.tbtnAdd.Size = new System.Drawing.Size(61, 22);
            this.tbtnAdd.Text = "添加";
            this.tbtnAdd.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbtnAdd_DropDownItemClicked);
            // 
            // tbtnEdit
            // 
            this.tbtnEdit.Enabled = false;
            this.tbtnEdit.Image = ((System.Drawing.Image)(resources.GetObject("tbtnEdit.Image")));
            this.tbtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnEdit.Name = "tbtnEdit";
            this.tbtnEdit.Size = new System.Drawing.Size(52, 22);
            this.tbtnEdit.Text = "修改";
            this.tbtnEdit.Click += new System.EventHandler(this.tbtnEdit_Click);
            // 
            // tbtnDelete
            // 
            this.tbtnDelete.Enabled = false;
            this.tbtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("tbtnDelete.Image")));
            this.tbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnDelete.Name = "tbtnDelete";
            this.tbtnDelete.Size = new System.Drawing.Size(52, 22);
            this.tbtnDelete.Text = "删除";
            this.tbtnDelete.Click += new System.EventHandler(this.tbtnDelete_Click);
            // 
            // tbtnTest
            // 
            this.tbtnTest.Enabled = false;
            this.tbtnTest.Image = ((System.Drawing.Image)(resources.GetObject("tbtnTest.Image")));
            this.tbtnTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnTest.Name = "tbtnTest";
            this.tbtnTest.Size = new System.Drawing.Size(52, 22);
            this.tbtnTest.Text = "测试";
            this.tbtnTest.Click += new System.EventHandler(this.tbtnTest_Click);
            // 
            // lstProvider
            // 
            this.lstProvider.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstProvider.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstProvider.Footer = null;
            this.lstProvider.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstProvider.HotTracking = true;
            this.lstProvider.ImageList = this.imageList1;
            this.lstProvider.Location = new System.Drawing.Point(0, 25);
            this.lstProvider.Name = "lstProvider";
            this.lstProvider.NoneItemText = "没有可显示的数据";
            this.lstProvider.RowNumberIndex = 0;
            this.lstProvider.ShowGridLines = false;
            this.lstProvider.Size = new System.Drawing.Size(796, 375);
            this.lstProvider.TabIndex = 1;
            this.lstProvider.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lstProvider_ItemSelectionChanged);
            this.lstProvider.DoubleClick += new System.EventHandler(this.lstProvider_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "database_16px_583331_easyicon.net.png");
            // 
            // frmSourceMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 400);
            this.Controls.Add(this.lstProvider);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSourceMgr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据源管理";
            this.Load += new System.EventHandler(this.frmSourceMgr_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Fireasy.Windows.Forms.TreeList lstProvider;
        private System.Windows.Forms.ToolStripButton tbtnEdit;
        private System.Windows.Forms.ToolStripButton tbtnDelete;
        private System.Windows.Forms.ToolStripButton tbtnTest;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tbtnSelect;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripDropDownButton tbtnAdd;

    }
}
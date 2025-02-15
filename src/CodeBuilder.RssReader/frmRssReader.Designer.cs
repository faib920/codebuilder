namespace CodeBuilder.RssReader
{
    partial class frmRssReader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRssReader));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbCategory = new System.Windows.Forms.ToolStripButton();
            this.tlbFav = new System.Windows.Forms.ToolStripButton();
            this.tlbRefresh = new System.Windows.Forms.ToolStripButton();
            this.lstItems = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlbOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbAddFav = new System.Windows.Forms.ToolStripMenuItem();
            this.lstFavItems = new Fireasy.Windows.Forms.TreeList();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlbOpen1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbMove = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbRemoveFav = new System.Windows.Forms.ToolStripMenuItem();
            this.lstCategory = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlbAddCat = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbAddRss = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbEditRss = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeListColumn3 = new Fireasy.Windows.Forms.TreeListColumn();
            this.lstFavCategory = new Fireasy.Windows.Forms.TreeList();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbCategory,
            this.tlbFav,
            this.tlbRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbCategory
            // 
            this.tlbCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbCategory.Image = ((System.Drawing.Image)(resources.GetObject("tlbCategory.Image")));
            this.tlbCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbCategory.Name = "tlbCategory";
            this.tlbCategory.Size = new System.Drawing.Size(23, 22);
            this.tlbCategory.Text = "栏目";
            this.tlbCategory.Click += new System.EventHandler(this.tlbCategory_Click);
            // 
            // tlbFav
            // 
            this.tlbFav.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbFav.Image = ((System.Drawing.Image)(resources.GetObject("tlbFav.Image")));
            this.tlbFav.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbFav.Name = "tlbFav";
            this.tlbFav.Size = new System.Drawing.Size(23, 22);
            this.tlbFav.Text = "收藏夹";
            this.tlbFav.Click += new System.EventHandler(this.tlbFav_Click);
            // 
            // tlbRefresh
            // 
            this.tlbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tlbRefresh.Image")));
            this.tlbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbRefresh.Name = "tlbRefresh";
            this.tlbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tlbRefresh.Text = "刷新";
            this.tlbRefresh.Click += new System.EventHandler(this.tlbRefresh_Click);
            // 
            // lstItems
            // 
            this.lstItems.AllowDrop = true;
            this.lstItems.AllowUpdateDataItem = false;
            this.lstItems.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstItems.BackColor = System.Drawing.SystemColors.Control;
            this.lstItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstItems.CheckAllChecked = false;
            this.lstItems.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lstItems.ContextMenuStrip = this.contextMenuStrip1;
            this.lstItems.DataSource = null;
            this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstItems.Footer = null;
            this.lstItems.FooterHeight = 28;
            this.lstItems.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstItems.HandCursor = false;
            this.lstItems.HeaderHeight = 28;
            this.lstItems.ItemHeight = 90;
            this.lstItems.Location = new System.Drawing.Point(160, 0);
            this.lstItems.Name = "lstItems";
            this.lstItems.NoneItemImage = null;
            this.lstItems.NoneItemText = "正在加载中，请稍候...";
            this.lstItems.RowNumberIndex = 0;
            this.lstItems.ShowHeader = false;
            this.lstItems.ShowPlusMinusLines = false;
            this.lstItems.Size = new System.Drawing.Size(640, 425);
            this.lstItems.SortKey = null;
            this.lstItems.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstItems.TabIndex = 3;
            this.lstItems.TabStop = false;
            this.lstItems.ItemDoubleClick += new Fireasy.Windows.Forms.TreeListItemDoubleClickEventHandler(this.lstItems_ItemDoubleClick);
            this.lstItems.CellMouseMove += new Fireasy.Windows.Forms.TreeListCellMouseMoveEventHandler(this.lstItems_CellMouseMove);
            this.lstItems.CellMouseLeave += new Fireasy.Windows.Forms.TreeListCellMouseLeaveEventHandler(this.lstItems_CellMouseLeave);
            this.lstItems.CellMouseUp += new Fireasy.Windows.Forms.TreeListCellMouseUpEventHandler(this.lstItems_CellMouseUp);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Spring = true;
            this.treeListColumn1.Text = "";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 638;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbOpen,
            this.tlbAddFav});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 52);
            // 
            // tlbOpen
            // 
            this.tlbOpen.Name = "tlbOpen";
            this.tlbOpen.Size = new System.Drawing.Size(138, 24);
            this.tlbOpen.Text = "打开网页";
            this.tlbOpen.Click += new System.EventHandler(this.tlbOpen_Click);
            // 
            // tlbAddFav
            // 
            this.tlbAddFav.Name = "tlbAddFav";
            this.tlbAddFav.Size = new System.Drawing.Size(138, 24);
            this.tlbAddFav.Text = "添加收藏";
            this.tlbAddFav.Click += new System.EventHandler(this.tlbAddFav_Click);
            // 
            // lstFavItems
            // 
            this.lstFavItems.AllowDrop = true;
            this.lstFavItems.AllowUpdateDataItem = false;
            this.lstFavItems.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstFavItems.BackColor = System.Drawing.SystemColors.Control;
            this.lstFavItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstFavItems.CheckAllChecked = false;
            this.lstFavItems.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lstFavItems.ContextMenuStrip = this.contextMenuStrip3;
            this.lstFavItems.DataSource = null;
            this.lstFavItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFavItems.Footer = null;
            this.lstFavItems.FooterHeight = 28;
            this.lstFavItems.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstFavItems.HandCursor = false;
            this.lstFavItems.HeaderHeight = 28;
            this.lstFavItems.ItemHeight = 90;
            this.lstFavItems.Location = new System.Drawing.Point(160, 0);
            this.lstFavItems.Name = "lstFavItems";
            this.lstFavItems.NoneItemImage = null;
            this.lstFavItems.NoneItemText = "正在加载中，请稍候...";
            this.lstFavItems.RowNumberIndex = 0;
            this.lstFavItems.ShowHeader = false;
            this.lstFavItems.ShowPlusMinusLines = false;
            this.lstFavItems.Size = new System.Drawing.Size(640, 425);
            this.lstFavItems.SortKey = null;
            this.lstFavItems.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstFavItems.TabIndex = 3;
            this.lstFavItems.TabStop = false;
            this.lstFavItems.ItemDoubleClick += new Fireasy.Windows.Forms.TreeListItemDoubleClickEventHandler(this.lstItems_ItemDoubleClick);
            this.lstFavItems.CellMouseMove += new Fireasy.Windows.Forms.TreeListCellMouseMoveEventHandler(this.lstItems_CellMouseMove);
            this.lstFavItems.CellMouseLeave += new Fireasy.Windows.Forms.TreeListCellMouseLeaveEventHandler(this.lstItems_CellMouseLeave);
            this.lstFavItems.CellMouseUp += new Fireasy.Windows.Forms.TreeListCellMouseUpEventHandler(this.lstItems_CellMouseUp);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbOpen1,
            this.tlbMove,
            this.tlbRemoveFav});
            this.contextMenuStrip3.Name = "contextMenuStrip1";
            this.contextMenuStrip3.Size = new System.Drawing.Size(139, 76);
            // 
            // tlbOpen1
            // 
            this.tlbOpen1.Name = "tlbOpen1";
            this.tlbOpen1.Size = new System.Drawing.Size(138, 24);
            this.tlbOpen1.Text = "打开网页";
            this.tlbOpen1.Click += new System.EventHandler(this.tlbOpen1_Click);
            // 
            // tlbMove
            // 
            this.tlbMove.Name = "tlbMove";
            this.tlbMove.Size = new System.Drawing.Size(138, 24);
            this.tlbMove.Text = "调整栏目";
            this.tlbMove.Click += new System.EventHandler(this.tlbMove_Click);
            // 
            // tlbRemoveFav
            // 
            this.tlbRemoveFav.Name = "tlbRemoveFav";
            this.tlbRemoveFav.Size = new System.Drawing.Size(138, 24);
            this.tlbRemoveFav.Text = "取消收藏";
            this.tlbRemoveFav.Click += new System.EventHandler(this.tlbRemoveFav_Click);
            // 
            // lstCategory
            // 
            this.lstCategory.AllowDragItem = true;
            this.lstCategory.AllowUpdateDataItem = false;
            this.lstCategory.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstCategory.BackColor = System.Drawing.SystemColors.Control;
            this.lstCategory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstCategory.CheckAllChecked = false;
            this.lstCategory.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn2});
            this.lstCategory.ContextMenuStrip = this.contextMenuStrip2;
            this.lstCategory.DataSource = null;
            this.lstCategory.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstCategory.Footer = null;
            this.lstCategory.FooterHeight = 28;
            this.lstCategory.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstCategory.HandCursor = false;
            this.lstCategory.HeaderHeight = 28;
            this.lstCategory.ImageList = this.imageList1;
            this.lstCategory.ItemHeight = 28;
            this.lstCategory.Location = new System.Drawing.Point(0, 0);
            this.lstCategory.Name = "lstCategory";
            this.lstCategory.NoneItemImage = null;
            this.lstCategory.NoneItemText = "没有可显示的数据";
            this.lstCategory.RowNumberIndex = 0;
            this.lstCategory.ShowGridLines = false;
            this.lstCategory.ShowHeader = false;
            this.lstCategory.ShowPlusMinus = true;
            this.lstCategory.Size = new System.Drawing.Size(157, 425);
            this.lstCategory.SortKey = null;
            this.lstCategory.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstCategory.TabIndex = 4;
            this.lstCategory.TabStop = false;
            this.lstCategory.Visible = false;
            this.lstCategory.ItemClick += new Fireasy.Windows.Forms.TreeListItemClickEventHandler(this.lstCategory_ItemClick);
            this.lstCategory.BeforeCellEditing += new Fireasy.Windows.Forms.TreeListBeforeCellEditingEventHandler(this.lstCategory_BeforeCellEditing);
            this.lstCategory.AfterCellEdited += new Fireasy.Windows.Forms.TreeListAfterCellEditedEventHandler(this.lstCategory_AfterCellEdited);
            this.lstCategory.AfterCellEditCanceled += new Fireasy.Windows.Forms.TreeListAfterCellEditCanceledEventHandler(this.lstCategory_AfterCellEditCanceled);
            this.lstCategory.ItemDragOver += new Fireasy.Windows.Forms.TreeListItemDragOverEventHandler(this.lstCategory_ItemDragOver);
            this.lstCategory.AfterItemDragDown += new Fireasy.Windows.Forms.TreeListAfterItemDragDownEventHandler(this.lstCategory_AfterItemDragDown);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Editable = true;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Spring = true;
            this.treeListColumn2.Text = "treeListColumn2";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 155;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbAddCat,
            this.tlbAddRss,
            this.tlbEditRss,
            this.tlbDelete});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(139, 100);
            // 
            // tlbAddCat
            // 
            this.tlbAddCat.Name = "tlbAddCat";
            this.tlbAddCat.Size = new System.Drawing.Size(138, 24);
            this.tlbAddCat.Text = "添加栏目";
            this.tlbAddCat.Click += new System.EventHandler(this.tlbAddCat_Click);
            // 
            // tlbAddRss
            // 
            this.tlbAddRss.Name = "tlbAddRss";
            this.tlbAddRss.Size = new System.Drawing.Size(138, 24);
            this.tlbAddRss.Text = "添加源...";
            this.tlbAddRss.Click += new System.EventHandler(this.tlbAddRss_Click);
            // 
            // tlbEditRss
            // 
            this.tlbEditRss.Name = "tlbEditRss";
            this.tlbEditRss.Size = new System.Drawing.Size(138, 24);
            this.tlbEditRss.Text = "修改源...";
            this.tlbEditRss.Click += new System.EventHandler(this.tlbEditRss_Click);
            // 
            // tlbDelete
            // 
            this.tlbDelete.Name = "tlbDelete";
            this.tlbDelete.Size = new System.Drawing.Size(138, 24);
            this.tlbDelete.Text = "删除";
            this.tlbDelete.Click += new System.EventHandler(this.tlbDelete_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "category.png");
            this.imageList1.Images.SetKeyName(1, "icons8-rss-16.png");
            this.imageList1.Images.SetKeyName(2, "icons8-rss-161.png");
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.Formatter = null;
            this.treeListColumn3.Image = null;
            this.treeListColumn3.Spring = true;
            this.treeListColumn3.Text = "treeListColumn3";
            this.treeListColumn3.Validator = null;
            this.treeListColumn3.Width = 155;
            // 
            // lstFavCategory
            // 
            this.lstFavCategory.AllowDragItem = true;
            this.lstFavCategory.AllowUpdateDataItem = false;
            this.lstFavCategory.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstFavCategory.BackColor = System.Drawing.SystemColors.Control;
            this.lstFavCategory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstFavCategory.CheckAllChecked = false;
            this.lstFavCategory.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn3});
            this.lstFavCategory.DataSource = null;
            this.lstFavCategory.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstFavCategory.Footer = null;
            this.lstFavCategory.FooterHeight = 28;
            this.lstFavCategory.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstFavCategory.HandCursor = false;
            this.lstFavCategory.HeaderHeight = 28;
            this.lstFavCategory.ImageList = this.imageList1;
            this.lstFavCategory.ItemHeight = 28;
            this.lstFavCategory.Location = new System.Drawing.Point(0, 0);
            this.lstFavCategory.Name = "lstFavCategory";
            this.lstFavCategory.NoneItemImage = null;
            this.lstFavCategory.NoneItemText = "没有可显示的数据";
            this.lstFavCategory.RowNumberIndex = 0;
            this.lstFavCategory.ShowGridLines = false;
            this.lstFavCategory.ShowHeader = false;
            this.lstFavCategory.ShowPlusMinus = true;
            this.lstFavCategory.Size = new System.Drawing.Size(157, 425);
            this.lstFavCategory.SortKey = null;
            this.lstFavCategory.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstFavCategory.TabIndex = 4;
            this.lstFavCategory.TabStop = false;
            this.lstFavCategory.Visible = false;
            this.lstFavCategory.ItemClick += new Fireasy.Windows.Forms.TreeListItemClickEventHandler(this.lstFavCategory_ItemClick);
            this.lstFavCategory.ItemDragOver += new Fireasy.Windows.Forms.TreeListItemDragOverEventHandler(this.lstFavCategory_ItemDragOver);
            this.lstFavCategory.AfterItemDragDown += new Fireasy.Windows.Forms.TreeListAfterItemDragDownEventHandler(this.lstFavCategory_AfterItemDragDown);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(157, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 425);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            this.splitter1.Visible = false;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(157, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 425);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            this.splitter2.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstItems);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.lstCategory);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 425);
            this.panel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lstFavItems);
            this.panel2.Controls.Add(this.splitter2);
            this.panel2.Controls.Add(this.lstFavCategory);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 425);
            this.panel2.TabIndex = 6;
            this.panel2.Visible = false;
            // 
            // frmRssReader
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRssReader";
            this.Text = "订阅";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmRssReader_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Fireasy.Windows.Forms.TreeList lstItems;
        private Fireasy.Windows.Forms.TreeList lstFavItems;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.ToolStripButton tlbRefresh;
        private Fireasy.Windows.Forms.TreeList lstCategory;
        private Fireasy.Windows.Forms.TreeList lstFavCategory;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton tlbCategory;
        private System.Windows.Forms.ToolStripButton tlbFav;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tlbOpen;
        private System.Windows.Forms.ToolStripMenuItem tlbAddFav;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem tlbAddCat;
        private System.Windows.Forms.ToolStripMenuItem tlbAddRss;
        private System.Windows.Forms.ToolStripMenuItem tlbDelete;
        private System.Windows.Forms.ToolStripMenuItem tlbEditRss;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem tlbOpen1;
        private System.Windows.Forms.ToolStripMenuItem tlbRemoveFav;
        private System.Windows.Forms.ToolStripMenuItem tlbMove;
    }
}
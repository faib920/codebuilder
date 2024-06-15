namespace CodeBuilder.RssReader
{
    partial class frmAddFavorite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddFavorite));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lstCategory = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlbAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "标题:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(77, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(382, 24);
            this.textBox1.TabIndex = 1;
            // 
            // lstCategory
            // 
            this.lstCategory.AllowDragItem = true;
            this.lstCategory.AllowUpdateDataItem = false;
            this.lstCategory.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstCategory.CheckAllChecked = false;
            this.lstCategory.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lstCategory.ContextMenuStrip = this.contextMenuStrip1;
            this.lstCategory.DataSource = null;
            this.lstCategory.Footer = null;
            this.lstCategory.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstCategory.HandCursor = false;
            this.lstCategory.ImageList = this.imageList1;
            this.lstCategory.Location = new System.Drawing.Point(77, 64);
            this.lstCategory.Name = "lstCategory";
            this.lstCategory.NoneItemImage = null;
            this.lstCategory.NoneItemText = "没有可显示的数据";
            this.lstCategory.RowNumberIndex = 0;
            this.lstCategory.ShowGridLines = false;
            this.lstCategory.ShowHeader = false;
            this.lstCategory.ShowPlusMinus = true;
            this.lstCategory.Size = new System.Drawing.Size(382, 151);
            this.lstCategory.SortKey = null;
            this.lstCategory.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstCategory.TabIndex = 5;
            this.lstCategory.TabStop = false;
            this.lstCategory.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lstCategory_ItemSelectionChanged);
            this.lstCategory.BeforeCellEditing += new Fireasy.Windows.Forms.TreeListBeforeCellEditingEventHandler(this.lstCategory_BeforeCellEditing);
            this.lstCategory.AfterCellEdited += new Fireasy.Windows.Forms.TreeListAfterCellEditedEventHandler(this.lstCategory_AfterCellEdited);
            this.lstCategory.AfterCellEditCanceled += new Fireasy.Windows.Forms.TreeListAfterCellEditCanceledEventHandler(this.lstCategory_AfterCellEditCanceled);
            this.lstCategory.ItemDragOver += new Fireasy.Windows.Forms.TreeListItemDragOverEventHandler(this.lstCategory_ItemDragOver);
            this.lstCategory.AfterItemDragDown += new Fireasy.Windows.Forms.TreeListAfterItemDragDownEventHandler(this.lstCategory_AfterItemDragDown);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Editable = true;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Spring = true;
            this.treeListColumn1.Text = "treeListColumn1";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 380;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbAdd,
            this.tlbRemove});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(135, 52);
            // 
            // tlbAdd
            // 
            this.tlbAdd.Name = "tlbAdd";
            this.tlbAdd.Size = new System.Drawing.Size(134, 24);
            this.tlbAdd.Text = "添加栏目";
            this.tlbAdd.Click += new System.EventHandler(this.tlbAdd_Click);
            // 
            // tlbRemove
            // 
            this.tlbRemove.Name = "tlbRemove";
            this.tlbRemove.Size = new System.Drawing.Size(134, 24);
            this.tlbRemove.Text = "删除栏目";
            this.tlbRemove.Click += new System.EventHandler(this.tlbRemove_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "category.png");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "栏目:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(482, 22);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 28);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(482, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmAddFavorite
            // 
            this.ClientSize = new System.Drawing.Size(580, 245);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstCategory);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddFavorite";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "收藏";
            this.Load += new System.EventHandler(this.frmAddFavorite_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private Fireasy.Windows.Forms.TreeList lstCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList imageList1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tlbAdd;
        private System.Windows.Forms.ToolStripMenuItem tlbRemove;
    }
}
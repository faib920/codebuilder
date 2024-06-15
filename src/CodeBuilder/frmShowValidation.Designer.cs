namespace CodeBuilder
{
    partial class frmShowValidation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowValidation));
            this.lstItems = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // lstItems
            // 
            this.lstItems.AllowUpdateDataItem = false;
            this.lstItems.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstItems.BackColor = System.Drawing.SystemColors.Control;
            this.lstItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstItems.CheckAllChecked = false;
            this.lstItems.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lstItems.DataSource = null;
            this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstItems.Footer = null;
            this.lstItems.FooterHeight = 28;
            this.lstItems.GroupFont = new System.Drawing.Font("宋体", 12F);
            this.lstItems.HandCursor = false;
            this.lstItems.HeaderHeight = 28;
            this.lstItems.ItemHeight = 28;
            this.lstItems.Location = new System.Drawing.Point(0, 0);
            this.lstItems.Name = "lstItems";
            this.lstItems.NoneItemImage = null;
            this.lstItems.NoneItemText = "没有可显示的数据";
            this.lstItems.RowNumberIndex = 0;
            this.lstItems.ShowAlternateBackColor = true;
            this.lstItems.ShowGridLines = false;
            this.lstItems.ShowHeader = false;
            this.lstItems.ShowPlusMinusLines = false;
            this.lstItems.Size = new System.Drawing.Size(631, 339);
            this.lstItems.SortKey = null;
            this.lstItems.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstItems.TabIndex = 6;
            this.lstItems.ItemClick += new Fireasy.Windows.Forms.TreeListItemClickEventHandler(this.lstItems_ItemClick);
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
            this.treeListColumn1.Width = 629;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "validate.png");
            // 
            // frmShowValidation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(631, 339);
            this.Controls.Add(this.lstItems);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShowValidation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "检验未通过";
            this.Load += new System.EventHandler(this.frmShowValidation_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Fireasy.Windows.Forms.TreeList lstItems;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.ImageList imageList1;
    }
}
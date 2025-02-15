
namespace CodeBuilder.Tools
{
    partial class frmTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTools));
            this.listView1 = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.mnuDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.AllowUpdateDataItem = false;
            this.listView1.AlternateBackColor = System.Drawing.Color.Empty;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView1.BackColor = System.Drawing.SystemColors.Control;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.CheckAllChecked = false;
            this.listView1.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.DataSource = null;
            this.listView1.Footer = null;
            this.listView1.FooterHeight = 28;
            this.listView1.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.listView1.HandCursor = false;
            this.listView1.HeaderHeight = 28;
            this.listView1.ImageList = this.imageList1;
            this.listView1.ItemHeight = 32;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.NoneItemImage = null;
            this.listView1.NoneItemText = "没有可显示的数据";
            this.listView1.RowNumberIndex = 0;
            this.listView1.ShowGridLines = false;
            this.listView1.ShowHeader = false;
            this.listView1.Size = new System.Drawing.Size(219, 478);
            this.listView1.SortKey = null;
            this.listView1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.listView1.TabIndex = 0;
            this.listView1.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
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
            this.treeListColumn1.Width = 217;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDefault});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 28);
            // 
            // mnuDefault
            // 
            this.mnuDefault.Name = "mnuDefault";
            this.mnuDefault.Size = new System.Drawing.Size(138, 24);
            this.mnuDefault.Text = "设为默认";
            this.mnuDefault.Click += new System.EventHandler(this.mnuDefault_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icons8-arrow-24.png");
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(237, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(554, 478);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(522, 68);
            this.label1.TabIndex = 0;
            this.label1.Text = "没有默认打开的工具，可以在左侧工具列表中选定工具条目并右键“设为默认”。";
            // 
            // frmTools
            // 
            this.ClientSize = new System.Drawing.Size(803, 502);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listView1);
            this.Name = "frmTools";
            this.Text = "常用工具合集";
            this.Load += new System.EventHandler(this.frmTools_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList listView1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuDefault;
        private System.Windows.Forms.Label label1;
    }
}
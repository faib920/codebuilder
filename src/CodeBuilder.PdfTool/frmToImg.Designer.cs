namespace CodeBuilder.PdfTool
{
    partial class frmToImg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmToImg));
            this.btnOpen = new System.Windows.Forms.Button();
            this.treeList1 = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn3 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn4 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnOption = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbImg3 = new System.Windows.Forms.RadioButton();
            this.rdbImg2 = new System.Windows.Forms.RadioButton();
            this.rdbImg1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbFile2 = new System.Windows.Forms.RadioButton();
            this.rdbFile1 = new System.Windows.Forms.RadioButton();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(4, 11);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(100, 28);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "打开文件(&O)";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // treeList1
            // 
            this.treeList1.AllowDrop = true;
            this.treeList1.AllowUpdateDataItem = false;
            this.treeList1.AlternateBackColor = System.Drawing.Color.Empty;
            this.treeList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeList1.BackColor = System.Drawing.SystemColors.Control;
            this.treeList1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeList1.CheckAllChecked = false;
            this.treeList1.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2,
            this.treeListColumn3,
            this.treeListColumn4});
            this.treeList1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeList1.DataSource = null;
            this.treeList1.Footer = null;
            this.treeList1.FooterHeight = 28;
            this.treeList1.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.treeList1.HandCursor = false;
            this.treeList1.HeaderHeight = 28;
            this.treeList1.ImageList = this.imageList1;
            this.treeList1.ItemHeight = 28;
            this.treeList1.Location = new System.Drawing.Point(0, 46);
            this.treeList1.Name = "treeList1";
            this.treeList1.NoneItemImage = null;
            this.treeList1.NoneItemText = "没有可显示的数据";
            this.treeList1.RowNumberIndex = 0;
            this.treeList1.ShowGridLines = false;
            this.treeList1.ShowPlusMinusLines = false;
            this.treeList1.Size = new System.Drawing.Size(1001, 493);
            this.treeList1.SortKey = null;
            this.treeList1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeList1.TabIndex = 2;
            this.treeList1.TabStop = false;
            this.treeList1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeList1_DragDrop);
            this.treeList1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeList1_DragEnter);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "源文件";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 300;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "目标文件";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 300;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.Formatter = null;
            this.treeListColumn3.Image = null;
            this.treeListColumn3.Text = "页数";
            this.treeListColumn3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.treeListColumn3.Validator = null;
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn4.DataFormat = "P0";
            this.treeListColumn4.DataType = Fireasy.Windows.Forms.TreeListCellDataType.Decimal;
            this.treeListColumn4.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn4.Formatter = null;
            this.treeListColumn4.Image = null;
            this.treeListColumn4.Text = "转换进度";
            this.treeListColumn4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.treeListColumn4.Validator = null;
            this.treeListColumn4.Width = 140;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRemove,
            this.mnuClear,
            this.mnuOpen});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 76);
            // 
            // mnuRemove
            // 
            this.mnuRemove.Name = "mnuRemove";
            this.mnuRemove.Size = new System.Drawing.Size(108, 24);
            this.mnuRemove.Text = "移除";
            this.mnuRemove.Click += new System.EventHandler(this.mnuRemove_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Name = "mnuClear";
            this.mnuClear.Size = new System.Drawing.Size(108, 24);
            this.mnuClear.Text = "清空";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(108, 24);
            this.mnuOpen.Text = "打开";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icons8-pdf-16.png");
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(110, 11);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(100, 28);
            this.btnConvert.TabIndex = 3;
            this.btnConvert.Text = "开始转换(&C)";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnOption
            // 
            this.btnOption.Location = new System.Drawing.Point(216, 11);
            this.btnOption.Name = "btnOption";
            this.btnOption.Size = new System.Drawing.Size(74, 28);
            this.btnOption.TabIndex = 4;
            this.btnOption.Text = "选项(&O)";
            this.btnOption.UseVisualStyleBackColor = true;
            this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(546, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(443, 222);
            this.panel1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdbImg3);
            this.groupBox2.Controls.Add(this.rdbImg2);
            this.groupBox2.Controls.Add(this.rdbImg1);
            this.groupBox2.Location = new System.Drawing.Point(12, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 56);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "图片格式";
            // 
            // rdbImg3
            // 
            this.rdbImg3.AutoSize = true;
            this.rdbImg3.Location = new System.Drawing.Point(150, 22);
            this.rdbImg3.Name = "rdbImg3";
            this.rdbImg3.Size = new System.Drawing.Size(45, 23);
            this.rdbImg3.TabIndex = 3;
            this.rdbImg3.Text = "Gif";
            this.rdbImg3.UseVisualStyleBackColor = true;
            this.rdbImg3.CheckedChanged += new System.EventHandler(this.rdbOption_CheckedChanged);
            // 
            // rdbImg2
            // 
            this.rdbImg2.AutoSize = true;
            this.rdbImg2.Location = new System.Drawing.Point(84, 22);
            this.rdbImg2.Name = "rdbImg2";
            this.rdbImg2.Size = new System.Drawing.Size(55, 23);
            this.rdbImg2.TabIndex = 2;
            this.rdbImg2.Text = "Jpeg";
            this.rdbImg2.UseVisualStyleBackColor = true;
            this.rdbImg2.CheckedChanged += new System.EventHandler(this.rdbOption_CheckedChanged);
            // 
            // rdbImg1
            // 
            this.rdbImg1.AutoSize = true;
            this.rdbImg1.Checked = true;
            this.rdbImg1.Location = new System.Drawing.Point(21, 22);
            this.rdbImg1.Name = "rdbImg1";
            this.rdbImg1.Size = new System.Drawing.Size(51, 23);
            this.rdbImg1.TabIndex = 1;
            this.rdbImg1.TabStop = true;
            this.rdbImg1.Text = "Png";
            this.rdbImg1.UseVisualStyleBackColor = true;
            this.rdbImg1.CheckedChanged += new System.EventHandler(this.rdbOption_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbFile2);
            this.groupBox1.Controls.Add(this.rdbFile1);
            this.groupBox1.Location = new System.Drawing.Point(12, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件名";
            // 
            // rdbFile2
            // 
            this.rdbFile2.AutoSize = true;
            this.rdbFile2.Checked = true;
            this.rdbFile2.Location = new System.Drawing.Point(156, 23);
            this.rdbFile2.Name = "rdbFile2";
            this.rdbFile2.Size = new System.Drawing.Size(92, 23);
            this.rdbFile2.TabIndex = 1;
            this.rdbFile2.TabStop = true;
            this.rdbFile2.Text = "创建子目录";
            this.rdbFile2.UseVisualStyleBackColor = true;
            this.rdbFile2.CheckedChanged += new System.EventHandler(this.rdbOption_CheckedChanged);
            // 
            // rdbFile1
            // 
            this.rdbFile1.AutoSize = true;
            this.rdbFile1.Location = new System.Drawing.Point(21, 23);
            this.rdbFile1.Name = "rdbFile1";
            this.rdbFile1.Size = new System.Drawing.Size(131, 23);
            this.rdbFile1.TabIndex = 0;
            this.rdbFile1.Text = "与源文件同一目录";
            this.rdbFile1.UseVisualStyleBackColor = true;
            this.rdbFile1.CheckedChanged += new System.EventHandler(this.rdbOption_CheckedChanged);
            // 
            // frmToImg
            // 
            this.ClientSize = new System.Drawing.Size(1001, 539);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOption);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.treeList1);
            this.Controls.Add(this.btnOpen);
            this.Name = "frmToImg";
            this.Text = "PDF 转图片";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private Fireasy.Windows.Forms.TreeList treeList1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn3;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn4;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnOption;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbFile2;
        private System.Windows.Forms.RadioButton rdbFile1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbImg2;
        private System.Windows.Forms.RadioButton rdbImg1;
        private System.Windows.Forms.RadioButton rdbImg3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuRemove;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuClear;
    }
}
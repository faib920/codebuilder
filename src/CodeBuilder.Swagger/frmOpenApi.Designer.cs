namespace CodeBuilder.Swagger
{
    partial class frmOpenApi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpenApi));
            this.label1 = new System.Windows.Forms.Label();
            this.cboUrl = new System.Windows.Forms.ComboBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lstTable = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeyword = new Fireasy.Windows.Forms.ComplexTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Json 文档地址：";
            // 
            // cboUrl
            // 
            this.cboUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboUrl.FormattingEnabled = true;
            this.cboUrl.Location = new System.Drawing.Point(110, 9);
            this.cboUrl.MaxDropDownItems = 10;
            this.cboUrl.Name = "cboUrl";
            this.cboUrl.Size = new System.Drawing.Size(487, 27);
            this.cboUrl.TabIndex = 1;
            this.cboUrl.SelectedIndexChanged += new System.EventHandler(this.cboUrl_SelectedIndexChanged);
            this.cboUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cboUrl_KeyUp);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(603, 8);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(80, 28);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "打开(&O)";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // lstTable
            // 
            this.lstTable.AllowUpdateDataItem = false;
            this.lstTable.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTable.BackColor = System.Drawing.SystemColors.Control;
            this.lstTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstTable.CheckAllChecked = false;
            this.lstTable.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstTable.DataSource = null;
            this.lstTable.Footer = null;
            this.lstTable.FooterHeight = 28;
            this.lstTable.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstTable.HandCursor = false;
            this.lstTable.HeaderHeight = 28;
            this.lstTable.ImageList = this.imageList1;
            this.lstTable.ItemHeight = 28;
            this.lstTable.Location = new System.Drawing.Point(16, 46);
            this.lstTable.Name = "lstTable";
            this.lstTable.NoneItemImage = null;
            this.lstTable.NoneItemText = "暂无数据，请输入Json文档地址";
            this.lstTable.RowNumberIndex = 0;
            this.lstTable.ShowCheckAllBoxOnHeader = true;
            this.lstTable.ShowCheckBoxes = true;
            this.lstTable.ShowPlusMinus = true;
            this.lstTable.ShowPlusMinusLines = false;
            this.lstTable.Size = new System.Drawing.Size(667, 440);
            this.lstTable.SortKey = null;
            this.lstTable.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstTable.TabIndex = 3;
            this.lstTable.AfterItemCheckChange += new Fireasy.Windows.Forms.TreeListItemAfterCheckedEventHandler(this.lstTable_AfterItemCheckChange);
            this.lstTable.CheckAllChanged += new Fireasy.Windows.Forms.TreeListCheckAllEventHandler(this.lstTable_CheckAllChanged);
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
            this.treeListColumn2.Text = "备注";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 240;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icons8-tag-16.png");
            this.imageList1.Images.SetKeyName(1, "model-select.png");
            this.imageList1.Images.SetKeyName(2, "swaggerLogo.png");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(603, 499);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(517, 499);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.Location = new System.Drawing.Point(288, 507);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 32;
            this.label3.Visible = false;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtKeyword.Location = new System.Drawing.Point(59, 501);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(246, 24);
            this.txtKeyword.TabIndex = 31;
            this.txtKeyword.WaterMarkText = "输入关键字或正则表达式敲回车键";
            this.txtKeyword.TextChanged += new System.EventHandler(this.txtKeyword_TextChanged);
            this.txtKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyDown);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 504);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 19);
            this.label2.TabIndex = 30;
            this.label2.Text = "筛选:";
            // 
            // frmOpenApi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(699, 539);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lstTable);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.cboUrl);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(715, 578);
            this.Name = "frmOpenApi";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Swagger 预览";
            this.Load += new System.EventHandler(this.frmOpenApi_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmOpenApi_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboUrl;
        private System.Windows.Forms.Button btnOpen;
        private Fireasy.Windows.Forms.TreeList lstTable;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label3;
        private Fireasy.Windows.Forms.ComplexTextBox txtKeyword;
        private System.Windows.Forms.Label label2;
    }
}
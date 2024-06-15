namespace CodeBuilder
{
    partial class frmGuide
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
            this.pnlSource = new System.Windows.Forms.Panel();
            this.btnSkip = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTemplate = new System.Windows.Forms.Panel();
            this.btnPrev2 = new System.Windows.Forms.Button();
            this.lstTemplate = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.btnNext2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlProfile = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbOpen = new System.Windows.Forms.ToolStripButton();
            this.btnPrev3 = new System.Windows.Forms.Button();
            this.pgrid = new System.Windows.Forms.PropertyGrid();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel3 = new System.Windows.Forms.Button();
            this.btnNext3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlPreBuild = new System.Windows.Forms.Panel();
            this.btnPrev4 = new System.Windows.Forms.Button();
            this.lstPart = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn3 = new Fireasy.Windows.Forms.TreeListColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancel4 = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlSource.SuspendLayout();
            this.pnlTemplate.SuspendLayout();
            this.pnlProfile.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlPreBuild.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSource
            // 
            this.pnlSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSource.Controls.Add(this.btnSkip);
            this.pnlSource.Controls.Add(this.label2);
            this.pnlSource.Controls.Add(this.btnCancel);
            this.pnlSource.Controls.Add(this.btnNext1);
            this.pnlSource.Controls.Add(this.label1);
            this.pnlSource.Location = new System.Drawing.Point(12, 13);
            this.pnlSource.Name = "pnlSource";
            this.pnlSource.Size = new System.Drawing.Size(664, 463);
            this.pnlSource.TabIndex = 0;
            this.pnlSource.Visible = false;
            // 
            // btnSkip
            // 
            this.btnSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSkip.Location = new System.Drawing.Point(409, 432);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(80, 28);
            this.btnSkip.TabIndex = 6;
            this.btnSkip.Text = "跳过(&K)";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Visible = false;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(16, 382);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(635, 41);
            this.label2.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(581, 432);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext1
            // 
            this.btnNext1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext1.Enabled = false;
            this.btnNext1.Location = new System.Drawing.Point(495, 432);
            this.btnNext1.Name = "btnNext1";
            this.btnNext1.Size = new System.Drawing.Size(80, 28);
            this.btnNext1.TabIndex = 1;
            this.btnNext1.Text = "下一步(&N)";
            this.btnNext1.UseVisualStyleBackColor = true;
            this.btnNext1.Click += new System.EventHandler(this.btnNext1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "1/4 选择数据源";
            // 
            // pnlTemplate
            // 
            this.pnlTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTemplate.Controls.Add(this.btnPrev2);
            this.pnlTemplate.Controls.Add(this.lstTemplate);
            this.pnlTemplate.Controls.Add(this.label3);
            this.pnlTemplate.Controls.Add(this.btnCancel2);
            this.pnlTemplate.Controls.Add(this.btnNext2);
            this.pnlTemplate.Controls.Add(this.label4);
            this.pnlTemplate.Location = new System.Drawing.Point(12, 13);
            this.pnlTemplate.Name = "pnlTemplate";
            this.pnlTemplate.Size = new System.Drawing.Size(664, 463);
            this.pnlTemplate.TabIndex = 1;
            this.pnlTemplate.Visible = false;
            // 
            // btnPrev2
            // 
            this.btnPrev2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev2.Location = new System.Drawing.Point(409, 432);
            this.btnPrev2.Name = "btnPrev2";
            this.btnPrev2.Size = new System.Drawing.Size(80, 28);
            this.btnPrev2.TabIndex = 1;
            this.btnPrev2.Text = "上一步(&P)";
            this.btnPrev2.UseVisualStyleBackColor = true;
            this.btnPrev2.Click += new System.EventHandler(this.btnPrev2_Click);
            // 
            // lstTemplate
            // 
            this.lstTemplate.AllowDemandLoadWhenScrollEnd = true;
            this.lstTemplate.AllowUpdateDataItem = false;
            this.lstTemplate.CheckAllChecked = false;
            this.lstTemplate.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lstTemplate.DataSource = null;
            this.lstTemplate.Footer = null;
            this.lstTemplate.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstTemplate.GroupHeight = 42;
            this.lstTemplate.HandCursor = false;
            this.lstTemplate.ItemHeight = 76;
            this.lstTemplate.Location = new System.Drawing.Point(3, 33);
            this.lstTemplate.Name = "lstTemplate";
            this.lstTemplate.NoneItemText = "没有可显示的数据";
            this.lstTemplate.RowNumberIndex = 0;
            this.lstTemplate.ShowGridLines = false;
            this.lstTemplate.ShowHeader = false;
            this.lstTemplate.Size = new System.Drawing.Size(658, 385);
            this.lstTemplate.SortKey = null;
            this.lstTemplate.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstTemplate.TabIndex = 0;
            this.lstTemplate.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.trlTemplate_ItemSelectionChanged);
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
            this.treeListColumn1.Width = 656;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(16, 382);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(635, 41);
            this.label3.TabIndex = 5;
            // 
            // btnCancel2
            // 
            this.btnCancel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel2.Location = new System.Drawing.Point(581, 432);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(80, 28);
            this.btnCancel2.TabIndex = 3;
            this.btnCancel2.Text = "取消(&C)";
            this.btnCancel2.UseVisualStyleBackColor = true;
            this.btnCancel2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext2
            // 
            this.btnNext2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext2.Enabled = false;
            this.btnNext2.Location = new System.Drawing.Point(495, 432);
            this.btnNext2.Name = "btnNext2";
            this.btnNext2.Size = new System.Drawing.Size(80, 28);
            this.btnNext2.TabIndex = 2;
            this.btnNext2.Text = "下一步(&N)";
            this.btnNext2.UseVisualStyleBackColor = true;
            this.btnNext2.Click += new System.EventHandler(this.btnNext2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F);
            this.label4.Location = new System.Drawing.Point(3, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "2/4 选择模板";
            // 
            // pnlProfile
            // 
            this.pnlProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProfile.Controls.Add(this.toolStrip1);
            this.pnlProfile.Controls.Add(this.btnPrev3);
            this.pnlProfile.Controls.Add(this.pgrid);
            this.pnlProfile.Controls.Add(this.label5);
            this.pnlProfile.Controls.Add(this.btnCancel3);
            this.pnlProfile.Controls.Add(this.btnNext3);
            this.pnlProfile.Controls.Add(this.label6);
            this.pnlProfile.Location = new System.Drawing.Point(12, 13);
            this.pnlProfile.Name = "pnlProfile";
            this.pnlProfile.Size = new System.Drawing.Size(664, 463);
            this.pnlProfile.TabIndex = 2;
            this.pnlProfile.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbOpen});
            this.toolStrip1.Location = new System.Drawing.Point(85, 34);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(26, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbOpen
            // 
            this.tlbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbOpen.Image = global::CodeBuilder.Properties.Resources.open;
            this.tlbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbOpen.Name = "tlbOpen";
            this.tlbOpen.Size = new System.Drawing.Size(23, 22);
            this.tlbOpen.Text = "打开外部文件";
            this.tlbOpen.Click += new System.EventHandler(this.tlbOpen_Click);
            // 
            // btnPrev3
            // 
            this.btnPrev3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev3.Location = new System.Drawing.Point(409, 432);
            this.btnPrev3.Name = "btnPrev3";
            this.btnPrev3.Size = new System.Drawing.Size(80, 28);
            this.btnPrev3.TabIndex = 1;
            this.btnPrev3.Text = "上一步(&P)";
            this.btnPrev3.UseVisualStyleBackColor = true;
            this.btnPrev3.Click += new System.EventHandler(this.btnPrev3_Click);
            // 
            // pgrid
            // 
            this.pgrid.Location = new System.Drawing.Point(3, 33);
            this.pgrid.Name = "pgrid";
            this.pgrid.Size = new System.Drawing.Size(658, 385);
            this.pgrid.TabIndex = 0;
            this.pgrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgrid_PropertyValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(16, 382);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(635, 41);
            this.label5.TabIndex = 5;
            // 
            // btnCancel3
            // 
            this.btnCancel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel3.Location = new System.Drawing.Point(581, 432);
            this.btnCancel3.Name = "btnCancel3";
            this.btnCancel3.Size = new System.Drawing.Size(80, 28);
            this.btnCancel3.TabIndex = 3;
            this.btnCancel3.Text = "取消(&C)";
            this.btnCancel3.UseVisualStyleBackColor = true;
            this.btnCancel3.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext3
            // 
            this.btnNext3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext3.Location = new System.Drawing.Point(495, 432);
            this.btnNext3.Name = "btnNext3";
            this.btnNext3.Size = new System.Drawing.Size(80, 28);
            this.btnNext3.TabIndex = 2;
            this.btnNext3.Text = "下一步(&N)";
            this.btnNext3.UseVisualStyleBackColor = true;
            this.btnNext3.Click += new System.EventHandler(this.btnNext3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F);
            this.label6.Location = new System.Drawing.Point(3, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 25);
            this.label6.TabIndex = 1;
            this.label6.Text = "3/4 设定变量";
            // 
            // pnlPreBuild
            // 
            this.pnlPreBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPreBuild.Controls.Add(this.btnPrev4);
            this.pnlPreBuild.Controls.Add(this.lstPart);
            this.pnlPreBuild.Controls.Add(this.label7);
            this.pnlPreBuild.Controls.Add(this.btnCancel4);
            this.pnlPreBuild.Controls.Add(this.btnOk);
            this.pnlPreBuild.Controls.Add(this.label8);
            this.pnlPreBuild.Location = new System.Drawing.Point(12, 13);
            this.pnlPreBuild.Name = "pnlPreBuild";
            this.pnlPreBuild.Size = new System.Drawing.Size(664, 463);
            this.pnlPreBuild.TabIndex = 3;
            this.pnlPreBuild.Visible = false;
            // 
            // btnPrev4
            // 
            this.btnPrev4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev4.Location = new System.Drawing.Point(409, 432);
            this.btnPrev4.Name = "btnPrev4";
            this.btnPrev4.Size = new System.Drawing.Size(80, 28);
            this.btnPrev4.TabIndex = 1;
            this.btnPrev4.Text = "上一步(&P)";
            this.btnPrev4.UseVisualStyleBackColor = true;
            this.btnPrev4.Click += new System.EventHandler(this.btnPrev4_Click);
            // 
            // lstPart
            // 
            this.lstPart.AllowDemandLoadWhenScrollEnd = true;
            this.lstPart.AllowUpdateDataItem = false;
            this.lstPart.CheckAllChecked = false;
            this.lstPart.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn2,
            this.treeListColumn3});
            this.lstPart.DataSource = null;
            this.lstPart.Footer = null;
            this.lstPart.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstPart.GroupHeight = 42;
            this.lstPart.HandCursor = false;
            this.lstPart.Location = new System.Drawing.Point(3, 33);
            this.lstPart.Name = "lstPart";
            this.lstPart.NoneItemText = "没有可显示的数据";
            this.lstPart.RowNumberIndex = 0;
            this.lstPart.ShowCheckAllBoxOnHeader = true;
            this.lstPart.ShowCheckBoxes = true;
            this.lstPart.ShowGridLines = false;
            this.lstPart.Size = new System.Drawing.Size(658, 385);
            this.lstPart.SortKey = null;
            this.lstPart.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstPart.TabIndex = 0;
            this.lstPart.AfterItemCheckChange += new Fireasy.Windows.Forms.TreeListItemAfterCheckedEventHandler(this.lstPart_AfterItemCheckChange);
            this.lstPart.CheckAllChanged += new Fireasy.Windows.Forms.TreeListCheckAllEventHandler(this.lstPart_CheckAllChanged);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "部件名称";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 200;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.Formatter = null;
            this.treeListColumn3.Image = null;
            this.treeListColumn3.Text = "输出文件";
            this.treeListColumn3.Validator = null;
            this.treeListColumn3.Width = 340;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(16, 382);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(635, 41);
            this.label7.TabIndex = 5;
            // 
            // btnCancel4
            // 
            this.btnCancel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel4.Location = new System.Drawing.Point(581, 432);
            this.btnCancel4.Name = "btnCancel4";
            this.btnCancel4.Size = new System.Drawing.Size(80, 28);
            this.btnCancel4.TabIndex = 3;
            this.btnCancel4.Text = "取消(&C)";
            this.btnCancel4.UseVisualStyleBackColor = true;
            this.btnCancel4.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(495, 432);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "生成(&G)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F);
            this.label8.Location = new System.Drawing.Point(3, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(162, 25);
            this.label8.TabIndex = 1;
            this.label8.Text = "4/4 选择生成部件";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Info;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label9);
            this.panel3.Location = new System.Drawing.Point(229, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(444, 30);
            this.panel3.TabIndex = 18;
            this.panel3.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(438, 19);
            this.label9.TabIndex = 0;
            this.label9.Text = "向导只在首次运行时自动弹出，以后可从【帮助】菜单中找到【使用向导】";
            // 
            // frmGuide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(688, 490);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnlTemplate);
            this.Controls.Add(this.pnlSource);
            this.Controls.Add(this.pnlProfile);
            this.Controls.Add(this.pnlPreBuild);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGuide";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "使用向导";
            this.Load += new System.EventHandler(this.frmGuide_Load);
            this.pnlSource.ResumeLayout(false);
            this.pnlSource.PerformLayout();
            this.pnlTemplate.ResumeLayout(false);
            this.pnlTemplate.PerformLayout();
            this.pnlProfile.ResumeLayout(false);
            this.pnlProfile.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlPreBuild.ResumeLayout(false);
            this.pnlPreBuild.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlTemplate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel2;
        private System.Windows.Forms.Button btnNext2;
        private System.Windows.Forms.Label label4;
        private Fireasy.Windows.Forms.TreeList lstTemplate;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Panel pnlProfile;
        private System.Windows.Forms.PropertyGrid pgrid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel3;
        private System.Windows.Forms.Button btnNext3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnPrev3;
        private System.Windows.Forms.Button btnPrev2;
        private System.Windows.Forms.Panel pnlPreBuild;
        private System.Windows.Forms.Button btnPrev4;
        private Fireasy.Windows.Forms.TreeList lstPart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCancel4;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label8;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlbOpen;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
    }
}
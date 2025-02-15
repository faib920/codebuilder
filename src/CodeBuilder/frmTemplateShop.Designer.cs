
namespace CodeBuilder
{
    partial class frmTemplateShop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTemplateShop));
            this.lvwTemplate = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.txtKeyword = new Fireasy.Windows.Forms.ComplexTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tlbDown = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbUse = new System.Windows.Forms.ToolStripMenuItem();
            this.tlbPub = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwTemplate
            // 
            this.lvwTemplate.AllowDemandLoadWhenScrollEnd = true;
            this.lvwTemplate.AllowUpdateDataItem = false;
            this.lvwTemplate.AlternateBackColor = System.Drawing.Color.Empty;
            this.lvwTemplate.BackColor = System.Drawing.SystemColors.Control;
            this.lvwTemplate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwTemplate.CheckAllChecked = false;
            this.lvwTemplate.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lvwTemplate.DataSource = null;
            this.lvwTemplate.Footer = null;
            this.lvwTemplate.FooterHeight = 28;
            this.lvwTemplate.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lvwTemplate.HandCursor = false;
            this.lvwTemplate.HeaderHeight = 28;
            this.lvwTemplate.ItemHeight = 100;
            this.lvwTemplate.Location = new System.Drawing.Point(12, 48);
            this.lvwTemplate.Name = "lvwTemplate";
            this.lvwTemplate.NoneItemImage = null;
            this.lvwTemplate.NoneItemText = "没有可显示的数据";
            this.lvwTemplate.RowNumberIndex = 0;
            this.lvwTemplate.ShowHeader = false;
            this.lvwTemplate.Size = new System.Drawing.Size(779, 361);
            this.lvwTemplate.SortKey = null;
            this.lvwTemplate.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lvwTemplate.TabIndex = 0;
            this.lvwTemplate.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lvwTemplate_ItemSelectionChanged);
            this.lvwTemplate.DemandLoadWhenScrollEnd += new System.EventHandler(this.lvwTemplate_DemandLoadWhenScrollEnd);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "treeListColumn1";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 750;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(710, 424);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 28);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(622, 424);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 28);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnLocation
            // 
            this.btnLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocation.Location = new System.Drawing.Point(496, 424);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(120, 28);
            this.btnLocation.TabIndex = 5;
            this.btnLocation.Text = "定位到文件夹(&L)";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Visible = false;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(69, 13);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(255, 24);
            this.txtKeyword.TabIndex = 9;
            this.txtKeyword.WaterMarkText = "输入关键字，停留1秒或回车后筛选";
            this.txtKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 19);
            this.label1.TabIndex = 10;
            this.label1.Text = "关键字:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Info;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(266, 197);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(317, 58);
            this.panel3.TabIndex = 16;
            this.panel3.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(268, 38);
            this.label5.TabIndex = 0;
            this.label5.Text = "滚动条拖至或滚至底部时加载下一页\r\n下载/更新标识选中行后下方有下载/更新按钮";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(738, 21);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(53, 28);
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbDown,
            this.tlbUse,
            this.tlbPub});
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 25);
            this.toolStripButton1.Text = "排序";
            // 
            // tlbDown
            // 
            this.tlbDown.Checked = true;
            this.tlbDown.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tlbDown.Name = "tlbDown";
            this.tlbDown.Size = new System.Drawing.Size(180, 24);
            this.tlbDown.Text = "下载次数";
            this.tlbDown.Click += new System.EventHandler(this.tlbDown_Click);
            // 
            // tlbUse
            // 
            this.tlbUse.Name = "tlbUse";
            this.tlbUse.Size = new System.Drawing.Size(180, 24);
            this.tlbUse.Text = "使用次数";
            this.tlbUse.Click += new System.EventHandler(this.tlbUse_Click);
            // 
            // tlbPub
            // 
            this.tlbPub.Name = "tlbPub";
            this.tlbPub.Size = new System.Drawing.Size(180, 24);
            this.tlbPub.Text = "更新时间";
            this.tlbPub.Click += new System.EventHandler(this.tlbPub_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::CodeBuilder.Properties.Resources.info;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton2.Text = "小贴士";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // frmTemplateShop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(803, 465);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwTemplate);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTemplateShop";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "在线模板商店";
            this.Load += new System.EventHandler(this.frmTemplateShop_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList lvwTemplate;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnLocation;
        private Fireasy.Windows.Forms.ComplexTextBox txtKeyword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem tlbDown;
        private System.Windows.Forms.ToolStripMenuItem tlbUse;
        private System.Windows.Forms.ToolStripMenuItem tlbPub;
    }
}
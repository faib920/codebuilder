
namespace CodeBuilder
{
    partial class frmPluginShop
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
            this.lvwPlugin = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.chkSource = new System.Windows.Forms.CheckBox();
            this.chkTemplate = new System.Windows.Forms.CheckBox();
            this.chkTool = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvwPlugin
            // 
            this.lvwPlugin.AllowDemandLoadWhenScrollEnd = true;
            this.lvwPlugin.AlternateBackColor = System.Drawing.Color.Empty;
            this.lvwPlugin.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lvwPlugin.DataSource = null;
            this.lvwPlugin.Footer = null;
            this.lvwPlugin.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lvwPlugin.HandCursor = false;
            this.lvwPlugin.ItemHeight = 100;
            this.lvwPlugin.Location = new System.Drawing.Point(12, 12);
            this.lvwPlugin.Name = "lvwPlugin";
            this.lvwPlugin.NoneItemText = "没有可显示的数据";
            this.lvwPlugin.RowNumberIndex = 0;
            this.lvwPlugin.ShowHeader = false;
            this.lvwPlugin.Size = new System.Drawing.Size(779, 401);
            this.lvwPlugin.SortKey = null;
            this.lvwPlugin.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lvwPlugin.TabIndex = 0;
            this.lvwPlugin.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lvwPlugin_ItemSelectionChanged);
            this.lvwPlugin.DemandLoadWhenScrollEnd += new System.EventHandler(this.lvwTemplate_DemandLoadWhenScrollEnd);
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
            this.btnClose.Location = new System.Drawing.Point(711, 426);
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
            this.btnUpdate.Location = new System.Drawing.Point(625, 426);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 28);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // chkSource
            // 
            this.chkSource.AutoSize = true;
            this.chkSource.Checked = true;
            this.chkSource.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSource.Location = new System.Drawing.Point(67, 429);
            this.chkSource.Name = "chkSource";
            this.chkSource.Size = new System.Drawing.Size(79, 24);
            this.chkSource.TabIndex = 4;
            this.chkSource.Text = "Source";
            this.chkSource.UseVisualStyleBackColor = true;
            this.chkSource.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkTemplate
            // 
            this.chkTemplate.AutoSize = true;
            this.chkTemplate.Checked = true;
            this.chkTemplate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTemplate.Location = new System.Drawing.Point(152, 429);
            this.chkTemplate.Name = "chkTemplate";
            this.chkTemplate.Size = new System.Drawing.Size(97, 24);
            this.chkTemplate.TabIndex = 5;
            this.chkTemplate.Text = "Template";
            this.chkTemplate.UseVisualStyleBackColor = true;
            this.chkTemplate.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkTool
            // 
            this.chkTool.AutoSize = true;
            this.chkTool.Checked = true;
            this.chkTool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTool.Location = new System.Drawing.Point(255, 429);
            this.chkTool.Name = "chkTool";
            this.chkTool.Size = new System.Drawing.Size(61, 24);
            this.chkTool.TabIndex = 6;
            this.chkTool.Text = "Tool";
            this.chkTool.UseVisualStyleBackColor = true;
            this.chkTool.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "筛选:";
            // 
            // frmPluginShop
            // 
            this.ClientSize = new System.Drawing.Size(803, 465);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkTool);
            this.Controls.Add(this.chkTemplate);
            this.Controls.Add(this.chkSource);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwPlugin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPluginShop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "在线插件商店";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPluginShop_FormClosing);
            this.Load += new System.EventHandler(this.frmPluginShop_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList lvwPlugin;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox chkSource;
        private System.Windows.Forms.CheckBox chkTemplate;
        private System.Windows.Forms.CheckBox chkTool;
        private System.Windows.Forms.Label label1;
    }
}
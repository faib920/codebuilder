
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
            this.lvwTemplate = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvwTemplate
            // 
            this.lvwTemplate.AllowDemandLoadWhenScrollEnd = true;
            this.lvwTemplate.AlternateBackColor = System.Drawing.Color.Empty;
            this.lvwTemplate.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lvwTemplate.DataSource = null;
            this.lvwTemplate.Footer = null;
            this.lvwTemplate.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lvwTemplate.HandCursor = false;
            this.lvwTemplate.ItemHeight = 100;
            this.lvwTemplate.Location = new System.Drawing.Point(12, 12);
            this.lvwTemplate.Name = "lvwTemplate";
            this.lvwTemplate.NoneItemText = "没有可显示的数据";
            this.lvwTemplate.RowNumberIndex = 0;
            this.lvwTemplate.ShowHeader = false;
            this.lvwTemplate.Size = new System.Drawing.Size(779, 401);
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
            this.btnClose.Location = new System.Drawing.Point(715, 428);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(627, 428);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfig.Location = new System.Drawing.Point(12, 428);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(75, 23);
            this.btnConfig.TabIndex = 4;
            this.btnConfig.Text = "配置(&G)";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnLocation
            // 
            this.btnLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocation.Location = new System.Drawing.Point(495, 428);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(119, 23);
            this.btnLocation.TabIndex = 5;
            this.btnLocation.Text = "定位到文件夹(&L)";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Visible = false;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // frmTemplateShop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 465);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwTemplate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTemplateShop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "在线模板商店";
            this.Load += new System.EventHandler(this.frmTemplateShop_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList lvwTemplate;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Button btnLocation;
    }
}
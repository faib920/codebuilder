using System.Windows.Forms;

namespace CodeBuilder
{
    partial class frmResource
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
            this.lstRes = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tlbDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstRes
            // 
            this.lstRes.AllowUpdateDataItem = false;
            this.lstRes.CheckAllChecked = false;
            this.lstRes.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1});
            this.lstRes.DataSource = null;
            this.lstRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRes.Footer = null;
            this.lstRes.GroupFont = new System.Drawing.Font("宋体", 12F);
            this.lstRes.HandCursor = false;
            this.lstRes.Location = new System.Drawing.Point(0, 25);
            this.lstRes.Name = "lstRes";
            this.lstRes.NoneItemText = "";
            this.lstRes.RowNumberIndex = 0;
            this.lstRes.ShowGridLines = false;
            this.lstRes.ShowHeader = false;
            this.lstRes.ShowPlusMinus = true;
            this.lstRes.Size = new System.Drawing.Size(241, 578);
            this.lstRes.SortKey = null;
            this.lstRes.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstRes.TabIndex = 12;
            this.lstRes.ItemDoubleClick += new Fireasy.Windows.Forms.TreeListItemDoubleClickEventHandler(this.lstRes_ItemDoubleClick);
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
            this.treeListColumn1.Width = 239;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbRefresh,
            this.tlbDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(241, 25);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbRefresh
            // 
            this.tlbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbRefresh.Image = global::CodeBuilder.Properties.Resources.refresh;
            this.tlbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbRefresh.Name = "tlbRefresh";
            this.tlbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tlbRefresh.Text = "刷新";
            this.tlbRefresh.Click += new System.EventHandler(this.tlbRefresh_Click);
            // 
            // tlbDelete
            // 
            this.tlbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbDelete.Image = global::CodeBuilder.Properties.Resources.clear;
            this.tlbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbDelete.Name = "tlbDelete";
            this.tlbDelete.Size = new System.Drawing.Size(23, 22);
            this.tlbDelete.Text = "删除";
            this.tlbDelete.Click += new System.EventHandler(this.tlbDelete_Click);
            // 
            // frmResource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 603);
            this.Controls.Add(this.lstRes);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmResource";
            this.Text = "资源";
            this.Load += new System.EventHandler(this.frmResource_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList lstRes;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlbDelete;
        private System.Windows.Forms.ToolStripButton tlbRefresh;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
    }
}
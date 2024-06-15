namespace CodeBuilder.PowerDesigner
{
    partial class OptionPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lstData = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstData
            // 
            this.lstData.AllowMoveNextItemOnEditing = true;
            this.lstData.AllowUpdateDataItem = false;
            this.lstData.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstData.BackColor = System.Drawing.SystemColors.Control;
            this.lstData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstData.CheckAllChecked = false;
            this.lstData.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstData.DataSource = null;
            this.lstData.Footer = null;
            this.lstData.FooterHeight = 28;
            this.lstData.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstData.HandCursor = false;
            this.lstData.HeaderHeight = 28;
            this.lstData.ItemHeight = 28;
            this.lstData.Location = new System.Drawing.Point(3, 25);
            this.lstData.Name = "lstData";
            this.lstData.NoneItemImage = null;
            this.lstData.NoneItemText = "没有可显示的数据";
            this.lstData.RowNumberIndex = 0;
            this.lstData.ShowPlusMinusLines = false;
            this.lstData.Size = new System.Drawing.Size(427, 385);
            this.lstData.SortKey = null;
            this.lstData.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstData.TabIndex = 2;
            this.lstData.AfterCellUpdated += new Fireasy.Windows.Forms.TreeListAfterCellUpdatedEventHandler(this.lstData_AfterCellUpdated);
            this.lstData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstData_KeyUp);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Editable = true;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Sortable = false;
            this.treeListColumn1.Text = "DBMS标识";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 200;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Editable = true;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Sortable = false;
            this.treeListColumn2.Text = "数据库类型";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 200;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "维护 PDM 文件中 DBMS 标识与数据库类型的对应关系";
            // 
            // OptionPanel
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstData);
            this.Name = "OptionPanel";
            this.Size = new System.Drawing.Size(433, 413);
            this.Load += new System.EventHandler(this.OptionPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList lstData;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.Label label1;
    }
}

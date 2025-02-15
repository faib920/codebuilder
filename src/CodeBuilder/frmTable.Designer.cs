namespace CodeBuilder
{
    partial class frmTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTable));
            this.lstObject = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSelAllTable = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelInvTable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSelAllColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelInvColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCollaps = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBuildPart = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblLocCount = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.txtKeyword = new Fireasy.Windows.Forms.ComplexTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkFilterMode = new System.Windows.Forms.CheckBox();
            this.chkRemark = new System.Windows.Forms.CheckBox();
            this.chkColumn = new System.Windows.Forms.CheckBox();
            this.chkTable = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.chkModify = new System.Windows.Forms.CheckBox();
            this.chkAdd = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstObject
            // 
            this.lstObject.AllowDragItem = true;
            this.lstObject.AllowDrop = true;
            this.lstObject.AllowMoveNextItemOnEditing = true;
            this.lstObject.AllowUpdateDataItem = true;
            this.lstObject.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstObject.BackColor = System.Drawing.SystemColors.Control;
            this.lstObject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstObject.CheckAllChecked = false;
            this.lstObject.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstObject.ContextMenuStrip = this.contextMenuStrip1;
            this.lstObject.DataSource = null;
            this.lstObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstObject.Footer = null;
            this.lstObject.FooterHeight = 28;
            this.lstObject.GroupFont = new System.Drawing.Font("宋体", 12F);
            this.lstObject.HandCursor = false;
            this.lstObject.HeaderHeight = 28;
            this.lstObject.ItemHeight = 28;
            this.lstObject.Location = new System.Drawing.Point(2, 40);
            this.lstObject.Name = "lstObject";
            this.lstObject.NoneItemImage = ((System.Drawing.Image)(resources.GetObject("lstObject.NoneItemImage")));
            this.lstObject.NoneItemText = "请点击“数据源”菜单，选择数据源";
            this.lstObject.RowNumberIndex = 0;
            this.lstObject.ShowAlternateBackColor = true;
            this.lstObject.ShowCheckBoxes = true;
            this.lstObject.ShowPlusMinus = true;
            this.lstObject.ShowPlusMinusLines = false;
            this.lstObject.Size = new System.Drawing.Size(689, 429);
            this.lstObject.SortKey = null;
            this.lstObject.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstObject.TabIndex = 5;
            this.lstObject.CellDataUpdated += new Fireasy.Windows.Forms.TreeListCellDataUpdatedEventHandler(this.lstObject_CellDataUpdated);
            this.lstObject.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lstObject_ItemSelectionChanged);
            this.lstObject.DemandLoad += new Fireasy.Windows.Forms.TreeListDemandLoadEventHandler(this.lstObject_DemandLoad);
            this.lstObject.BeforeCellEditing += new Fireasy.Windows.Forms.TreeListBeforeCellEditingEventHandler(this.lstObject_BeforeCellEditing);
            this.lstObject.ItemCheckChanged += new Fireasy.Windows.Forms.TreeListItemCheckChangeEventHandler(this.lstObject_ItemCheckChanged);
            this.lstObject.ItemDragOver += new Fireasy.Windows.Forms.TreeListItemDragOverEventHandler(this.lstObject_ItemDragOver);
            this.lstObject.AfterItemDragDown += new Fireasy.Windows.Forms.TreeListAfterItemDragDownEventHandler(this.lstObject_AfterItemDragDown);
            this.lstObject.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstObject_DragDrop);
            this.lstObject.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstObject_DragEnter);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.DataKey = "Name";
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "名称";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 340;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.DataKey = "Description";
            this.treeListColumn2.Editable = true;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "备注";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 300;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSelAllTable,
            this.mnuSelInvTable,
            this.toolStripMenuItem1,
            this.mnuSelAllColumn,
            this.mnuSelInvColumn,
            this.toolStripMenuItem2,
            this.mnuCollaps,
            this.toolStripSeparator3,
            this.mnuFind,
            this.mnuRemove,
            this.mnuClear,
            this.toolStripSeparator2,
            this.toolStripMenuItem3,
            this.mnuColumns,
            this.toolStripSeparator1,
            this.mnuBuild,
            this.mnuBuildPart});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(229, 322);
            // 
            // mnuSelAllTable
            // 
            this.mnuSelAllTable.Name = "mnuSelAllTable";
            this.mnuSelAllTable.Size = new System.Drawing.Size(228, 24);
            this.mnuSelAllTable.Text = "选择所有表";
            this.mnuSelAllTable.Click += new System.EventHandler(this.mnuSelAllTable_Click);
            // 
            // mnuSelInvTable
            // 
            this.mnuSelInvTable.Name = "mnuSelInvTable";
            this.mnuSelInvTable.Size = new System.Drawing.Size(228, 24);
            this.mnuSelInvTable.Text = "反向选择表";
            this.mnuSelInvTable.Click += new System.EventHandler(this.mnuSelInvTable_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(225, 6);
            // 
            // mnuSelAllColumn
            // 
            this.mnuSelAllColumn.Name = "mnuSelAllColumn";
            this.mnuSelAllColumn.Size = new System.Drawing.Size(228, 24);
            this.mnuSelAllColumn.Text = "选择所有列";
            this.mnuSelAllColumn.Click += new System.EventHandler(this.mnuSelAllColumn_Click);
            // 
            // mnuSelInvColumn
            // 
            this.mnuSelInvColumn.Name = "mnuSelInvColumn";
            this.mnuSelInvColumn.Size = new System.Drawing.Size(228, 24);
            this.mnuSelInvColumn.Text = "反向选择列";
            this.mnuSelInvColumn.Click += new System.EventHandler(this.mnuSelInvColumn_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(225, 6);
            // 
            // mnuCollaps
            // 
            this.mnuCollaps.Name = "mnuCollaps";
            this.mnuCollaps.Size = new System.Drawing.Size(228, 24);
            this.mnuCollaps.Text = "全部折叠";
            this.mnuCollaps.Click += new System.EventHandler(this.mnuCollaps_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(225, 6);
            // 
            // mnuFind
            // 
            this.mnuFind.Name = "mnuFind";
            this.mnuFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mnuFind.Size = new System.Drawing.Size(228, 24);
            this.mnuFind.Text = "查找...";
            this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
            // 
            // mnuRemove
            // 
            this.mnuRemove.Name = "mnuRemove";
            this.mnuRemove.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuRemove.Size = new System.Drawing.Size(228, 24);
            this.mnuRemove.Text = "移除";
            this.mnuRemove.Click += new System.EventHandler(this.mnuRemove_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Name = "mnuClear";
            this.mnuClear.Size = new System.Drawing.Size(228, 24);
            this.mnuClear.Text = "清空";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(225, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(228, 24);
            this.toolStripMenuItem3.Text = "辅助功能";
            // 
            // mnuColumns
            // 
            this.mnuColumns.Name = "mnuColumns";
            this.mnuColumns.Size = new System.Drawing.Size(228, 24);
            this.mnuColumns.Text = "定制列头...";
            this.mnuColumns.Click += new System.EventHandler(this.mnuColumns_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(225, 6);
            // 
            // mnuBuild
            // 
            this.mnuBuild.Name = "mnuBuild";
            this.mnuBuild.Size = new System.Drawing.Size(228, 24);
            this.mnuBuild.Text = "生成预览";
            this.mnuBuild.Click += new System.EventHandler(this.mnuBuild_Click);
            // 
            // mnuBuildPart
            // 
            this.mnuBuildPart.Name = "mnuBuildPart";
            this.mnuBuildPart.Size = new System.Drawing.Size(228, 24);
            this.mnuBuildPart.Text = "生成预览（单一部件）";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblLocCount);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.txtKeyword);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(689, 38);
            this.panel1.TabIndex = 6;
            this.panel1.Visible = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.Location = new System.Drawing.Point(348, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 22;
            this.label3.Visible = false;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Marlett", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.label2.Location = new System.Drawing.Point(2, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "6";
            this.toolTip1.SetToolTip(this.label2, "打开选项");
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel2.Location = new System.Drawing.Point(674, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(12, 12);
            this.panel2.TabIndex = 16;
            this.panel2.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblLocCount
            // 
            this.lblLocCount.AutoSize = true;
            this.lblLocCount.Location = new System.Drawing.Point(441, 9);
            this.lblLocCount.Name = "lblLocCount";
            this.lblLocCount.Size = new System.Drawing.Size(0, 12);
            this.lblLocCount.TabIndex = 15;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(373, 5);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(60, 27);
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = "下一个";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(57, 7);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(309, 21);
            this.txtKeyword.TabIndex = 11;
            this.txtKeyword.WaterMarkText = "输入关键字或正则表达式，停留1秒或敲回车键";
            this.txtKeyword.TextChanged += new System.EventHandler(this.txtKeyword_TextChanged);
            this.txtKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "查找:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkFilterMode);
            this.panel3.Controls.Add(this.chkRemark);
            this.panel3.Controls.Add(this.chkColumn);
            this.panel3.Controls.Add(this.chkTable);
            this.panel3.Location = new System.Drawing.Point(213, 157);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(206, 93);
            this.panel3.TabIndex = 19;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // chkFilterMode
            // 
            this.chkFilterMode.AutoSize = true;
            this.chkFilterMode.BackColor = System.Drawing.SystemColors.Info;
            this.chkFilterMode.Location = new System.Drawing.Point(15, 54);
            this.chkFilterMode.Name = "chkFilterMode";
            this.chkFilterMode.Size = new System.Drawing.Size(72, 16);
            this.chkFilterMode.TabIndex = 3;
            this.chkFilterMode.Text = "筛选模式";
            this.chkFilterMode.UseVisualStyleBackColor = false;
            this.chkFilterMode.CheckedChanged += new System.EventHandler(this.chkFilterMode_CheckedChanged);
            // 
            // chkRemark
            // 
            this.chkRemark.AutoSize = true;
            this.chkRemark.BackColor = System.Drawing.SystemColors.Info;
            this.chkRemark.Checked = true;
            this.chkRemark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemark.Location = new System.Drawing.Point(135, 21);
            this.chkRemark.Name = "chkRemark";
            this.chkRemark.Size = new System.Drawing.Size(48, 16);
            this.chkRemark.TabIndex = 2;
            this.chkRemark.Text = "备注";
            this.chkRemark.UseVisualStyleBackColor = false;
            this.chkRemark.CheckedChanged += new System.EventHandler(this.chkFillOpt_CheckedChanged);
            // 
            // chkColumn
            // 
            this.chkColumn.AutoSize = true;
            this.chkColumn.BackColor = System.Drawing.SystemColors.Info;
            this.chkColumn.Checked = true;
            this.chkColumn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColumn.Location = new System.Drawing.Point(69, 21);
            this.chkColumn.Name = "chkColumn";
            this.chkColumn.Size = new System.Drawing.Size(48, 16);
            this.chkColumn.TabIndex = 1;
            this.chkColumn.Text = "字段";
            this.chkColumn.UseVisualStyleBackColor = false;
            this.chkColumn.CheckedChanged += new System.EventHandler(this.chkFillOpt_CheckedChanged);
            // 
            // chkTable
            // 
            this.chkTable.AutoSize = true;
            this.chkTable.BackColor = System.Drawing.SystemColors.Info;
            this.chkTable.Checked = true;
            this.chkTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTable.Location = new System.Drawing.Point(15, 21);
            this.chkTable.Name = "chkTable";
            this.chkTable.Size = new System.Drawing.Size(36, 16);
            this.chkTable.TabIndex = 0;
            this.chkTable.Text = "表";
            this.chkTable.UseVisualStyleBackColor = false;
            this.chkTable.CheckedChanged += new System.EventHandler(this.chkFillOpt_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.SystemColors.Info;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(135, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "删除";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // chkModify
            // 
            this.chkModify.AutoSize = true;
            this.chkModify.BackColor = System.Drawing.SystemColors.Info;
            this.chkModify.Checked = true;
            this.chkModify.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkModify.Location = new System.Drawing.Point(74, 21);
            this.chkModify.Name = "chkModify";
            this.chkModify.Size = new System.Drawing.Size(48, 16);
            this.chkModify.TabIndex = 1;
            this.chkModify.Text = "修改";
            this.chkModify.UseVisualStyleBackColor = false;
            // 
            // chkAdd
            // 
            this.chkAdd.AutoSize = true;
            this.chkAdd.BackColor = System.Drawing.SystemColors.Info;
            this.chkAdd.Checked = true;
            this.chkAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdd.Location = new System.Drawing.Point(15, 21);
            this.chkAdd.Name = "chkAdd";
            this.chkAdd.Size = new System.Drawing.Size(48, 16);
            this.chkAdd.TabIndex = 0;
            this.chkAdd.Text = "新增";
            this.chkAdd.UseVisualStyleBackColor = false;
            // 
            // frmTable
            // 
            this.AllowEndUserDocking = false;
            this.ClientSize = new System.Drawing.Size(693, 471);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lstObject);
            this.Controls.Add(this.panel1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTable";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "对象列表";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList lstObject;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuSelAllTable;
        private System.Windows.Forms.ToolStripMenuItem mnuSelInvTable;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuSelAllColumn;
        private System.Windows.Forms.ToolStripMenuItem mnuSelInvColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuBuild;
        private System.Windows.Forms.ToolStripMenuItem mnuBuildPart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuFind;
        private System.Windows.Forms.Panel panel1;
        private Fireasy.Windows.Forms.ComplexTextBox txtKeyword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblLocCount;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mnuColumns;
        private System.Windows.Forms.ToolStripMenuItem mnuClear;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chkTable;
        private System.Windows.Forms.CheckBox chkRemark;
        private System.Windows.Forms.CheckBox chkColumn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem mnuRemove;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox chkModify;
        private System.Windows.Forms.CheckBox chkAdd;
        private System.Windows.Forms.CheckBox chkFilterMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem mnuCollaps;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
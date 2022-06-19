namespace CodeBuilder
{
    partial class frmTemplateEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn3 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn4 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn5 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAddRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lstPart = new Fireasy.Windows.Forms.TreeList();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tlbAddRoot = new System.Windows.Forms.ToolStripButton();
            this.tlbAddGroup = new System.Windows.Forms.ToolStripButton();
            this.tlbAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tlbDelete = new System.Windows.Forms.ToolStripButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lstExt = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn6 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAddExt = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelExt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbAddExt = new System.Windows.Forms.ToolStripButton();
            this.tlbSelect = new System.Windows.Forms.ToolStripButton();
            this.tlbDelExt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tlbUseBase = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.lstRes = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn7 = new Fireasy.Windows.Forms.TreeListColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "标识:";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(83, 23);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(228, 24);
            this.txtId.TabIndex = 0;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Editable = true;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "组/部件名称";
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
            this.treeListColumn2.Text = "模板文件";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 120;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.Editable = true;
            this.treeListColumn3.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.Formatter = null;
            this.treeListColumn3.Image = null;
            this.treeListColumn3.Text = "输出文件";
            this.treeListColumn3.Validator = null;
            this.treeListColumn3.Width = 200;
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn4.Editable = true;
            this.treeListColumn4.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn4.Formatter = null;
            this.treeListColumn4.Image = null;
            this.treeListColumn4.Text = "循环规则";
            this.treeListColumn4.Validator = null;
            // 
            // treeListColumn5
            // 
            this.treeListColumn5.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn5.Editable = true;
            this.treeListColumn5.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn5.Formatter = null;
            this.treeListColumn5.Image = null;
            this.treeListColumn5.Text = "语法";
            this.treeListColumn5.Validator = null;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddRoot,
            this.mnuAddGroup,
            this.mnuAdd,
            this.toolStripSeparator1,
            this.mnuDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 106);
            // 
            // mnuAddRoot
            // 
            this.mnuAddRoot.Image = global::CodeBuilder.Properties.Resources.add2;
            this.mnuAddRoot.Name = "mnuAddRoot";
            this.mnuAddRoot.Size = new System.Drawing.Size(180, 24);
            this.mnuAddRoot.Text = "添加顶级组";
            this.mnuAddRoot.Click += new System.EventHandler(this.mnuAddRoot_Click);
            // 
            // mnuAddGroup
            // 
            this.mnuAddGroup.Image = global::CodeBuilder.Properties.Resources.add1;
            this.mnuAddGroup.Name = "mnuAddGroup";
            this.mnuAddGroup.Size = new System.Drawing.Size(180, 24);
            this.mnuAddGroup.Text = "添加组";
            this.mnuAddGroup.Click += new System.EventHandler(this.mnuAddGroup_Click);
            // 
            // mnuAdd
            // 
            this.mnuAdd.Image = global::CodeBuilder.Properties.Resources.add3;
            this.mnuAdd.Name = "mnuAdd";
            this.mnuAdd.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.mnuAdd.Size = new System.Drawing.Size(180, 24);
            this.mnuAdd.Text = "添加部件";
            this.mnuAdd.Click += new System.EventHandler(this.mnuAdd_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Image = global::CodeBuilder.Properties.Resources.delete;
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(180, 24);
            this.mnuDelete.Text = "删除";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(682, 587);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(768, 587);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnLocation
            // 
            this.btnLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLocation.Location = new System.Drawing.Point(36, 587);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(119, 28);
            this.btnLocation.TabIndex = 7;
            this.btnLocation.Text = "定位到文件夹(&L)";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(83, 62);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(228, 24);
            this.txtName.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "名称:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(321, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(165, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "唯一标识，确定后不能修改";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(321, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 19);
            this.label5.TabIndex = 10;
            this.label5.Text = "显示在菜单上";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(36, 109);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(812, 466);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lstPart);
            this.tabPage1.Controls.Add(this.toolStrip2);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(804, 434);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "部件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lstPart
            // 
            this.lstPart.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstPart.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2,
            this.treeListColumn3,
            this.treeListColumn4,
            this.treeListColumn5});
            this.lstPart.ContextMenuStrip = this.contextMenuStrip1;
            this.lstPart.DataSource = null;
            this.lstPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPart.Footer = null;
            this.lstPart.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstPart.HandCursor = false;
            this.lstPart.Location = new System.Drawing.Point(3, 28);
            this.lstPart.Name = "lstPart";
            this.lstPart.NoneItemText = "右键弹出菜单点击“添加组”或“添加部件”";
            this.lstPart.RowNumberIndex = 0;
            this.lstPart.ShowPlusMinus = true;
            this.lstPart.ShowPlusMinusLines = false;
            this.lstPart.Size = new System.Drawing.Size(798, 403);
            this.lstPart.SortKey = null;
            this.lstPart.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstPart.TabIndex = 4;
            this.lstPart.BeforeCellEditing += new Fireasy.Windows.Forms.TreeListBeforeCellEditingEventHandler(this.lstPart_BeforeCellEditing);
            this.lstPart.AfterCellUpdated += new Fireasy.Windows.Forms.TreeListAfterCellUpdatedEventHandler(this.lstPart_AfterCellUpdated);
            this.lstPart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstPart_KeyDown);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbAddRoot,
            this.tlbAddGroup,
            this.tlbAdd,
            this.toolStripSeparator3,
            this.tlbDelete});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(798, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tlbAddRoot
            // 
            this.tlbAddRoot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbAddRoot.Image = global::CodeBuilder.Properties.Resources.add2;
            this.tlbAddRoot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbAddRoot.Name = "tlbAddRoot";
            this.tlbAddRoot.Size = new System.Drawing.Size(23, 22);
            this.tlbAddRoot.Text = "添加顶级组";
            this.tlbAddRoot.Click += new System.EventHandler(this.mnuAddRoot_Click);
            // 
            // tlbAddGroup
            // 
            this.tlbAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbAddGroup.Image = global::CodeBuilder.Properties.Resources.add1;
            this.tlbAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbAddGroup.Name = "tlbAddGroup";
            this.tlbAddGroup.Size = new System.Drawing.Size(23, 22);
            this.tlbAddGroup.Text = "添加组";
            this.tlbAddGroup.Click += new System.EventHandler(this.mnuAddGroup_Click);
            // 
            // tlbAdd
            // 
            this.tlbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbAdd.Image = global::CodeBuilder.Properties.Resources.add3;
            this.tlbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbAdd.Name = "tlbAdd";
            this.tlbAdd.Size = new System.Drawing.Size(23, 22);
            this.tlbAdd.Text = "添加部件";
            this.tlbAdd.Click += new System.EventHandler(this.mnuAdd_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tlbDelete
            // 
            this.tlbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbDelete.Image = global::CodeBuilder.Properties.Resources.delete;
            this.tlbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbDelete.Name = "tlbDelete";
            this.tlbDelete.Size = new System.Drawing.Size(23, 22);
            this.tlbDelete.Text = "删除";
            this.tlbDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lstExt);
            this.tabPage3.Controls.Add(this.toolStrip1);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(804, 434);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "扩展";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lstExt
            // 
            this.lstExt.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstExt.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn6});
            this.lstExt.ContextMenuStrip = this.contextMenuStrip2;
            this.lstExt.DataSource = null;
            this.lstExt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstExt.Footer = null;
            this.lstExt.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstExt.HandCursor = false;
            this.lstExt.Location = new System.Drawing.Point(3, 28);
            this.lstExt.Name = "lstExt";
            this.lstExt.NoneItemText = "";
            this.lstExt.RowNumberIndex = 0;
            this.lstExt.ShowPlusMinus = true;
            this.lstExt.ShowPlusMinusLines = false;
            this.lstExt.Size = new System.Drawing.Size(798, 403);
            this.lstExt.SortKey = null;
            this.lstExt.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstExt.TabIndex = 6;
            this.lstExt.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.lstExt_ItemSelectionChanged);
            this.lstExt.BeforeCellEditing += new Fireasy.Windows.Forms.TreeListBeforeCellEditingEventHandler(this.lstExt_BeforeCellEditing);
            // 
            // treeListColumn6
            // 
            this.treeListColumn6.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn6.Editable = true;
            this.treeListColumn6.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn6.Formatter = null;
            this.treeListColumn6.Image = null;
            this.treeListColumn6.Text = "文件名";
            this.treeListColumn6.Validator = null;
            this.treeListColumn6.Width = 500;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddExt,
            this.mnuSelect,
            this.mnuDelExt});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(181, 98);
            // 
            // mnuAddExt
            // 
            this.mnuAddExt.Enabled = false;
            this.mnuAddExt.Image = global::CodeBuilder.Properties.Resources.add1;
            this.mnuAddExt.Name = "mnuAddExt";
            this.mnuAddExt.Size = new System.Drawing.Size(180, 24);
            this.mnuAddExt.Text = "添加";
            this.mnuAddExt.Click += new System.EventHandler(this.mnuAddExt_Click);
            // 
            // mnuSelect
            // 
            this.mnuSelect.Enabled = false;
            this.mnuSelect.Image = global::CodeBuilder.Properties.Resources.browse;
            this.mnuSelect.Name = "mnuSelect";
            this.mnuSelect.Size = new System.Drawing.Size(180, 24);
            this.mnuSelect.Text = "从文件添加";
            this.mnuSelect.Click += new System.EventHandler(this.mnuSelect_Click);
            // 
            // mnuDelExt
            // 
            this.mnuDelExt.Enabled = false;
            this.mnuDelExt.Image = global::CodeBuilder.Properties.Resources.delete;
            this.mnuDelExt.Name = "mnuDelExt";
            this.mnuDelExt.Size = new System.Drawing.Size(180, 24);
            this.mnuDelExt.Text = "删除";
            this.mnuDelExt.Click += new System.EventHandler(this.mnuDelExt_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbAddExt,
            this.tlbSelect,
            this.tlbDelExt,
            this.toolStripSeparator4,
            this.toolStripButton7});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(798, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbAddExt
            // 
            this.tlbAddExt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbAddExt.Enabled = false;
            this.tlbAddExt.Image = global::CodeBuilder.Properties.Resources.add1;
            this.tlbAddExt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbAddExt.Name = "tlbAddExt";
            this.tlbAddExt.Size = new System.Drawing.Size(23, 22);
            this.tlbAddExt.Text = "添加";
            this.tlbAddExt.Click += new System.EventHandler(this.mnuAddExt_Click);
            // 
            // tlbSelect
            // 
            this.tlbSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbSelect.Enabled = false;
            this.tlbSelect.Image = global::CodeBuilder.Properties.Resources.browse;
            this.tlbSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbSelect.Name = "tlbSelect";
            this.tlbSelect.Size = new System.Drawing.Size(23, 22);
            this.tlbSelect.Text = "从文件添加";
            this.tlbSelect.Click += new System.EventHandler(this.mnuSelect_Click);
            // 
            // tlbDelExt
            // 
            this.tlbDelExt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbDelExt.Enabled = false;
            this.tlbDelExt.Image = global::CodeBuilder.Properties.Resources.delete;
            this.tlbDelExt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbDelExt.Name = "tlbDelExt";
            this.tlbDelExt.Size = new System.Drawing.Size(23, 22);
            this.tlbDelExt.Text = "删除";
            this.tlbDelExt.Click += new System.EventHandler(this.mnuDelExt_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbUseBase});
            this.toolStripButton7.Image = global::CodeBuilder.Properties.Resources.options;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(29, 22);
            this.toolStripButton7.Text = "选项";
            // 
            // tlbUseBase
            // 
            this.tlbUseBase.Checked = true;
            this.tlbUseBase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tlbUseBase.Name = "tlbUseBase";
            this.tlbUseBase.Size = new System.Drawing.Size(276, 24);
            this.tlbUseBase.Text = "使用基础类（base.cs;base.vb）";
            this.tlbUseBase.Click += new System.EventHandler(this.tlbUseBase_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.lstRes);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(804, 434);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "资源";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(4, 414);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(303, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "直接拖拽文件或文件夹到列表中，按 Delete 键删除";
            // 
            // lstRes
            // 
            this.lstRes.AllowDrop = true;
            this.lstRes.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstRes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRes.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn7});
            this.lstRes.DataSource = null;
            this.lstRes.Footer = null;
            this.lstRes.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstRes.HandCursor = false;
            this.lstRes.Location = new System.Drawing.Point(3, 3);
            this.lstRes.Name = "lstRes";
            this.lstRes.NoneItemText = "";
            this.lstRes.RowNumberIndex = 0;
            this.lstRes.Size = new System.Drawing.Size(798, 404);
            this.lstRes.SortKey = null;
            this.lstRes.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstRes.TabIndex = 0;
            this.lstRes.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstRes_DragDrop);
            this.lstRes.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstRes_DragEnter);
            this.lstRes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstRes_KeyUp);
            // 
            // treeListColumn7
            // 
            this.treeListColumn7.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn7.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn7.Formatter = null;
            this.treeListColumn7.Image = null;
            this.treeListColumn7.Text = "资源路径";
            this.treeListColumn7.Validator = null;
            this.treeListColumn7.Width = 500;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "语言:";
            // 
            // cboLanguage
            // 
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Items.AddRange(new object[] {
            "CSharp",
            "VB",
            "Java"});
            this.cboLanguage.Location = new System.Drawing.Point(572, 61);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(213, 27);
            this.cboLanguage.TabIndex = 13;
            // 
            // frmTemplateEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(867, 629);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(820, 510);
            this.Name = "frmTemplateEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模板编辑";
            this.Load += new System.EventHandler(this.frmTemplateEditor_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn3;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn4;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuAdd;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Fireasy.Windows.Forms.TreeList lstRes;
        private System.Windows.Forms.ToolStripMenuItem mnuAddGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuAddRoot;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLanguage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem mnuAddExt;
        private System.Windows.Forms.ToolStripMenuItem mnuDelExt;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn6;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn7;
        private Fireasy.Windows.Forms.TreeList lstExt;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Fireasy.Windows.Forms.TreeList lstPart;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tlbAddExt;
        private System.Windows.Forms.ToolStripButton tlbDelExt;
        private System.Windows.Forms.ToolStripButton tlbAddRoot;
        private System.Windows.Forms.ToolStripButton tlbAddGroup;
        private System.Windows.Forms.ToolStripButton tlbAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tlbDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton7;
        private System.Windows.Forms.ToolStripMenuItem tlbUseBase;
        private System.Windows.Forms.ToolStripMenuItem mnuSelect;
        private System.Windows.Forms.ToolStripButton tlbSelect;
    }
}
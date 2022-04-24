namespace CodeBuilder
{
    partial class frmProfile
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbOpen = new System.Windows.Forms.ToolStripButton();
            this.tlbSave = new System.Windows.Forms.ToolStripButton();
            this.tlbSaveAs = new System.Windows.Forms.ToolStripButton();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbOpen,
            this.tlbSave,
            this.tlbSaveAs});
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(351, 25);
            this.toolStrip1.TabIndex = 1;
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
            // tlbSave
            // 
            this.tlbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbSave.Image = global::CodeBuilder.Properties.Resources.save;
            this.tlbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbSave.Name = "tlbSave";
            this.tlbSave.Size = new System.Drawing.Size(23, 22);
            this.tlbSave.Text = "保存到文件";
            this.tlbSave.Click += new System.EventHandler(this.tlbSave_Click);
            // 
            // tlbSaveAs
            // 
            this.tlbSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbSaveAs.Image = global::CodeBuilder.Properties.Resources.saveas;
            this.tlbSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbSaveAs.Name = "tlbSaveAs";
            this.tlbSaveAs.Size = new System.Drawing.Size(23, 22);
            this.tlbSaveAs.Text = "另存为文件";
            this.tlbSaveAs.Click += new System.EventHandler(this.tlbSaveAs_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(2, 27);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(351, 440);
            this.propertyGrid1.TabIndex = 2;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // frmProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 469);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProfile";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.Text = "变量";
            this.Load += new System.EventHandler(this.frmProfile_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlbOpen;
        private System.Windows.Forms.ToolStripButton tlbSave;
        private System.Windows.Forms.ToolStripButton tlbSaveAs;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}
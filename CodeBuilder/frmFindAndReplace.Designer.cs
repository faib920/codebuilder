namespace CodeBuilder
{
    partial class frmFindAndReplace
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblReplaceWith = new System.Windows.Forms.Label();
            this.txtLookFor = new System.Windows.Forms.TextBox();
            this.txtReplaceWith = new System.Windows.Forms.TextBox();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.btnHighlightAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFindPrevious = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "搜索词:";
            // 
            // lblReplaceWith
            // 
            this.lblReplaceWith.AutoSize = true;
            this.lblReplaceWith.Location = new System.Drawing.Point(18, 55);
            this.lblReplaceWith.Name = "lblReplaceWith";
            this.lblReplaceWith.Size = new System.Drawing.Size(51, 19);
            this.lblReplaceWith.TabIndex = 2;
            this.lblReplaceWith.Text = "替换为:";
            // 
            // txtLookFor
            // 
            this.txtLookFor.Location = new System.Drawing.Point(76, 16);
            this.txtLookFor.Name = "txtLookFor";
            this.txtLookFor.Size = new System.Drawing.Size(223, 24);
            this.txtLookFor.TabIndex = 1;
            // 
            // txtReplaceWith
            // 
            this.txtReplaceWith.Location = new System.Drawing.Point(76, 52);
            this.txtReplaceWith.Name = "txtReplaceWith";
            this.txtReplaceWith.Size = new System.Drawing.Size(223, 24);
            this.txtReplaceWith.TabIndex = 3;
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(318, 49);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(109, 28);
            this.btnFindNext.TabIndex = 6;
            this.btnFindNext.Text = "查找下一个(&N)";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(318, 115);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(109, 28);
            this.btnReplace.TabIndex = 7;
            this.btnReplace.Text = "替换(&R)";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(318, 82);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(109, 28);
            this.btnReplaceAll.TabIndex = 9;
            this.btnReplaceAll.Text = "替换所有(&A)";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // chkMatchWholeWord
            // 
            this.chkMatchWholeWord.AutoSize = true;
            this.chkMatchWholeWord.Location = new System.Drawing.Point(175, 85);
            this.chkMatchWholeWord.Name = "chkMatchWholeWord";
            this.chkMatchWholeWord.Size = new System.Drawing.Size(80, 23);
            this.chkMatchWholeWord.TabIndex = 5;
            this.chkMatchWholeWord.Text = "全字匹配";
            this.chkMatchWholeWord.UseVisualStyleBackColor = true;
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(76, 85);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(93, 23);
            this.chkMatchCase.TabIndex = 4;
            this.chkMatchCase.Text = "大小写匹配";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // btnHighlightAll
            // 
            this.btnHighlightAll.Location = new System.Drawing.Point(318, 82);
            this.btnHighlightAll.Name = "btnHighlightAll";
            this.btnHighlightAll.Size = new System.Drawing.Size(109, 28);
            this.btnHighlightAll.TabIndex = 8;
            this.btnHighlightAll.Text = "查找并高亮显示";
            this.btnHighlightAll.UseVisualStyleBackColor = true;
            this.btnHighlightAll.Visible = false;
            this.btnHighlightAll.Click += new System.EventHandler(this.btnHighlightAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(318, 149);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Location = new System.Drawing.Point(318, 16);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(109, 28);
            this.btnFindPrevious.TabIndex = 6;
            this.btnFindPrevious.Text = "查找上一个(&P)";
            this.btnFindPrevious.UseVisualStyleBackColor = true;
            this.btnFindPrevious.Click += new System.EventHandler(this.btnFindPrevious_Click);
            // 
            // frmFindAndReplace
            // 
            this.AcceptButton = this.btnReplace;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(453, 196);
            this.Controls.Add(this.chkMatchCase);
            this.Controls.Add(this.chkMatchWholeWord);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnHighlightAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFindPrevious);
            this.Controls.Add(this.btnFindNext);
            this.Controls.Add(this.txtReplaceWith);
            this.Controls.Add(this.txtLookFor);
            this.Controls.Add(this.lblReplaceWith);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFindAndReplace";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查找和替换";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindAndReplaceForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblReplaceWith;
        private System.Windows.Forms.TextBox txtLookFor;
        private System.Windows.Forms.TextBox txtReplaceWith;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.CheckBox chkMatchWholeWord;
        private System.Windows.Forms.CheckBox chkMatchCase;
        private System.Windows.Forms.Button btnHighlightAll;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFindPrevious;
    }
}

namespace CodeBuilder.Tools.Tools
{
    partial class TextConverter
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
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtResult = new ICSharpCode.TextEditor.TextEditorControl();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFormatter = new ICSharpCode.TextEditor.TextEditorControl();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSource = new ICSharpCode.TextEditor.TextEditorControl();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSample = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnManage = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "分隔符";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "{Tab}",
            "|",
            ";",
            ","});
            this.comboBox1.Location = new System.Drawing.Point(141, 171);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 6;
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResult.IsReadOnly = false;
            this.txtResult.Location = new System.Drawing.Point(87, 437);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(535, 214);
            this.txtResult.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 440);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "转换结果";
            // 
            // txtFormatter
            // 
            this.txtFormatter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFormatter.IsReadOnly = false;
            this.txtFormatter.Location = new System.Drawing.Point(87, 40);
            this.txtFormatter.Name = "txtFormatter";
            this.txtFormatter.Size = new System.Drawing.Size(535, 121);
            this.txtFormatter.TabIndex = 3;
            this.txtFormatter.Text = "/// <summary>\r\n/// {1}\r\n/// </summary>\r\n[JsonProperty(\"{0}\")]\r\npublic string {0} " +
    "{{ get; set; }}";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "转换的模板";
            // 
            // txtSource
            // 
            this.txtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSource.IsReadOnly = false;
            this.txtSource.Location = new System.Drawing.Point(87, 204);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(535, 219);
            this.txtSource.TabIndex = 1;
            this.txtSource.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "待转换文本";
            // 
            // btnSample
            // 
            this.btnSample.Location = new System.Drawing.Point(542, 169);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(80, 28);
            this.btnSample.TabIndex = 8;
            this.btnSample.Text = "示例(&I)";
            this.btnSample.UseVisualStyleBackColor = true;
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(278, 173);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "帕斯卡命名";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "粘贴进去";
            // 
            // comboBox2
            // 
            this.comboBox2.DisplayMember = "Key";
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(87, 5);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(246, 20);
            this.comboBox2.TabIndex = 11;
            this.comboBox2.ValueMember = "Value";
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "选择模板";
            // 
            // btnManage
            // 
            this.btnManage.Location = new System.Drawing.Point(339, 4);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(80, 28);
            this.btnManage.TabIndex = 13;
            this.btnManage.Text = "管理(&M)";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(456, 169);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(80, 28);
            this.btnConvert.TabIndex = 14;
            this.btnConvert.Text = "转换(&C)";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // TextConverter
            // 
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnSample);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFormatter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label1);
            this.Name = "TextConverter";
            this.Size = new System.Drawing.Size(707, 655);
            this.Load += new System.EventHandler(this.PropertyGenerator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private ICSharpCode.TextEditor.TextEditorControl txtResult;
        private System.Windows.Forms.Label label3;
        private ICSharpCode.TextEditor.TextEditorControl txtFormatter;
        private System.Windows.Forms.Label label2;
        private ICSharpCode.TextEditor.TextEditorControl txtSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Button btnConvert;
    }
}

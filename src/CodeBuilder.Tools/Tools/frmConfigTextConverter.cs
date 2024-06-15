// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class frmConfigTextConverter : FormBase
    {
        private readonly IDevHosting _hosting;
        private bool _isAdding;

        public frmConfigTextConverter(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        private void frmConfigTextConverter_Load(object sender, EventArgs e)
        {
            var list = TextConvertConfig.LoadConfig(_hosting);

            foreach (var k in list)
            {
                comboBox1.Items.Add(k);
            }

            if (list.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = comboBox1.SelectedIndex != -1;

            txtFormatter.Text = ((TextConvertConfig.KeyValue)comboBox1.SelectedItem).Value;
            txtFormatter.Refresh();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var list = new List<TextConvertConfig.KeyValue>();
            foreach (var item in comboBox1.Items)
            {
                var kv = (TextConvertConfig.KeyValue)item;
                list.Add(kv);
            }

            TextConvertConfig.SaveConfig(_hosting, list);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Visible)
            {
                var index = comboBox1.Items.Add(new TextConvertConfig.KeyValue { Key = txtName.Text, Value = txtFormatter.Text });
                comboBox1.SelectedIndex = index;
                txtName.Visible = false;
                comboBox1.Visible = true;
                btnAdd.Text = "+";
                btnDelete.Text = "-";

                _isAdding = false;
            }
            else
            {
                _isAdding = true;

                txtName.Text = string.Empty;
                txtName.Visible = true;
                comboBox1.Visible = false;
                txtFormatter.Text = string.Empty;
                txtFormatter.Refresh();
                btnAdd.Text = "√";
                btnDelete.Text = "×";
                txtName.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtName.Visible)
            {
                txtName.Visible = false;
                comboBox1.Visible = true;
                btnAdd.Text = "+";
                btnDelete.Text = "-";

                _isAdding = false;

                comboBox1_SelectedIndexChanged(null, null);
            }
            else if (comboBox1.Items.Count > 0)
            {
                if (_hosting.ShowConfirm("是否删除当前模板?") == ShowMsgButton.Yes)
                {
                    comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
                    if (comboBox1.Items.Count > 0)
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    else
                    {
                        btnDelete.Enabled = false;

                        txtFormatter.Text = string.Empty;
                        txtFormatter.Refresh();
                    }
                }
            }
            else
            {
                btnDelete.Enabled = false;

                txtFormatter.Text = string.Empty;
                txtFormatter.Refresh();
            }
        }

        private void txtFormatter_TextChanged(object sender, EventArgs e)
        {
            if (!_isAdding && comboBox1.SelectedIndex != -1)
            {
                var kv = ((TextConvertConfig.KeyValue)comboBox1.SelectedItem);
                kv.Value = txtFormatter.Text;
            }
        }
    }
}

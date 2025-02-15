// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using CodeBuilder.Core.Source;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeBuilder.Core.Designer
{
    public partial class ForeignKeyEditorForm : FormBase
    {
        private Column _column;

        public ForeignKeyEditorForm(Column column)
        {
            InitializeComponent();
            this._column = column;
        }

        public object Value
        {
            get
            {
                return _column.ForeignKey;
            }
        }

        private void ForeignKeyEditorForm_Load(object sender, EventArgs e)
        {
            LoadTables();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _column.BindForeignKey(null);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnBind_Click(object sender, EventArgs e)
        {
            if (!lstObject.HasSelectedItems)
            {
                MessageBox.Show("请选择要绑定的列。", "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!(lstObject.SelectedItems[0].Tag is Column target))
            {
                MessageBox.Show("请选择要绑定的列。", "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            _column.BindForeignKey(new Reference(target.Owner, target, _column.Owner, _column));
            DialogResult = DialogResult.OK;
            Close();
        }

        private void listBox1_DemandLoad(object sender, TreeListItemEventArgs e)
        {
            LoadColumnNodes(e.Item);
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                timer1.Enabled = false;
                FindAndFiltering(txtKeyword.Text);
            }
            else
            {
                timer1.Stop();
                timer1.Start();
            }
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            label2.Visible = txtKeyword.Text.Length > 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            FindAndFiltering(txtKeyword.Text);
        }

        private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e)
        {
            LoadTables();

            if (!string.IsNullOrEmpty(txtKeyword.Text))
            {
                FindAndFiltering(txtKeyword.Text);
            }
        }

        private void LoadTables()
        {
            lblLocCount.Text = string.Empty;

            TreeListItem selected = null;
            lstObject.BeginUpdate();
            lstObject.Items.Clear();

            foreach (var table in _column.Owner.Host.Tables.Where(s => !s.IsView))
            {
                if (table == _column.Owner)
                {
                    continue;
                }

                if (chkPrimaryKey.Checked && !table.Columns.Any(s => s.IsPrimaryKey))
                {
                    continue;
                }

                var titem = lstObject.Items.Add(table.Name);
                titem.Image = Properties.Resources.table;
                titem.Cells[1].Value = table.Description;
                titem.Tag = table;

                titem.ShowExpanded = table.Columns.Count > 0;

                if (_column.ForeignKey != null && _column.ForeignKey.PkTable.Equals(table))
                {
                    titem.Expended = true;
                    titem.ShowExpanded = false;

                    foreach (var item in titem.Items)
                    {
                        var column = item.Tag as Column;
                        if (_column.ForeignKey.PkColumn.Equals(column))
                        {
                            item.Selected = true;
                            selected = item;
                        }
                    }
                }
            }

            lstObject.EndUpdate();

            if (selected != null)
            {
                selected.EnsureVisible();
            }
        }

        private void FindAndFiltering(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                lstObject.Filtering(null);
                return;
            }

            var showItems = new List<TreeListItem>();

            lstObject.BeginUpdate();
            foreach (var item in lstObject.Items)
            {
                var table = item.Tag as Table;
                var findColumns = false;
                foreach (var column in table.Columns)
                {
                    if (string.IsNullOrEmpty(keyword) ? false : Regex.IsMatch(column.Name, keyword, RegexOptions.IgnoreCase) &&
                        (chkPrimaryKey.Checked ? column.IsPrimaryKey : true))
                    {
                        findColumns = true;
                        break;
                    }
                }

                if (findColumns)
                {
                    showItems.Add(item);

                    if (!item.IsDemandLoad)
                    {
                        item.ShowExpanded = false;
                        LoadColumnNodes(item);
                        item.Expended = true;
                    }
                }
            }
            lstObject.EndUpdate();

            lstObject.Filtering(s =>
            {
                if (s.Level == 0 && s.Tag is Table table)
                {
                    var isfilter = showItems.Contains(s) || (string.IsNullOrEmpty(keyword) ? false : (Regex.IsMatch(table.Name, keyword, RegexOptions.IgnoreCase)));
                    return isfilter;
                }
                else if (s.Tag is Column column)
                {
                    if (!showItems.Contains(s.Parent))
                    {
                        return true;
                    }

                    var isfilter = string.IsNullOrEmpty(keyword) ? false : (Regex.IsMatch(column.Name, keyword, RegexOptions.IgnoreCase));
                    return isfilter && (chkPrimaryKey.Checked ? column.IsPrimaryKey : true);
                }

                return false;
            });
        }

        private void LoadColumnNodes(TreeListItem item)
        {
            var table = item.Tag as Table;
            item.IsDemandLoad = true;

            foreach (var column in table.Columns)
            {
                if (chkPrimaryKey.Checked && !column.IsPrimaryKey)
                {
                    continue;
                }

                var citem = item.Items.Add(column.Name);
                citem.Tag = column;
                if (column.IsPrimaryKey)
                {
                    citem.Image = Properties.Resources.pk;
                }
                else
                {
                    citem.Image = Properties.Resources.column;
                }

                citem.Cells[1].Value = column.Description;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            txtKeyword.Text = string.Empty;
            FindAndFiltering(string.Empty);
        }
    }
}

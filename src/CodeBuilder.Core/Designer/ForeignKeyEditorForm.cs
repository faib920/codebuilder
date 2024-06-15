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
        private int _filterIndex;
        private List<TreeListItem> _filterItems = null;

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
            if (lstObject.SelectedItems.Count == 0)
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
                FindAndLocation(txtKeyword.Text);
            }
            else
            {
                timer1.Stop();
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            FindAndLocation(txtKeyword.Text);
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            if ((_filterItems?.Count ?? 0) <= 0)
            {
                return;
            }

            if (_filterIndex >= _filterItems.Count)
            {
                _filterIndex = 0;
            }

            var item = _filterItems[_filterIndex++];
            if (item.Level > 0 && !item.Parent.Expended)
            {
                item.Parent.Expended = true;
            }

            lblLocCount.Text = $"第 {_filterIndex} 个，共搜索到 {_filterItems.Count} 个";

            item.Selected = true;
            item.EnsureVisible();
            lstObject.Focus();
        }

        private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e)
        {
            LoadTables();

            if (!string.IsNullOrEmpty(txtKeyword.Text))
            {
                FindAndLocation(txtKeyword.Text);
            }
        }

        private void LoadTables()
        {
            _filterItems = null;
            _filterIndex = 0;
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

        private void FindAndLocation(string keyword)
        {
            _filterItems = new List<TreeListItem>();
            _filterIndex = 0;

            foreach (var item in lstObject.Items)
            {
                var filter = string.IsNullOrEmpty(keyword) ? false : Regex.IsMatch(item.Text, keyword, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(item.Cells[1].Text, keyword, RegexOptions.IgnoreCase);
                if (filter)
                {
                    _filterItems.Add(item);
                }

                var color = filter ? Color.LightSkyBlue : Color.Empty;
                if (item.BackgroundColor != color)
                {
                    item.BackgroundColor = color;
                }

                var table = item.Tag as Table;
                var findColumns = false;
                if (item.Items.Count == 0)
                {
                    foreach (var column in table.Columns)
                    {
                        if (string.IsNullOrEmpty(keyword) ? false : Regex.IsMatch(column.Name, keyword, RegexOptions.IgnoreCase) ||
                            Regex.IsMatch(column.Description, keyword, RegexOptions.IgnoreCase))
                        {
                            findColumns = true;
                            break;
                        }
                    }

                    if (findColumns)
                    {
                        item.ShowExpanded = false;
                        LoadColumnNodes(item);
                        SearchColumns(keyword, item);
                    }
                }
                else
                {
                    SearchColumns(keyword, item);
                }

            }

            if (_filterItems.Count > 0)
            {
                var item = _filterItems[_filterIndex++];

                lblLocCount.Text = $"第 {_filterIndex} 个，共搜索到 {_filterItems.Count} 个";
                item.Selected = true;
                item.EnsureVisible();
            }
            else
            {
                lblLocCount.Text = string.Empty;
            }
        }

        private void LoadColumnNodes(TreeListItem item)
        {
            var table = item.Tag as Table;

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

        private void SearchColumns(string keyword, TreeListItem item)
        {
            foreach (var citem in item.Items)
            {
                var filter = string.IsNullOrEmpty(keyword) ? false : Regex.IsMatch(citem.Text, keyword, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(citem.Cells[1].Text, keyword, RegexOptions.IgnoreCase);
                if (filter && chkPrimaryKey.Checked && !(citem.Tag as Column).IsPrimaryKey)
                {
                    filter = false;
                }

                if (filter)
                {
                    _filterItems.Add(citem);
                }

                var color = filter ? Color.LightSkyBlue : Color.Empty;
                if (citem.BackgroundColor != color)
                {
                    citem.BackgroundColor = color;
                }
            }
        }
    }
}

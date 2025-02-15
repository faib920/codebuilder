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
using CodeBuilder.Core.Source;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeBuilder.DbSchema
{
    public partial class frmTableSelector : FormBase
    {
        private PdmDefinition _definition;
        private readonly IDevHosting _hosting;
        private readonly List<string> _selectedNames;

        public frmTableSelector(IDevHosting hosting, List<string> selectedNames)
            : base()
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
            _selectedNames = selectedNames;
        }

        public frmTableSelector(IDevHosting hosting, PdmDefinition definition, List<string> selectedNames)
            : this(hosting, selectedNames)
        {
            _definition = definition;
        }

        public List<Table> Selected { get; private set; }

        private void frmTableSelector_Load(object sender, EventArgs e)
        {
            lstTable.BeginUpdate();
            LoadSchemas(lstTable.Items, _definition.Schemas);
            lstTable.EndUpdate();
        }

        private void LoadSchemas(TreeListItemCollection items, List<PdmSchema> schemas)
        {
            foreach (var p in schemas)
            {
                var item = new TreeListItem(p.Name);
                items.Add(item);
                LoadTables(item.Items, p.Tables);
                item.Expended = true;
                item.ImageIndex = 0;
            }
        }

        private void LoadTables(TreeListItemCollection items, List<PdmTable> tables)
        {
            foreach (var t in tables)
            {
                var item = new TreeListItem(t.Name);
                item.Tag = t;
                item.ImageIndex = t.IsView ? 2 : 1;
                items.Add(item);
                item.Cells[1].Value = t.Description;

                if (_selectedNames.Contains(t.Name))
                {
                    item.Checked = true;
                }
            }
        }

        private void CheckedItems(TreeListItemCollection items, bool @checked)
        {
            foreach (var item in items)
            {
                item.Checked = @checked;
                CheckedItems(item.Items, @checked);
            }
        }

        private void GetSelectedTables(TreeListItemCollection items)
        {
            foreach (var item in items)
            {
                var t = item.Tag as PdmTable;
                if (t == null)
                {
                    GetSelectedTables(item.Items);
                }
                else if (item.Checked)
                {
                    Selected.Add(t);
                }
            }
        }

        private void lstTable_AfterItemCheckChange(object sender, TreeListItemEventArgs e)
        {
            CheckedItems(e.Item.Items, e.Item.Checked);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Selected = new List<Table>();

            GetSelectedTables(lstTable.Items);

            if (Selected.Count == 0)
            {
                _hosting.ShowWarn("至少选择一个以上的表。");
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void lstTable_CheckAllChanged(object sender, TreeListCheckAllEventArgs e)
        {
            CheckedItems(lstTable.Items, e.Checked);
        }

        private void txtKeyword_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                FindAndFiltering();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            txtKeyword.Text = string.Empty;
            FindAndFiltering();
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = txtKeyword.Text.Length > 0;
        }

        private void FindAndFiltering()
        {
            if (txtKeyword.Text.Length == 0)
            {
                lstTable.Filtering(null);
            }
            else
            {
                lstTable.Filtering(s =>
                {
                    return s.Items.HasVisiableItems ||
                        Regex.IsMatch(s.Text, txtKeyword.Text, RegexOptions.IgnoreCase) ||
                        (s.Cells.Count > 1 && Regex.IsMatch(s.Cells[1].Text, txtKeyword.Text, RegexOptions.IgnoreCase));
                });
            }
        }

    }
}

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
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeBuilder.PDManer
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

        public List<Tuple<PdmEntity, Table>> Selected { get; private set; }

        private void frmTableSelector_Load(object sender, EventArgs e)
        {
            lstTable.BeginUpdate();
            if (_definition.ViewGroups.Count > 0)
            {
                LoadGroups(lstTable.Items, _definition.ViewGroups);
            }
            else
            {
                LoadTables(lstTable.Items, _definition.Entities);
            }
            lstTable.EndUpdate();
        }

        private void LoadGroups(TreeListItemCollection items, List<PdmViewGroup> groups)
        {
            foreach (var g in groups)
            {
                var item = new TreeListItem(g.DefName);
                item.Tag = g;
                item.ImageIndex = 0;
                items.Add(item);

                LoadTables(item.Items, g.Entities);
                item.Expended = true;
            }
        }

        private void LoadTables(TreeListItemCollection items, List<PdmEntity> tables)
        {
            foreach (var t in tables)
            {
                var item = new TreeListItem(t.DefKey);
                item.Tag = t;
                item.ImageIndex = 1;
                items.Add(item);
                item.Cells[1].Value = t.DefName;

                if (_selectedNames.Contains(t.DefKey))
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
                var t = item.Tag as PdmEntity;
                if (t == null)
                {
                    GetSelectedTables(item.Items);
                }
                else if (item.Checked)
                {
                    Selected.Add(Tuple.Create(t, _definition.ConvertToTable(_hosting.ServiceProvider, t)));
                }
            }

            ProcessReferences();
        }

        private void lstTable_AfterItemCheckChange(object sender, TreeListItemEventArgs e)
        {
            CheckedItems(e.Item.Items, e.Item.Checked);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Selected = new List<Tuple<PdmEntity, Table>>();

            GetSelectedTables(lstTable.Items);

            if (Selected.Count == 0)
            {
                _hosting.ShowWarn("至少选择一个以上的表。");
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void ProcessReferences()
        {
            foreach (var tuple in Selected)
            {
                foreach (var rel in tuple.Item1.Correlations)
                {
                    var relEntity = _definition.Entities.FirstOrDefault(s => s.Id == rel.RefEntity);
                    var relTuple = Selected.FirstOrDefault(s => s.Item1 == relEntity);
                    if (relTuple.Item2 != null)
                    {
                        var myField = tuple.Item1.Fields.FirstOrDefault(s => s.Id == rel.MyField);
                        var relField = relEntity.Fields.FirstOrDefault(s => s.Id == rel.RefField);
                        if (myField == null || relField == null)
                        {
                            continue;
                        }

                        var myColumn = tuple.Item2.Columns.FirstOrDefault(s => s.Name == myField.DefKey);
                        var relColumn = relTuple.Item2.Columns.FirstOrDefault(s => s.Name == relField.DefKey);

                        if (myColumn == null || relColumn == null)
                        {
                            continue;
                        }

                        if (rel.MyRows == "n")
                        {
                            myColumn.BindForeignKey(new Reference(relTuple.Item2, relColumn, tuple.Item2, myColumn));
                        }
                        else
                        {
                            myColumn.BindForeignKey(new Reference(tuple.Item2, myColumn, relTuple.Item2, relColumn));
                        }
                    }
                }
            }
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

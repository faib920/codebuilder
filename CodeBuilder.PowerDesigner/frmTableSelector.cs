// -----------------------------------------------------------------------
// <copyright company="Fireasy"
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

namespace CodeBuilder.PowerDesigner
{
    public partial class frmTableSelector : FormBase
    {
        private PdmDefinition _definition;
        private readonly IDevHosting _hosting;

        public frmTableSelector(IDevHosting hosting)
            : base()
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
        }

        public frmTableSelector(IDevHosting hosting, PdmDefinition definition)
            : this(hosting)
        {
            _definition = definition;
        }

        public List<Table> Selected { get; private set; }

        private void frmTableSelector_Load(object sender, EventArgs e)
        {
            lstTable.BeginUpdate();
            LoadCategories(lstTable.Items, _definition.Packages, _definition.Diagrams);
            lstTable.EndUpdate();
        }

        private void LoadCategories(TreeListItemCollection items, List<PdmPackage> packages, List<PdmDiagram> diagrams)
        {
            foreach (var p in packages)
            {
                var item = new TreeListItem(p.Name);
                items.Add(item);
                LoadCategories(item.Items, p.Packages, p.Diagrams);
                item.Expended = true;
                item.ImageIndex = 0;
            }

            foreach (var d in diagrams)
            {
                var item = new TreeListItem(d.Name);
                item.ImageIndex = 1;
                item.Tag = d;
                items.Add(item);

                LoadTables(item.Items, d.Tables);
                item.Expended = true;
            }
        }

        private void LoadTables(TreeListItemCollection items, List<PdmTable> tables)
        {
            foreach (var t in tables)
            {
                var item = new TreeListItem(t.Name);
                item.Tag = t;
                item.ImageIndex = 2;
                items.Add(item);
                item.Cells[1].Value = t.Description;
            }
        }

        private void CheckedItems(TreeListItemCollection items)
        {
            foreach (var item in items)
            {
                item.Checked = true;
                CheckedItems(item.Items);
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
            if (e.Item.Checked)
            {
                CheckedItems(e.Item.Items);
            }
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

        private void btnAll_Click(object sender, EventArgs e)
        {
            foreach (var item in lstTable.Items)
            {
                item.Checked = true;
                CheckedItems(item.Items);
            }
        }
    }
}

// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmTableSelector : Form
    {
        private List<Table> _tables;
        private List<Table> _saved;
        private readonly IDevHosting _hosting;

        public frmTableSelector()
        {
            InitializeComponent();
            Icon = Util.GetIcon();
        }

        public frmTableSelector(IDevHosting hosting, IEnumerable<Table> tables)
            : this()
        {
            _tables = tables.ToList();
            _saved = new List<Table>();
            FillTables(string.Empty);
            _hosting = hosting;
        }

        public List<Table> Selected { get; private set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            _tables.Clear();
            base.OnClosing(e);
        }

        private void FillTables(string keyword)
        {
            lstTable.BeginUpdate();
            lstTable.Items.Clear();

            IEnumerable<Table> ts = _tables;

            if (!string.IsNullOrEmpty(keyword))
            {
                ts = ts.Where(s => Regex.IsMatch(s.Name, keyword, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(s.Description, keyword, RegexOptions.IgnoreCase));
            }

            foreach (var t in ts)
            {
                var item = new TreeListItem();
                item.Tag = t;
                item.Image = t.IsView ? Properties.Resources.view : Properties.Resources.table;
                lstTable.Items.Add(item);

                item.Cells[0].Value = t.Name;
                item.Cells[1].Value = t.Description;
            }

            lstTable.EndUpdate();
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                timer1.Enabled = false;
                FillTables(txtKeyword.Text);
            }
            else
            {
                timer1.Start();
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            foreach (var item in lstTable.Items)
            {
                item.Checked = true;
            }
        }

        private void btnInv_Click(object sender, EventArgs e)
        {
            foreach (var item in lstTable.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            FillTables(txtKeyword.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Selected = new List<Table>();

            var items = lstSaved.Visible ? lstSaved.Items : lstTable.Items;

            foreach (var item in items)
            {
                if (item.Checked && !Selected.Contains((Table)item.Tag))
                {
                    Selected.Add((Table)item.Tag);
                }
            }

            if (Selected.Count == 0)
            {
                _hosting.ShowWarn("至少选择一个以上的表。");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var item in lstTable.Items)
            {
                var table = (Table)item.Tag;
                if (item.Checked && !_saved.Contains(table))
                {
                    var item1 = new TreeListItem();
                    item1.Tag = table;
                    item1.Image = table.IsView ? Properties.Resources.view : Properties.Resources.table;
                    lstSaved.Items.Add(item1);

                    item1.Cells[0].Value = table.Name;
                    item1.Cells[1].Value = table.Description;
                    item1.Checked = true;

                    _saved.Add(table);
                }
            }

            if (lstSaved.Items.Count > 0 && !lstSaved.Visible)
            {
                Height += 200;
                lstTable.Height = Height - lstSaved.Height - 185;
                btnAll.Top = btnInv.Top = btnSave.Top = lstSaved.Top - btnSave.Height - 10;
                lstSaved.Visible = btnClear.Visible = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _saved.Clear();
            lstSaved.Items.Clear();

            Height -= 200;
            lstTable.Height = Height - 140;
            btnAll.Top = btnInv.Top = btnSave.Top = btnOk.Top;
            lstSaved.Visible = btnClear.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            FillTables(txtKeyword.Text);
        }
    }
}

using CodeBuilder.Core.Forms;
using CodeBuilder.Core.Source;
using Fireasy.Windows.Forms;
using System;
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
            TreeListItem selected = null;
            listBox1.BeginUpdate();

            foreach (var t in _column.Owner.Host.Tables)
            {
                if (t == _column.Owner)
                {
                    continue;
                }

                var f = listBox1.Items.Add(t.Name);
                f.Image = Properties.Resources.table;
                f.Cells[1].Value = t.Description;

                foreach (var c in t.Columns)
                {
                    var s = f.Items.Add(c.Name);
                    s.Tag = c;
                    if (c.IsPrimaryKey)
                    {
                        s.Image = Properties.Resources.pk;
                    }
                    else
                    {
                        s.Image = Properties.Resources.column;
                    }

                    s.Cells[1].Value = c.Description;

                    if (_column.ForeignKey != null)
                    {
                        if (_column.ForeignKey.PkTable == t && _column.ForeignKey.PkColumn == c)
                        {
                            s.Selected = true;
                            selected = s;
                        }
                    }
                }
            }

            listBox1.EndUpdate();

            if (selected != null)
            {
                selected.EnsureVisible();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _column.BindForeignKey(null);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnBind_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要绑定的列。", "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!(listBox1.SelectedItems[0].Tag is Column target))
            {
                MessageBox.Show("请选择要绑定的列。", "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            _column.BindForeignKey(new Reference(target.Owner, target, _column.Owner, _column));
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

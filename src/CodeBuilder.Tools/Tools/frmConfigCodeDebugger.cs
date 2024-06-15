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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class frmConfigCodeDebugger : FormBase
    {
        private readonly IDevHosting _hosting;

        public frmConfigCodeDebugger(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        private void frmConfigCodeDebugger_Load(object sender, System.EventArgs e)
        {
            foreach (var ass in CodeDebugConfig.GlobalAssemblies)
            {
                var item = lvwAssembly.Items.Add(GetFileName(ass));
                item.ForeColor = Color.DarkRed;
                item.ImageIndex = 0;
            }

            foreach (var ass in CodeDebugConfig.LoadConfig(_hosting))
            {
                var item = lvwAssembly.Items.Add(GetFileName(ass));
                item.ImageIndex = 0;
            }
        }

        private void tbtnAdd_Click(object sender, System.EventArgs e)
        {
            using (var dialog = new OpenFileDialog { InitialDirectory = _hosting.WorkPath, Multiselect = true, Filter = ".Net Assembly(*.dll)|*.dll" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in dialog.FileNames)
                    {
                        if (lvwAssembly.Items.Any(s => s.Text.Equals(GetFileName(file), StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }

                        var item = lvwAssembly.Items.Add(GetFileName(file));
                        item.ImageIndex = 0;
                    }
                }
            }
        }

        private void tbtnDelete_Click(object sender, System.EventArgs e)
        {
            if (lvwAssembly.SelectedItems.Count > 0)
            {
                if (lvwAssembly.SelectedItems[0].ForeColor != Color.Empty)
                {
                    _hosting.ShowWarn($"程序集 \"{lvwAssembly.SelectedItems[0].Text}\" 是必需的，不能删除。");
                    return;
                }

                lvwAssembly.Items.Remove(lvwAssembly.SelectedItems[0]);
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            var list = new List<string>();

            foreach (var item in lvwAssembly.Items.Where(s => s.ForeColor == Color.Empty))
            {
                list.Add(item.Text);
            }

            CodeDebugConfig.SaveConfig(_hosting, list);

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private string GetFileName(string path)
        {
            var lf = path.LastIndexOf("\\");
            return path.Substring(lf + 1);
        }
    }
}

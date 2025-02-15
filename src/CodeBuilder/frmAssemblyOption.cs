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
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmAssemblyOption : FormBase
    {
        private readonly IDevHosting _hosting;
        private readonly Func<string[]> _getter;

        public frmAssemblyOption(IDevHosting hosting, string category, Func<string[]> getter)
        {
            InitializeComponent();
            _hosting = hosting;
            _getter = getter;

            Text = $"管理 {category} 引用的程序集";
        }

        public string[] Assemblies { get; private set; }

        private void frmConfigCodeDebugger_Load(object sender, System.EventArgs e)
        {
            var list = _getter();

            foreach (var ass in list)
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
            if (lvwAssembly.HasSelectedItems)
            {
                lvwAssembly.Items.Remove(lvwAssembly.SelectedItems[0]);
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            var list = new List<string>();

            foreach (var item in lvwAssembly.Items)
            {
                list.Add(item.Text);
            }

            Assemblies = list.ToArray();

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private string GetFileName(string path)
        {
            var lf = path.LastIndexOf("\\");
            return lf >= 0 ? path.Substring(lf + 1) : path;
        }
    }
}

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
using Fireasy.Common.Extensions;
using Fireasy.Data;
using Fireasy.Data.Extensions;
using Fireasy.Data.Provider;
using Fireasy.Windows.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmSourceMgr : FormBase
    {
        private string _sourceFileName = null;
        private List<DbSourceStruct> _sources = null;
        private readonly IDevHosting _hosting;

        public frmSourceMgr(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _sourceFileName = Path.Combine(hosting.WorkPath, "config", "datasources.cfg");
            _hosting = hosting;
        }

        public DbSourceStruct DbConStr { get; private set; }

        private void frmSourceMgr_Load(object sender, EventArgs e)
        {
            LoadProviderTypes();
            LoadDataSources();
        }

        private void LoadProviderTypes()
        {
            var providerManager = _hosting.ServiceProvider.TryGetService<IProviderManager>();
            foreach (var p in providerManager.GetSupportedProviders().OrderBy(s => s.Description))
            {
                var name = p.Description == p.Alais ? p.Description : $"{p.Alais} ({p.Description})";
                tbtnAdd.DropDownItems.Add(new ToolStripMenuItem(name) { Tag = p });
            }
        }

        private void LoadDataSources()
        {
            var content = File.ReadAllText(_sourceFileName);
            _sources = JsonConvert.DeserializeObject<List<DbSourceStruct>>(content);

            lstProvider.Items.Clear();

            foreach (var s in _sources)
            {
                var t = new TreeListItem();
                lstProvider.Items.Add(t);
                t.GroupKey = s.Type;
                t.ImageIndex = 0;
                t.Tag = s;
                t.Cells[0].Value = s.Name;
                t.Cells[1].Value = s.ConnectionString;
            }

            lstProvider.Grouping(true);
        }

        private void SaveDataSources()
        {
            var content = JsonConvert.SerializeObject(_sources);

            Util.TryOperateFile(_sourceFileName, () => File.WriteAllText(_sourceFileName, content, Encoding.UTF8));
        }

        private async void tbtnTest_Click(object sender, EventArgs e)
        {
            var item = lstProvider.SelectedItems[0];
            var databaseFactory = _hosting.ServiceProvider.TryGetService<IDatabaseFactory>();
            var providerManager = _hosting.ServiceProvider.TryGetService<IProviderManager>();
            var provider = providerManager.GetDefinedProvider(item.GroupKey);
            if (provider == null)
            {
                _hosting.ShowError(string.Format("没有发现 {0} 的数据库适配器。", item.Group));
            }

            using (var db = databaseFactory.CreateDatabase(item.GroupKey, item.Cells[1].Value.ToString()))
            {
                try
                {
                    db.Provider.UpdateConnectionParameter(db.ConnectionString, p => p.ConnectTimeout = "3");

                    Cursor = Cursors.WaitCursor;

                    var exp = await db.TryConnectAsync();
                    if (exp == null)
                    {
                        _hosting.ShowInfo(string.Format("{0} 连接成功。", item.Text));
                    }
                    else
                    {
                        throw exp;
                    }
                }
                catch (Exception exp)
                {
                    _hosting.ShowError(string.Format("{0} 连接失败。详细信息如下：\n\n{1}", item.Text, exp.Message));
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void tbtnAdd_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var source = new DbSourceStruct { Type = ((ProviderDescriptor)e.ClickedItem.Tag).Alais };
            using (var frm = new frmSourceEdit(_hosting, source, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var item = new TreeListItem();
                    lstProvider.Items.Add(item);
                    item.GroupKey = source.Type;
                    item.ImageIndex = 0;
                    item.Tag = source;
                    item.Cells[0].Value = source.Name;
                    item.Cells[1].Value = source.ConnectionString;

                    _sources.Add(source);
                    SaveDataSources();

                    lstProvider.Grouping(true);
                }
            }
        }

        private void tbtnEdit_Click(object sender, EventArgs e)
        {
            if (!lstProvider.HasSelectedItems)
            {
                return;
            }

            var item = lstProvider.SelectedItems[0];
            var source = item.Tag as DbSourceStruct;

            using (var frm = new frmSourceEdit(_hosting, source))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    item.Cells[0].Value = source.Name;
                    item.Cells[1].Value = source.ConnectionString;

                    SaveDataSources();
                }
            }
        }

        private void tbtnDelete_Click(object sender, EventArgs e)
        {
            if (!lstProvider.HasSelectedItems)
            {
                return;
            }

            var item = lstProvider.SelectedItems[0];

            if (_hosting.ShowConfirm("确认删除 " + item.Text + "吗?") == ShowMsgButton.No)
            {
                return;
            }

            var source = item.Tag as DbSourceStruct;

            lstProvider.Items.Remove(item);
            _sources.Remove(source);

            SaveDataSources();
            lstProvider.Grouping(true);
        }

        private void tbtnSelect_Click(object sender, EventArgs e)
        {
            if (!lstProvider.HasSelectedItems)
            {
                return;
            }

            var item = lstProvider.SelectedItems[0];
            DbConStr = new DbSourceStruct { Type = item.GroupKey, ConnectionString = item.Cells[1].Value.ToString() };
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void lstProvider_ItemDoubleClick(object sender, TreeListItemEventArgs e)
        {
            tbtnSelect_Click(null, null);
        }

        private void lstProvider_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            tbtnEdit.Enabled = tbtnSelect.Enabled = tbtnDelete.Enabled = tbtnTest.Enabled = lstProvider.HasSelectedItems;
        }
    }
}

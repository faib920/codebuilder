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
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.Swagger
{
    public partial class frmOpenApi : FormBase
    {
        private readonly IDevHosting _hosting;
        private readonly string _url;
        private CancellationTokenSource _cancelToken;

        public frmOpenApi(IDevHosting hosting, Func<IEnumerable<object>> historyGetter, string url = null)
        {
            InitializeComponent();

            Icon = Util.GetIcon();
            _hosting = hosting;
            _url = url;
        }

        public List<Table> Selected { get; private set; }

        public string Url => cboUrl.Text;

        private void frmOpenApi_Load(object sender, EventArgs e)
        {
            var config = Config.Load();
            if (config.SwaggerUrls != null)
            {
                foreach (var u in config.SwaggerUrls)
                {
                    cboUrl.Items.Add(u);
                }
            }

            if (!string.IsNullOrEmpty(_url))
            {
                cboUrl.Text = _url;
                btnOpen_Click(null, null);
            }
            else if (cboUrl.Items.Count == 0)
            {
                cboUrl.Text = "http://localhost:5000/swagger/swagger.json";
            }
        }

        private void frmOpenApi_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _cancelToken != null)
            {
                _cancelToken.Cancel();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _cancelToken?.Cancel();

            base.OnClosing(e);
        }

        private async void btnOpen_Click(object sender, System.EventArgs e)
        {
            if (cboUrl.Text.Trim().Length == 0)
            {
                _hosting.ShowInfo("请复制 Swagger 文档页面中的 swagger.json 链接到文本框中。");
                return;
            }

            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            handler.ClientCertificateOptions = ClientCertificateOption.Automatic;

            var client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromMinutes(5);
            Cursor = Cursors.WaitCursor;
            btnOpen.Enabled = btnOk.Enabled = btnCancel.Enabled = false;

            _cancelToken = new CancellationTokenSource();

            try
            {
                var response = await client.GetAsync(cboUrl.Text, _cancelToken.Token);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    lstTable.Items.Clear();
                    lstTable.BeginUpdate();

                    var json = await response.Content.ReadAsStringAsync();


                    var objects = new SwaggerParser(_hosting.ServiceProvider).Parse(json);
                    if (objects != null)
                    {
                        FillItems(objects);

                        var config = Config.Load();
                        config.AddSwaggerUrl(cboUrl.Text);
                        Config.Save();
                    }
                }
                else
                {
                    _hosting.ShowError("无法解析 Swagger 文档地址" + Environment.NewLine + $"{(int)response.StatusCode}({response.StatusCode}) - {response.ReasonPhrase}");
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp);
            }

            lstTable.EndUpdate();
            Cursor = Cursors.Default;
            btnOpen.Enabled = btnOk.Enabled = btnCancel.Enabled = true;
        }

        private void FillItems(Dictionary<string, List<TNode>> dict)
        {
            foreach (var k in dict)
            {
                var tag = lstTable.Items.Add(k.Key);
                tag.ImageIndex = 0;

                foreach (var v in k.Value)
                {
                    var s = tag.Items.Add(v.Name);
                    s.Cells[1].Value = v.Table.Description;
                    s.Tag = v;
                    s.ImageIndex = 1;
                }

                tag.Expended = true;
            }
        }

        private void lstTable_AfterItemCheckChange(object sender, Fireasy.Windows.Forms.TreeListItemEventArgs e)
        {
            if (e.Item.Level == 0)
            {
                foreach (var item in e.Item.Items)
                {
                    item.Checked = e.Item.Checked;
                }
            }
        }

        private void GetSelectedTables(TreeListItemCollection items)
        {
            foreach (var item in items)
            {
                var t = item.Tag as TNode;
                if (t == null)
                {
                    GetSelectedTables(item.Items);
                }
                else if (item.Checked && !Selected.Any(s => s.Name == t.Name))
                {
                    Selected.Add(t.Table);
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Selected = new List<Table>();

            GetSelectedTables(lstTable.Items);

            if (Selected.Count == 0)
            {
                _hosting.ShowWarn("至少选择一个以上的对象。");
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cboUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOpen_Click(null, null);
        }

        private void cboUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOpen_Click(null, null);
            }
        }
    }
}

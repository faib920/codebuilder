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
using CodeBuilder.Core.Template;
using Fireasy.Windows.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmTemplate : DockFormBase
    {
        internal const string WndName = "Template";

        private readonly DevHosting _hosting;

        public frmTemplate(DevHosting hosting)
        {
            InitializeComponent();
            Config.Instance.AddWindow(WndName).Save();

            Icon = Properties.Resources.template;
            _hosting = hosting;
        }

        public Action<TemplateFile> OpenAct { get; set; }

        public Action<TemplateDefinition> OpenTemplateAct { get; set; }

        public Action TemplateCodeFileChangeAct { get; set; }

        public Action TemplateAct { get; set; }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Config.Instance.RemoveWindow(WndName).Save();
            base.OnFormClosed(e);
        }

        public void Reload()
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                return;
            }

            tlbEdit.Enabled = false;
            _hosting.Template = TemplateParser.Parse(_hosting.Template.ConfigFileName);

            var storage = _hosting.TemplateProvider.GetStorage(_hosting.Template);

            lstPart.Items.Clear();
            lstPart.BeginUpdate();

            FillItems(lstPart.Items, storage.Directories, storage.Files);

            lstPart.EndUpdate();
        }

        private void FillItems(TreeListItemCollection items, List<TemplateDirectory> directories, List<TemplateFile> files)
        {
            foreach (var dir in directories)
            {
                var item = new TreeListItem(dir.Name);
                items.Add(item);

                item.Image = Properties.Resources.category;

                if (TryParseColor(dir.Color, out var color))
                {
                    item.ForeColor = color;
                }

                FillItems(item.Items, dir.Directories, dir.Files);

                item.Expended = true;
            }

            foreach (var file in files)
            {
                var item = new TreeListItem(file.ToString());
                items.Add(item);
                item.Tag = file;
                item.Image = Properties.Resources.fileT;

                if (TryParseColor(file.Color, out var color))
                {
                    item.ForeColor = color;
                }
            }
        }

        private void frmTemplate_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void lstPart_ItemDoubleClick(object sender, TreeListItemEventArgs e)
        {
            if (!(e.Item.Tag is TemplateFile file))
            {
                return;
            }

            OpenAct?.Invoke(file);
        }

        private void lstPart_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            tlbEdit.Enabled = lstPart.HasSelectedItems && lstPart.SelectedItems[0].Tag is TemplateFile;
        }

        private void tlbNew_Click(object sender, EventArgs e)
        {
            using (var frm = new frmTemplateEditor(_hosting))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TemplateAct();
                }
            }
        }

        private void tlbEdit_Click(object sender, EventArgs e)
        {
            if (!lstPart.HasSelectedItems)
            {
                return;
            }

            var item = lstPart.SelectedItems[0];
            if (!(item.Tag is TemplateFile file))
            {
                return;
            }

            OpenAct?.Invoke(file);
        }

        private void tlbRefresh_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void tlbCopy_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            using (var frm = new frmTemplateCopy(_hosting) { Template = _hosting.Template })
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _hosting.ShowInfo("模板复制成功，请从主窗口【模板】菜单中选择。");
                    TemplateAct();
                }
            }
        }

        private void tlbEditAsCode_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            if (OpenTemplateAct != null && _hosting.Template != null)
            {
                OpenTemplateAct(_hosting.Template);
            }
        }

        private void tlbEditTemp_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            using (var frm = new frmTemplateEditor(_hosting) { Template = _hosting.Template })
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TemplateAct();
                    Reload();

                    if (frm.IsCodeFileChanged)
                    {
                        TemplateCodeFileChangeAct?.Invoke();
                    }
                }
            }
        }

        private void tlbHelp_Click(object sender, EventArgs e)
        {
            Process.Start(WebHelper.GetRedirectUrl(_hosting, "/docs/codebuilder-template"));
        }

        private void tlbExport_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            using (var dialog = new SaveFileDialog()
            {
                Filter = "模板定义包(*.tdp)|*.tdp",
                Title = "导出模板",
                FileName = _hosting.Template.Name + ".tdp"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var bytes = TemplateHelper.ZipPackage(_hosting, _hosting.Template);

                    File.WriteAllBytes(dialog.FileName, bytes);
                }
            }
        }

        private async void tlbShare_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            if (!IdentityHelper.CheckAuthorized(_hosting))
            {
                return;
            }

            if (_hosting.ShowConfirm("是否将模板提交到模板商店，以分享给其他 Coder 使用?") == ShowMsgButton.No)
            {
                return;
            }

            var version = _hosting.Template.Version;
            var commitLog = string.Empty;

            using (var frm = new frmTemplateCommit(_hosting) { Version = version })
            {
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                version = frm.Version;
                commitLog = frm.CommitLog;
            }

            var bytes = TemplateHelper.ZipPackage(_hosting, _hosting.Template);

            var client = new HttpClient().AddAccessToken();
            using (var content = new MultipartFormDataContent())
            {
                var filecontent = new ByteArrayContent(bytes);
                filecontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "template.tdp"
                };
                content.Add(filecontent);
                content.Add(new StringContent(_hosting.TemplateProvider.Name), "category");
                content.Add(new StringContent(_hosting.Template.Id), "code");
                content.Add(new StringContent(_hosting.Template.Name), "name");
                content.Add(new StringContent(_hosting.Template.Description), "description");
                content.Add(new StringContent(_hosting.Template.Language), "language");
                content.Add(new StringContent(version.ToString()), "version");
                content.Add(new StringContent(commitLog), "commit");

                var response = await client.PostAsync(Consts.TemplateServerUrl + "/commit", content);
                if (!response.CheckAuthorized(_hosting))
                {
                    return;
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _hosting.ShowError("无法连接到模板服务器。\r\n" + response.ReasonPhrase);
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeAnonymousType(json, new { status = 0, msg = string.Empty });
                if (result.status == 0)
                {
                    _hosting.ShowError(result.msg);
                    return;
                }

                _hosting.Template.Version = version;
                TemplateUnity.SaveTemplateDefinition(_hosting, _hosting.Template);

                _hosting.ShowInfo(result.msg);
            }
        }

        private bool TryParseColor(string strColor, out Color color)
        {
            if (!string.IsNullOrEmpty(strColor))
            {
                var carray = strColor.Split(',');
                color = Color.FromArgb(int.Parse(carray[0]), int.Parse(carray[1]), int.Parse(carray[2]));
                return true;
            }

            color = default(Color);
            return false;
        }
    }
}

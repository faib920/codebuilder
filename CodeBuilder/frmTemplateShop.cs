// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Forms;
using Fireasy.Common.Composition;
using Fireasy.Common.Serialization;
using Fireasy.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmTemplateShop : FormBase
    {
        private int _page = 0;
        private readonly IDevHosting _hosting;

        public frmTemplateShop(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
        }

        public List<string> ChangedTemplates { get; set; } = new List<string>();

        private async void frmTemplateShop_Load(object sender, EventArgs e)
        {
            lvwTemplate.Renderer = new TemplateTreeListRenderer();

            await LoadTemplates();
        }

        private async Task LoadTemplates()
        {
            Cursor = Cursors.WaitCursor;

            var client = new HttpClient();
            OnlineTemplateData data = null;
            try
            {
                var response = await client.GetAsync(Config.Instance.TemplateServerUrl + "?page=" + _page + "&rows=20");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _hosting.ShowError("无法连接到模板服务器。\r\n" + response.ReasonPhrase);
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                data = new JsonSerializer().Deserialize<OnlineTemplateData>(content);
                if (data.LastPage)
                {
                    lvwTemplate.AllowDemandLoadWhenScrollEnd = false;
                }
            }
            catch (SerializationException exp)
            {
                _hosting.ShowError("所提供的服务返回的元数据无效。");
                return;
            }
            catch (Exception exp)
            {
                _hosting.ShowError("无法连接到模板服务器。\r\n" + exp.Message);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            var templates = new List<OnlineTemplate>();
            foreach (var tmp in data.Items)
            {
                if (TemplateUnity.TryGet(tmp.Category, tmp.Code, out TemplateDefinition definition))
                {
                    tmp.IsInstalled = true;
                    tmp.NeedUpdate = definition.Version < tmp.Version;
                    tmp.Template = definition;
                }

                tmp.Weight = tmp.NeedUpdate ? 10 : !tmp.IsInstalled ? 5 : 0;
                templates.Add(tmp);
            }

            foreach (var tmp in templates.OrderByDescending(s => s.Weight))
            {
                var item = new TreeListItem(tmp.Code);
                item.Tag = tmp;
                lvwTemplate.Items.Add(item);
            }
        }

        private void lvwTemplate_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            if (lvwTemplate.SelectedItems.Count == 0)
            {
                btnUpdate.Visible = false;
                return;
            }

            var tmp = lvwTemplate.SelectedItems[0].Tag as OnlineTemplate;

            if (tmp.NeedUpdate)
            {
                btnUpdate.Visible = true;
                btnLocation.Visible = true;
                btnUpdate.Text = "更新(&U)";
            }
            else if (!tmp.IsInstalled)
            {
                btnUpdate.Visible = true;
                btnLocation.Visible = false;
                btnUpdate.Text = "下载(&D)";
            }
            else
            {
                btnUpdate.Visible = false;
                btnLocation.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvwTemplate.SelectedItems.Count == 0)
            {
                return;
            }

            var tmp = lvwTemplate.SelectedItems[0].Tag as OnlineTemplate;
            if (string.IsNullOrEmpty(tmp.PackageUrl))
            {
                _hosting.ShowConfirm("无效的模板路径 PackageUrl。");
                return;
            }

            var msg = "确定" + (tmp.IsInstalled ? "更新" : "下载") + "模板 \"" + tmp.Name + "\" 到本地吗?";
            if (tmp.IsInstalled)
            {
                msg += "\n\r建议更新前对本地模板进行备份。";
            }

            if (_hosting.ShowConfirm(msg) == ShowMsgButton.No)
            {
                return;
            }

            var providers = Imports.GetServices<ITemplateProvider>();
            var provider = providers.FirstOrDefault(s => s.Name == tmp.Category);
            if (provider == null)
            {
                _hosting.ShowWarn("找不到匹配的模板引擎。");
                return;
            }

            Cursor = Cursors.WaitCursor;

            try
            {
                var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync(tmp.PackageUrl);

                UnzipPackage(provider, bytes);

                tmp.IsInstalled = true;
                tmp.NeedUpdate = false;
                lvwTemplate.Invalidate();

                ChangedTemplates.Add(tmp.Code);

                var response = await client.PostAsync(Config.Instance.TemplateServerUrl + "/download/" + tmp.Code, null);

                btnUpdate.Visible = false;
            }
            catch (Exception exp)
            {
                _hosting.ShowError("无法连接到模板路径。" + exp.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UnzipPackage(ITemplateProvider provider, byte[] bytes)
        {
            using (var memory = new MemoryStream(bytes))
            using (var archive = new ZipInputStream(memory))
            {
                ZipEntry entry;
                while ((entry = archive.GetNextEntry()) != null)
                {
                    var directoryName = Path.GetDirectoryName(entry.Name);
                    var fileName = Path.GetFileName(entry.Name);
                    var path = string.Empty;

                    if (directoryName.StartsWith("extension.common"))
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensions", "common");
                    }
                    else if (directoryName.StartsWith("extension.profile"))
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensions", "profile");
                    }
                    else if (directoryName.StartsWith("extension.schema"))
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensions", "schema");
                    }
                    else if (directoryName.StartsWith("template"))
                    {
                        path = provider.WorkDir;
                    }

                    directoryName = directoryName.IndexOf('\\') != -1 ? string.Join("\\", directoryName.Split('\\').Skip(1)) : null;

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        path = Path.Combine(path, directoryName);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var fullName = Path.Combine(path, fileName);

                        using (var writer = File.Create(fullName))
                        {
                            var bufferSize = 2048;
                            var buffer = new byte[2048];

                            while ((bufferSize = archive.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                writer.Write(buffer, 0, bufferSize);
                            }
                        }
                    }
                }
            }
        }

        private async void btnConfig_Click(object sender, EventArgs e)
        {
            if (new frmTemplateShopConfig().ShowDialog() == DialogResult.OK)
            {
                _page = 0;
                lvwTemplate.AllowDemandLoadWhenScrollEnd = true;
                lvwTemplate.Items.Clear();

                await LoadTemplates();
            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            if (lvwTemplate.SelectedItems.Count == 0)
            {
                return;
            }

            var tmp = lvwTemplate.SelectedItems[0].Tag as OnlineTemplate;
            if (!tmp.IsInstalled)
            {
                return;
            }

            var root = Path.Combine(_hosting.TemplateProvider.WorkDir, tmp.Template.Id);
            Process.Start(root);
        }

        private async void lvwTemplate_DemandLoadWhenScrollEnd(object sender, EventArgs e)
        {
            _page++;
            await LoadTemplates();
        }
    }

    internal class TemplateTreeListRenderer : TreeListRenderer
    {
        public override void DrawCell(TreeListCellRenderEventArgs e)
        {
            var high = e.Cell.Item.Selected && e.Cell.Item.TreeList.Focused;
            var color = high ? SystemColors.HighlightText : SystemColors.WindowText;
            var tcolor = high ? SystemColors.HighlightText : Color.Blue;
            var font = new Font("微软雅黑", e.Cell.Item.TreeList.Font.Size + 2);
            var sb = new SolidBrush(color);
            var strFormat = new StringFormat();
            strFormat.Trimming = StringTrimming.EllipsisCharacter;
            strFormat.FormatFlags = StringFormatFlags.NoWrap;
            var tmp = e.Cell.Item.Tag as OnlineTemplate;
            e.Graphics.DrawString(tmp.Name, font, new SolidBrush(tcolor), e.Bounds.X + 10, e.Bounds.Y + 10);
            e.Graphics.DrawString(tmp.Description, e.Cell.Item.TreeList.Font, sb, new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 45, 660, 40), strFormat);

            strFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString("分类:" + tmp.Category, e.Cell.Item.TreeList.Font, sb, e.Bounds.X + 10, e.Bounds.Y + 85, strFormat);
            e.Graphics.DrawString("作者:" + tmp.Author, e.Cell.Item.TreeList.Font, sb, e.Bounds.X + 230, e.Bounds.Y + 85, strFormat);

            e.Graphics.DrawString("版本:" + tmp.Version.ToString("0.00"), e.Cell.Item.TreeList.Font, sb, e.Bounds.X + 440, e.Bounds.Y + 85, strFormat);
            e.Graphics.DrawString("日期:" + tmp.PublishTime, e.Cell.Item.TreeList.Font, sb, e.Bounds.Width - 130, e.Bounds.Y + 85, strFormat);

            if (tmp.NeedUpdate)
            {
                e.Graphics.DrawImage(high ? Properties.Resources.update1 : Properties.Resources.update, e.Bounds.Width - 30, e.Bounds.Y + 10, 24, 24);
            }
            else if (tmp.IsInstalled)
            {
                e.Graphics.DrawImage(high ? Properties.Resources.down_ok1 : Properties.Resources.down_ok, e.Bounds.Width - 30, e.Bounds.Y + 10, 24, 24);
            }

            font.Dispose();
            sb.Dispose();
        }

        public override void DrawCellGridLines(Graphics graphics, Rectangle bounds)
        {
            graphics.DrawLine(new Pen(Color.FromArgb(240, 240, 250)), bounds.X, bounds.Bottom, bounds.Width, bounds.Bottom);
        }
    }
}

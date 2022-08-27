// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
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
    public partial class frmPluginShop : FormBase
    {
        private int _page = 0;
        private readonly IDevHosting _hosting;
        private bool _isNeedRestart;

        public frmPluginShop(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
        }

        public Action OnUpdated { get; set; }

        private async void frmPluginShop_Load(object sender, EventArgs e)
        {
            lvwPlugin.Renderer = new PlugTreeListRenderer1();

            await LoadPlugins();
        }

        private async Task LoadPlugins()
        {
            Cursor = Cursors.WaitCursor;

            var client = new HttpClient();
            OnlinePluginData data = null;
            try
            {
                var filters = new List<string>();
                if (chkSource.Checked )
                {
                    filters.Add("source");
                }
                if (chkTemplate.Checked)
                {
                    filters.Add("template");
                }
                if (chkTool.Checked)
                {
                    filters.Add("tool");
                }

                var response = await client.GetAsync(Config.PluginServerUrl + "?page=" + _page + "&rows=20&filter=" + string.Join("|", filters));
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _hosting.ShowError("无法连接到插件服务器。\r\n" + response.ReasonPhrase);
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                data = new JsonSerializer().Deserialize<OnlinePluginData>(content);
                if (data.LastPage)
                {
                    lvwPlugin.AllowDemandLoadWhenScrollEnd = false;
                }
            }
            catch (SerializationException exp)
            {
                _hosting.ShowError("所提供的服务返回的元数据无效。");
                return;
            }
            catch (Exception exp)
            {
                _hosting.ShowError("无法连接到插件服务器。\r\n" + exp.Message);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            var plugs = new List<OnlinePlugin>();
            foreach (var plug in data.Items)
            {
                var fileName = Path.Combine(Application.StartupPath, plug.Code + ".dll");
                if (File.Exists(fileName))
                {
                    plug.IsInstalled = true;
                    plug.NeedUpdate = VersionHelper.CompareVersion(fileName, plug.Version) > 0;
                }

                plug.Weight = plug.NeedUpdate ? 10 : !plug.IsInstalled ? 5 : 0;
                plugs.Add(plug);
            }

            foreach (var plug in plugs.OrderByDescending(s => s.Weight))
            {
                var item = new TreeListItem(plug.Code);
                item.Tag = plug;
                lvwPlugin.Items.Add(item);
            }
        }

        private void lvwPlugin_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            if (lvwPlugin.SelectedItems.Count == 0)
            {
                btnUpdate.Visible = false;
                return;
            }

            var plugin = lvwPlugin.SelectedItems[0].Tag as OnlinePlugin;

            if (plugin.NeedUpdate)
            {
                btnUpdate.Visible = true;
                btnUpdate.Text = "更新(&U)";
            }
            else if (!plugin.IsInstalled)
            {
                btnUpdate.Visible = true;
                btnUpdate.Text = "下载(&D)";
            }
            else
            {
                btnUpdate.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvwPlugin.SelectedItems.Count == 0)
            {
                return;
            }

            var plugin = lvwPlugin.SelectedItems[0].Tag as OnlinePlugin;
            if (string.IsNullOrEmpty(plugin.PackageUrl))
            {
                _hosting.ShowWarn("无效的插件路径 PackageUrl。");
                return;
            }

            if (!string.IsNullOrEmpty(plugin.ReqVersion) && VersionHelper.CompareVersion(Process.GetCurrentProcess().MainModule.FileName, plugin.ReqVersion) > 0)
            {
                _hosting.ShowWarn("无法" + (plugin.IsInstalled ? "更新" : "下载") + "，需要 CodeBuilder 主程序版本为 " + plugin.ReqVersion + " 及以上。");
                return;
            }

            var msg = "确定" + (plugin.IsInstalled ? "更新" : "下载") + "插件 \"" + plugin.Name + "\" 到本地吗?";

            if (_hosting.ShowConfirm(msg) == ShowMsgButton.No)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            try
            {
                var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync(plugin.PackageUrl);

                UnzipPackage(plugin.IsInstalled, bytes);

                if (plugin.IsInstalled && !_isNeedRestart)
                {
                    _isNeedRestart = true;
                }

                plugin.IsInstalled = true;
                plugin.NeedUpdate = false;
                lvwPlugin.Invalidate();

                OnUpdated?.Invoke();

                var response = await client.PostAsync(Config.PluginServerUrl + "/download/" + plugin.Code, null);

                btnUpdate.Visible = false;
            }
            catch (Exception exp)
            {
                _hosting.ShowWarn("无法连接到插件路径。");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UnzipPackage(bool update, byte[] bytes)
        {
            using (var memory = new MemoryStream(bytes))
            using (var archive = new ZipInputStream(memory))
            {
                ZipEntry entry;
                while ((entry = archive.GetNextEntry()) != null)
                {
                    var directoryName = Path.GetDirectoryName(entry.Name);
                    var fileName = Path.GetFileName(entry.Name);
                    var path = update ? Config.PluginStoragePath : Application.StartupPath;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

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

        private async void lvwTemplate_DemandLoadWhenScrollEnd(object sender, EventArgs e)
        {
            _page++;
            await LoadPlugins();
        }

        private void frmPluginShop_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isNeedRestart)
            {
                _hosting.ShowInfo("更新了插件，但需要重启 CodeBuilder 才能生效。");
            }
        }

        private async void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            _page = 0;
            lvwPlugin.Items.Clear();
            await LoadPlugins();
        }
    }

    internal class PlugTreeListRenderer1 : TreeListRenderer
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
            var tmp = e.Cell.Item.Tag as OnlinePlugin;
            e.Graphics.DrawString(tmp.Name, font, new SolidBrush(tcolor), e.Bounds.X + 10, e.Bounds.Y + 10);
            e.Graphics.DrawString(tmp.Description, e.Cell.Item.TreeList.Font, sb, new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 45, 660, 40), strFormat);

            strFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString("分类:" + tmp.Category, e.Cell.Item.TreeList.Font, sb, e.Bounds.X + 10, e.Bounds.Y + 85, strFormat);
            e.Graphics.DrawString("作者:" + tmp.Author, e.Cell.Item.TreeList.Font, sb, e.Bounds.X + 230, e.Bounds.Y + 85, strFormat);

            e.Graphics.DrawString("版本:" + tmp.Version, e.Cell.Item.TreeList.Font, sb, e.Bounds.X + 440, e.Bounds.Y + 85, strFormat);
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

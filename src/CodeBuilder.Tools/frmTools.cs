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
using CodeBuilder.Core.Tool;
using CodeBuilder.Tools.Tools;
using Fireasy.Windows.Forms;
using Fireasy.Windows.Forms.Theme;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder.Tools
{
    public partial class frmTools : DockFormBase, IContextMenuManager
    {
        private readonly IDevHosting _hosting;

        public frmTools(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
        }

        private void frmTools_Load(object sender, System.EventArgs e)
        {
            listView1.Skin.Item.Selected = new GradientColorSkin(SystemColors.HighlightText, SystemColors.Highlight, SystemColors.Window, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            listView1.Skin.InactiveItem.Selected = new GradientColorSkin(SystemColors.ControlText, SystemColors.ControlDark, SystemColors.Window, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);

            SetToolItem<UrlEncodeConverter>(listView1.Items.Add("Url编码转换器"));
            SetToolItem<Base64Converter>(listView1.Items.Add("Base64编码转换器"));
            SetToolItem<UnicodeConverter>(listView1.Items.Add("Unicode编码转换器"));
            SetToolItem<NamingConverter>(listView1.Items.Add("命名转换器"));
            SetToolItem<TextConverter>(listView1.Items.Add("文本转换器"));
            SetToolItem<GuidGenerator>(listView1.Items.Add("GUID生成器"));
            SetToolItem<CollComparer>(listView1.Items.Add("集合比较器"));
            SetToolItem<FileComparer>(listView1.Items.Add("文件比较器"));
            SetToolItem<CodeDebuger>(listView1.Items.Add("代码调试器"));
            SetToolItem<PythonCodeDebuger>(listView1.Items.Add("Python代码调试器"));

            var defaultTool = RegistryHelper.GetValue<string>("DefaultTool");
            if (!string.IsNullOrEmpty(defaultTool))
            {
                var item = listView1.Items.FirstOrDefault(s => ((Type)s.Tag).Name == defaultTool);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        private TreeListItem SetToolItem<T>(TreeListItem item)
        {
            item.ImageIndex = 0;
            item.Tag = typeof(T);
            return item;
        }

        private void mnuDefault_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            RegistryHelper.SetValue("DefaultTool", ((Type)listView1.SelectedItems[0].Tag).Name);
        }

        private void listView1_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            label1.Visible = false;

            var type = listView1.SelectedItems[0].Tag as Type;
            UserControl control = null;
            foreach (UserControl c in panel1.Controls.OfType<UserControl>())
            {
                if (c.GetType() == type)
                {
                    c.Visible = true;
                    control = c;
                }
                else
                {
                    c.Visible = false;
                }
            }

            if (control == null)
            {
                control = Activator.CreateInstance(type) as UserControl;
                if (control is IDevHostingAccessor aware)
                {
                    aware.Hosting = _hosting;
                }

                control.Dock = DockStyle.Fill;
                control.Visible = false;
                Controls.Add(control);
                panel1.Controls.Add(control);
                control.Visible = true;
            }
        }

        IEnumerable<ToolStripItem> IContextMenuManager.GetContextMenuItems()
        {
            yield return new ToolStripMenuItem("添加桌面快捷方式", null, (o, e) =>
            {
                ToolShortcutHelper.Create(_hosting, "GeneralTools");
            });
        }
    }
}

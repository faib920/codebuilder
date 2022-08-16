// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using CodeBuilder.Tools.Tools;
using Fireasy.Common.Extensions;
using System;
using System.Windows.Forms;

namespace CodeBuilder.Tools
{
    public partial class frmTools : DockFormBase, IClosableDockManaged
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
            listView1.Items.Add("Base64转换器").Tag = typeof(Base64Converter);
            listView1.Items.Add("命名转换器").Tag = typeof(NamingConverter);
            listView1.Items.Add("属性生成器").Tag = typeof(PropertyGenerator);
            listView1.Items.Add("GUID生成器").Tag = typeof(GuidGenerator);
            listView1.Items.Add("集合比较器").Tag = typeof(CollComparer);
        }

        private void listView1_ItemClick(object sender, Fireasy.Windows.Forms.TreeListItemEventArgs e)
        {
            var type = e.Item.Tag as Type;
            UserControl control = null;
            foreach (UserControl c in panel1.Controls)
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
                control = type.New<UserControl>();
                if (control is IDevHostingAccessor aware)
                {
                    aware.Hosting = _hosting;
                }

                control.Location = new System.Drawing.Point(0, 0);
                control.Size = panel1.Size;
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                Controls.Add(control);
                panel1.Controls.Add(control);
            }
        }
    }
}

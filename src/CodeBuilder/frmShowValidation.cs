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
using CodeBuilder.Core.Validations;
using Fireasy.Windows.Forms;
using Fireasy.Windows.Forms.Theme;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmShowValidation : FormBase
    {
        private readonly ValidateResult _result;
        private readonly Action<ValidateEntry> _showValidationAct;

        public frmShowValidation(ValidateResult result, Action<ValidateEntry> showValidationAct)
        {
            InitializeComponent();
            _result = result;
            _showValidationAct = showValidationAct;
        }

        private void frmShowValidation_Load(object sender, System.EventArgs e)
        {
            lstItems.Renderer = new MyTreeListRenderer(imageList1);
            foreach (var entry in _result.GetEntries())
            {
                TreeListItem item = null;
                if (entry.Object == null)
                {
                    item = lstItems.Items.Add(entry.Message);
                }
                else if (entry.Object is Profile profile)
                {
                    item = lstItems.Items.Add($"【变量】{entry.PropertyName} {entry.Message}");
                }
                else if (entry.Object is Table table)
                {
                    item = lstItems.Items.Add($"【属性】表 {table._Name} 的 {entry.PropertyName} {entry.Message}");
                }
                else if (entry.Object is Column column)
                {
                    item = lstItems.Items.Add($"【属性】字段 {column.Owner._Name}.{column._Name} 的 {entry.PropertyName} {entry.Message}");
                }
                else
                {
                    item = lstItems.Items.Add(entry.Message);
                }

                if (item != null)
                {
                    item.Tag = entry;
                }
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            lstItems.Items[0].Selected = true;
            _showValidationAct?.Invoke(lstItems.Items[0].Tag as ValidateEntry);
        }

        private void lstItems_ItemClick(object sender, TreeListItemEventArgs e)
        {
            var item = lstItems.SelectedItems.FirstOrDefault();
            if (item != null)
            {
                _showValidationAct?.Invoke(item.Tag as ValidateEntry);
            }
        }

        private class MyTreeListRenderer: TreeListRenderer
        {
            private readonly ImageList _imageList;

            public MyTreeListRenderer(ImageList imageList)
            {
                _imageList = imageList;
            }

            public override void DrawCell(TreeListCellRenderEventArgs e)
            {
                if (_imageList.Images.Count == 0)
                {
                    return;
                }

                var color = e.DrawState == DrawState.Selected ? e.TreeList.Skin.Item.Selected.TextColor : e.TreeList.Skin.WorkArea.TextColor;
                var strFormat = new StringFormat();
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.FormatFlags = StringFormatFlags.NoWrap;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                var rect = new Rectangle(5, e.Bounds.Top, 16, e.Bounds.Height).Middle(13, 13);

                e.Graphics.UseAntiAlias(() =>
                {
                    e.Graphics.DrawImage(_imageList.Images[0], rect);
                });

                var offsetX = e.Cell.Text?.StartsWith("【") == true ? 16 : 22;
                rect = new Rectangle(offsetX, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height);

                e.Graphics.DrawString(e.Cell.Text, e.TreeList.Font, new SolidBrush(color), rect, strFormat);
            }

            protected override IColorSkin GetItemBackgroundSkin(TreeListItemRenderEventArgs e)
            {
                return e.Item.Selected ? e.TreeList.Skin.Item.Selected : e.TreeList.Skin.WorkArea;
            }
        }
    }
}

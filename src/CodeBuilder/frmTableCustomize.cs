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
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using Fireasy.Windows.Forms;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmTableCustomize : FormBase
    {
        private readonly IDevHosting _hosting;

        public frmTableCustomize(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
        }

        private void frmTableCustomize_Load(object sender, System.EventArgs e)
        {
            var schemaMgr = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();
            var mappers = schemaMgr.GetPropertyMaps<Column>().Where(s => s.IsUICustomized);

            foreach (var c in Config.Instance.Columns)
            {
                var map = mappers.FirstOrDefault(s => s.Name == c);
                if (map != null)
                {
                    var item = lstColumn.Items.Add(map.DisplayName);
                    item.Checked = true;
                    item.Tag = map.Name;
                    item.ImageIndex = 0;
                }
            }
            foreach (var map in mappers.Where(s => !Config.Instance.Columns.Contains(s.Name)))
            {
                var item = lstColumn.Items.Add(map.DisplayName);
                item.Tag = map.Name;
                item.ImageIndex = 0;

                if (Config.Instance.Columns.Count == 0 && (map.Name == nameof(Column.Name) || map.Name == nameof(Column.Description)))
                {
                    item.Checked = true;
                }
            }
        }

        private void lstColumn_ItemDragOver(object sender, Fireasy.Windows.Forms.TreeListItemDragOverEventArgs e)
        {
            if (e.Position == DragPosition.Children || e.Source.Index == 0 || (e.Item.Index == 0 && e.Position == DragPosition.Before))
            {
                e.Dragable = false;
            }
        }

        private void lstColumn_BeforeItemCheckChange(object sender, TreeListItemCancelEventArgs e)
        {
            if (e.Item.Index == 0)
            {
                e.Cancel = true;
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            Config.Instance.Columns = lstColumn.Items.Where(s => s.Checked).Select(s => (string)s.Tag).ToList();
            Config.Instance.Save();

            DialogResult = DialogResult.OK;
        }
    }
}

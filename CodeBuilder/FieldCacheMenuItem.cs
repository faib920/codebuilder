using CodeBuilder.Core.Source;
using CodeBuilder.Core.Variable;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder
{
    public delegate void FieldInsertEventHandler(string field);

    public class FieldCacheMenuItem : ToolStripMenuItem
    {
        private ToolStripMenuItem _mnuTableFields = new ToolStripMenuItem("Table");
        private ToolStripMenuItem _mnuColumnFields = new ToolStripMenuItem("Column");
        private ToolStripMenuItem _mnuReferFields = new ToolStripMenuItem("Reference");
        private ToolStripMenuItem _mnuProfileFields = new ToolStripMenuItem("Profile");

        public event FieldInsertEventHandler OnFieldInsert;

        public FieldCacheMenuItem()
        {
            DropDownItems.Add(_mnuTableFields);
            DropDownItems.Add(_mnuColumnFields);
            DropDownItems.Add(_mnuReferFields);
            DropDownItems.Add(_mnuProfileFields);
            InitInsertMenus();
        }

        private void InitInsertMenus()
        {
            InitMenuFields(_mnuTableFields, SchemaExtensionManager.GetPropertyMaps<Table>());
            InitMenuFields(_mnuColumnFields, SchemaExtensionManager.GetPropertyMaps<Column>());
            InitMenuFields(_mnuReferFields, SchemaExtensionManager.GetPropertyMaps<Reference>());
            InitMenuFields(_mnuProfileFields, ProfileExtensionManager.GetPropertyMaps());
        }

        private void InitMenuFields(ToolStripMenuItem menu, List<PropertyMap> properties)
        {
            foreach (var p in properties)
            {
                var item = new ToolStripMenuItem(string.Format("{0} ({1})", p.Name, p.TypeName));
                item.ToolTipText = p.Description;
                item.Tag = p.Name;
                item.Click += (o, e) =>
                    {
                        OnFieldInsert?.Invoke(item.Tag as string);
                    };

                menu.DropDownItems.Add(item);
            }
        }
    }
}

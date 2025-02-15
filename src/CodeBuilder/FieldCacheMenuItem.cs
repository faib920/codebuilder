// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
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
        private ToolStripMenuItem _mnuIndexFields = new ToolStripMenuItem("Index");
        private ToolStripMenuItem _mnuIndexColumnFields = new ToolStripMenuItem("IndexColumn");
        private ToolStripMenuItem _mnuProfileFields = new ToolStripMenuItem("Profile");
        private readonly IDevHosting _hosting;

        public event FieldInsertEventHandler OnFieldInsert;

        public FieldCacheMenuItem(IDevHosting hosting)
        {
            DropDownItems.Add(_mnuTableFields);
            DropDownItems.Add(_mnuColumnFields);
            DropDownItems.Add(_mnuReferFields);
            DropDownItems.Add(_mnuIndexFields);
            DropDownItems.Add(_mnuIndexColumnFields);
            DropDownItems.Add(_mnuProfileFields);
            _hosting = hosting;
            InitializeMenus();
        }

        public void InitializeMenus()
        {
            var schemaExtManager = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();
            var profileExtManager = _hosting.ServiceProvider.TryGetService<IProfileExtensionManager>();

            InitMenuFields(_mnuTableFields, schemaExtManager.GetPropertyMaps<Table>());
            InitMenuFields(_mnuColumnFields, schemaExtManager.GetPropertyMaps<Column>());
            InitMenuFields(_mnuReferFields, schemaExtManager.GetPropertyMaps<Reference>());
            InitMenuFields(_mnuIndexFields, schemaExtManager.GetPropertyMaps<Index>());
            InitMenuFields(_mnuIndexColumnFields, schemaExtManager.GetPropertyMaps<IndexColumn>());
            InitMenuFields(_mnuProfileFields, profileExtManager.GetPropertyMaps());
        }

        private void InitMenuFields(ToolStripMenuItem menu, List<PropertyMap> properties)
        {
            if (properties == null)
            {
                return;
            }

            menu.DropDownItems.Clear();

            foreach (var p in properties)
            {
                var item = new ToolStripMenuItem(string.Format("{0} ({1})", p.Name, p.TypeName));
                item.ToolTipText = p.Description?.Replace("。", string.Empty);
                item.Tag = p.Name;
                item.Name = "mnuInsert";
                item.Click += (o, e) =>
                    {
                        OnFieldInsert?.Invoke(item.Tag as string);
                    };

                menu.DropDownItems.Add(item);
            }
        }
    }
}

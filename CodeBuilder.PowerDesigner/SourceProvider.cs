// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace CodeBuilder.PowerDesigner
{
    [Export(typeof(ISourceProvider))]
    public class SourceProvider : ISourceProvider
    {
        private IDevHosting _hosting;
        private string _pdmFileName;

        public string Name
        {
            get { return "Power Designer"; }
        }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public List<Table> Preview(SourceOption option)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Power Designer|*.pdm";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }

                _pdmFileName = dialog.FileName;
            }

            var pdm = PdmParser.Parse(_pdmFileName);
            using (var frm = new frmTableSelector(_hosting, pdm))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    return frm.Selected;
                }
            }

            return null;
        }

        public List<Table> GetSchema(List<Table> tables, TableSchemaProcessHandler processHandler)
        {
            try
            {
                return InternalGetSchemas(tables, processHandler);
            }
            catch (Exception exp)
            {
                ErrorMessageBox.Show("获取数据架构时出错。", exp);
                return null;
            }
        }

        private List<Table> InternalGetSchemas(List<Table> tables, TableSchemaProcessHandler processHandler)
        {
            var result = new List<Table>();
            var parser = new PdmParser(_pdmFileName);
            var tableCount = tables.Count;
            var index = 0;

            var calc = new Func<int, int>(i =>
                {
                    return (int)((i / (tableCount * 1.0)) * 100);
                });

            var host = new Host();

            foreach (PdmTable t in tables)
            {
                if (Processor.IsCancellationRequested())
                {
                    return null;
                }

                if (processHandler != null)
                {
                    processHandler(t.Name, calc(++index));
                }

                var table = parser.ParseTable(t);
                if (table == null)
                {
                    continue;
                }

                result.Add(table);
                host.Attach(table);
            }

            parser.ParseReferences(tables, result);

            return result;
        }

        public List<string> GetHistory()
        {
            return new List<string>();
        }
    }
}

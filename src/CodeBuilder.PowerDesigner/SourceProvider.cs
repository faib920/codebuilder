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
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.PowerDesigner
{
    [Export(typeof(ISourceProvider))]
    public class SourceProvider : ISourceProvider, IConfigureSupported
    {
        private IDevHosting _hosting;
        private string _pdmFileName;
        private HistoryStorage<string> _historyStorage;

        public string Name => "Power Designer";

        public Image Icon => Properties.Resources.powerdesigner;

        public string Description => "支持从 Power Designer 设计的 pdm 文件中导入数据表结构。";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
            _historyStorage = new HistoryStorage<string>(Path.Combine(_hosting.WorkPath, "history.powerdesigner"), 10);
        }

        public async Task<List<Table>> PreviewAsync(SourceOption option, CancellationToken cancellationToken = default)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Physical Data Model|*.pdm";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }

                _pdmFileName = dialog.FileName;
            }

            var pdm = PdmParser.Parse(_pdmFileName);
            using (var frm = new frmTableSelector(_hosting, pdm, option.Selected))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    option.Append = frm.Append;

                    _historyStorage.Add(_pdmFileName);
                    _hosting.OnSourceHistoryChanged();

                    return frm.Selected;
                }
            }

            return null;
        }

        public async Task<List<Table>> FromHistoryAsync(object history, SourceOption option, CancellationToken cancellationToken = default)
        {
            _pdmFileName = history.ToString();
            if (!File.Exists(_pdmFileName))
            {
                return null;
            }

            var pdm = PdmParser.Parse(_pdmFileName);
            using (var frm = new frmTableSelector(_hosting, pdm, option.Selected))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    option.Append = frm.Append;

                    _historyStorage.Add(_pdmFileName);
                    _hosting.OnSourceHistoryChanged();

                    return frm.Selected;
                }
            }

            return null;
        }

        public async Task<List<Table>> GetSchemaAsync(List<Table> tables, TableSchemaProcessHandler processHandler, CancellationToken cancellationToken = default)
        {
            try
            {
                return InternalGetSchemas(tables, processHandler, cancellationToken);
            }
            catch (Exception exp)
            {
                ErrorMessageBox.Show("获取数据架构时出错。", exp);
                return null;
            }
        }

        private List<Table> InternalGetSchemas(List<Table> tables, TableSchemaProcessHandler processHandler, CancellationToken cancellationToken = default)
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
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                if (processHandler != null)
                {
                    processHandler(t.Name, calc(++index));
                }

                var table = parser.ParseTable(_hosting.ServiceProvider, t);
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

        public IEnumerable<object> GetHistory()
        {
            return _historyStorage.Items;
        }

        public void ClearHistory()
        {
            _historyStorage.Clear();
        }

        public void DeleteHistory(object record)
        {
            _historyStorage.Delete(record.ToString());
        }

        UserControl IConfigureSupported.GetOptionPanel()
        {
            return new OptionPanel(_hosting);
        }
    }
}

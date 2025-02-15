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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.PDManer
{
    [Export(typeof(ISourceProvider))]
    public class SourceProvider : ISourceProvider, IConfigureSupported
    {
        private IDevHosting _hosting;
        private string _pdmFileName;
        private HistoryStorage<string> _historyStorage;

        public string Name => "PDManer";

        public Image Icon => Properties.Resources.pdmaner;

        public string Description => "支持从 PDManer 设计的 pdma.json 文件中导入数据表结构。";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
            _historyStorage = new HistoryStorage<string>(Path.Combine(_hosting.WorkPath, "history.pdmaner"), 10);
        }

        public async Task<List<Table>> PreviewAsync(SourceOption option, CancellationToken cancellationToken = default)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "PDManer|*.pdma.json";
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
                    _historyStorage.Add(_pdmFileName);
                    _hosting.OnSourceHistoryChanged();

                    return frm.Selected.Select(s => s.Item2).ToList();
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
                    _historyStorage.Add(_pdmFileName);
                    _hosting.OnSourceHistoryChanged();

                    return frm.Selected.Select(s => s.Item2).ToList();
                }
            }

            return null;
        }

        public async Task<List<Table>> GetSchemaAsync(List<Table> tables, TableSchemaProcessHandler processHandler, CancellationToken cancellationToken = default)
        {
            var host = new Host();
            tables.ForEach(s => host.Attach(s));

            return tables;
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

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
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.Swagger
{
    [Export(typeof(ISourceProvider))]
    public class SourceProvider : ISourceProvider
    {
        private IDevHosting _hosting;
        private HistoryStorage<string> _historyStorage;

        public string Name => "Swagger";

        public System.Drawing.Image Icon => Properties.Resources.swagger;

        public string Description => "从 Swagger 的 Json 文档里发现可识别的 Model。";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
            _historyStorage = new HistoryStorage<string>(Path.Combine(_hosting.WorkPath, "history.swagger"), 10);
        }

        public async Task<List<Table>> FromHistoryAsync(object history, SourceOption option, CancellationToken cancellationToken = default)
        {
            var url = history.ToString();
            using (var frm = new frmOpenApi(_hosting, () => GetHistory(), url))
            {
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }

                _historyStorage.Add(url);
                _hosting.OnSourceHistoryChanged();

                return frm.Selected;
            }
        }

        public async Task<List<Table>> GetSchemaAsync(List<Table> tables, TableSchemaProcessHandler processHandler, CancellationToken cancellationToken = default)
        {
            return tables;
        }

        public async Task<List<Table>> PreviewAsync(SourceOption option, CancellationToken cancellationToken = default)
        {
            using (var frm = new frmOpenApi(_hosting, () => GetHistory()))
            {
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }

                _historyStorage.Add(frm.Url);
                _hosting.OnSourceHistoryChanged();

                return frm.Selected;
            }
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
    }
}

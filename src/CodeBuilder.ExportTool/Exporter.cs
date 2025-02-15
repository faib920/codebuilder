// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CB = CodeBuilder.Core.Source;

namespace CodeBuilder.ExportTool
{
    [Export(typeof(IToolProvider))]
    public class Exporter : MultipleToolProviderBase, IAsyncMultipleToolProvider, IMultiplePreExecuteSupported
    {
        private string _fileName;

        public override string Name
        {
            get { return "导出到文件"; }
        }

        public override Form Execute(string name, params object[] arguments)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Form> ExecuteAsync(string name, CancellationToken cancellationToken, params object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                return null;
            }

            switch (arguments[0])
            {
                case "word":
                    await ExportToWord(GetTables(), cancellationToken);
                    break;
                case "excel":
                    await ExportExcel(GetTables(), cancellationToken);
                    break;
                case "excel_multi-sheets":
                    await ExportExcelMultiSheets(GetTables(), cancellationToken);
                    break;
                case "wordTemplate":
                    new WordExporter(_hosting).DesignTemplate();
                    break;
                case "excelTemplate":
                    new ExcelExporter(_hosting).DesignTemplate();
                    break;
            }

            return null;
        }

        public bool CanExecutable(string name, params object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                return false;
            }

            _fileName = null;

            switch (arguments[0])
            {
                case "word":
                    using (var dialog = new SaveFileDialog { Filter = "Word(*.docx)|*.docx", FileName = "数据库设计说明书.docx" })
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            _fileName = dialog.FileName;
                            return true;
                        }
                    }
                    break;
                case "excel":
                case "excel_multi-sheets":
                    using (var dialog = new SaveFileDialog { Filter = "Excel(*.xlsx)|*.xlsx", FileName = "数据库设计说明书.xlsx" })
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            _fileName = dialog.FileName;
                            return true;
                        }
                    }
                    break;
                case "wordTemplate":
                case "excelTemplate":
                    return true;
            }

            return false;
        }

        public override IEnumerable<IToolMenu> SubItems
        {
            get
            {
                return new List<IToolMenu>
                {
                    new ToolMenuItem("导出到 Word", "word"),
                    new ToolMenuItem("导出到 Excel", "excel"),
                    new ToolMenuItem("导出到 Excel（多Sheet）", "excel_multi-sheets"),
                    new ToolMenuSeparator(),
                    new ToolMenuItem("设计 Word 模板", "wordTemplate"),
                    new ToolMenuItem("设计 Excel 模板", "excelTemplate")
                };
            }
        }

        private IEnumerable<CB.Table> GetTables()
        {
            var tables = _hosting.GetTables();
            if (tables.Count() == 0)
            {
                _hosting.ShowWarn("还没有选择数据源。");
                return null;
            }

            return tables;
        }

        private async Task ExportToWord(IEnumerable<CB.Table> tables, CancellationToken cancellationToken)
        {
            if (tables == null || tables.Count() == 0 || string.IsNullOrEmpty(_fileName))
            {
                return;
            }

            try
            {
                new WordExporter(_hosting).Export(tables, _fileName, cancellationToken);
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp);
            }
        }

        private async Task ExportExcel(IEnumerable<CB.Table> tables, CancellationToken cancellationToken)
        {
            if (tables == null || tables.Count() == 0 || string.IsNullOrEmpty(_fileName))
            {
                return;
            }

            try
            {
                new ExcelExporter(_hosting).Export(tables, _fileName, cancellationToken);
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp);
            }
        }

        private async Task ExportExcelMultiSheets(IEnumerable<CB.Table> tables, CancellationToken cancellationToken)
        {
            if (tables == null || tables.Count() == 0 || string.IsNullOrEmpty(_fileName))
            {
                return;
            }

            try
            {
                new ExcelExporter(_hosting).ExportMultiSheets(tables, _fileName, cancellationToken);
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp);
            }
        }
    }
}

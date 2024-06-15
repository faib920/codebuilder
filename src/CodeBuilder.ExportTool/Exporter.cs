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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using CB = CodeBuilder.Core.Source;

namespace CodeBuilder.ExportTool
{
    [Export(typeof(IToolProvider))]
    public class Exporter : MultipleToolProviderBase
    {
        public override string Name
        {
            get { return "导出到文件"; }
        }

        public override Form Execute(string name, object parameter)
        {
            switch (parameter)
            {
                case "word":
                    ExportToWord(GetTables());
                    break;
                case "excel":
                    ExportExcel(GetTables());
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

        public override IEnumerable<IToolMenu> SubItems
        {
            get
            {
                return new List<IToolMenu>
                {
                    new ToolMenuItem("导出到 Word", "word"),
                    new ToolMenuItem("导出到 Excel", "excel"),
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

        private void ExportToWord(IEnumerable<CB.Table> tables)
        {
            if (tables == null || tables.Count() == 0)
            {
                return;
            }

            var fileName = string.Empty;
            using (var dialog = new SaveFileDialog { Filter = "Word(*.docx)|*.docx", FileName = "数据库设计说明书.docx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    new WordExporter(_hosting).Export(tables, dialog.FileName);
                }
            }
        }

        private void ExportExcel(IEnumerable<CB.Table> tables)
        {
            if (tables == null || tables.Count() == 0)
            {
                return;
            }

            var fileName = string.Empty;
            using (var dialog = new SaveFileDialog { Filter = "Excel(*.xlsx)|*.xlsx", FileName = "数据库设计说明书.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    new ExcelExporter(_hosting).Export(tables, dialog.FileName);
                }
            }
        }
    }
}

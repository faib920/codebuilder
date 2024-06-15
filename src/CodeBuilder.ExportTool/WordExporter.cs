// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CB = CodeBuilder.Core.Source;

namespace CodeBuilder.ExportTool
{
    public class WordExporter
    {
        private readonly IDevHosting _hosting;
        private readonly string _template;

        public WordExporter(IDevHosting hosting)
        {
            _hosting = hosting;
            _template = Path.Combine(_hosting.WorkPath, "template.docx");
        }

        /// <summary>
        /// 导出到Word
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="fileName"></param>
        public void Export(IEnumerable<CB.Table> tables, string fileName)
        {
            if (!File.Exists(_template))
            {
                _hosting.ShowError("未找到模板文件 " + _template + "。");
                return;
            }

            using (var tempStream = File.OpenRead(_template))
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var document = new XWPFDocument(tempStream);
                var processor = new TemplateProcessor(document);

                processor.Process(tables);

                processor.Clear();

                document.Write(fileStream);

                Process.Start(fileName);
            }
        }

        public void DesignTemplate()
        {
            if (!File.Exists(_template))
            {
                _hosting.ShowError("未找到模板文件 " + _template + "。");
                return;
            }

            Process.Start(_template);
        }

        private class TemplateProcessor
        {
            private readonly XWPFDocument _document;
            private List<IBodyElement> _elements;
            private int _startIndex = 0;

            public TemplateProcessor(XWPFDocument document)
            {
                _document = document;
                _elements = new List<IBodyElement>();

                XWPFParagraph tempParag = null;

                var index = 0;
                foreach (var element in document.BodyElements)
                {
                    if (element is XWPFParagraph parag && parag.ParagraphText == "Normal.dotm")
                    {
                        tempParag = parag;
                        _startIndex = index;
                        continue;
                    }

                    if (tempParag != null)
                    {
                        _elements.Add(element);
                    }

                    index++;
                }
                _document = document;
            }

            public void Process(IEnumerable<CB.Table> tables)
            {
                foreach (var tb in tables)
                {
                    CloneElements(tb, _elements);
                }
            }

            public void Clear()
            {
                for (var i = 0; i < _elements.Count + 1; i++)
                {
                    _document.RemoveBodyElement(_startIndex);
                }
            }

            private void CloneElements(CB.Table table, IList<IBodyElement> elements)
            {
                foreach (var e in elements)
                {
                    switch (e.ElementType)
                    {
                        case BodyElementType.PARAGRAPH:
                            CloneParagraph(e as XWPFParagraph, table);
                            break;
                        case BodyElementType.TABLE:
                            CloneTable(e as XWPFTable, table);
                            break;
                    }
                }
            }

            private XWPFParagraph CloneParagraph(XWPFParagraph sourceParag, CB.Table table)
            {
                var targetParag = _document.CreateParagraph();
                var sourceCTP = sourceParag.GetCTP();
                var targetCTP = targetParag.GetCTP();

                targetCTP.pPr = sourceCTP.pPr;
                targetCTP.rsidP = sourceCTP.rsidP;
                targetCTP.rsidDel = sourceCTP.rsidDel;

                for (int y = 0; y < sourceParag.Runs.Count; y++)
                {
                    var tbRun = targetParag.CreateRun();
                    var targetRun = tbRun.GetCTR();

                    var run = sourceParag.Runs[y];
                    var runCTR = run.GetCTR();
                    targetRun.rPr = runCTR.rPr;
                    targetRun.rsidRPr = runCTR.rsidRPr;
                    targetRun.rsidR = runCTR.rsidR;
                    var text = targetRun.AddNewT();

                    text.Value = ExpressionHelper.EvaluateTable(run.Text, table);
                }

                return targetParag;
            }

            private XWPFTable CloneTable(XWPFTable sourceTable, CB.Table table)
            {
                var sourceCTTbl = sourceTable.GetCTTbl();

                var targetTable = _document.CreateTable();
                var targetCTTbl = targetTable.GetCTTbl();

                targetCTTbl.tblPr = sourceCTTbl.tblPr;
                targetCTTbl.tblGrid = sourceCTTbl.tblGrid;

                for (int i = 0; i < sourceTable.Rows.Count; i++)
                {
                    var sourceRow = sourceTable.Rows[i];

                    if (IsTemplateRow(sourceRow))
                    {
                        foreach (var column in table.Columns)
                        {
                            CloneRow(sourceRow, targetTable, table, column);
                        }
                    }
                    else
                    {
                        CloneRow(sourceRow, targetTable, table);
                    }
                }

                targetTable.RemoveRow(0);

                return targetTable;
            }

            private XWPFTableRow CloneRow(XWPFTableRow sourceRow, XWPFTable targetTable, CB.Table table, CB.Column column = null)
            {
                var tbRow = targetTable.CreateRow();
                var targetRow = tbRow.GetCTRow();
                tbRow.RemoveCell(0);
                targetRow.trPr = sourceRow.GetCTRow().trPr;
                targetRow.trPr = sourceRow.GetCTRow().trPr;
                targetRow.trPr = sourceRow.GetCTRow().trPr;
                targetRow.trPr = sourceRow.GetCTRow().trPr;
                for (int j = 0; j < sourceRow.GetTableCells().Count; j++)
                {
                    var tbCell = tbRow.CreateCell();
                    tbCell.RemoveParagraph(0);
                    var targetCell = tbCell.GetCTTc();
                    var cell = sourceRow.GetTableCells()[j];
                    targetCell.tcPr = cell.GetCTTc().tcPr;
                    for (int z = 0; z < cell.Paragraphs.Count; z++)
                    {
                        var tbPhs = tbCell.AddParagraph();
                        var para = cell.Paragraphs[z];

                        tbPhs.Alignment = para.Alignment;
                        tbPhs.VerticalAlignment = para.VerticalAlignment;

                        var str = string.Join(string.Empty, para.Runs.Select(v => v.Text));
                        var value = ExpressionHelper.EvaluateTable(str, table);
                        if (column != null)
                        {
                            value = ExpressionHelper.EvaluateColumn(value, column);
                        }

                        var tbRun = tbPhs.CreateRun();
                        var targetRun = tbRun.GetCTR();

                        var run = para.Runs[0];
                        var runCTR = run.GetCTR();
                        targetRun.rPr = runCTR.rPr;
                        targetRun.rsidRPr = runCTR.rsidRPr;
                        targetRun.rsidR = runCTR.rsidR;
                        var text = targetRun.AddNewT();
                        text.Value = value;
                    }
                }

                return tbRow;
            }

            private bool IsTemplateRow(XWPFTableRow row)
            {
                for (int j = 0; j < row.GetTableCells().Count; j++)
                {
                    var cell = row.GetTableCells()[j];

                    for (int z = 0; z < cell.Paragraphs.Count; z++)
                    {
                        var para = cell.Paragraphs[z];

                        var text = string.Join(string.Empty, para.Runs.Select(v => v.Text));
                        if (ExpressionHelper.IsColumnTemplate(text))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}

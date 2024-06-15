// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CB = CodeBuilder.Core.Source;

namespace CodeBuilder.ExportTool
{
    public class ExcelExporter
    {
        private readonly IDevHosting _hosting;
        private readonly string _template;

        public ExcelExporter(IDevHosting hosting)
        {
            _hosting = hosting;
            _template = Path.Combine(_hosting.WorkPath, "template.xlsx");
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
                var workbook = new XSSFWorkbook(tempStream);
                var sheet = workbook.GetSheetAt(0);

                var processor = new TemplateProcessor(sheet);

                processor.Process(tables);

                processor.Clear();

                workbook.Write(fileStream);

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
            private int _lastRowNum;
            private readonly ISheet _sheet;
            private List<CellRangeAddress> _regions;
            private List<int> _addRows = new List<int>();

            public TemplateProcessor(ISheet sheet)
            {
                _lastRowNum = sheet.LastRowNum;
                _sheet = sheet;
                _regions = sheet.MergedRegions;
            }

            public void Process(IEnumerable<CB.Table> tables)
            {
                foreach (var tb in tables)
                {
                    _addRows.Add(_sheet.LastRowNum - _lastRowNum);

                    CloneTable(tb);
                }
            }

            public void Clear()
            {
                //删除模板行
                _sheet.ShiftRows(_lastRowNum + 1, _sheet.LastRowNum, 0 - (_lastRowNum + 1), true, true);

                //重新合并单元格
                foreach (var i in _addRows)
                {
                    foreach (var reg in _regions)
                    {
                        _sheet.AddMergedRegion(new CellRangeAddress(reg.FirstRow + i, reg.LastRow + i, reg.FirstColumn, reg.LastColumn));
                    }
                }
            }

            private void CloneTable(CB.Table table)
            {
                for (var i = 0; i <= _lastRowNum; i++)
                {
                    var sourceRow = _sheet.GetRow(i);

                    if (IsTemplateRow(sourceRow))
                    {
                        foreach (var column in table.Columns)
                        {
                            CloneRow(sourceRow, table, column);
                        }
                    }
                    else
                    {
                        CloneRow(sourceRow, table);
                    }
                }

                _sheet.CreateRow(_sheet.LastRowNum + 1).CreateCell(0);
            }

            private void CloneRow(IRow sourceRow, CB.Table table, CB.Column column = null)
            {
                var targetRow = _sheet.CreateRow(_sheet.LastRowNum + 1);
                targetRow.Height = sourceRow.Height;

                for (var j = 0; j < sourceRow.LastCellNum; j++)
                {
                    var sourceCell = sourceRow.GetCell(j);
                    var targetCell = targetRow.CreateCell(j);

                    targetCell.CellStyle = sourceCell.CellStyle;

                    switch (sourceCell.CellType)
                    {
                        case CellType.Boolean:
                            targetCell.SetCellValue(sourceCell.BooleanCellValue);
                            break;
                        case CellType.Numeric:
                            targetCell.SetCellValue(sourceCell.NumericCellValue);
                            break;
                        case CellType.String:
                            var str = sourceCell.StringCellValue;
                            var value = ExpressionHelper.EvaluateTable(str, table);
                            if (column != null)
                            {
                                value = ExpressionHelper.EvaluateColumn(value, column);
                            }
                            targetCell.SetCellValue(value);
                            break;
                    }
                }

                for (var j = 0; j < sourceRow.LastCellNum; j++)
                {
                    var sourceCell = sourceRow.GetCell(j);
                    var targetCell = targetRow.GetCell(j);

                    foreach (var reg in _regions)
                    {
                        if (!targetCell.IsMergedCell && reg.ContainsRow(sourceRow.RowNum) && reg.ContainsColumn(sourceCell.ColumnIndex))
                        {
                            _sheet.AddMergedRegion(new CellRangeAddress(
                                targetRow.RowNum,
                                targetRow.RowNum,
                                reg.MinColumn,
                                reg.MaxColumn));
                        }
                    }
                }
            }

            private bool IsTemplateRow(IRow row)
            {
                for (var j = 0; j < row.LastCellNum; j++)
                {
                    var sourceCell = row.GetCell(j);
                    if (sourceCell.CellType == CellType.String && ExpressionHelper.IsColumnTemplate(sourceCell.StringCellValue))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}

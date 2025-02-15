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
using NPOI.HSSF.Util;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CB = CodeBuilder.Core.Source;

namespace CodeBuilder.ExportTool
{
    public class ExcelExporter
    {
        private readonly IDevHosting _hosting;
        private readonly string _template;
        private int _catalogRowNum = 0;
        private ICellStyle _lnkStyle;

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
        public void Export(IEnumerable<CB.Table> tables, string fileName, CancellationToken cancellationToken)
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

                var catalogSheet = GetCatalogSheet(workbook);

                var sheetIndex = catalogSheet == null ? 0 : 1;
                var sheet = workbook.GetSheetAt(sheetIndex);

                var processor = new TemplateProcessor(sheet);
                var dicCatalog = processor.Process(tables, (s, i) => _hosting.ShowProgress(s, i), cancellationToken);
                processor.Clear();

                if (catalogSheet != null)
                {
                    var cprocessor = new CatalogProcess(catalogSheet) { Catalog = dicCatalog };
                    cprocessor.Process(tables, null, cancellationToken);
                    cprocessor.Clear();
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                workbook.Write(fileStream);

                Process.Start(fileName);
            }
        }

        /// <summary>
        /// 导出到Word
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="fileName"></param>
        public void ExportMultiSheets(IEnumerable<CB.Table> tables, string fileName, CancellationToken cancellationToken)
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

                var catalogSheet = GetCatalogSheet(workbook);

                var sheetIndex = catalogSheet == null ? 0 : 1;
                var sheet = workbook.GetSheetAt(sheetIndex);

                var dicCatalog = new Dictionary<CB.Table, string>();
                var dicLongName = new Dictionary<string, int>();

                var total = tables.Count();
                var i = 0;

                foreach (var table in tables)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    _hosting.ShowProgress($"正在导出 {table.Name}", (int)((i++ / (total * 1.0)) * 100));

                    var sheetName = GetShortSheetName(dicLongName, table.Name);
                    var newSheet = sheet.CopySheet(sheetName);

                    var processor = new TemplateProcessor(newSheet);
                    dicCatalog.Add(table, processor.Process(table));
                    processor.Clear();
                }

                if (catalogSheet != null)
                {
                    var cprocessor = new CatalogProcess(catalogSheet) { Catalog = dicCatalog };
                    cprocessor.Process(tables, null, cancellationToken);
                    cprocessor.Clear();
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                workbook.RemoveSheetAt(sheetIndex);

                workbook.Write(fileStream);

                Process.Start(fileName);
            }
        }

        private string GetShortSheetName(Dictionary<string, int> dic, string tableName)
        {
            var sheetName = tableName;
            if (sheetName.Length > 30)
            {
                var shortName = sheetName.Substring(0, 30);
                if (!dic.TryGetValue(shortName, out var index))
                {
                    index = 1;
                    dic.Add(shortName, index);
                }
                else
                {
                    index++;
                    dic[shortName] = index;
                }

                return shortName + index;
            }

            return sheetName;
        }

        private ISheet GetCatalogSheet(XSSFWorkbook workbook)
        {
            var sheet = workbook.GetSheet("目录") ?? workbook.GetSheet("Catalog");
            return sheet;
        }

        private void AddTableToCatalog(ISheet sheet, CB.Table table, ISheet linkSheet)
        {
            if (sheet == null)
            {
                return;
            }

            var row = sheet.CreateRow(_catalogRowNum);
            var cell1 = row.CreateCell(0);
            cell1.SetCellValue(table.Name);
            cell1.Hyperlink = new XSSFHyperlink(HyperlinkType.Document) { Address = $"'{table.Name}'!A1" };
            cell1.CellStyle = _lnkStyle;
            var cell2 = row.CreateCell(1);
            cell2.SetCellValue(table.Description);

            _catalogRowNum++;
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
            protected int _lastRowNum;
            protected readonly ISheet _sheet;
            protected List<CellRangeAddress> _regions;
            protected List<int> _addRows = new List<int>();

            public TemplateProcessor(ISheet sheet)
            {
                _lastRowNum = sheet.LastRowNum;
                _sheet = sheet;
                _regions = sheet.MergedRegions;
            }

            public Dictionary<CB.Table, string> Process(IEnumerable<CB.Table> tables, Action<string, int> progress, CancellationToken cancellationToken = default)
            {
                var total = tables.Count();
                var i = 0;

                var dic = new Dictionary<CB.Table, string>();

                foreach (var tb in tables)
                {
                    if (cancellationToken.IsCancellationRequested == true)
                    {
                        break;
                    }

                    progress?.Invoke($"正在导出 {tb.Name}", (int)((i++ / (total * 1.0)) * 100));

                    _addRows.Add(_sheet.LastRowNum - _lastRowNum);

                    dic.Add(tb, CloneTable(tb));
                }

                return dic;
            }

            public string Process(CB.Table table)
            {
                _addRows.Add(_sheet.LastRowNum - _lastRowNum);

                return CloneTable(table);
            }

            public virtual void Clear()
            {
                //删除模板行
                _sheet.ShiftRows(_lastRowNum + 1, _sheet.LastRowNum, 0 - (_lastRowNum + 1), true, true);

                //重新合并单元格
                foreach (var i in _addRows)
                {
                    foreach (var reg in _regions)
                    {
                        try
                        {
                            _sheet.AddMergedRegion(new CellRangeAddress(reg.FirstRow + i, reg.LastRow + i, reg.FirstColumn, reg.LastColumn));
                        }
                        catch { }
                    }
                }
            }

            protected virtual string CloneTable(CB.Table table)
            {
                IRow firstRow = null;

                for (var i = 0; i <= _lastRowNum; i++)
                {
                    var sourceRow = _sheet.GetRow(i);

                    if (IsTemplateRow(sourceRow))
                    {
                        foreach (var column in table.Columns)
                        {
                            var row = CloneRow(sourceRow, table, column);
                            if (firstRow == null)
                            {
                                firstRow = row;
                            }
                        }
                    }
                    else
                    {
                        var row = CloneRow(sourceRow, table);
                        if (firstRow == null)
                        {
                            firstRow = row;
                        }
                    }
                }

                _sheet.CreateRow(_sheet.LastRowNum + 1).CreateCell(0);

                return $"'{_sheet.SheetName}'!A{firstRow.RowNum - _lastRowNum}";
            }

            protected virtual IRow CloneRow(IRow sourceRow, CB.Table table, CB.Column column = null)
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

                return targetRow;
            }

            protected virtual bool IsTemplateRow(IRow row)
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

        private class CatalogProcess : TemplateProcessor
        {
            private int _tempRows = 0;

            public Dictionary<CB.Table, string> Catalog { get; set; }

            public CatalogProcess(ISheet sheet)
                : base(sheet)
            {
                for (var i = 0; i <= _sheet.LastRowNum; i++)
                {
                    var sourceRow = _sheet.GetRow(i);

                    if (IsTemplateRow(sourceRow))
                    {
                        _tempRows++;
                    }
                }
            }

            protected override string CloneTable(CB.Table table)
            {
                for (var i = 0; i <= _lastRowNum; i++)
                {
                    var sourceRow = _sheet.GetRow(i);

                    if (IsTemplateRow(sourceRow))
                    {
                        var row = CloneRow(sourceRow, table, null);
                        if (Catalog?.TryGetValue(table, out var rowNum) == true)
                        {
                            row.GetCell(0).Hyperlink = new XSSFHyperlink(HyperlinkType.Document) { Address = rowNum };
                        }
                    }
                }

                return string.Empty;
            }

            protected override bool IsTemplateRow(IRow row)
            {
                for (var j = 0; j < row.LastCellNum; j++)
                {
                    var sourceCell = row.GetCell(j);
                    if (sourceCell.CellType == CellType.String && ExpressionHelper.IsTableTemplate(sourceCell.StringCellValue))
                    {
                        return true;
                    }
                }

                return false;
            }

            public override void Clear()
            {
                //删除模板行
                _sheet.ShiftRows(_lastRowNum + 1, _sheet.LastRowNum, 0 - _tempRows, true, true);

                //重新合并单元格
                foreach (var i in _addRows)
                {
                    foreach (var reg in _regions)
                    {
                        try
                        {
                            _sheet.AddMergedRegion(new CellRangeAddress(reg.FirstRow + i, reg.LastRow + i, reg.FirstColumn, reg.LastColumn));
                        }
                        catch { }
                    }
                }
            }
        }
    }
}

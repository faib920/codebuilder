using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace CodeBuilder.ExportTool
{
    [Export(typeof(IToolProvider))]
    public class Exporter : IMultipleToolProvider
    {
        private IDevHosting _hosting;

        public string Name
        {
            get { return "导出..."; }
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void Execute(string name, object parameter)
        {
            if (parameter == "template")
            {
                _hosting.ShowInfo("将数据表导致到其他格式的文件。");
                return;
            }

            var tables = _hosting.GetTables();
            if (tables.Count() == 0)
            {
                _hosting.ShowWarn("还没有选择数据源。");
                return;
            }

            switch (parameter)
            {
                case "word":
                    ExportToWord(tables);
                    break;
            }
        }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public IEnumerable<IToolMenu> SubItems
        {
            get
            {
                return new List<IToolMenu>
                {
                    new ToolMenuItem("导致到 WORD", "word"),
                    new ToolMenuItem("导致到 PDF", "pdf"),
                    new ToolMenuSeparator(),
                    new ToolMenuItem("设计 WORD 模板", "template")
                };
            }
        }

        private void ExportToWord(IEnumerable<Table> tables)
        {
            var template = Path.Combine(_hosting.WorkPath, "template.docx");
            if (!File.Exists(template))
            {
                _hosting.ShowError("未找到模板文件 " + template + "。");
                return;
            }

            using (var stream = File.OpenRead(template))
            using (var file = new FileStream(Path.Combine(_hosting.WorkPath, "export1.docx"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                var doc = new XWPFDocument(stream);
                var elements = new List<IBodyElement>(doc.BodyElements);

                foreach (var tb in tables)
                {
                    CloneElements(doc, tb, elements);
                }

                for (var i = 0; i < elements.Count; i++)
                {
                    doc.RemoveBodyElement(0);
                }

                doc.Write(file);
            }
        }

        private void CloneElements(XWPFDocument doc, Table table, IList<IBodyElement> elements)
        {
            foreach (var e in elements)
            {
                switch (e.ElementType)
                {
                    case BodyElementType.PARAGRAPH:
                        CloneParagraph(doc, e as XWPFParagraph);
                        break;
                    case BodyElementType.TABLE:
                        CloneTable(doc, e as XWPFTable);
                        break;
                }
            }
        }

        private XWPFParagraph CloneParagraph(XWPFDocument doc, XWPFParagraph source)
        {
            var ph = doc.CreateParagraph();
            var run = ph.CreateRun();
            run.SetText(source.Text);

            return ph;
        }

        private XWPFTable CloneTable(XWPFDocument doc, XWPFTable source)
        {
            var tt = doc.CreateTable();

            return tt;
        }
    }
}

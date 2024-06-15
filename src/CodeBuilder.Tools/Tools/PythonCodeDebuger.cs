// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Compiler;
using ICSharpCode.TextEditor.Document;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class PythonCodeDebuger : UserControl, IDevHostingAccessor
    {
        private List<string> _assemblies = new List<string>();
        private string _fileName;

        public PythonCodeDebuger()
        {
            InitializeComponent();

            txtCode.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("Python");
        }

        public IDevHosting Hosting { get; set; }

        private void CodeDebuger_Load(object sender, EventArgs e)
        {
            txtCode.Font = new System.Drawing.Font(txtCode.Font.FontFamily, (int)Hosting.GetConfig("FontSize"));

            _assemblies = CodeDebugConfig.LoadConfig(Hosting);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var source = txtCode.Text;
            if (!HasDefMain(source))
            {
                source = $@"
def main():
{IndentText(source, 4)}
";
            }

            ThreadHelper.Start(() =>
            {
                RunCode(source);
            });
        }

        private void RunCode(string code)
        {
            var savedWriter = Console.Out;

            try
            {
                this.Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.WaitCursor;
                }));

                Debug.Listeners.Clear();

                using (var writer = new ConsoleWriter(txtOutput))
                using (var memoryStream = new MemoryStream())
                {
                    Console.SetOut(writer);

                    var engine = Python.CreateEngine();

                    engine.Runtime.IO.SetOutput(memoryStream, writer);

                    var scope = engine.CreateScope();
                    engine.Execute(code, scope);

                    var func = scope.GetVariable("main");
                    if (func != null)
                    {
                        var result = func();

                        var array = memoryStream.ToArray();
                        if (array.Length > 0)
                        {
                            Console.Write(Encoding.UTF8.GetString(array));
                        }

                        if (result != null)
                        {
                            Console.WriteLine("return: " + result);
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: 找不到 main 函数");
                    }
                }
            }
            catch (CodeCompileException exp)
            {
                Hosting.ShowError("代码运行出错:\n" + exp.Message);
            }
            catch (Exception exp)
            {
                Hosting.ShowError(exp);
            }
            finally
            {
                this.Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.Default;
                }));

                Console.SetOut(savedWriter);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Hosting.WorkPath, "code snippets");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var dialog = new SaveFileDialog() { Filter = "Python Source Snippet(*.pysn)|*.pysn", InitialDirectory = path, FileName = GetFileName() })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _fileName = dialog.FileName;
                    File.WriteAllText(dialog.FileName, txtCode.Text);
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Hosting.WorkPath, "code snippets");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var dialog = new OpenFileDialog() { Filter = "Python Source Snippet(*.pysn)|*.pysn", InitialDirectory = path })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _fileName = dialog.FileName;
                    txtCode.Text = File.ReadAllText(dialog.FileName);
                    txtCode.Refresh();
                }
            }
        }

        private string GetFileName()
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                return string.Empty;
            }

            var idx = _fileName.LastIndexOf("\\");
            return idx >= 0 ? _fileName.Substring(idx + 1) : _fileName;
        }

        private string IndentText(string text, int spacesToAdd)
        {
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (var i = 0; i < lines.Length; i++)
            {
                lines[i] = new string(' ', spacesToAdd) + lines[i];
            }

            return string.Join(Environment.NewLine, lines);
        }

        private bool HasDefMain(string text)
        {
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            var hasLineRem = false;

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith("'''"))
                {
                    hasLineRem = !hasLineRem;
                }

                if (hasLineRem)
                {
                    continue;
                }

                if (lines[i].Trim().StartsWith("#"))
                {
                    continue;
                }

                if (lines[i].Trim().StartsWith("def "))
                {
                    return true;
                }
            }

            return false;
        }

        private class ConsoleWriter : TextWriter
        {
            private readonly RichTextBox _textBox;
            private StringBuilder _stringBuilder = new StringBuilder();

            public ConsoleWriter(RichTextBox textBox)
            {
                _textBox = textBox;
            }

            public override void Write(char value)
            {
                if (value == '\n')
                {
                    _textBox.Invoke(new Action(() =>
                    {
                        _textBox.Text += _stringBuilder.ToString();
                        _textBox.Refresh();
                    }));

                    _stringBuilder.Clear();
                }
                else
                {
                    _stringBuilder.Append(value);
                }
            }

            public override Encoding Encoding => Encoding.UTF8;
        }

    }
}

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
using Fireasy.Common.Extensions;
using ICSharpCode.TextEditor.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class CodeDebuger : UserControl, IDevHostingAccessor
    {
        private string _fileName;

        public CodeDebuger()
        {
            InitializeComponent();

            txtCode.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
        }

        public IDevHosting Hosting { get; set; }

        private void CodeDebuger_Load(object sender, EventArgs e)
        {
            txtCode.Font = new System.Drawing.Font(txtCode.Font.FontFamily, (int)Hosting.GetConfig("FontSize"));
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var source = txtCode.Text;
            var usings = new List<string>();
            usings.Add("using System;");
            usings.Add("using System.Collections;");
            usings.Add("using System.Collections.Generic;");

            var assemblies = CodeDebugConfig.GlobalAssemblies.Union(CodeDebugConfig.LoadConfig(Hosting)).Distinct(StringComparer.OrdinalIgnoreCase).Distinct().ToList();

            if (assemblies.Any(s => s.Equals("system.dll", StringComparison.OrdinalIgnoreCase)))
            {
                usings.Add("using System.Text;");
                usings.Add("using System.Text.RegularExpressions;");
            }

            if (assemblies.Any(s => s.Equals("fireasy.common.dll", StringComparison.OrdinalIgnoreCase)))
            {
                usings.Add("using Fireasy.Common;");
                usings.Add("using Fireasy.Common.Extensions;");
            }

            var useCodeBuilderCore = false;
            if (assemblies.Any(s => s.Equals("codebuilder.core.dll", StringComparison.OrdinalIgnoreCase)))
            {
                usings.Add("using CodeBuilder.Core;");
                useCodeBuilderCore = true;
            }

            var newSource = new StringBuilder();

            foreach (var str in source.Split('\r'))
            {
                if (str.Trim().StartsWith("using ") && str.IndexOf("(") == -1)
                {
                    if (!usings.Contains(str.Trim()))
                    {
                        usings.Add(str.Trim());
                    }
                }
                else
                {
                    newSource.AppendLine(str);
                }
            }

            var compilerManager = Hosting.ServiceProvider.TryGetService<ICodeCompilerManager>();
            var compiler = compilerManager.CreateCompiler("c#");

            var options = new ConfigureOptions();
            assemblies.ForEach(s => options.Assemblies.Add(s));

            var code = $@"
{string.Join("\r", usings)}

public class Tester
{{
    public static object DebugWrite({(useCodeBuilderCore ? "IDevHosting Hosting" : "")})
    {{
    {newSource + "\nreturn null;"}
    }}
}}
";
            ThreadHelper.Start(() =>
            {
                RunCode(compiler, options, useCodeBuilderCore, code);
            });
        }

        private void RunCode(ICodeCompiler compiler, ConfigureOptions options, bool useCodeBuilderCore, string code)
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
                {
                    Console.SetOut(writer);

                    object value;
                    if (useCodeBuilderCore)
                    {
                        var func = compiler.CompileDelegate<Func<IDevHosting, object>>(code, options: options);
                        value = func(Hosting);
                    }
                    else
                    {
                        var func = compiler.CompileDelegate<Func<object>>(code, options: options);
                        value = func();
                    }

                    if (value != null)
                    {
                        Console.WriteLine("return: " + value);
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

        private void btnAssembly_Click(object sender, EventArgs e)
        {
            using (var frm = new frmConfigCodeDebugger(Hosting))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Hosting.WorkPath, "code snippets");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var dialog = new SaveFileDialog() { Filter = "C# Source Snippet(*.cssn)|*.cssn", InitialDirectory = path, FileName = GetFileName() })
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

            using (var dialog = new OpenFileDialog() { Filter = "C# Source Snippet(*.cssn)|*.cssn", InitialDirectory = path })
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

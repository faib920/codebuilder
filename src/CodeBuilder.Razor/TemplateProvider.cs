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
using CodeBuilder.Core.Template;
using Fireasy.Common.Compiler;
using Microsoft.CSharp;
using RazorEngine.Compilation;
using RazorEngine.Templating;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Razor;
using System.Web.Razor.Parser;
using System.Windows.Forms;

namespace CodeBuilder.Razor
{
    [Export(typeof(ITemplateProvider))]
    public class TemplateProvider : ITemplateProvider, IConfigureSupported
    {
        private IDevHosting _hosting;
        private static bool _hasReplaceService;

        static TemplateProvider()
        {
            FileTypeHelper.Register(".cshtml", "Razor模板文件|*.cshtml");
        }

        public string Name
        {
            get { return "Razor Template"; }
        }

        public string WorkDir { get; private set; }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
            WorkDir = Path.Combine(_hosting.WorkPath, "templates\\Razor");
        }

        public List<TemplateDefinition> GetTemplates()
        {
            return TemplateParser.ParseAll(WorkDir);
        }

        public TemplateStorage GetStorage(TemplateDefinition definition)
        {
            var storage = new TemplateStorage();

            if (!Directory.Exists(WorkDir))
            {
                return storage;
            }

            var pub = new TemplateDirectory("public");
            var baseFile = Path.Combine(WorkDir, "public", "base.cshtml");
            if (File.Exists(baseFile))
            {
                pub.Files.Add(new TemplateFile("base.cshtml", baseFile, "C#"));
            }

            if (definition.Public?.Count > 0)
            {
                foreach (var item in definition.Public)
                {
                    var info = new FileInfo(Path.Combine(WorkDir, "public", item));
                    if (info.Exists && !pub.Files.Any(s => s.Name.Equals(item, StringComparison.OrdinalIgnoreCase)))
                    {
                        pub.Files.Add(new TemplateFile(item, info.FullName, "C#"));
                    }
                }
            }

            storage.Directories.Add(pub);
            storage.FromDefinition(definition);

            return storage;
        }

        public async Task<GenerateResult> GenerateFilesAsync(TemplateOption option, List<Table> tables, CodeGenerateHandler handler, CancellationToken cancellationToken = default)
        {
            try
            {
                return await GenerateInternalAsync(option, tables, handler, cancellationToken);
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp);
                return null;
            }
        }

        public UserControl GetOptionPanel()
        {
            return new OptionPanel(_hosting);
        }

        private async Task<GenerateResult> GenerateInternalAsync(TemplateOption option, List<Table> tables, CodeGenerateHandler handler, CancellationToken cancellationToken = default)
        {
            _hosting.ConsoleInfo("开始使用 Razor 模板 " + option.Template.Name + " 生成代码");

            var result = new GenerateResult();
            var references = new List<Reference>();
            foreach (var table in tables)
            {
                references.AddRange(table.ForeignKeys);
            }

            var tparts = option.Partitions.Where(s => s.Loop == PartitionLoop.Tables).ToList();
            var nparts = option.Partitions.Where(s => s.Loop == PartitionLoop.None).ToList();
            var count = tparts.Count * tables.Count + nparts.Count;
            var index = 0;

            var calc = new Func<int, int>(i =>
                {
                    return (int)((i / (count * 1.0)) * 100);
                });

            ReplaceCompilerService();
            InitializeNamespaces();

            foreach (var table in tables)
            {
                var model = new { Tables = tables, References = references, Current = table, option.Profile };
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                _hosting.ConsoleInfo($"正在生成表 {table.Name} 的相关部件...");

                foreach (var part in tparts)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    handler?.Invoke(table.Name, calc(++index));

                    var info = new FileInfo(part.FilePath);
                    var content = ReplaceIncludeTemplate(info.DirectoryName, part.Content);
                    var code = RazorEngine.Razor.Parse(content, model);
                    ProcessPartitionCodeFile(part, table, option.Profile, option, code, result);
                }
            }

            foreach (var part in nparts)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                handler?.Invoke("全局", calc(++index));
                _hosting.ConsoleInfo($"正在生成全局的 {part.Name} 部件...");

                var model = new { Tables = tables, References = references, option.Profile };
                var info = new FileInfo(part.FilePath);
                var content = ReplaceIncludeTemplate(info.DirectoryName, part.Content);
                var code = RazorEngine.Razor.Parse(content, model);
                ProcessPartitionCodeFile(part, null, option.Profile, option, code, result);
            }

            if (option.WriteToDisk)
            {
                ResourceWriter.Write(option.Template, option.Profile, option.OutputDirectory);
            }

            return result;
        }

        private void ProcessPartitionCodeFile(PartitionDefinition part, object schema, object profile, TemplateOption option, string content, GenerateResult result)
        {
            var respart = new GeneratePartitionResult(part, content);
            if (option.WriteToDisk && respart.WriteToDisk)
            {
                PartitionWriter.Write(respart, schema, profile, option.OutputDirectory, !option.SkipWhenFileExists);
            }

            result.Partitions.Add(respart);
        }

        private string ReplaceIncludeTemplate(string workDir, string content)
        {
            var regex = new Regex(@"(?is)\@\{include\(([^\{\}]*?)\)\}");
            return regex.Replace(content, delegate (Match m)
                {
                    if (m.Groups.Count < 2)
                    {
                        return m.Groups[0].Value;
                    }

                    var fileName = m.Groups[1].Value;
                    fileName = new Uri(new Uri(workDir, true), fileName, true).AbsolutePath;
                    return GetTemplate(fileName);
                });
        }

        private string GetTemplate(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        private void InitializeNamespaces()
        {
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("System");
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("System.Text");
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("System.Collections");
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("System.Collections.Generic");
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("System.Data");
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("System.Dynamic");
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("Fireasy.Common");
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("Fireasy.Common.Extensions");
            RazorEngine.Razor.DefaultTemplateService.Namespaces.Add("CodeBuilder.Core.Source");
        }

        private void ReplaceCompilerService()
        {
            if (!_hasReplaceService)
            {
                var compilerService = new DynamicCompilerService(_hosting, new RazorEngine.Compilation.CSharp.CSharpRazorCodeLanguage(false), new CSharpCodeProvider(), null);
                var property = typeof(RazorEngine.Razor).GetProperty(nameof(RazorEngine.Razor.DefaultTemplateService), BindingFlags.Public | BindingFlags.Static);
                if (property != null)
                {
                    var templateService = new TemplateService(compilerService);
                    property.SetMethod.Invoke(null, new object[] { templateService });
                }

                _hasReplaceService = true;
            }
        }

        private class DynamicCompilerService : DirectCompilerServiceBase
        {
            private readonly IDevHosting _hosting;
            private readonly CodeDomProvider _codeDomProvider;

            public DynamicCompilerService(IDevHosting hosting, RazorCodeLanguage codeLanguage, CodeDomProvider codeDomProvider, MarkupParser markupParser)
                : base(codeLanguage, codeDomProvider, markupParser)
            {
                _hosting = hosting;
                _codeDomProvider = codeDomProvider;
            }

            public override string BuildTypeNameInternal(Type type, bool isDynamic)
            {
                if (!type.IsGenericType)
                {
                    return type.FullName;
                }

                return type.Namespace + "." + type.Name.Substring(0, type.Name.IndexOf('`')) + "<" + (isDynamic ? "dynamic" : string.Join(", ", from t in type.GetGenericArguments()
                                                                                                                                                select BuildTypeNameInternal(t, CompilerServices.IsDynamicType(t)))) + ">";
            }

            private CompilerResults Compile(TypeContext context)
            {
                var codeCompileUnit = GetCodeCompileUnit(context.ClassName, context.TemplateContent, context.Namespaces, context.TemplateType, context.ModelType);
                var compilerParameters = new CompilerParameters();
                compilerParameters.GenerateInMemory = true;
                compilerParameters.GenerateExecutable = false;
                compilerParameters.IncludeDebugInformation = false;
                compilerParameters.CompilerOptions = "/target:library /optimize";
                compilerParameters.ReferencedAssemblies.AddRange(AssemblyConfig.GlobalAssemblies.Union(AssemblyConfig.LoadConfig(_hosting)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
                return _codeDomProvider.CompileAssemblyFromDom(compilerParameters, codeCompileUnit);
            }

            public override Type CompileType(TypeContext context)
            {
                var compilerResults = Compile(context);
                if (compilerResults.Errors != null && compilerResults.Errors.Count > 0)
                {
                    var sb = new StringBuilder();
                    foreach (CompilerError error in compilerResults.Errors)
                    {
                        if (error.IsWarning)
                        {
                            continue;
                        }

                        sb.AppendLine($"({error.Line},{error.Column}) {error.ErrorText}");
                    }

                    throw new TemplateCompileException($"Razor 模板编译失败!", new CodeCompileException(sb.ToString()));
                }

                return compilerResults.CompiledAssembly.GetType("CompiledRazorTemplates.Dynamic." + context.ClassName);
            }
        }
    }
}

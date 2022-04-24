// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using Fireasy.Common.Caching;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeBuilder.Razor
{
    [Export(typeof(ITemplateProvider))]
    public class TemplateProvider : ITemplateProvider
    {
        private IDevHosting _hosting;

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

            var pub = new TemplateDirectory("public");

            foreach (var f in Directory.GetFiles(Path.Combine(WorkDir, "public"), "*.cshtml"))
            {
                pub.Files.Add(new TemplateFile(new FileInfo(f).Name, f, "C#"));
            }

            storage.Directories.Add(pub);

            foreach (var p in definition.Partitions)
            {
                storage.Files.Add(new TemplateFile(string.Format("{1} ({0})", p.Name, p.FileName), p.FilePath, "C#"));
            }

            return storage;
        }

        public List<GenerateResult> GenerateFiles(TemplateOption option, List<Table> tables, CodeGenerateHandler handler)
        {
            try
            {
                return GenerateInternal(option, tables, handler);
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp.Message);
                return null;
            }
        }

        private List<GenerateResult> GenerateInternal(TemplateOption option, List<Table> tables, CodeGenerateHandler handler)
        {
            var result = new List<GenerateResult>();
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

            InitializeNamespaces();

            foreach (var table in tables)
            {
                var model = new { Tables = tables, References = references, Current = table, option.Profile };
                if (Processor.IsCancellationRequested())
                {
                    break;
                }

                foreach (var part in tparts)
                {
                    if (Processor.IsCancellationRequested())
                    {
                        break;
                    }

                    handler?.Invoke(table.Name, calc(++index));

                    var info = new FileInfo(part.FilePath);
                    var content = ReplaceIncludeTemplate(info.DirectoryName, part.Content);
                    var code = RazorEngine.Razor.Parse(content, model);
                    var r = ProcessPartitionCodeFile(part, table, option.Profile, option, code);
                    if (r != null)
                    {
                        result.Add(r);
                    }
                }
            }

            foreach (var part in nparts)
            {
                if (Processor.IsCancellationRequested())
                {
                    break;
                }

                handler?.Invoke("全局", calc(++index));

                var model = new { Tables = tables, References = references, option.Profile };
                var info = new FileInfo(part.FilePath);
                var content = ReplaceIncludeTemplate(info.DirectoryName, part.Content);
                var code = RazorEngine.Razor.Parse(content, model);
                var r = ProcessPartitionCodeFile(part, null, option.Profile, option, code);
                if (r != null)
                {
                    result.Add(r);
                }
            }

            if (option.WriteToDisk)
            {
                ResourceWriter.Write(option.Template, option.Profile, option.OutputDirectory);
            }

            return result;
        }

        private GenerateResult ProcessPartitionCodeFile(PartitionDefinition part, object schema, object profile, TemplateOption option, string content)
        {
            var result = new GenerateResult(part, content);
            if (option.WriteToDisk && result.WriteToDisk)
            {
                PartitionWriter.Write(result, schema, profile, option.OutputDirectory);
            }

            return result;
        }

        private string ReplaceIncludeTemplate(string workDir, string content)
        {
            var regex = new Regex(@"(?is)\@\{include\(([^\{\}]*?)\)\}");
            return regex.Replace(content, delegate(Match m)
                {
                    if (m.Groups.Count < 2)
                    {
                        return m.Groups[0].Value;
                    }

                    var fileName = m.Groups[1].Value;
                    fileName = new Uri(new Uri(workDir), fileName).AbsolutePath;
                    return GetTemplate(fileName);
                });
        }

        private string GetTemplate(string fileName)
        {
            var cache = CacheManagerFactory.CreateManager();
            var expir = new FileDependency(fileName);
            return cache.TryGet(fileName, () => File.ReadAllText(fileName), () => expir);
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
    }
}

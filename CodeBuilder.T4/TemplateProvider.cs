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
using Microsoft.VisualStudio.TextTemplating;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeBuilder.T4
{
    [Export(typeof(ITemplateProvider))]
    public class TemplateProvider : ITemplateProvider
    {
        private IDevHosting _hosting;

        static TemplateProvider()
        {
            FileTypeHelper.Register(".tt", "TT模板文件|*.tt");
        }

        public string Name
        {
            get { return "VS T4 Template"; }
        }

        public string WorkDir { get; private set; }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
            WorkDir = Path.Combine(_hosting.WorkPath, "templates\\T4");
        }

        public List<TemplateDefinition> GetTemplates()
        {
            return TemplateParser.ParseAll(WorkDir);
        }

        public TemplateStorage GetStorage(TemplateDefinition definition)
        {
            var storage = new TemplateStorage();

            var pub = new TemplateDirectory("public");

            foreach (var f in Directory.GetFiles(Path.Combine(WorkDir, "public"), "*.tt"))
            {
                pub.Files.Add(new TemplateFile(new FileInfo(f).Name, f, "C#"));
            }

            storage.Directories.Add(pub);
            storage.FromDefinition(definition);
            return storage;
        }

        public List<GenerateResult> GenerateFiles(TemplateOption option, List<Table> tables, CodeGenerateHandler handler)
        {
            var scope = new SingleAppDomainScope();
            try
            {
                return GenerateInternal(option, tables, handler);
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp.Message);
                return null;
            }
            finally
            {
                scope.Dispose();
            }
        }

        private List<GenerateResult> GenerateInternal(TemplateOption option, List<Table> tables, CodeGenerateHandler handler)
        {
            var proxyBuilder = new ProxyBuilder();
            var result = new List<GenerateResult>();
            var rtable = proxyBuilder.Rebuild(tables);
            var rprofile = proxyBuilder.Rebuild(option.Profile);
            var guids = new GuidDispatcher();

            var references = new List<dynamic>();
            foreach (var table in rtable)
            {
                references.AddRange(table.ForeignKeys);
            }

            var assemblyList = option.DynamicAssemblies;
            assemblyList.AddRange(proxyBuilder.GetAssemblyList());

            var partitions = option.Partitions.Select(s => s.Name).ToList();

            var path = Path.Combine(_hosting.WorkPath, "templates\\T4");
            var host = new TemplateHost(path, rtable, references, assemblyList, partitions, guids);
            var engine = new Engine();

            host.Profile = rprofile;

            var tparts = option.Partitions.Where(s => s.Loop == PartitionLoop.Tables).ToList();
            var nparts = option.Partitions.Where(s => s.Loop == PartitionLoop.None).ToList();

            var count = tparts.Count * tables.Count + nparts.Count;
            var index = 0;

            var calc = new Func<int, int>(i =>
                {
                    return (int)((i / (count * 1.0)) * 100);
                });

            foreach (var table in rtable)
            {
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

                    host.TemplateFile = part.FilePath;
                    handler?.Invoke(table.Name, calc(++index));

                    host.Current = table;
                    var content = engine.ProcessTemplate(part.Content, host);
                    var r = ProcessPartitionCodeFile(part, host, table, rprofile, option, content);
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

                host.TemplateFile = part.FilePath;
                var content = engine.ProcessTemplate(part.Content, host);
                var r = ProcessPartitionCodeFile(part, host, null, rprofile, option, content);
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

        private GenerateResult ProcessPartitionCodeFile(PartitionDefinition part, TemplateHost host, object schema, object profile, TemplateOption option, string content)
        {
            if (host.Errors.HasErrors)
            {
                var sb = new StringBuilder();
                sb.AppendLine("生成 " + (schema ?? "全局") + " 的 " + part.Name + " 时出错。");
                foreach (CompilerError error in host.Errors)
                {
                    sb.AppendLine(error.ErrorText);
                }

                _hosting.ShowError(sb.ToString());
                return null;
            }
            else
            {
                var result = new GenerateResult(part, content);
                if (option.WriteToDisk && result.WriteToDisk)
                {
                    PartitionWriter.Write(result, schema, profile, option.OutputDirectory);
                }

                return result;
            }
        }
    }
}

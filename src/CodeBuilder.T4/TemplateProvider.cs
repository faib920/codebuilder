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
using Microsoft.VisualStudio.TextTemplating;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.T4
{
    [Export(typeof(ITemplateProvider))]
    public class TemplateProvider : ITemplateProvider, IConfigureSupported
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

            if (!Directory.Exists(WorkDir))
            {
                return storage;
            }

            var pub = new TemplateDirectory("public");

            var baseFile = Path.Combine(WorkDir, "public", "base.tt");
            if (File.Exists(baseFile))
            {
                pub.Files.Add(new TemplateFile("base.tt", baseFile, "C#"));
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
            var scope = new SingleAppDomainScope();
            try
            {
                return await GenerateInternalAsync(option, tables, handler, cancellationToken);
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp);
                return null;
            }
            finally
            {
                scope.Dispose();
            }
        }

        UserControl IConfigureSupported.GetOptionPanel()
        {
            return new OptionPanel(_hosting);
        }

        private async Task<GenerateResult> GenerateInternalAsync(TemplateOption option, List<Table> tables, CodeGenerateHandler handler, CancellationToken cancellationToken = default)
        {
            _hosting.ConsoleInfo("开始使用 T4 模板 " + option.Template.Name + " 生成代码");

            var proxyBuilder = new ProxyBuilder();
            var result = new GenerateResult();
            var proxy = proxyBuilder.Rebuild(_hosting.ServiceProvider, option.Template, option.Profile, tables);
            var guids = new GuidDispatcher();

            var references = new List<dynamic>();
            foreach (var table in proxy.Tables)
            {
                references.AddRange(table.ForeignKeys);
            }

            var assemblyList = option.DynamicAssemblies;
            assemblyList.AddRange(AssemblyConfig.GlobalAssemblies.Union(AssemblyConfig.LoadConfig(_hosting)).Distinct(StringComparer.OrdinalIgnoreCase));
            assemblyList.AddRange(proxyBuilder.GetAssemblyList());

            var partitions = option.Partitions.Select(s => s.Name).ToList();

            var path = Path.Combine(_hosting.WorkPath, "templates\\T4");
            var debugger = new Debugger((_hosting as ILogQueueSupported)?.GetQueue());
            var host = new TemplateHost(path, proxy.Tables, references, assemblyList, partitions, guids, debugger);
            using (var engine = new Engine())
            {
                host.Profile = proxy.Profile;
                host.DbType = tables.FirstOrDefault()?.Host?.DbType;

                var tparts = option.Partitions.Where(s => s.Loop == PartitionLoop.Tables).ToList();
                var nparts = option.Partitions.Where(s => s.Loop == PartitionLoop.None).ToList();

                var count = tparts.Count * tables.Count + nparts.Count;
                var index = 0;

                var calc = new Func<int, int>(i =>
                {
                    return (int)((i / (count * 1.0)) * 100);
                });

                foreach (var table in proxy.Tables)
                {
                    _hosting.ConsoleInfo($"正在生成表 {table.Name} 的相关部件...");

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    foreach (var part in tparts)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return null;
                        }

                        host.TemplateFile = part.FilePath;
                        handler?.Invoke(table.Name, calc(++index));

                        host.Current = table;
                        var content = engine.ProcessTemplate(part.Content, host);
                        ProcessPartitionCodeFile(part, host, table, proxy.Profile, option, content, result);
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

                    host.TemplateFile = part.FilePath;
                    var content = engine.ProcessTemplate(part.Content, host);
                    ProcessPartitionCodeFile(part, host, null, proxy.Profile, option, content, result);
                }

                if (option.WriteToDisk)
                {
                    ResourceWriter.Write(option.Template, option.Profile, option.OutputDirectory);
                }

                return result;
            }
        }

        private void ProcessPartitionCodeFile(PartitionDefinition part, TemplateHost host, dynamic schema, object profile, TemplateOption option, string content, GenerateResult result)
        {
            if (host.Errors.HasErrors)
            {
                var sb = new StringBuilder();
                foreach (CompilerError error in host.Errors)
                {
                    sb.Append($"{Environment.NewLine}  [{error.Line}, {error.Column}] {error.ErrorText}");
                }

                sb.Insert(0, "生成 " + (schema == null ? "全局" : schema.Name) + " 的部件 " + part.Name + " 时出错。");

                if (option.WriteToDisk)
                {
                    _hosting.ConsoleError(sb.ToString());
                }
                else
                {
                    _hosting.ShowError(sb.ToString());
                }

                result.HasError = true;
            }
            else
            {
                var respart = new GeneratePartitionResult(part, content);
                if (option.WriteToDisk && respart.WriteToDisk)
                {
                    PartitionWriter.Write(respart, schema, profile, option.OutputDirectory, !option.SkipWhenFileExists);
                }

                result.Partitions.Add(respart);
            }
        }
    }
}

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
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.NVelocity
{
    [Export(typeof(ITemplateProvider))]
    public class TemplateProvider : ITemplateProvider
    {
        private IDevHosting _hosting;

        static TemplateProvider()
        {
            FileTypeHelper.Register(".vm", "Velocity模板文件|*.vm");
        }

        public string Name
        {
            get { return "NVelocity Template"; }
        }

        public string WorkDir { get; private set; }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
            WorkDir = Path.Combine(_hosting.WorkPath, "templates\\NVelocity");
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

            var baseFile = Path.Combine(WorkDir, "public", "base.vm");
            if (File.Exists(baseFile))
            {
                pub.Files.Add(new TemplateFile("base.vm", baseFile, "C#"));
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
            return null;
        }

        private async Task<GenerateResult> GenerateInternalAsync(TemplateOption option, List<Table> tables, CodeGenerateHandler handler, CancellationToken cancellationToken = default)
        {
            _hosting.ConsoleInfo("开始使用 NVelocity 模板 " + option.Template.Name + " 生成代码");

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

            var engine = new VelocityEngine();

            foreach (var table in tables)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                _hosting.ConsoleInfo($"正在生成表 {table.Name} 的相关部件...");

                var context = new VelocityContext();
                context.Put("Tables", tables);
                context.Put("References", references);
                context.Put("Current", table);
                context.Put("Profile", option.Profile);

                foreach (var part in tparts)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    handler?.Invoke(table.Name, calc(++index));

                    var props = new ExtendedProperties();
                    var info = new FileInfo(part.FilePath);

                    props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
                    props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, info.DirectoryName);
                    props.AddProperty(RuntimeConstants.COUNTER_INITIAL_VALUE, "0");
                    engine.Init(props);

                    ProcessPartitionCodeFile(engine, context, part, table, option, result);
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

                var context = new VelocityContext();
                context.Put("Tables", tables);
                context.Put("References", references);
                context.Put("Profile", option.Profile);

                var props = new ExtendedProperties();
                var info = new FileInfo(part.FilePath);

                props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
                props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, info.DirectoryName);
                props.AddProperty(RuntimeConstants.COUNTER_INITIAL_VALUE, "0");
                engine.Init(props);

                ProcessPartitionCodeFile(engine, context, part, null, option, result);
            }

            return result;
        }

        private void ProcessPartitionCodeFile(VelocityEngine engine, VelocityContext context, PartitionDefinition part, object schema, TemplateOption option, GenerateResult result)
        {
            using (var writer = new StringWriter())
            {
                var info = new FileInfo(part.FilePath);

                engine.MergeTemplate(info.Name, "utf-8", context, writer);
                var r = new GeneratePartitionResult(part, writer.ToString());
                if (option.WriteToDisk && r.WriteToDisk)
                {
                    PartitionWriter.Write(r, schema, option.Profile, option.OutputDirectory, !option.SkipWhenFileExists);
                }

                result.Partitions.Add(r);
            }
        }

    }
}

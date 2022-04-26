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
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

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
            get { return "NVelocity"; }
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

            foreach (var f in Directory.GetFiles(Path.Combine(WorkDir, "public"), "*.vm"))
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
                _hosting.ShowError(exp);
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

            var engine = new VelocityEngine();

            foreach (var table in tables)
            {
                var context = new VelocityContext();
                context.Put("Tables", tables);
                context.Put("References", references);
                context.Put("Current", table);
                context.Put("Profile", option.Profile);

                foreach (var part in option.Partitions)
                {
                    var props = new ExtendedProperties();
                    var info = new FileInfo(part.FilePath);

                    props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
                    props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, info.DirectoryName);
                    props.AddProperty(RuntimeConstants.COUNTER_INITIAL_VALUE, "0");
                    engine.Init(props);

                    using (var writer = new StringWriter())
                    {
                        engine.MergeTemplate(info.Name, "utf-8", context, writer);
                        var r = new GenerateResult(part, writer.ToString());
                        if (option.WriteToDisk && r.WriteToDisk)
                        {
                            PartitionWriter.Write(r, table, option.Profile, option.OutputDirectory);
                        }

                        result.Add(r);
                    }
                }
            }

            return result;
        }
    }
}

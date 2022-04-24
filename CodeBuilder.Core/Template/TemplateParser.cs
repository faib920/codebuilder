// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Logging;
using Fireasy.Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 模板解析器。
    /// </summary>
    public class TemplateParser
    {
        public static TemplateDefinition Parse(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var path = new FileInfo(filePath).Directory.FullName;

            try
            {
                var fileName = new FileInfo(filePath).Name;
                fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                var cfgContent = File.ReadAllText(filePath);
                var json = new JsonSerializer();
                var definition = json.Deserialize<TemplateDefinition>(cfgContent);
                if (definition != null)
                {
                    definition.TemplateDirectory = Path.Combine(path, fileName);
                    definition.Id = fileName;
                    definition.TId = Path.GetFileName(path) + "." + fileName;
                    definition.ConfigFileName = filePath;
                }

                FormatFilePath(definition.Groups, definition.Partitions, path, fileName);
                definition.Resources.AddRange(LoadResources(Path.Combine(path, fileName)));

                return definition;
            }
            catch (Exception exp)
            {
                var logger = LoggerFactory.CreateLogger();
                if (logger != null)
                {
                    logger.Error("解析模板定义文件 " + filePath + " 时出错。", exp);
                }

                return null;
            }
        }

        public static List<TemplateDefinition> ParseAll(string path)
        {
            var templates = new List<TemplateDefinition>();

            if (!Directory.Exists(path))
            {
                return templates;
            }

            foreach (var fileName in Directory.GetFiles(path, "*.template"))
            {
                var definition = TemplateParser.Parse(fileName);
                if (definition != null)
                {
                    templates.Add(definition);
                }
            }

            return templates;
        }

        private static string[] LoadResources(string path)
        {
            path = Path.Combine(path, "Resources");
            if (!Directory.Exists(path))
            {
                return new string[0];
            }

            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Select(s => s.Substring(path.Length + 1)).ToArray();
        }

        private static void FormatFilePath(List<GroupDefinition> groups, List<PartitionDefinition> partitions, string path, string fileName)
        {
            foreach (var g in groups)
            {
                FormatFilePath(g.Groups, g.Partitions, path, fileName);
            }

            partitions.ForEach(s => s.FilePath = Path.Combine(path, fileName, s.FileName));
        }
    }
}

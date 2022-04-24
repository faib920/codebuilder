// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Extensions;
using Fireasy.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CodeBuilder
{
    public class Config
    {
        private Font _font;

        static Config()
        {
            Instance = Read();
        }

        public static Config Instance;

        public string TemplateProvider { get; set; }

        public string TemplateFileName { get; set; }

        public string OutputDirectory { get; set; }

        public string Encoding { get; set; }

        public string Profile { get; set; }

        public bool CheckUpdate { get; set; }

        public bool Source_View { get; set; }

        public static string PluginServerUrl { get; } = "http://fireasy.cn/api/codebuilder/plugins";

        public static string PluginStoragePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "codebuilder_plugins");

        public string TemplateServerUrl { get; set; }

        public static Config Read()
        {
            var json = new JsonSerializer();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "app.cfg");
            var content = File.ReadAllText(path);
            return json.Deserialize<Config>(content);
        }

        public static void Save()
        {
            var option = new JsonSerializeOption { Indent = true };
            var json = new JsonSerializer(option);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "app.cfg");
            var content = json.Serialize(Config.Instance);
            File.WriteAllText(path, content, System.Text.Encoding.UTF8);
        }

        public static List<string> GetPartitionConfig(string template)
        {
            var json = new JsonSerializer();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "prebuild.cfg");
            if (!File.Exists(path))
            {
                return null;
            }

            var content = File.ReadAllText(path);
            var dict = json.Deserialize<Dictionary<string, string>>(content);
            if (dict.TryGetValue(template, out string str))
            {
                return str.Split(',').ToList();
            }

            return null;
        }

        public static void SavePartitionConfig(string template, List<string> partitions)
        {
            var dict = new Dictionary<string, string>();

            var json = new JsonSerializer();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "prebuild.cfg");
            if (File.Exists(path))
            {
                var content1 = File.ReadAllText(path);
                dict = json.Deserialize<Dictionary<string, string>>(content1);
            }

            dict.AddOrReplace(template, string.Join(",", partitions));

            var content = json.Serialize(dict);
            File.WriteAllText(path, content, System.Text.Encoding.UTF8);
        }
    }
}

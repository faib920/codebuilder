// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeBuilder
{
    public class Config
    {
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

        public bool SkipWhenFileExists { get; set; }

        public static string PluginStoragePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "codebuilder_plugins");

        public List<string> Windows { get; set; } = new List<string>();

        public Dictionary<string, int> WindowSets { get; set; } = new Dictionary<string, int>();

        public TemplateGroupStyle TemplateGroup { get; set; }

        public List<string> Columns { get; set; }

        public bool Guided { get; set; }

        public int FontSize { get; set; } = 10;

        public string UserAccount { get; set; }

        public string AccessToken { get; set; }

        public int LogLevel { get; set; }

        public TemplateAnalysisConfig TemplateAnalysis { get; set; } = new TemplateAnalysisConfig();

        public class TemplateAnalysisConfig
        {
            public string Directory { get; set; }

            public string Ignore { get; set; } = "debug;obj;bin;*.dll;*.exe";

            public int Sample { get; set; } = 4;
        }

        public Config AddWindow(string wnd)
        {
            if (!Windows.Contains(wnd))
            {
                Windows.Add(wnd);
            }

            return this;
        }

        public Config RemoveWindow(string wnd)
        {
            if (Windows.Contains(wnd))
            {
                Windows.Remove(wnd);
            }

            return this;
        }

        public void Save()
        {
            var path = Path.Combine(Util.GetWorkPath(), "config", "app.cfg");
            var content = JsonConvert.SerializeObject(this);

            Util.TryOperateFile(path, () => File.WriteAllText(path, content, System.Text.Encoding.UTF8));
        }

        public static Config Read()
        {
            var path = Path.Combine(Util.GetWorkPath(), "config", "app.cfg");
            var content = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<Config>(content);

            if (!content.Contains("\"" + nameof(Windows) + "\""))
            {
                config.AddWindow(frmExtension.WndName).AddWindow(frmTemplate.WndName);
            }

            return config;
        }

        public static List<string> GetPartitionConfig(string template)
        {
            var path = Path.Combine(Util.GetWorkPath(), "config", "prebuild.cfg");
            if (!File.Exists(path))
            {
                return null;
            }

            var content = File.ReadAllText(path);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            if (dict.TryGetValue(template, out string str))
            {
                return str.Split(',').ToList();
            }

            return null;
        }

        public static void SavePartitionConfig(string template, List<string> partitions)
        {
            var dict = new Dictionary<string, string>();

            var path = Path.Combine(Util.GetWorkPath(), "config", "prebuild.cfg");
            if (File.Exists(path))
            {
                var content1 = File.ReadAllText(path);
                dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(content1);
            }

            dict.AddOrReplace(template, string.Join(",", partitions));

            var content = JsonConvert.SerializeObject(dict);

            Util.TryOperateFile(path, () => File.WriteAllText(path, content, System.Text.Encoding.UTF8));
        }

        public enum TemplateGroupStyle
        {
            None = 0,
            Language,
            Category
        }
    }
}

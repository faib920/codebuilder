// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeBuilder
{
    public class PlugInConfig
    {
        public List<string> Installing { get; set; } = new List<string>();

        public List<string> Removed { get; set; } = new List<string>();

        public static void Remove(List<string> items)
        {
            var json = new JsonSerializer();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "plugin.cfg");
            PlugInConfig cfg;
            string content;
            if (!File.Exists(path))
            {
                cfg = new PlugInConfig();
            }
            else
            {
                content = File.ReadAllText(path);
                cfg = json.Deserialize<PlugInConfig>(content);
            }
            cfg.Removed.AddRange(items);
            content = json.Serialize(cfg);
            File.WriteAllText(path, content, Encoding.UTF8);
        }

        public static PlugInConfig Get()
        {
            var json = new JsonSerializer();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "plugin.cfg");
            if (!File.Exists(path))
            {
                return new PlugInConfig();
            }
            var content = File.ReadAllText(path);
            return json.Deserialize<PlugInConfig>(content);
        }

        public static void Clear()
        {
            var json = new JsonSerializer();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "plugin.cfg");
            if (!File.Exists(path))
            {
                return;
            }
            var cfg = new PlugInConfig();
            var content = json.Serialize(cfg);
            File.WriteAllText(path, content, Encoding.UTF8);
        }
    }
}

// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Serialization;
using Fireasy.Windows.Forms;
using Newtonsoft.Json;
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
                cfg = JsonConvert.DeserializeObject<PlugInConfig>(content);
            }
            cfg.Removed.AddRange(items);
            content = JsonConvert.SerializeObject(cfg);

            Util.TryOperateFile(path, () => File.WriteAllText(path, content, Encoding.UTF8));
        }

        public static PlugInConfig Get()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "plugin.cfg");
            if (!File.Exists(path))
            {
                return new PlugInConfig();
            }
            var content = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<PlugInConfig>(content);
        }

        public static void Clear()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "plugin.cfg");
            if (!File.Exists(path))
            {
                return;
            }
            var cfg = new PlugInConfig();
            var content = JsonConvert.SerializeObject(cfg);

            Util.TryOperateFile(path, () => File.WriteAllText(path, content, Encoding.UTF8));
        }
    }
}

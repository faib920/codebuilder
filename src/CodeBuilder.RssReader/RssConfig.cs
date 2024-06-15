// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeBuilder.RssReader
{
    public class RssConfig
    {
        public List<RssCategory> View { get; set; } = new List<RssCategory>();

        public List<RssFavCategory> Favorite { get; set; } = new List<RssFavCategory>();

        public static RssConfig LoadConfig()
        {
            var path = Path.Combine(Util.GetWorkPath(), "config", "rss.cfg");
            if (!File.Exists(path))
            {
                return new RssConfig();
            }

            var content = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<RssConfig>(content);

            config.View.Insert(0, new RssCategory
            {
                IsSystem = true,
                Items = new List<RssItem>
                {
                    new RssItem
                    {
                        Name = "fireasy.cn",
                        Url = "http://www.fireasy.cn/rss",
                        Checked = true
                    }
                }
            });

            return config;
        }

        public static void SaveConfig(RssConfig config)
        {
            var path = Path.Combine(Util.GetWorkPath(), "config", "rss.cfg");
            config.View = config.View.Where(s => s.IsSystem == false || s.IsSystem == null).ToList();
            var content = JsonConvert.SerializeObject(config, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            File.WriteAllText(path, content, Encoding.UTF8);
        }
    }

    public class RssCategory
    {
        public string Name { get; set; }

        public bool? IsSystem { get; set; }

        public List<RssItem> Items { get; set; } = new List<RssItem>();
    }

    public class RssFavCategory
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public List<RssFavCategory> Children { get; set; } = new List<RssFavCategory>();
    }

    public class RssItem
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public bool? Checked { get; set; }
    }
}

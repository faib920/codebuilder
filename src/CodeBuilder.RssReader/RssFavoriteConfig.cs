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
    public class RssFavoriteConfig
    {
        public static List<RssFavoriteItem> LoadFavorites(List<string> catgoryIds)
        {
            var path = Path.Combine(Util.GetWorkPath(), "config", "rss_favs.cfg");
            if (!File.Exists(path))
            {
                return new List<RssFavoriteItem>();
            }

            var content = File.ReadAllText(path);
            var items = JsonConvert.DeserializeObject<List<RssFavoriteItem>>(content);

            if (catgoryIds?.Count > 0)
            {
                return items.Where(s => catgoryIds.Contains(s.CategoryId)).ToList();
            }

            return items;
        }

        public static void AddFavorite(string categoryId, string title, string url, string desc)
        {
            List<RssFavoriteItem> items = null;
            var path = Path.Combine(Util.GetWorkPath(), "config", "rss_favs.cfg");
            if (!File.Exists(path))
            {
                items = new List<RssFavoriteItem>();
            }
            else
            {
                var content = File.ReadAllText(path);
                items = JsonConvert.DeserializeObject<List<RssFavoriteItem>>(content);
            }

            var item = items.FirstOrDefault(s => s.Url == url);
            if (item == null)
            {
                items.Add(new RssFavoriteItem
                {
                    CategoryId = categoryId,
                    Name = title,
                    Url = url,
                    Description = desc,
                });
            }
            else
            {
                item.Name = title;
                item.CategoryId = categoryId;
            }

            var content1 = JsonConvert.SerializeObject(items);
            File.WriteAllText(path, content1, Encoding.UTF8);
        }

        public static void RemoveFavorite(string url)
        {
            List<RssFavoriteItem> items = null;
            var path = Path.Combine(Util.GetWorkPath(), "config", "rss_favs.cfg");
            if (!File.Exists(path))
            {
                items = new List<RssFavoriteItem>();
            }
            else
            {
                var content = File.ReadAllText(path);
                items = JsonConvert.DeserializeObject<List<RssFavoriteItem>>(content);
            }

            if (items.RemoveAll(s => s.Url == url) > 0)
            {
                var content1 = JsonConvert.SerializeObject(items, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                File.WriteAllText(path, content1, Encoding.UTF8);
            }
        }
    }

    public class RssFavoriteItem
    {
        public string CategoryId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
    }

}

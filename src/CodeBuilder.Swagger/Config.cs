// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CodeBuilder.Swagger
{
    public class Config
    {
        private static Config _instance = null;
        public List<string> SwaggerUrls { get; set; } = new List<string>();

        public static Config Load()
        {
            if (_instance == null)
            {
                var fileName = Path.Combine(DevHostingHolder.Instance.WorkPath, "config", "swagger.cfg");
                if (File.Exists(fileName))
                {
                    var content = File.ReadAllText(fileName, Encoding.UTF8);
                    _instance = JsonSerializer.Deserialize<Config>(content);
                }
                else
                {
                    _instance = new Config();
                }
            }

            return _instance;
        }

        public static void Save()
        {
            var fileName = Path.Combine(DevHostingHolder.Instance.WorkPath, "config", "swagger.cfg");
            var content = JsonSerializer.Serialize(_instance);
            File.WriteAllText(fileName, content, Encoding.UTF8);
        }

        public void AddSwaggerUrl(string url)
        {
            _instance.SwaggerUrls.RemoveAll(s => s.Equals(url, System.StringComparison.OrdinalIgnoreCase));
            _instance.SwaggerUrls.Insert(0, url);
        }
    }
}

// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CodeBuilder.PowerDesigner
{
    public class DbTypeManager
    {
        static readonly Dictionary<string, Dictionary<string, string>> _cache = new Dictionary<string, Dictionary<string, string>>();

        static DbTypeManager()
        {
            var configFileName = Path.Combine(DevHostingHolder.Instance.WorkPath, "config", "pd.cfg");
            var json = new JsonSerializer();
            _cache = json.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(configFileName));
        }

        public static DbType? GetDbType(string databaseType, string dataType)
        {
            var sub = _cache.FirstOrDefault(s => databaseType.Contains(s.Key));

            if (sub.Value != null)
            {
                if (sub.Value.TryGetValue(dataType.ToLower(), out string dbType))
                {
                    if (Enum.TryParse(dbType, out DbType result))
                    {
                        return result;
                    }
                }

                throw new ArgumentException("在配置文件 config\\pd.cfg 的 " + sub.Key + " 配置中找不到类型 " + dataType + " 的映射，你可以自行添加相应的配置。");
            }

            throw new ArgumentException("在配置文件 config\\pd.cfg 中找不到与 " + databaseType + " 数据库类型相匹配的类型映射，你可以自行添加相应的配置。");
        }
    }
}

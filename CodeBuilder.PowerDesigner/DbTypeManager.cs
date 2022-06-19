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
            Dictionary<string, string> sub = _cache.FirstOrDefault(s => databaseType.Contains(s.Key)).Value;

            if (sub != null)
            {
                if (sub.TryGetValue(dataType.ToLower(), out string dbType))
                {
                    if (Enum.TryParse(dbType, out DbType result))
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }
}

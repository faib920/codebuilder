// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace CodeBuilder.PowerDesigner
{
    public class DBMSManager
    {
        static Dictionary<string, string> _mappers = new Dictionary<string, string>();
        static string _configFileName = string.Empty;

        static DBMSManager()
        {
            _configFileName = Path.Combine(DevHostingHolder.Instance.WorkPath, "config", "pd.cfg");
            if (!File.Exists(_configFileName))
            {
                _configFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "pd.cfg");
            }

            _mappers = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_configFileName));
        }

        public static DbType? GetDbType(string databaseType, string dataType)
        {
            foreach (var map in _mappers)
            {
                if (databaseType.Contains(map.Key))
                {
                    return DataTypeManager.GetDataType(map.Value, dataType);
                }
            }

            throw new ArgumentException("在配置文件 config\\pd.cfg 中找不到 " + databaseType + " 对应于标准的数据库数据类型配置，你可以自行添加相应的配置。");
        }

        public static Dictionary<string, string> GetMappers()
        {
            return _mappers;
        }

        public static void SaveMappers(Dictionary<string, string> mappers)
        {
            _mappers = mappers;
            File.WriteAllText(_configFileName, JsonConvert.SerializeObject(mappers), Encoding.UTF8);
        }
    }
}

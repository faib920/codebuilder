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
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace CodeBuilder.PDManer
{
    public class DbTypeManager
    {
        static string _configFileName;
        static Dictionary<string, string> _mappers = new Dictionary<string, string>();

        static DbTypeManager()
        {
            _configFileName = Path.Combine(DevHostingHolder.Instance.WorkPath, "config", "pdman.cfg");
            if (!File.Exists(_configFileName))
            {
                _configFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "pdman.cfg");
            }
            _mappers = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_configFileName));
        }

        public static DbType? GetDbType(string dataType)
        {
            if (_mappers.TryGetValue(dataType.ToLower(), out string dbType))
            {
                if (Enum.TryParse(dbType, out DbType result))
                {
                    return result;
                }
            }

            throw new ArgumentException("在配置文件 config\\pdman.cfg 中找不到类型 " + dataType + " 的映射，你可以自行添加相应的配置。");
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

// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 数据类型管理器。
    /// </summary>
    public static class DataTypeManager
    {
        private static Dictionary<string, Dictionary<string, DbType>> _dataTypes;

        /// <summary>
        /// 获取数据库类型。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetDatabaseKeys()
        {
            return GetConfig().Keys;
        }

        /// <summary>
        /// 获取指定数据库的数据类型字典。
        /// </summary>
        /// <param name="database">数据库标识</param>
        /// <returns></returns>
        public static Dictionary<string, DbType> GetDataTypes(string database)
        {
            if (!string.IsNullOrEmpty(database) && GetConfig().TryGetValue(database, out var dataTypes))
            {
                return dataTypes;
            }

            return new Dictionary<string, DbType>();
        }

        /// <summary>
        /// 获取指定数据库对应字段类型的 <see cref="DbType"/>。
        /// </summary>
        /// <param name="database">数据库标识。</param>
        /// <param name="dataType">数据类型。</param>
        /// <returns></returns>
        public static DbType? GetDataType(string database, string dataType)
        {
            if (!string.IsNullOrEmpty(database) && GetConfig().TryGetValue(database, out var dataTypes))
            {
                foreach (var key in dataTypes.Keys.Where(s => !s.StartsWith("@")))
                {
                    if (!key.StartsWith("@") && key.Equals(dataType, StringComparison.OrdinalIgnoreCase))
                    {
                        return dataTypes[key];
                    }
                    else if (key.StartsWith("@") && Regex.IsMatch(dataType, key))
                    {
                        return dataTypes[key];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 保存对应数据库的数据类型字典。
        /// </summary>
        /// <param name="database">数据库标识。</param>
        /// <param name="dataTypes">数据类型字典。</param>
        public static void SaveDataTypes(string database, Dictionary<string, DbType> dataTypes) 
        {
            if (string.IsNullOrEmpty(database))
            {
                return;
            }

            if (GetConfig().ContainsKey(database))
            {
                if (dataTypes == null)
                {
                    _dataTypes.Remove(database);
                }
                else
                {
                    _dataTypes[database] = dataTypes;
                }

                Save();
            }
            else if (dataTypes != null)
            {
                _dataTypes.Add(database, dataTypes);

                Save();
            }
        }

        private static Dictionary<string, Dictionary<string, DbType>> GetConfig()
        {
            if (_dataTypes == null)
            {
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "datatypes.cfg");
                if (File.Exists(fileName))
                {
                    _dataTypes = new Dictionary<string, Dictionary<string, DbType>>(JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, DbType>>>(File.ReadAllText(fileName)), StringComparer.OrdinalIgnoreCase);
                }
            }

            return _dataTypes;
        }

        private static void Save()
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.Converters.Add(new DbTypeConvert());

            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "datatypes.cfg");
            var json = JsonConvert.SerializeObject(_dataTypes, settings);
            Util.TryOperateFile(fileName, () => File.WriteAllText(fileName, json));

            _dataTypes = null;
        }

        private class DbTypeConvert : JsonConverter<DbType>
        {
            public override DbType ReadJson(JsonReader reader, Type objectType, DbType existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var value = reader.ReadAsString();
                if (Enum.TryParse<DbType>(value, out var dbType))
                {
                    return dbType;
                }

                return DbType.String;
            }

            public override void WriteJson(JsonWriter writer, DbType value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}

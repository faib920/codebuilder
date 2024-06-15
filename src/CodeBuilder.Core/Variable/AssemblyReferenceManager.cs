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
using System.IO;
using System.Text;

namespace CodeBuilder.Core.Variable
{
    /// <summary>
    /// 程序集关联管理类。
    /// </summary>
    public class AssemblyReferenceManager
    {
        static AssemblyReferenceManager()
        {
            var fileName = Path.Combine(DevHostingHolder.Instance.WorkPath, "config", "assemblies.cfg");
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                CommonAssemblies = ((string)obj.Common).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                ProfileAssemblies = ((string)obj.Profile).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                SchemaAssemblies = ((string)obj.Schema).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (obj is IDictionary<string, object> dict)
                {
                    AssemblyCatlog = new Dictionary<string, string[]>();
                    foreach (var kvp in dict)
                    {
                        if (kvp.Value != null)
                        {
                            AssemblyCatlog.Add(kvp.Key, kvp.Value.ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置公共的关联程序集。
        /// </summary>
        public static string[] CommonAssemblies { get; set; }

        /// <summary>
        /// 获取或设置变量的关联程序集。
        /// </summary>
        public static string[] ProfileAssemblies { get; set; }

        /// <summary>
        /// 获取或设置架构的关联程序集。
        /// </summary>
        public static string[] SchemaAssemblies { get; set; }

        /// <summary>
        /// 获取总的关联程序集，以字典的方式提供。
        /// </summary>
        public static Dictionary<string, string[]> AssemblyCatlog { get; private set; }

        public static void Save()
        {
            var fileName = Path.Combine(DevHostingHolder.Instance.WorkPath, "config", "assemblies.cfg");
            var json = JsonConvert.SerializeObject(new
            {
                Common = string.Join(";", CommonAssemblies),
                Profile = string.Join(";", ProfileAssemblies),
                Schema = string.Join(";", SchemaAssemblies)
            });

            File.WriteAllText(fileName, json, Encoding.UTF8);
        }
    }
}

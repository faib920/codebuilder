// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;
using System.Collections.Generic;

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 模板定义。
    /// </summary>
    public class TemplateDefinition
    {
        public TemplateDefinition()
        {
            Partitions = new List<PartitionDefinition>();
            Groups = new List<GroupDefinition>();
            Resources = new List<string>();
        }

        /// <summary>
        /// 获取或设置标识。
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; }

        /// <summary>
        /// 获取或设置模板标识。
        /// </summary>
        [JsonIgnore]
        public string TId { get; set; }

        /// <summary>
        /// 获取或设置模板名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置配置文件名称。
        /// </summary>
        [JsonIgnore]
        public string ConfigFileName { get; set; }

        /// <summary>
        /// 获取或设置模板所在目录。
        /// </summary>
        [JsonIgnore]
        public string TemplateDirectory { get; set; }

        /// <summary>
        /// 获取或设置作者。
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置版本。
        /// </summary>
        public decimal Version { get; set; }

        /// <summary>
        /// 获取或设置模板备注。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置语言。
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 获取或设置分组。
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 获取或设置公用脚本。
        /// </summary>
        public List<string> Public { get; set; }

        /// <summary>
        /// 获取部件定义列表。
        /// </summary>
        public List<PartitionDefinition> Partitions { get; private set; }

        /// <summary>
        /// 获取组定义列表。
        /// </summary>
        public List<GroupDefinition> Groups { get; private set; }

        /// <summary>
        /// 获取资源列表。
        /// </summary>
        [JsonIgnore]
        public List<string> Resources { get; private set; }

        /// <summary>
        /// 获取或设置模板扩展。
        /// </summary>
        public TemplateExtension Extension { get; set; } = new TemplateExtension();

        /// <summary>
        /// 获取所有部件列表。
        /// </summary>
        /// <returns></returns>
        public List<PartitionDefinition> GetAllPartitions()
        {
            var result = new List<PartitionDefinition>();
            GetGroupPartitions(Groups, result);

            foreach (var part in Partitions)
            {
                result.Add(part);
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj is TemplateDefinition tmp)
            {
                return TId == tmp.TId && JsonConvert.SerializeObject(this) == JsonConvert.SerializeObject(tmp);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return TId.GetHashCode();
        }

        /// <summary>
        /// 递归获取组及部件。
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="result"></param>
        private void GetGroupPartitions(List<GroupDefinition> groups, List<PartitionDefinition> result)
        {
            foreach (var group in groups)
            {
                result.AddRange(group.Partitions);
                GetGroupPartitions(group.Groups, result);
            }
        }
    }
}

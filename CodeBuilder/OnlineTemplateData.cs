// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;
using System.Collections.Generic;

namespace CodeBuilder
{
    internal class OnlineTemplateData
    {
        /// <summary>
        /// 项目。
        /// </summary>
        public List<OnlineTemplate> Items { get; set; }

        /// <summary>
        /// 总数。
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 总页数。
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 是否最后一页。
        /// </summary>
        public bool LastPage { get; set; }
    }

    internal class OnlineTemplate
    {
        /// <summary>
        /// 分类。
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 编码（唯一）。
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 作者。
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 版本。
        /// </summary>
        public decimal Version { get; set; }

        /// <summary>
        /// 下载的包路径。
        /// </summary>
        public string PackageUrl { get; set; }

        /// <summary>
        /// 发布时间。
        /// </summary>
        public string PublishTime { get; set; }

        /// <summary>
        /// 本地是否安装。
        /// </summary>
        public bool IsInstalled { get; set; }

        /// <summary>
        /// 是否需要更新。
        /// </summary>
        public bool NeedUpdate { get; set; }

        /// <summary>
        /// 模板定义。
        /// </summary>
        public TemplateDefinition Template { get; set; }

        /// <summary>
        /// 权重（用于排序）。
        /// </summary>
        public int Weight { get; set; }
    }
}

﻿// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 数据源选项。
    /// </summary>
    public class SourceOption
    {
        /// <summary>
        /// 获取或设置是否读取视图。
        /// </summary>
        public bool View { get; set; }

        /// <summary>
        /// 获取或设置是否跳过获取架构。
        /// </summary>
        public bool SkipSchema { get; set; }

        /// <summary>
        /// 获取或设置选中的数据表名称。
        /// </summary>
        public List<string> Selected { get; set; }
    }
}

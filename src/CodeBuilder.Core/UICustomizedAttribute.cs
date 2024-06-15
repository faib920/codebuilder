// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 表示可定制化。
    /// </summary>
    public class UICustomizedAttribute : Attribute
    {
        public UICustomizedAttribute(string name, int width)
        {
            Name = name;
            Width = width;
        }

        /// <summary>
        /// 获取或设置显示的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置显示的宽度。
        /// </summary>
        public int Width { get; set; }
    }
}

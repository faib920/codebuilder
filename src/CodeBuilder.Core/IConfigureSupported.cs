// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    public interface IConfigureSupported : IPlugin
    {
        /// <summary>
        /// 获取配置面板。
        /// </summary>
        /// <returns></returns>
        UserControl GetOptionPanel();
    }

    public interface IConfigurableControl
    {
        /// <summary>
        /// 保存。
        /// </summary>
        /// <returns></returns>
        bool SaveChanges();
    }
}

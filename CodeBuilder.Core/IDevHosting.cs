// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 提供开发环境中的相关属性和方法。
    /// </summary>
    public interface IDevHosting
    {
        /// <summary>
        /// 获取工作目录。
        /// </summary>
        string WorkPath { get; }

        /// <summary>
        /// 获取主窗体。
        /// </summary>
        IWin32Window MainWindow { get; }

        /// <summary>
        /// 获取容器。
        /// </summary>
        IWin32Window DockContainer { get; }

        /// <summary>
        /// 获取当前的数据源提供者。
        /// </summary>
        ISourceProvider SourceProvider { get; }

        /// <summary>
        /// 获取当前的模板提供者。
        /// </summary>
        ITemplateProvider TemplateProvider { get; }

        /// <summary>
        /// 获取当前的模板定义。
        /// </summary>
        TemplateDefinition Template { get; }

        /// <summary>
        /// 获取变量。
        /// </summary>
        Profile Profile { get; }

        /// <summary>
        /// 在属性窗口中显示对象。
        /// </summary>
        /// <param name="obj"></param>
        void ViewInPropGrid(object obj);

        /// <summary>
        /// 控制台输出信息。
        /// </summary>
        /// <param name="msg"></param>
        void ConsoleInfo(string msg);

        /// <summary>
        /// 控制台输出错误信息。
        /// </summary>
        /// <param name="msg"></param>
        void ConsoleError(string msg);

        /// <summary>
        /// 弹出显示信息。
        /// </summary>
        /// <param name="msg"></param>
        void ShowInfo(string msg);

        /// <summary>
        /// 弹出显示错误信息。
        /// </summary>
        /// <param name="msg"></param>
        void ShowError(string msg);

        /// <summary>
        /// 弹出显示错误信息。
        /// </summary>
        /// <param name="exp"></param>
        void ShowError(Exception exp);

        /// <summary>
        /// 弹出显示警告信息。
        /// </summary>
        /// <param name="msg"></param>
        void ShowWarn(string msg);

        /// <summary>
        /// 弹出显示询问信息。
        /// </summary>
        /// <param name="msg"></param>
        ShowMsgButton ShowConfirm(string msg, int buttons = 2);

        /// <summary>
        /// 获取所有表。
        /// </summary>
        /// <returns></returns>
        IEnumerable<Table> GetTables();
    }

    /// <summary>
    /// 提供开发环境的访问。
    /// </summary>
    public interface IDevHostingAccessor
    {
        /// <summary>
        /// 获取或设置开发环境。
        /// </summary>
        IDevHosting Hosting { get; set; }
    }

    public enum ShowMsgButton
    {
        Yes,
        No,
        Cancel
    }
}

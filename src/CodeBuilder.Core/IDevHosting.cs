// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
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
        /// 获取应用程序服务提供者。
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取是否身份认证。
        /// </summary>
        bool IsAuthorized { get; }

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

        void Hit(string key);

        /// <summary>
        /// 数据源历史变动
        /// </summary>
        Action OnSourceHistoryChanged { get; }

        /// <summary>
        /// 显示进度条。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="percent">百分比。</param>
        void ShowProgress(string message, int percent);

        /// <summary>
        /// 隐藏进度条。
        /// </summary>
        void HideProgress();

        /// <summary>
        /// 打开文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="language"></param>
        void OpenFile(string fileName, string language = null);

        void ShowAbout(string app = null, string version = null);

        void ShowDonate();

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        object GetConfig(string name);

        /// <summary>
        /// 获取函数集。
        /// </summary>
        dynamic Funcs { get; }

        /// <summary>
        /// 启动工具
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        void Start(string name, params object[] arguments);
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

    public static class DevHostingHolder
    {
        public static IDevHosting Instance { get; private set; }

        public static THosting Hold<THosting>(this THosting hosting) where THosting : IDevHosting
        {
            Instance = hosting;
            return hosting;
        }
    }
}

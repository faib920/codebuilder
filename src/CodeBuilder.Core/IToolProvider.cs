// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 工具提供者插件。
    /// </summary>
    public interface IToolProvider : IPlugin
    {
        /// <summary>
        /// 执行调用工具。
        /// </summary>
        /// <param name="arguments"></param>
        Form Execute(params object[] arguments);
    }

    /// <summary>
    /// 提供工具执行前的检查。
    /// </summary>
    public interface IPreExecuteSupported
    {
        /// <summary>
        /// 检查是否能执行。
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        bool CanExecutable(params object[] arguments);
    }

    /// <summary>
    /// 异步的工具提供者插件
    /// </summary>
    public interface IAsyncToolProvider : IToolProvider
    {
        /// <summary>
        /// 执行调用工具。
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="arguments"></param>
        Task<Form> ExecuteAsync(CancellationToken cancellationToken = default, params object[] arguments);
    }

    /// <summary>
    /// 多重工具提供者插件。
    /// </summary>
    public interface IMultipleToolProvider : IToolProvider
    {
        /// <summary>
        /// 执行调用工具。
        /// </summary>
        /// <param name="name">子工具名称。</param>
        /// <param name="arguments"></param>
        Form Execute(string name, params object[] arguments);

        /// <summary>
        /// 获取工具子菜单。
        /// </summary>
        IEnumerable<IToolMenu> SubItems { get; }
    }

    /// <summary>
    /// 提供工具执行前的检查。
    /// </summary>
    public interface IMultiplePreExecuteSupported
    {
        /// <summary>
        /// 检查是否能执行。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        bool CanExecutable(string name, params object[] arguments);
    }

    /// <summary>
    /// 异步的多重工具提供者插件。
    /// </summary>
    public interface IAsyncMultipleToolProvider : IMultipleToolProvider
    {
        /// <summary>
        /// 执行调用工具。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        Task<Form> ExecuteAsync(string name, CancellationToken cancellationToken = default, params object[] arguments);
    }

    /// <summary>
    /// 工具菜单接口。
    /// </summary>
    public interface IToolMenu
    {
    }

    /// <summary>
    /// 工具菜单。
    /// </summary>
    public class ToolMenuItem : IToolMenu
    {
        public ToolMenuItem(string name, object parameter)
        {
            Name = name;
            Parameter = parameter;
        }

        public string Name { get; set; }

        public object Parameter { get; set; }

        public List<IToolMenu> SubItems { get; set; } = new List<IToolMenu>();
    }

    /// <summary>
    /// 分隔符。
    /// </summary>
    public class ToolMenuSeparator : IToolMenu
    {
    }
}

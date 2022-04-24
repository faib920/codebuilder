// -----------------------------------------------------------------------
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
    /// 工具提供者插件。
    /// </summary>
    public interface IToolProvider : IPlugin
    {
        /// <summary>
        /// 执行调用工具。
        /// </summary>
        void Execute();
    }

    public interface IMultipleToolProvider : IToolProvider
    {
        void Execute(string name, object parameter);

        IEnumerable<IToolMenu> SubItems { get; }
    }

    public interface IToolMenu
    {
    }

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

    public class ToolMenuSeparator : IToolMenu
    {
    }
}

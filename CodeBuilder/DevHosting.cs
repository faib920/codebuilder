using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder
{
    public sealed class DevHosting : IDevHosting
    {
        internal Action<object> ViewInPropGridAct;
        internal Action<string> ConsoleInfoAct;
        internal Action<string> ConsoleErrorAct;
        internal Func<IEnumerable<Table>> GetTablesFunc;
        private ITemplateProvider _templateProvider;

        public string WorkPath
        {
            get { return Application.StartupPath; }
        }

        public IWin32Window MainWindow { get; set; }

        public IWin32Window DockContainer { get; set; }

        public ISourceProvider SourceProvider { get; set; }

        public ITemplateProvider TemplateProvider
        {
            get { return _templateProvider; }
            set
            {
                _templateProvider = value;
                PartitionWriter.ClearCache();
            }
        }

        public TemplateDefinition Template { get; set; }

        public Profile Profile { get; set; }

        public void ViewInPropGrid(object obj)
        {
            ViewInPropGridAct?.Invoke(obj);
        }

        public void ConsoleInfo(string msg)
        {
            ConsoleInfoAct?.Invoke(msg);
        }

        public void ConsoleError(string msg)
        {
            ConsoleErrorAct?.Invoke(msg);
        }

        public void ShowInfo(string msg)
        {
            MessageBox.Show(DockContainer, msg, "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowError(string msg)
        {
            MessageBox.Show(DockContainer, msg, "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowError(Exception exp)
        {
            ErrorMessageBox.Show(DockContainer, "CodeBuilder", exp);
        }

        public void ShowWarn(string msg)
        {
            MessageBox.Show(DockContainer, msg, "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public ShowMsgButton ShowConfirm(string msg, int buttons = 2)
        {
            var ret = MessageBox.Show(DockContainer, msg, "CodeBuilder", buttons == 2 ? MessageBoxButtons.YesNo : MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (ret)
            {
                case DialogResult.Yes:
                    return ShowMsgButton.Yes;
                case DialogResult.No:
                    return ShowMsgButton.No;
                default:
                    return ShowMsgButton.Cancel;
            }
        }

        /// <summary>
        /// 获取所有表。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> GetTables()
        {
            return GetTablesFunc();
        }
    }
}

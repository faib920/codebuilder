// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using Fireasy.Common;
using Fireasy.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace CodeBuilder
{
    public sealed class DevHosting : IDevHosting
    {
        internal Action<object> ViewInPropGridAct;
        internal Action<string, int> ProgressAct;
        internal Action<string, string> OpenFileAct;
        internal Action ShowAboutAct;
        internal Action ShowDonateAct;
        internal Func<IEnumerable<Table>> GetTablesFunc;
        internal Action<int, DateTime, string> LogPollAct;
        private ITemplateProvider _templateProvider;
        private TemplateDefinition _template;
        private Queue<LogData> _logQueue = new Queue<LogData>();
        private System.Threading.Timer _logTimer;
        private bool _isQueueLog = false;

        public DevHosting()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(builder => builder.AddConsole());
            services = services.AddFireasy(options => 
                options.DiscoverOptions
                    .SetUseAnalyzers(false)
                    .AddAssemblyFilterPredicate(assembly => !assembly.GetName().Name.StartsWith("Fireasy") && !assembly.GetName().Name.StartsWith("CodeBuilder")))
                .Services;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IDevHosting>(this);
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<FormCloseHandler>();
            ServiceProvider = services.BuildServiceProvider();

            _logTimer = new System.Threading.Timer(PollLogMessage, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(120));
        }

        public IServiceProvider ServiceProvider { get; private set; }

        public bool IsAuthorized { get; set; }

        public string WorkPath => Util.GetWorkPath();

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

        public TemplateDefinition Template
        {
            get { return _template; }
            set
            {
                if (_template != null && _template.Equals(value))
                {
                    return;
                }

                _template = value;
                LocalDynamicCache.ClearPartitionOutputParsers();
            }
        }

        public Profile Profile { get; set; }

        public void ViewInPropGrid(object obj)
        {
            ViewInPropGridAct?.Invoke(obj);
        }

        public void ConsoleInfo(string msg)
        {
            if (Config.Instance.LogLevel == 0)
            {
                _logTimer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(10));

                _logQueue.Enqueue(new LogData(0, msg));
            }
        }

        public void ConsoleError(string msg)
        {
            _logTimer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(10));

            _logQueue.Enqueue(new LogData(1, msg));
        }

        public void ShowInfo(string msg)
        {
            if (MainWindow is Control control)
            {
                control.Invoke(new Action(() =>
                {
                    MessageBox.Show(MainWindow, msg, "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
        }

        public void ShowError(string msg)
        {
            if (MainWindow is Control control)
            {
                control.Invoke(new Action(() =>
                {
                    MessageBox.Show(MainWindow, msg, "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }

        public void ShowError(Exception exp)
        {
            if (MainWindow is Control control)
            {
                control.Invoke(new Action(() =>
                {
                    ErrorMessageBox.Show(MainWindow, "CodeBuilder", exp);
                }));
            }
        }

        public void ShowWarn(string msg)
        {
            if (MainWindow is Control control)
            {
                control.Invoke(new Action(() =>
                {
                    MessageBox.Show(MainWindow, msg, "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }));
            }
        }

        public ShowMsgButton ShowConfirm(string msg, int buttons = 2)
        {
            var ret = DialogResult.None;
            if (MainWindow is Control control)
            {
                control.Invoke(new Action(() =>
                {
                    ret = MessageBox.Show(MainWindow, msg, "CodeBuilder", buttons == 2 ? MessageBoxButtons.YesNo : MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                }));
            }

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

        public IEnumerable<Table> GetTables()
        {
            return GetTablesFunc();
        }

        public void Hit(string key)
        {
            var client = new HttpClient();
            var version = GetType().Assembly.GetName().Version;
            client.PostAsync($"{Consts.ApiUrl}/hit?softKey=CodeBuilder&key=" + key + "&version=" + version, null);
        }

        public Action OnSourceHistoryChanged { get; set; }

        public void ShowProgress(string message, int percent)
        {
            ProgressAct?.Invoke(message, percent);
        }

        public void HideProgress()
        {
            ProgressAct?.Invoke(string.Empty, 100);
        }

        public void OpenFile(string fileName, string language = null)
        {
            OpenFileAct?.Invoke(fileName, language);
        }

        public void ShowAbout(string app = null, string version = null)
        {
            ShowAboutAct?.Invoke();
        }

        public void ShowDonate()
        {
            ShowDonateAct?.Invoke();
        }

        public object GetConfig(string name)
        {
            switch (name)
            {
                case "FontSize":
                    return Config.Instance.FontSize;
                case "AccessToken":
                    return Config.Instance.AccessToken;
            }

            return null;
        }

        private void PollLogMessage(object state)
        {
            if (_isQueueLog)
            {
                return;
            }

            var hasQueue = _logQueue.Count > 0;
            _isQueueLog = true;
            while (_logQueue.Count > 0)
            {
                var log = _logQueue.Dequeue();
                LogPollAct?.Invoke(log.Type, log.Time, log.Message);
            }

            if (hasQueue)
            {
                _logTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(120));
            }

            _isQueueLog = false;
        }

        private class LogData
        {
            public LogData(int type, string message)
            {
                Time = DateTime.Now;
                Type = type;
                Message = message;
            }

            public DateTime Time { get; }

            public int Type { get; }

            public string Message { get; }
        }
    }
}

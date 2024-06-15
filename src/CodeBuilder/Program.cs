// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Extensions;
using Fireasy.Composition;
using Fireasy.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace CodeBuilder
{
    static class Program
    {
        public static ApplicationContext Context { get; set; }

        private static frmStart _frmStart;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Thread.Sleep(1000);

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var syntaxProvider = new FileSyntaxModeProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "syntaxs"));
            HighlightingManager.Manager.AddSyntaxModeFileProvider(syntaxProvider);

            if (args?.Length > 0)
            {
                if (args[0].StartsWith("-tool:"))
                {
                    RunTool(args);
                    return;
                }
            }

            RemovePlugins();
            UpdatePlugins();
            Util.ClearTempFiles();
            CheckVersion();

            var fileName = args?.FirstOrDefault();
            Context = new ApplicationContext();
            _frmStart = new frmStart(Context, fileName);
            Context.MainForm = _frmStart;

            Application.Run(Context);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //var log = LoggerFactory.CreateLogger();
            //log.Error("应用程序错误", e.Exception);
            ErrorMessageBox.Show("应用程序错误", e.Exception);

            if (_frmStart != null)
            {
                _frmStart.Close();
            }
        }

        static void CheckVersion()
        {
            if (!File.Exists("AutoUpdate.exe"))
            {
                return;
            }

            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version.xml");
            var xml = new XmlDocument();
            xml.Load(file);

            var node = xml.SelectSingleNode("//local/check-date");
            if (Config.Instance.CheckUpdate || node == null || node.InnerText.To<DateTime>().AddDays(7) <= DateTime.Today)
            {
                var startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = "AutoUpdate.exe";
                startInfo.Verb = "runas";

                Process.Start(startInfo);

                if (node == null)
                {
                    node = xml.CreateNode(XmlNodeType.Element, "check-date", string.Empty);
                    xml.SelectSingleNode("//local").AppendChild(node);
                }

                node.InnerText = DateTime.Today.ToShortDateString();

                try
                {
                    xml.Save(file);
                }
                catch { }
            }
        }

        static void RemovePlugins()
        {
            var cfg = PlugInConfig.Get();
            if (cfg.Removed.Count > 0)
            {
                cfg.Removed.ForEach(s =>
                {
                    File.Delete(Path.Combine(Application.StartupPath, s + ".dll"));
                });

                PlugInConfig.Clear();
            }
        }

        static void UpdatePlugins()
        {
            if (!Directory.Exists(Config.PluginStoragePath))
            {
                return;
            }

            try
            {
                RecursiveDirectory(string.Empty);
            }
            catch (Exception exp)
            {
                ErrorMessageBox.Show("插件更新失败，请重新下载更新!", exp);
            }
            finally
            {
                Directory.Delete(Config.PluginStoragePath, true);
            }
        }

        private static void RecursiveDirectory(string subpath)
        {
            var path1 = string.IsNullOrEmpty(subpath) ? Application.StartupPath : Path.Combine(Application.StartupPath, subpath);
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }

            var path2 = string.IsNullOrEmpty(subpath) ? Config.PluginStoragePath : Path.Combine(Config.PluginStoragePath, subpath);
            foreach (var file in Directory.GetFiles(path2, "*.*"))
            {
                var info = new FileInfo(file);
                var destFileName = Path.Combine(path1, info.Name);
                if (File.Exists(destFileName) && ".usercfg".Equals(info.Extension, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                File.Copy(file, destFileName, true);
            }

            foreach (var dir in Directory.GetDirectories(path2))
            {
                RecursiveDirectory(dir.Substring(Config.PluginStoragePath.Length + 1));
            }
        }

        private static bool RunTool(string[] args)
        {
            var hosting = new DevHosting().Hold();

            var providers = hosting.ServiceProvider.GetExportedServices<IToolProvider>();
            var toolName = args[0].Substring(6);
            var toolProvider = providers.FirstOrDefault(s => s.GetType().Name == toolName);
            var version = toolProvider.GetType().Assembly.GetName().Version.ToString();
            hosting.ShowAboutAct = () => new frmAbout(hosting, "ApiTester", version).ShowDialog();
            hosting.ShowDonateAct = () => new frmDonate().ShowDialog();

            if (toolProvider != null)
            {
                toolProvider.Initialize(hosting);
                var form = toolProvider.Execute();
                if (form != null)
                {
                    hosting.MainWindow = form;
                    Context = new ApplicationContext();
                    Context.MainForm = form;

                    Application.Run(Context);
                    return true;
                }
            }

            MessageBox.Show($"无法启用工具 {toolName}!", "CodeBuilder", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }
    }
}

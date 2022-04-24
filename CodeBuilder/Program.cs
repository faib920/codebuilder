// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Extensions;
using Fireasy.Common.Logging;
using Fireasy.Windows.Forms;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace CodeBuilder
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Application.ThreadException += Application_ThreadException;
            RemovePlugins();
            UpdatePlugins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Util.ClearTempFiles();
            WriteRegistryPath();
            CheckVersion();

            Application.Run(new frmMain());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var log = LoggerFactory.CreateLogger();
            log.Error("应用程序错误", e.Exception);
            ErrorMessageBox.Show("应用程序错误", e.Exception);
        }

        static void CheckVersion()
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version.xml");
            var xml = new XmlDocument();
            xml.Load(file);

            var node = xml.SelectSingleNode("//local/check-date");
            if (Config.Instance.CheckUpdate || node == null || node.InnerText.To<DateTime>().AddDays(7) <= DateTime.Today)
            {
                Process.Start("AutoUpdate.exe");
                if (node == null)
                {
                    node = xml.CreateNode(XmlNodeType.Element, "check-date", string.Empty);
                    xml.SelectSingleNode("//local").AppendChild(node);
                }

                node.InnerText = DateTime.Today.ToShortDateString();

                xml.Save(file);
            }
        }

        static void WriteRegistryPath()
        {
            var regpath = "Software\\Fireasy\\CodeBuilder";
            var reg = Registry.CurrentUser.OpenSubKey(regpath);
            if (reg == null)
            {
                reg = Registry.CurrentUser.CreateSubKey(regpath, RegistryKeyPermissionCheck.ReadWriteSubTree);

                reg.SetValue("Path", AppDomain.CurrentDomain.BaseDirectory);
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
            }

            PlugInConfig.Clear();
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
    }
}

// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Extensions;
using System;
using System.IO;
using System.Windows.Forms;

namespace CodeBuilder.Core.Tool
{
    public class ToolShortcutHelper
    {
        public static void Create(IDevHosting hosting, string toolName)
        {
            try
            {
                var desk = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                var linkFile = Path.Combine(desk, toolName + ".lnk");
                var shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(linkFile);
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.WorkingDirectory = hosting.WorkPath;
                shortcut.Arguments = "-tool:" + toolName;
                shortcut.Save();

                hosting.ShowInfo("已添加桌面快捷方式！");
            }
            catch (Exception exp)
            {
                hosting.ShowError("桌面快捷方式添加失败！\n" + exp.Output());
            }
        }
    }
}

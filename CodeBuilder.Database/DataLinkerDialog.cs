// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    /// <summary>
    /// 数据源配置对话框。
    /// </summary>
    internal sealed class DataLinkerDialog : Component, IConnectionConfig
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// 显示对话框。
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public DialogResult ShowDialog(IntPtr handle)
        {
            DialogResult ret = DialogResult.Cancel;
            Type type = Type.GetTypeFromProgID("Datalinks", true);
            object link = Activator.CreateInstance(type);
            if (handle != IntPtr.Zero)
            {
                type.InvokeMember("hWnd", BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance, null, link, new object[] { handle });
            }
            if (string.IsNullOrEmpty(ConnectionString))
            {
                object conn = type.InvokeMember("PromptNew", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, link, null);
                if (conn != null)
                {
                    ret = DialogResult.OK;
                    Type connType = conn.GetType();
                    ConnectionString = connType.InvokeMember("ConnectionString", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null, conn, null).ToString();
                    Marshal.ReleaseComObject(conn);
                }
            }
            else
            {
                Type connType = Type.GetTypeFromProgID("ADODB.Connection", true);
                object conn = Activator.CreateInstance(connType);
                connType.InvokeMember("ConnectionString", BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance, null, conn, new object[] { ConnectionString });
                if (type.InvokeMember("PromptEdit", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, link, new object[] { conn }).ToString() == "True")
                {
                    ret = DialogResult.OK;
                    ConnectionString = connType.InvokeMember("ConnectionString", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null, conn, null).ToString();
                }
                Marshal.ReleaseComObject(conn);
            }
            Marshal.ReleaseComObject(link);

            return ret;
        }
    }
}

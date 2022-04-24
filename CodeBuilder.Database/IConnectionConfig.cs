// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public interface IConnectionConfig
    {
        string ConnectionString { get; set; }

        DialogResult ShowDialog(IntPtr handle);
    }
}

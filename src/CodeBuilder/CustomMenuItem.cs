// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using System.Windows.Forms;

namespace CodeBuilder
{
    public class CustomMenuItem : ToolStripMenuItem
    {
        public CustomMenuItem(string text)
            : base(text)
        {
        }

        public DisplayStyle DisplayStyle { get; set; }
    }
}

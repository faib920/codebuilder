// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class FileComparer : UserControl, IDevHostingAccessor
    {
        public FileComparer()
        {
            InitializeComponent();
        }

        public IDevHosting Hosting { get; set; }

        private void btnOpen1_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dialog.FileName;
                }
            }
        }

        private void btnOpen2_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dialog.FileName;
                }
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                Hosting.ShowWarn("请选择文件。");
                return;
            }

            if (textBox1.Text.Equals(textBox2.Text, StringComparison.OrdinalIgnoreCase))
            {
                Hosting.ShowInfo("两个文件内容完全一致。");
                return;
            }

            var buffsize = 10240;
            var fileBytes1 = new byte[buffsize];
            var fileBytes2 = new byte[buffsize];
            var offset = 0L;

            Cursor = Cursors.WaitCursor;

            try
            {
                using (var stream1 = new FileStream(textBox1.Text, FileMode.Open))
                using (var stream2 = new FileStream(textBox2.Text, FileMode.Open))
                {
                    if (stream1.Length != stream2.Length)
                    {
                        Hosting.ShowWarn("两个文件长度不一致。");
                        return;
                    }

                    var length = stream1.Length;

                    while (offset < length)
                    {
                        var count = stream1.Read(fileBytes1, 0, buffsize);
                        stream2.Read(fileBytes2, 0, buffsize);

                        for (var i = 0; i < count; i++)
                        {
                            if (fileBytes1[i] != fileBytes2[i])
                            {
                                Hosting.ShowWarn("两个文件内容不一致。");
                                return;
                            }
                        }
                        offset += count;
                    }

                    Hosting.ShowInfo("两个文件内容完全一致。");
                }
            }
            catch (Exception exp)
            {
                Hosting.ShowError(exp);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}

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
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class FileEncodeDetector : UserControl, IDevHostingAccessor
    {
        public FileEncodeDetector()
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

        private void btnDetect_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || !File.Exists(textBox1.Text))
            {
                Hosting.ShowWarn("请选择文件。");
                return;
            }

            var encoding = DetectFileEncoding(textBox1.Text);
            Hosting.ShowInfo($"文件编码格式为：{encoding}");
        }

        public static string DetectFileEncoding(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var binaryStream = new BinaryReader(fileStream))
            {
                var buffer = new byte[4];
                binaryStream.Read(buffer, 0, 4);

                if (buffer.Length >= 3 && buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76) return "UTF7";
                if (buffer.Length >= 3 && buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf) return "UTF8(BOM)";
                if (buffer.Length >= 2 && buffer[0] == 0xef && buffer[1] == 0xbb) return "UTF8";
                if (buffer.Length >= 2 && buffer[0] == 0xff && buffer[1] == 0xfe) return "Unicode(UTF-16LE)";
                if (buffer.Length >= 2 && buffer[0] == 0xfe && buffer[1] == 0xff) return "BigEndianUnicode(UTF-16BE)";
                if (buffer.Length >= 3 && buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff) return "UTF32";
                if (buffer.Length >= 3 && buffer[0] == 0xff && buffer[1] == 0xfe && buffer[2] == 0 && buffer[3] == 0) return "UTF-32(LE)";
                if (buffer.Length >= 3 && buffer[0] == 0xf7 && buffer[1] == 0x64 && buffer[2] == 0x4c) return "UTF-1";
                if (buffer.Length >= 3 && buffer[0] == 0xdd && buffer[1] == 0x73 && buffer[2] == 0x66 && buffer[3] == 0x73) return "UTF-EBCDIC";
                if (buffer.Length >= 3 && buffer[0] == 0x0e && buffer[1] == 0xfe && buffer[2] == 0xff) return "SCSU";
                if (buffer.Length >= 3 && buffer[0] == 0xfb && buffer[1] == 0xee && buffer[2] == 0x28) return "BOCU-1";
                if (buffer.Length >= 3 && buffer[0] == 0x84 && buffer[1] == 0x31 && buffer[2] == 0x95 && buffer[3] == 0x33) return "GB-18030";

                return "ASCII";
            }
        }
    }
}

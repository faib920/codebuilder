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
using Fireasy.Windows.Forms;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.PdfTool
{
    public partial class frmToImg : DockFormBase, IContextMenuManager, ICloseManager
    {
        private Popup _popup;
        private bool _isCancellation;
        private int _total;
        private int _index;
        private readonly IDevHosting _hosting;
        private ImageFormat _imageFormat = ImageFormat.Png;

        public frmToImg(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();

            treeList1.Renderer.DecorationRendererFactory = new ProgressTreeListRendererFactory();
            _popup = new Popup(panel1) { DropShadowEnabled = false, Font = Font, Resizable = false };
            _hosting = hosting;
        }

        IEnumerable<ToolStripItem> IContextMenuManager.GetContextMenuItems()
        {
            yield break;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog { Filter = "PDF(*.pdf)|*.pdf", Multiselect = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (treeList1.Items.Count > 0)
                    {
                        if (_hosting.ShowConfirm("是否清空已选择的文件?") == ShowMsgButton.Yes)
                        {
                            treeList1.Items.Clear();
                        }
                    }

                    Cursor = Cursors.WaitCursor;
                    treeList1.BeginUpdate();

                    foreach (var fileName in dialog.FileNames)
                    {
                        if (treeList1.Items.Any(s => s.Text.Equals(fileName, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }

                        var item = new TreeListItem { Text = fileName };
                        treeList1.Items.Add(item);
                        item.Cells[1].Value = GetTargetFileName(fileName);
                        item.Cells[2].Value = GetPageCount(fileName);
                        item.ImageIndex = 0;
                    }

                    Cursor = Cursors.Default;
                    treeList1.EndUpdate();

                    _total = 0;
                    _index = 0;
                    foreach (var item in treeList1.Items)
                    {
                        _total += (int)item.Cells[2].Value;
                    }
                }
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            _isCancellation = false;
            var time = Processor.Run(this, async calcelToken =>
            {
                var tasks = treeList1.Items.Select(s => ConvertToImage(s));
                await Task.WhenAll(tasks);
            }, () => _isCancellation = true);

            _hosting.HideProgress();
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            _popup.Show(btnOption);
        }

        private int GetPageCount(string fileName)
        {
            try
            {
                using (var document = PdfDocument.Load(fileName))
                {
                    return document.PageCount;
                }
            }
            catch
            {
                return 0;
            }
        }

        private async Task ConvertToImage(TreeListItem item)
        {
            var fileInfo = new FileInfo(item.Cells[1].Text);
            if (!fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            if (_isCancellation || (int)item.Cells[2].Value == 0)
            {
                return;
            }

            using (var document = PdfDocument.Load(item.Cells[0].Text))
            {
                var count = document.PageCount;
                var sizes = document.PageSizes;

                for (var i = 0; i < count; i++)
                {
                    if (_isCancellation)
                    {
                        return;
                    }

                    var width = sizes[i].Width;
                    var height = sizes[i].Height;

                    var rat = ((i + 1) / (count * 1m));

                    _hosting.ShowProgress($"正在转换 {item.Cells[0].Text}...", -1);

                    this.Invoke(() =>
                    {
                        item.Cells[3].Value = rat;
                    });

                    var fileName = item.Cells[1].Text.Replace("{Page}", (i + 1).ToString());

                    using (var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                    using (var image = document.Render(i, width, height, PdfRenderFlags.Annotations))
                    {
                        image.Save(stream, _imageFormat);
                    }
                }
            }
        }

        private string GetTargetFileName(string fileName)
        {
            var ext = rdbImg1.Checked ? "png" : (rdbImg2.Checked ? "jpeg" : "gif");
            var lastDot = fileName.LastIndexOf('.');
            if (rdbFile1.Checked)
            {
                return fileName.Substring(0, lastDot) + "_{Page}." + ext;
            }

            return fileName.Substring(0, lastDot) + "\\{Page}." + ext;
        }

        private void ApplyOption()
        {
            foreach (var item in treeList1.Items)
            {
                item.Cells[1].Value = GetTargetFileName(item.Cells[0].Text);
            }
        }

        private class ProgressTreeListRendererFactory : TreeListDecorationRendererFactory
        {
            public override TreeListDecorationRenderer CreateDecorationRenderer(TreeListCell cell)
            {
                if (cell.Column.Index == 3)
                {
                    return new TreeListProgressDecorationRenderer { DisplayValue = true };
                }

                return base.CreateDecorationRenderer(cell);
            }
        }

        private void rdbOption_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == rdbImg1)
            {
                _imageFormat = ImageFormat.Png;
            }
            else if (sender == rdbImg2)
            {
                _imageFormat = ImageFormat.Jpeg;
            }
            else if (sender == rdbImg3)
            {
                _imageFormat = _imageFormat = ImageFormat.Gif;
            }

            ApplyOption();
        }

        private void mnuRemove_Click(object sender, EventArgs e)
        {
            if (treeList1.HasSelectedItems)
            {
                treeList1.Items.Remove(treeList1.SelectedItems[0]);
            }
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            treeList1.Items.Clear();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            if (treeList1.HasSelectedItems)
            {
                var fileInfo = new FileInfo(treeList1.SelectedItems[0].Cells[1].Text);
                if (fileInfo.Directory.Exists)
                {
                    Process.Start(fileInfo.DirectoryName);
                }
            }
        }

        private void treeList1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void treeList1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var file in files)
            {
                if (!File.Exists(file) || !new FileInfo(file).Extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (treeList1.Items.Any(s => s.Text.Equals(file, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var item = new TreeListItem { Text = file };
                treeList1.Items.Add(item);
                item.Cells[1].Value = GetTargetFileName(file);
                item.Cells[2].Value = GetPageCount(file);
                item.ImageIndex = 0;
            }
        }
    }
}

// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using Fireasy.Common.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CodeBuilder.PDManer
{
    public class SourceGuide : ISourceGuide
    {
        private Dictionary<int, Control> _controls = new Dictionary<int, Control>();
        private string[] _titles = new[] { "PDManer - 选择文件", "PDManer - 选择数据表" };

        Image ISourceGuide.Image => Properties.Resources.pdmaner;

        string ISourceGuide.Description => "读取使用 PDManer 工具设计的数据库模型。";

        void ISourceGuide.Reset()
        {
            _controls.ForEach(s => s.Value.Dispose());
            _controls.Clear();
        }

        (string Title, bool HasNext, Control Control) ISourceGuide.Show(int step, params object[] arguments)
        {
            if (!_controls.TryGetValue(step, out var control))
            {
                switch (step)
                {
                    case 1:
                        control = new Guide_SelectFile();
                        break;
                }

                _controls.Add(step, control);
            }

            return (_titles[step - 1], step < 3, control);
        }
    }
}

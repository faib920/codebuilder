// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CodeBuilder.Core.Forms
{
    public class FormCloseHandler
    {
        private readonly IDevHosting _hosting;
        private List<Form> _forms = new List<Form>();
        private System.Threading.Timer _timer = null;

        public FormCloseHandler(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public bool TryAddClosingForm(Form form)
        {
            if (_forms.Contains(form))
            {
                return false;
            }

            _forms.Add(form);
            if (_timer == null)
            {
                _timer = new System.Threading.Timer(DoClosing, null, 200, 0);
            }
            else
            {
                _timer.Change(200, 0);
            }

            return true;
        }

        public void Clear()
        {
            _forms.Clear();
        }

        public IEnumerable<Form> GetForms() => _forms;

        private void DoClosing(object status)
        {
            if (_forms.Count > 0)
            {
                var changed = _forms.OfType<IChangeManager>().Where(s => s.IsChanged).ToList();
                if (changed.Count > 0)
                {
                    var array = changed.Select(s => ((Form)s).Text.Replace(" *", ""));
                    if (array.Count() > 10)
                    {
                        array = array.Take(10).Union(new[] { "......" });
                    }

                    var windowText = string.Join("\n", array);
                    var msg = _hosting.ShowConfirm($"以下{changed.Count}个窗口的内容已有改变，关闭前是否先保存?\n\n" + windowText, 3);
                    if (msg == ShowMsgButton.Yes)
                    {
                        _forms.ForEach(s =>
                        {
                            if (s is IChangeManager changeManager && changeManager.IsChanged)
                            {
                                s.Invoke(new Action(() =>
                                {
                                    changeManager.SaveChanges(false);
                                }));
                            }

                            s.Invoke(new Action(() => s.Close()));
                        });
                    }
                    else if (msg == ShowMsgButton.No)
                    {
                        _forms.ForEach(s => s.Invoke(new Action(() => s.Close())));
                    }
                }
                else
                {
                    _forms.ForEach(s => s.Invoke(new Action(() => s.Close())));
                }

                _forms.Clear();
            }

            _timer.Dispose();
            _timer = null;
        }
    }
}

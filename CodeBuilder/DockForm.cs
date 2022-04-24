using System;

namespace CodeBuilder
{
    public class DockForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public Action CloseAct { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            if (CloseAct != null)
            {
                CloseAct();
            }

            base.OnClosed(e);
        }
    }
}

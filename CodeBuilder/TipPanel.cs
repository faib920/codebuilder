using System;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class TipPanel : UserControl
    {
        public TipPanel()
        {
            InitializeComponent();
        }

        public string Title { get; set; }

        public string Message { get; set; }

        private void tipPanel_Click(object sender, EventArgs e)
        {
            frmTip.Show(Parent, this, Title, Message);
        }
    }
}

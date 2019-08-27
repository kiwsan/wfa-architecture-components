using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class SearchBoxForm : Form
    {
        public SearchBoxForm()
        {
            InitializeComponent();
        }

        private void SearchBoxComponent_DoWork(object sender, DoWorkEventArgs e)
        {
            string param = (string)e.Argument;

            for (int i = 0; i < 100; i++)
            {
                if (this.SearchBoxComponent.CancellationPending)
                {
                    e.Cancel = this.SearchBoxComponent.CancellationPending;

                    break;
                }

                this.SearchBoxComponent.ReportProgress(i + 1, param);

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        private object SearchBoxComponent_StartClicked()
        {
            return "param";
        }

    }
}

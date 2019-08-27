using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BackgroundWorkerComponents
{
    public partial class SearchBoxComponent : UserControl
    {
        public SearchBoxComponent()
        {
            this.SubmitText = "&SEARCH";
            this.CancelText = "&CANCEL";

            InitializeComponent();
        }

        private bool _running;
        private string _buttonText;

        [Category("Settings")]
        public string SubmitText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                if (this.BtnSearch != null)
                {
                    this.BtnSearch.Text = _buttonText;
                }
            }
        }

        [Category("Settings")]
        public string CancelText { get; set; }

        private void SearchBoxComponent_Load(object sender, EventArgs e)
        {
            this.BtnSearch.Text = this.SubmitText;
        }

        public void ReportProgress(int progress, string value)
        {
            this.BackgroundWorker.ReportProgress(progress, value);
        }

        public bool CancellationPending
        {
            get
            {
                if (!this.BackgroundWorker.IsBusy) { return false; }

                return this.BackgroundWorker.CancellationPending;
            }
        }

        public event DoWorkEventHandler DoWork;
        public event Func<object> StartClicked;

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.DoWork != null)
            {
                this.DoWork(sender, e);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var percentage = e.ProgressPercentage;
            if (e.UserState != null)
            {
                LblStatus.Text = "Loading...";
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Cancelled");
                this.LblStatus.Text = "Cancelled";
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
                MessageBox.Show("Done");
                this.LblStatus.Text = "Ready";
            }

            this.BtnSearch.Text = this.SubmitText;
            this.BtnSearch.Enabled = true;

            _running = false;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (!_running)
            {
                if (this.StartClicked != null)
                {

                    this.LblStatus.Text = "";
                    _running = true;
                    this.BtnSearch.Text = this.CancelText;
                    this.BackgroundWorker.RunWorkerAsync(this.StartClicked());
                }
            }
            else
            {
                this.BtnSearch.Enabled = false;
                this.BackgroundWorker.CancelAsync();
            }
        }

    }
}

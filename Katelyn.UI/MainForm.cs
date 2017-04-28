using Katelyn.Core;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Katelyn.UI
{
    public partial class MainForm : Form
    {
        private BackgroundWorker _worker;
        private int _errorCount;
        private int _successCount;

        public MainForm()
        {
            InitializeComponent();

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += WorkerDoWork;
            _worker.ProgressChanged += WorkerProgressChanged;
            _worker.RunWorkerCompleted += WorkerCompleted;
        }

        public CrawlResult GetCrawlResult()
        {
            throw new NotImplementedException();
        }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            var config = e.Argument as CrawlerConfig;
            config.Listener = new BackgroundWorkerListener(worker);

            Crawler.Crawl(config);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Completed...
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage; //Progress-Value
            var userState = (string)e.UserState; //can be used to pass values to the progress-changed-event

            switch (progress)
            {
                case (int)ProgressType.Information:
                    OutputListBox.Items.Add(userState);
                    break;
                case (int)ProgressType.RequestSuccess:
                    _successCount++;
                    OutputListBox.Items.Add(userState);
                    OutputTab.Text = $"{_successCount} Output";
                    break;
                case (int)ProgressType.RequestError:
                    _errorCount++;
                    ErrorListBox.Items.Add(userState);
                    ErrorTab.Text = $"{_errorCount} Errors";
                    break;
                case (int)ProgressType.Complete:
                    OutputListBox.Items.Add(userState);
                    ResetUI();

                    if (_errorCount > 0)
                    {
                        MainTabControl.SelectedTab = ErrorTab;
                    }

                    break;

            }

            OutputListBox.SelectedIndex = OutputListBox.Items.Count - 1;
        }

        private void CrawlStart_Click(object sender, EventArgs e)
        {
            try
            {
                CrawlStart.Enabled = false;
                CrawlProgress.Style = ProgressBarStyle.Marquee;
                var config = new CrawlerConfig
                {
                    MaxDepth = int.Parse(CrawlDepth.Text),
                    CrawlerFlags = CrawlerFlags.IncludeFailureCheck | CrawlerFlags.IncludeImages | CrawlerFlags.IncludeLinks | CrawlerFlags.IncludeScripts | CrawlerFlags.IncludeStyles,
                };

                if (IsLocalPath(CrawlAddress.Text))
                {
                    config.FilePath = CrawlAddress.Text;
                }
                else
                {
                    config.RootAddress = new Uri(CrawlAddress.Text);
                }

                if (!_worker.IsBusy)
                {
                    _worker.RunWorkerAsync(config);
                }
            }
            catch (Exception ex)
            {
                ResetUI();
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            _errorCount = 0;
            _successCount = 0;

            ErrorListBox.Items.Clear();
            OutputListBox.Items.Clear();

            OutputTab.Text = "Output";
            ErrorTab.Text = "Errors";
        }


        private void ResetUI()
        {
            CrawlStart.Enabled = true;
            CrawlProgress.Style = ProgressBarStyle.Blocks;
        }

        private static bool IsLocalPath(string path)
        {
            if (path.StartsWith("http:\\"))
            {
                return false;
            }

            return new Uri(path).IsFile;
        }
    }
}

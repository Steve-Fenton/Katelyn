using Katelyn.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Katelyn.UI
{
    public partial class MainForm : Form
    {
        private BackgroundWorker _worker;
        private IList<CrawlResult> _requests = new List<CrawlResult>();
        private IList<CrawlError> _errors = new List<CrawlError>();
        private IList<CrawlResult> _externalLinks = new List<CrawlResult>();
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

        public CrawlSummary GetCrawlResult()
        {
            throw new NotImplementedException();
        }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            var config = e.Argument as UICrawlerConfig;
            config.Listener = new BackgroundWorkerListener(worker, config.StoreResult, "c:\\temp\\crawl");

            Crawler.Crawl(config);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Completed...
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage; //Progress-Value

            switch (progress)
            {
                case (int)ProgressType.Information:
                    // TODO: Tell user it has started
                    //OutputListBox.Items.Add(userState);
                    break;
                case (int)ProgressType.RequestSuccess:
                    _successCount++;

                    var crawlRequest = (CrawlResult)e.UserState;
                    _requests.Add(crawlRequest);

                    BindCrawlGrid();

                    OutputTab.Text = $"{_successCount} Output";
                    break;
                case (int)ProgressType.RequestError:
                    _errorCount++;
                    var crawlError = (CrawlError)e.UserState;
                    _errors.Add(crawlError);

                    BindErrorGrid();
                    
                    ErrorTab.Text = $"{_errorCount} Errors";
                    break;
                case (int)ProgressType.ExternalLink:
                    var externalRequest = (CrawlResult)e.UserState;

                    if (!_externalLinks.Any(l => l.Address == externalRequest.Address))
                    {
                        _externalLinks.Add(externalRequest);
                    }

                    BindExternalGrid();
                    break;
                case (int)ProgressType.Complete:
                    // TODO: Tell user it has completed
                    //OutputListBox.Items.Add(userState);
                    ResetUI();

                    if (_errorCount > 0)
                    {
                        MainTabControl.SelectedTab = ErrorTab;
                    }

                    break;
            }
        }

        private void BindCrawlGrid()
        {
            var bindingSource = new BindingSource();
            bindingSource.DataSource = _requests;
            CrawlOutput.AutoGenerateColumns = true;
            CrawlOutput.DataSource = bindingSource;
        }

        private void BindErrorGrid()
        {
            var bindingSource = new BindingSource();
            bindingSource.DataSource = _errors;
            ErrorGridView.AutoGenerateColumns = true;
            ErrorGridView.DataSource = bindingSource;
        }

        private void BindExternalGrid()
        {
            var bindingSource = new BindingSource();
            bindingSource.DataSource = _externalLinks;
            ExternalGridView.AutoGenerateColumns = true;
            ExternalGridView.DataSource = bindingSource;
        }

        private void CrawlStart_Click(object sender, EventArgs e)
        {
            try
            {
                CrawlStart.Enabled = false;
                CrawlProgress.Style = ProgressBarStyle.Marquee;
                var config = new UICrawlerConfig
                {
                    StoreResult = StoreResultCheckBox.Checked,
                    MaxDepth = int.Parse(CrawlDepth.Text),
                    HtmlContentExpression = (string.IsNullOrWhiteSpace(StringForRegex.Text)) ? null : new Regex(StringForRegex.Text),
                    CrawlerFlags = CrawlerFlags.IncludeFailureCheck | CrawlerFlags.IncludeImages | CrawlerFlags.IncludeLinks | CrawlerFlags.IncludeScripts | CrawlerFlags.IncludeStyles | CrawlerFlags.IncludeRobots,
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

            _externalLinks = new List<CrawlResult>();
            BindExternalGrid();

            _errors = new List<CrawlError>();
            BindErrorGrid();

            _requests = new List<CrawlResult>();
            BindCrawlGrid();

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
            if (path.StartsWith("http:\\", StringComparison.InvariantCultureIgnoreCase) || path.StartsWith("https:\\", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return new Uri(path).IsFile;
        }

        private void ColumnHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch(CrawlOutput.Columns[e.ColumnIndex].HeaderText)
            {
                case "Address":
                    _requests = _requests.OrderBy(r => r.Address).ToList();
                    break;
                case "ParentAddress":
                    _requests = _requests.OrderBy(r => r.ParentAddress).ToList();
                    break;
                case "ContentType":
                    _requests = _requests.OrderBy(r => r.ContentType).ToList();
                    break;
                case "Duration":
                    _requests = _requests.OrderByDescending(r => r.Duration).ToList();
                    break;
                case "StatusCode":
                    _requests = _requests.OrderBy(r => r.StatusCode).ToList();
                    break;
            }

            BindCrawlGrid();
        }

        private void ErrorColumnHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (ErrorGridView.Columns[e.ColumnIndex].HeaderText)
            {
                case "Address":
                    _errors = _errors.OrderBy(r => r.Address).ToList();
                    break;
                case "ParentAddress":
                    _errors = _errors.OrderBy(r => r.ParentAddress).ToList();
                    break;
                case "ContentType":
                    _errors = _errors.OrderBy(r => r.ContentType).ToList();
                    break;
                case "Duration":
                    _errors = _errors.OrderByDescending(r => r.Duration).ToList();
                    break;
                case "StatusCode":
                    _errors = _errors.OrderBy(r => r.StatusCode).ToList();
                    break;
            }

            BindErrorGrid();
        }

        private void ExternalColumnHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (CrawlOutput.Columns[e.ColumnIndex].HeaderText)
            {
                case "Address":
                    _externalLinks = _externalLinks.OrderBy(r => r.Address).ToList();
                    break;
                case "ParentAddress":
                    _externalLinks = _externalLinks.OrderBy(r => r.ParentAddress).ToList();
                    break;
                case "ContentType":
                    _externalLinks = _externalLinks.OrderBy(r => r.ContentType).ToList();
                    break;
                case "Duration":
                    _externalLinks = _externalLinks.OrderByDescending(r => r.Duration).ToList();
                    break;
                case "StatusCode":
                    _externalLinks = _externalLinks.OrderBy(r => r.StatusCode).ToList();
                    break;
            }

            BindExternalGrid();
        }
    }
}

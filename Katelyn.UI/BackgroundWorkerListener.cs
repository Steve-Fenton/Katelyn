using Katelyn.Core;
using System;
using System.ComponentModel;
using System.IO;

namespace Katelyn.UI
{
    public class BackgroundWorkerListener
        : IListener
    {
        private BackgroundWorker _worker;
        private bool _storeResult;
        private DirectoryInfo _outputDirectory;
        protected int ErrorCount;
        protected int SuccessCount;

        public BackgroundWorkerListener(BackgroundWorker worker, bool storeResult, string outputPath)
        {
            _worker = worker;
            _storeResult = storeResult;

            if (_storeResult)
            {
                _outputDirectory = new DirectoryInfo(Path.Combine(outputPath, "katelyn", DateTime.UtcNow.ToFileTimeUtc().ToString()));
                _outputDirectory.Create();
            }
        }

        public virtual void OnSuccess(CrawlResult request)
        {
            SuccessCount++;

            _worker.ReportProgress((int)ProgressType.RequestSuccess, request);
        }

        public virtual void OnError(CrawlResult request, Exception exception)
        {
            ErrorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            _worker.ReportProgress((int)ProgressType.RequestError, new CrawlError(request, exception.Message));
        }

        public void OnDocumentLoaded(CrawlResult request)
        {
            if (!_storeResult)
            {
                return;
            }

            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            string fileName = request.Address;

            foreach (char c in invalid)
            {
                fileName = fileName.Replace(c.ToString(), "-");
            }

            File.WriteAllText(Path.Combine(_outputDirectory.FullName, fileName + ".html"), request.Document);
        }

        public void OnThirdPartyAddress(CrawlResult request)
        {
            _worker.ReportProgress((int)ProgressType.ExternalLink, request);
        }

        public void OnStart()
        {
            _worker.ReportProgress((int)ProgressType.Information, "Katelyn - Well known for Crawling");
        }

        public virtual void OnEnd()
        {
            _worker.ReportProgress((int)ProgressType.Complete, $"Katelyn - Finished Crawling. {SuccessCount}/{SuccessCount + ErrorCount} succeeded.");
        }

        public virtual CrawlSummary GetCrawlResult()
        {
            return new CrawlSummary
            {
                ErrorCount = ErrorCount,
                SuccessCount = SuccessCount
            };
        }
    }
}
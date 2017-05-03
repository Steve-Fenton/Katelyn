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

        public virtual void OnSuccess(string address, string parent)
        {
            SuccessCount++;

            _worker.ReportProgress((int)ProgressType.RequestSuccess, $"OK {address} Found on {parent}");
        }

        public virtual void OnError(string address, string parent, Exception exception)
        {
            ErrorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            _worker.ReportProgress((int)ProgressType.RequestError, $"Exception from {address} Found on {parent} {exception.Message}");
        }

        public void OnDocumentLoaded(string address, string parent, string document)
        {
            if (!_storeResult)
            {
                return;
            }

            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            string fileName = address;

            foreach (char c in invalid)
            {
                fileName = fileName.Replace(c.ToString(), "-");
            }

            File.WriteAllText(Path.Combine(_outputDirectory.FullName, fileName + ".html"), document);
        }

        public void OnStart()
        {
            _worker.ReportProgress((int)ProgressType.Information, "Katelyn - Well known for Crawling");
        }

        public virtual void OnEnd()
        {
            _worker.ReportProgress((int)(int)ProgressType.Complete, $"Katelyn - Finished Crawling. {SuccessCount}/{SuccessCount + ErrorCount} succeeded.");
        }

        public virtual CrawlResult GetCrawlResult()
        {
            return new CrawlResult
            {
                ErrorCount = ErrorCount,
                SuccessCount = SuccessCount
            };
        }
    }
}
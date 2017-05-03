using Katelyn.Core;
using System;
using System.ComponentModel;

namespace Katelyn.UI
{
    public class BackgroundWorkerListener
        : IListener
    {
        private BackgroundWorker _worker;
        protected int ErrorCount;
        protected int SuccessCount;

        public bool IsRunning { get; private set; } = true;

        public BackgroundWorkerListener(BackgroundWorker worker)
        {
            _worker = worker;
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
            // TODO: Store results... can be used for comparison later...
            return;
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
using Katelyn.Core;
using System;
using System.Collections.Generic;

namespace Katelyn.Tests
{
    public abstract class TestBase
        : IListener
    {
        protected int _errorCount;
        protected int _successCount;
        private IList<string> _errors = new List<string>();

        public void OnSuccess(CrawlRequest request)
        {
            _successCount++;
        }

        public void OnError(CrawlRequest request, Exception exception)
        {
            _errorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            _errors.Add($"{request.Address} found on {request.ParentAddress} - {exception.Message}");
        }

        public void OnDocumentLoaded(CrawlRequest request)
        {
            return;
        }

        public void OnStart()
        {
            return;
        }

        public abstract void OnEnd();

        public CrawlResult GetCrawlResult()
        {
            return new CrawlResult
            {
                ErrorCount = _errorCount,
                SuccessCount = _successCount
            };
        }
    }
}
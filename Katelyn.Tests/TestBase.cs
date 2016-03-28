using Katelyn.Core;
using System;
using System.Collections.Generic;

namespace Katelyn.Tests
{
    public abstract class TestBase
        : IListener
    {
        protected int _errorCount = 0;
        protected int _successCount = 0;
        private IList<string> _errors = new List<string>();

        public void OnSuccess(string address)
        {
            _successCount++;
        }

        public void OnError(string address, Exception exception)
        {
            _errorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            _errors.Add($"{address} {exception.Message}");
        }

        public abstract void OnEnd();

        public CrawlResult GetCrawlResult()
        {
            return new CrawlResult
            {
                ErrorCount = _errorCount,
                SuccessCount = _successCount,
            };
        }
    }
}

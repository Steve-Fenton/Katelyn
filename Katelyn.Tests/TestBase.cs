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

        public void OnSuccess(string address, string parent)
        {
            _successCount++;
        }

        public void OnError(string address, string parent, Exception exception)
        {
            _errorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            _errors.Add($"{address} found on {parent} - {exception.Message}");
        }

        public void OnStart()
        {
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

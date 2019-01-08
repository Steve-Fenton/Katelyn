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
        protected List<string> _crawledAddresses = new List<string>();
        protected List<string> _thirdPartyAddresses = new List<string>();
        private IList<string> _errors = new List<string>();

        public void OnSuccess(CrawlResult request)
        {
            _successCount++;
        }

        public void OnError(CrawlResult request, Exception exception)
        {
            _errorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            _errors.Add($"{request.Address} found on {request.ParentAddress} - {exception.Message}");
        }

        public void OnDocumentLoaded(CrawlResult request)
        {
            _crawledAddresses.Add(request.Address);
        }

        public void OnThirdPartyAddress(CrawlResult request)
        {
            _thirdPartyAddresses.Add(address.Address);
        }

        public void OnStart()
        {
            return;
        }

        public abstract void OnEnd();

        public CrawlSummary GetCrawlResult()
        {
            return new CrawlSummary
            {
                ErrorCount = _errorCount,
                SuccessCount = _successCount
            };
        }
    }
}
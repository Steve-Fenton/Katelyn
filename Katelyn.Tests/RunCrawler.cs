using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Katelyn.Tests
{
    [TestClass]
    public partial class IntegrationTest
        : IListener
    {
        private int _errorCount = 0;
        private int _successCount = 0;
        private IList<string> _errors = new List<string>();

        [TestMethod]
        public void RunCrawler()
        {
            // TODO: Test Page?
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://localhost/"),
                Listener = this,
                MaxDepth = 2,
                CrawlerFlags = CrawlerFlags.IncludeLinks
            };

            Crawler.Crawl(config);
        }

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

        public void OnEnd()
        {
            _errorCount.ShouldBe(0);
            _successCount.ShouldBe(1);
        }
    }
}
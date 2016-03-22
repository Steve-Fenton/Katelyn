using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Katelyn.Tests
{
    [TestClass]
    public class IntegrationTest
        : IListener
    {
        public void OnEnd()
        {
            _errorCount.ShouldBe(0);
            _successCount.ShouldBeGreaterThan(0);
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

        public void OnSuccess(string address)
        {
            _successCount++;
        }

        [TestMethod]
        public void RunCrawler()
        {
            // TODO: Test Page?
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://env-dev-01/"),
                Listener = this,
                MaxDepth = 1
            };

            Crawler.Crawl(config);
        }

        private int _errorCount = 0;
        private IList<string> _errors = new List<string>();
        private int _successCount = 0;
    }
}
using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    [TestClass]
    public class RunCrawlerTest
        : TestBase
    {
        [TestMethod]
        public void RunCrawler()
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://localhost:51746/"),
                Listener = this,
                MaxDepth = 2,
                CrawlerFlags = CrawlerFlags.IncludeLinks | CrawlerFlags.IncludeFailureCheck
            };

            Crawler.Crawl(config);
        }

        public override void OnEnd()
        {
            _errorCount.ShouldBe(0);
            _successCount.ShouldBe(1);
        }
    }
}
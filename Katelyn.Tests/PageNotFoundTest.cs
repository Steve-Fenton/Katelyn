using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    [TestClass]
    public class PageNotFoundTest
        : TestBase
    {
        [TestMethod]
        public void PageNotFound()
        {
            // TODO: Test Page?
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://PageNotFound/"),
                Listener = this,
                MaxDepth = 2,
                CrawlerFlags = CrawlerFlags.IncludeLinks
            };

            Crawler.Crawl(config);
        }

        public override void OnEnd()
        {
            _errorCount.ShouldBe(1);
            _successCount.ShouldBe(0);
        }
    }
}
using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    [TestClass]
    public class WhenAPageSignalsAKatelynError
        : TestBase
    {
        [TestMethod]
        public void TheCrawlShouldRecordAnError()
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://localhost:51746/katelyn-error.html"),
                Listener = this,
                MaxDepth = 2,
                CrawlerFlags = CrawlerFlags.IncludeFailureCheck
            };

            Crawler.Crawl(config);
        }

        public override void OnEnd()
        {
            _errorCount.ShouldBe(1);
            _errors.ShouldContain("http://localhost:51746/katelyn-error.html found on  - At 275 - KATELYN:ERRORS(1)");

            _successCount.ShouldBe(0);
        }
    }
}
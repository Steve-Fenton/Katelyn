using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    [TestClass]
    public class RunCrawlerWithScriptsTest
        : TestBase
    {
        [TestMethod]
        public void RunCrawlerWithScripts()
        {
            // TODO: Test Page?
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://localhost/"),
                Listener = this,
                MaxDepth = 2,
            };

            config.CrawlerFlags |= CrawlerFlags.IncludeLinks;
            config.CrawlerFlags |= CrawlerFlags.IncludeScripts;

            Crawler.Crawl(config);
        }

        public override void OnEnd()
        {
            _errorCount.ShouldBe(0);
            _successCount.ShouldBe(3);
        }
    }
}
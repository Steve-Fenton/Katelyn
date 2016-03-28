using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    [TestClass]
    public class RunCrawlerWithScriptsStylesImagesTest
        : TestBase
    {
        [TestMethod]
        public void RunCrawlerWithScriptsStylesImages()
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
            config.CrawlerFlags |= CrawlerFlags.IncludeStyles;
            config.CrawlerFlags |= CrawlerFlags.IncludeImages;

            Crawler.Crawl(config);
        }

        public override void OnEnd()
        {
            _errorCount.ShouldBe(0);
            _successCount.ShouldBe(5);
        }
    }
}
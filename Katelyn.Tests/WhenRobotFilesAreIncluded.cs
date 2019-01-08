using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    [TestClass]
    public class WhenRobotFilesAreIncluded
        : TestBase
    {
        [TestMethod]
        public void TheRobotsFileShouldBeCrawledAndSitemapLinksFollowed()
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://localhost:51746/"),
                Listener = this,
                MaxDepth = 10,
            };

            config.CrawlerFlags |= CrawlerFlags.IncludeRobots;

            Crawler.Crawl(config);
        }

        public override void OnEnd()
        {
            _errorCount.ShouldBe(0);


            _successCount.ShouldBe(4);
            _crawledAddresses.Contains("http://localhost:51746/").ShouldBeTrue();
            _crawledAddresses.Contains("http://localhost:51746/robots.txt").ShouldBeTrue();
            _crawledAddresses.Contains("http://localhost:51746/sitemap.xml").ShouldBeTrue();
            _crawledAddresses.Contains("http://localhost:51746/only-in-sitemap.html").ShouldBeTrue();
        }
    }
}
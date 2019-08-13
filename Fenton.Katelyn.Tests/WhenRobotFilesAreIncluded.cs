using Katelyn.Core;
using NUnit.Framework;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    public class WhenRobotFilesAreIncluded
        : TestBase
    {
        [Test]
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
            _crawledAddresses.ShouldContain("http://localhost:51746/");
            _crawledAddresses.ShouldContain("http://localhost:51746/robots.txt");
            _crawledAddresses.ShouldContain("http://localhost:51746/sitemap.xml");
            _crawledAddresses.ShouldContain("http://localhost:51746/only-in-sitemap.html");
        }
    }
}
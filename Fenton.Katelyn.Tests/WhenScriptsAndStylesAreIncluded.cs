using Katelyn.Core;
using NUnit.Framework;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    public class WhenScriptsAndStylesAreIncluded
        : TestBase
    {
        [Test]
        public void ScriptTagsAndLinksTagsShouldBeCrawled()
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://localhost:51746/"),
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

            _successCount.ShouldBe(4);
            _crawledAddresses.ShouldContain("http://localhost:51746/");
            _crawledAddresses.ShouldContain("http://localhost:51746/app.css");
            _crawledAddresses.ShouldContain("http://localhost:51746/app.js");
            _crawledAddresses.ShouldContain("http://localhost:51746/fentonicon.png");
        }
    }
}
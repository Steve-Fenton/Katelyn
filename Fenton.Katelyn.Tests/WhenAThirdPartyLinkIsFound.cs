using Katelyn.Core;
using NUnit.Framework;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    public class WhenAThirdPartyLinkIsFound
        : TestBase
    {
        [Test]
        public void TheThirdPartyLinkShouldBeLoggedButNotCrawled()
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
            _crawledAddresses.ShouldContain("http://localhost:51746/");
            _crawledAddresses.ShouldNotContain("https://example.com/");

            _thirdPartyAddresses.Count.ShouldBe(1);
            _thirdPartyAddresses.ShouldContain("https://example.com/");
        }
    }
}
using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    [TestClass]
    public class WhenAThirdPartyLinkIsFound
        : TestBase
    {
        [TestMethod]
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
            _crawledAddresses.Contains("http://localhost:51746/").ShouldBeTrue();
            _crawledAddresses.Contains("https://example.com/").ShouldBeFalse();

            _thirdPartyAddresses.Count.ShouldBe(1);
            _thirdPartyAddresses.Contains("https://example.com/").ShouldBeTrue();
        }
    }
}
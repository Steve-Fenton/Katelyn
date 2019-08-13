using Katelyn.Core;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Katelyn.Tests
{
    public class WhenAPartnerLinkIsFound
        : TestBase
    {
        [Test]
        public void ThePartnerLinkShouldBeCrawled()
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://localhost:51746/partner.html"),
                Listener = this,
                MaxDepth = 5,
                PartnerSites = new List<Uri> { new Uri("https://example.com") },
                CrawlerFlags = CrawlerFlags.IncludeLinks
            };

            Crawler.Crawl(config);
        }

        public override void OnEnd()
        {
            _errorCount.ShouldBe(0);

            _successCount.ShouldBe(2);
            _crawledAddresses.ShouldContain("http://localhost:51746/partner.html");
            _crawledAddresses.ShouldContain("https://example.com/");
        }
    }
}
using Katelyn.Core;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace Katelyn.Tests
{
    public class WhenAPageDoesNotExist
        : TestBase
    {
        [Test]
        public void TheCrawlShouldRecordAnError()
        {
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
            _errors
                .Any(err => err.Contains("http://pagenotfound/ found on"))
                .ShouldBeTrue();

            _successCount.ShouldBe(0);
        }
    }
}
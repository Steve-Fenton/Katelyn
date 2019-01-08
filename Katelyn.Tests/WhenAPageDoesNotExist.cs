﻿using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Katelyn.Tests
{
    [TestClass]
    public class WhenAPageDoesNotExist
        : TestBase
    {
        [TestMethod]
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
            _errors.ShouldContain("http://pagenotfound/ found on  - The remote name could not be resolved: 'pagenotfound'");

            _successCount.ShouldBe(0);
        }
    }
}
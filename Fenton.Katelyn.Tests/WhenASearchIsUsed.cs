﻿using Katelyn.Core;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Katelyn.Tests
{
    public class WhenASearchIsUsed
        : TestBase
    {
        [Test]
        public void ThenMatchesShouldBeFound()
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri("http://localhost:51746/"),
                Listener = this,
                MaxDepth = 1,
                CrawlerFlags = CrawlerFlags.IncludeLinks,
                HtmlContentExpression = new Regex("#search-link")
            };

            Crawler.Crawl(config);
        }

        public override void OnEnd()
        {
            _errorCount.ShouldBe(1);
            _errors
                .Any(err => err.Contains("http://localhost:51746/ found on") && err.Contains("#search-link"))
                .ShouldBeTrue();

            _successCount.ShouldBe(0);
        }
    }
}
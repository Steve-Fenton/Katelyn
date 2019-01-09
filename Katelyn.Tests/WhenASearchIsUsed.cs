using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Text.RegularExpressions;

namespace Katelyn.Tests
{
    [TestClass]
    public class WhenASearchIsUsed
        : TestBase
    {
        [TestMethod]
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
            _errors.ShouldContain("http://localhost:51746/ found on  - At 385 - #search-link");

            _successCount.ShouldBe(0);
        }
    }
}
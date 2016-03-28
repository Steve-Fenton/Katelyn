using CLAP;
using Katelyn.Core;
using Katelyn.Core.Listeners;
using System;

namespace Katelyn.ConsoleRunner
{
    public class KatelynRunner
    {
        [Verb]
        public static int Crawl(string address, int maxDepth = 5, bool includeImages = false, bool includeLinks = true, bool includeScripts = false, bool includeStyles = false)
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri(address),
                Listener = new ConsoleListener(),
                MaxDepth = maxDepth
            };

            if (includeLinks)
            {
                config.CrawlerFlags |= CrawlerFlags.IncludeLinks;
            }

            if (includeImages)
            {
                config.CrawlerFlags |= CrawlerFlags.IncludeImages;
            }

            if (includeScripts)
            {
                config.CrawlerFlags |= CrawlerFlags.IncludeScripts;
            }

            if (includeStyles)
            {
                config.CrawlerFlags |= CrawlerFlags.IncludeStyles;
            }

            Crawler.Crawl(config);

            return (config.Listener.GetCrawlResult().ErrorCount == 0)
                ? (int)ExitCode.Success
                : (int)ExitCode.CrawlError;
        }
    }
}
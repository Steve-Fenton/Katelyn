using CLAP;
using Katelyn.Core;
using Katelyn.Core.Listeners;
using System;

namespace Katelyn.ConsoleRunner
{
    public class KatelynRunner
    {
        [Verb]
        public static void Crawl(string address, int maxDepth = 5, bool includeImages = false, bool includeLinks = false, bool includeScripts = false, bool includeStyles = false, bool verbose = false)
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri(address),
                Listener = (verbose) ? new ConsoleListener() : new SparseConsoleListener(),
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

            if (config.Listener.GetCrawlResult().ErrorCount == 0)
            {
                Environment.Exit((int)ExitCode.CrawlError);
            }
        }
    }
}
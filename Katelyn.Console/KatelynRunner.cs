using CLAP;
using Katelyn.Core;
using Katelyn.Core.Listeners;
using System;

namespace Katelyn.ConsoleRunner
{
    public class KatelynRunner
    {
        [Verb]
        public static void Crawl(string address)
        {
            bool verbose = true;

            Crawl(address, verbose);
        }

        [Verb]
        public static void Crawl(string address, bool verbose)
        {
            var config = GetQuickConfig(address, verbose);

            Crawler.Crawl(config);

            if (config.Listener.GetCrawlResult().ErrorCount == 0)
            {
                Environment.Exit((int)ExitCode.CrawlError);
            }
        }

        [Verb]
        public static void Crawl(string address, bool quick, bool verbose, int maxDepth, bool includeImages, bool includeLinks, bool includeScripts, bool includeStyles, int delay)
        {
            CrawlerConfig config = (quick)
                ? GetQuickConfig(address, verbose)
                : GetComplexConfig(address, maxDepth, includeImages, includeLinks, includeScripts, includeStyles, verbose, delay);

            Crawler.Crawl(config);

            if (config.Listener.GetCrawlResult().ErrorCount == 0)
            {
                Environment.Exit((int)ExitCode.CrawlError);
            }
        }

        private static CrawlerConfig GetQuickConfig(string address, bool verbose)
        {
            return new CrawlerConfig
            {
                RootAddress = new Uri(address),
                Listener = (verbose) ? new ConsoleListener() : new SparseConsoleListener(),
                MaxDepth = 100,
                CrawlDelay = TimeSpan.Zero,
                CrawlerFlags = CrawlerFlags.IncludeImages | CrawlerFlags.IncludeLinks | CrawlerFlags.IncludeScripts | CrawlerFlags.IncludeStyles,
            };
        }

        private static CrawlerConfig GetComplexConfig(string address, int maxDepth, bool includeImages, bool includeLinks, bool includeScripts, bool includeStyles, bool verbose, int delay)
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri(address),
                Listener = (verbose) ? new ConsoleListener() : new SparseConsoleListener()
            };

            if (maxDepth > 0)
            {
                config.MaxDepth = maxDepth;
            }

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

            if (delay > 0)
            {
                config.CrawlDelay = TimeSpan.FromMilliseconds(delay);
            }

            return config;
        }
    }
}
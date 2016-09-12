using CLAP;
using Katelyn.Core;
using Katelyn.Core.Listeners;
using Newtonsoft.Json;
using System;

namespace Katelyn.ConsoleRunner
{
    public class KatelynRunner
    {
        [Verb]
        public static void Crawl([Required]string address, [DefaultValue(true)] bool verbose)
        {
            var config = GetQuickConfig(address, verbose);

            Console.WriteLine(JsonConvert.SerializeObject(config));

            Crawler.Crawl(config);

            if (config.Listener.GetCrawlResult().ErrorCount == 0)
            {
                Environment.Exit((int)ExitCode.CrawlError);
            }
        }

        [Verb]
        public static void CrawlWith(
            [Required]string address,
            [DefaultValue(true)]bool verbose,
            [DefaultValue(true)]bool includeImages,
            [DefaultValue(true)]bool includeLinks,
            [DefaultValue(true)]bool includeScripts,
            [DefaultValue(true)]bool includeStyles,
            [DefaultValue(100)]int maxDepth,
            [DefaultValue(0)]int delay)
        {
            CrawlerConfig config = GetComplexConfig(address, verbose, includeImages, includeLinks, includeScripts, includeStyles, maxDepth, delay);

            Console.WriteLine(JsonConvert.SerializeObject(config));

            Crawler.Crawl(config);

            if (config.Listener.GetCrawlResult().ErrorCount > 0)
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

        private static CrawlerConfig GetComplexConfig(string address, bool verbose, bool includeImages, bool includeLinks, bool includeScripts, bool includeStyles, int maxDepth, int delay)
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
using CLAP;
using Katelyn.Core;
using Katelyn.Core.Listeners;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Katelyn.ConsoleRunner
{
#pragma warning disable RECS0014 // If all fields, properties and methods members are static, the class can be made static.

    public class KatelynRunner
#pragma warning restore RECS0014 // If all fields, properties and methods members are static, the class can be made static.
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
            [DefaultValue(true)]bool includeFailureCheck,
            [DefaultValue(true)]bool includeRobots,
            [DefaultValue(100)]int maxDepth,
            [DefaultValue(0)]int delay,
            [DefaultValue("")]string searchExpression,
            [DefaultValue("")]string partnerSites)
        {
            var config = GetComplexConfig(address, verbose, includeImages, includeLinks, includeScripts, includeStyles, includeFailureCheck, includeRobots, maxDepth, delay, searchExpression, partnerSites);

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
                CrawlerFlags = CrawlerFlags.IncludeImages | CrawlerFlags.IncludeLinks | CrawlerFlags.IncludeScripts | CrawlerFlags.IncludeStyles | CrawlerFlags.IncludeFailureCheck
            };
        }

        private static CrawlerConfig GetComplexConfig(string address, bool verbose, bool includeImages, bool includeLinks, bool includeScripts, bool includeStyles, bool includeFailureCheck, bool includeRobots, int maxDepth, int delay, string searchExpression, string partnerSites)
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri(address),
                Listener = (verbose) ? new ConsoleListener() : new SparseConsoleListener()
            };

            if (!string.IsNullOrWhiteSpace(searchExpression))
            {
                config.HtmlContentExpression = new Regex(searchExpression);
            }

            if (!string.IsNullOrWhiteSpace(partnerSites))
            {
                config.PartnerSites = partnerSites.Split(',').Select(s => new Uri(s)).ToList();
            }

            if (maxDepth > 0)
            {
                config.MaxDepth = maxDepth;
            }

            if (delay > 0)
            {
                config.CrawlDelay = TimeSpan.FromMilliseconds(delay);
            }

            config.AddCrawlerFlag(() => includeLinks, CrawlerFlags.IncludeLinks);
            config.AddCrawlerFlag(() => includeImages, CrawlerFlags.IncludeImages);
            config.AddCrawlerFlag(() => includeScripts, CrawlerFlags.IncludeScripts);
            config.AddCrawlerFlag(() => includeStyles, CrawlerFlags.IncludeStyles);
            config.AddCrawlerFlag(() => includeFailureCheck, CrawlerFlags.IncludeFailureCheck);
            config.AddCrawlerFlag(() => includeRobots, CrawlerFlags.IncludeRobots);

            return config;
        }
    }
}
using CLAP;
using Katelyn.Core;
using Katelyn.Core.Listeners;
using System;

namespace Katelyn.ConsoleRunner
{
    public class KatelynRunner
    {
        [Verb]
        public static void Crawl(string address, int maxDepth = 5)
        {
            var config = new CrawlerConfig
            {
                RootAddress = new Uri(address),
                Listener = new ConsoleListener(),
                MaxDepth = maxDepth
            };

            Crawler.Crawl(config);
        }
    }
}
using System;

namespace Katelyn.Core
{
    public class CrawlerConfig
    {
        public Uri RootAddress { get; set; }

        public IListener Listener { get; set; }

        public int MaxDepth { get; set; } = 5;

        public CrawlerFlags CrawlerFlags { get; set; } = CrawlerFlags.None;
    }
}
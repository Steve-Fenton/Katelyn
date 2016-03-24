using System;

namespace Katelyn.Core
{
    public class CrawlerConfig
    {
        public IListener Listener { get; set; }

        public int MaxDepth { get; set; } = 5;

        public Uri RootAddress { get; set; }
    }
}
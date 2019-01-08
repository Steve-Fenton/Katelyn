using System;
using System.Text.RegularExpressions;

namespace Katelyn.Core
{
    public class CrawlerConfig
    {
        public Uri RootAddress { get; set; }

        public string FilePath { get; set; }

        public IListener Listener { get; set; }

        public int MaxDepth { get; set; } = 5;

        public CrawlerFlags CrawlerFlags { get; set; } = CrawlerFlags.None;

        public TimeSpan CrawlDelay { get; set; } = TimeSpan.Zero;

        public Regex HtmlContentExpression { get; set; } = null;

        public void AddCrawlerFlag(Func<bool> predicate, CrawlerFlags flag)
        {
            if (predicate())
            {
                CrawlerFlags |= flag;
            }
        }
    }
}
using Katelyn.Core;

namespace Katelyn.UI
{
    public class UICrawlerConfig : CrawlerConfig
    {
        public bool StoreResult { get; set; }

        public bool ErrorsOnly { get; set; }
    }
}

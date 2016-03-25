using System;

namespace Katelyn.Core
{
    [Flags]
    public enum CrawlerFlags
    {
        None = 0,
        IncludeScripts = 1,
        IncludeImages = 2,
        IncludeStyles = 4,
    }
}
namespace Katelyn.Core;

[Flags]
public enum CrawlerFlags
{
    None = 0,
    IncludeImages = 1 << 0,
    IncludeLinks = 1 << 1,
    IncludeScripts = 1 << 2,
    IncludeStyles = 1 << 3,
    IncludeFailureCheck = 1 << 4,
    IncludeRobots = 1 << 5
}
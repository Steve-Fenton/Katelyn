namespace Katelyn.Tests;

public class WhenScriptsAreIncluded
    : TestBase
{
    [Test]
    public void ScriptTagsShouldBeCrawled()
    {
        var config = new CrawlerConfig
        {
            RootAddress = new Uri("http://localhost:51746/"),
            Listener = this,
            MaxDepth = 2,
        };

        config.CrawlerFlags |= CrawlerFlags.IncludeLinks;
        config.CrawlerFlags |= CrawlerFlags.IncludeScripts;

        Crawler.Crawl(config);
    }

    public override void OnEnd()
    {
        _errorCount.ShouldBe(0);

        _successCount.ShouldBe(2);
        _crawledAddresses.ShouldContain("http://localhost:51746/");
        _crawledAddresses.ShouldContain("http://localhost:51746/app.js");
    }
}
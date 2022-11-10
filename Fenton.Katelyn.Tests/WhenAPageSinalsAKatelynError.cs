namespace Katelyn.Tests;

public class WhenAPageSignalsAKatelynError
    : TestBase
{
    [Test]
    public void TheCrawlShouldRecordAnError()
    {
        var config = new CrawlerConfig
        {
            RootAddress = new Uri("http://localhost:51746/katelyn-error.html"),
            Listener = this,
            MaxDepth = 2,
            CrawlerFlags = CrawlerFlags.IncludeFailureCheck
        };

        Crawler.Crawl(config);
    }

    public override void OnEnd()
    {
        _errorCount.ShouldBe(1);
        _errors
            .Any(err => err.Contains("http://localhost:51746/katelyn-error.html found on") && err.Contains("KATELYN:ERRORS(1)"))
            .ShouldBeTrue();

        _successCount.ShouldBe(0);
    }
}
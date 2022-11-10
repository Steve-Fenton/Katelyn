using System.Diagnostics;

namespace Katelyn.Core;

public class CrawlTimer
{
    private Stopwatch _stopwatch = new Stopwatch();

    private CrawlTimer()
    {
        _stopwatch.Start();
    }

    public static CrawlTimer Start()
    {
        return new CrawlTimer();
    }

    public long Stop()
    {
        _stopwatch.Stop();
        return _stopwatch.ElapsedMilliseconds;
    }
}

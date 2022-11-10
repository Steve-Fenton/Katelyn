using System.IO;

namespace Katelyn.Core.Listeners;

public class ConsoleListener
    : IListener
{
    protected readonly ConsoleColor GoodForeground = ConsoleColor.Green;
    protected readonly ConsoleColor BadForeground = ConsoleColor.Red;
    protected readonly ConsoleColor InfoForeground = ConsoleColor.Gray;
    protected readonly ConsoleColor MinimalForeground = ConsoleColor.DarkGray;
    protected readonly ConsoleColor TitleForeground = ConsoleColor.White;
    protected int ErrorCount;
    protected int SuccessCount;

    public virtual void OnSuccess(CrawlResult request)
    {
        SuccessCount++;

        Console.ForegroundColor = GoodForeground;
        Console.Write($"OK {request.Address} ");
        Console.ForegroundColor = InfoForeground;
        Console.WriteLine($"{request.Duration}ms");
        Console.ForegroundColor = MinimalForeground;
        Console.WriteLine($"   Found on {request.ParentAddress}");
    }

    public virtual void OnError(CrawlResult request, Exception exception)
    {
        ErrorCount++;

        while (exception.InnerException != null)
        {
            exception = exception.InnerException;
        }

        Console.ForegroundColor = BadForeground;
        TextWriter errorWriter = Console.Error;
        errorWriter.WriteLine($"Exception from {request.Address}");
        errorWriter.WriteLine($"   Found on {request.ParentAddress}");
        errorWriter.WriteLine($"   {exception.Message}");
    }

    public void OnDocumentLoaded(CrawlResult request)
    {
        //Console.ForegroundColor = InfoForeground;
        //Console.WriteLine($"Document loaded: {request.Address} ({request.Duration}ms)");
        //Console.WriteLine($"   Found on {request.ParentAddress}");
    }

    public void OnThirdPartyAddress(CrawlResult request)
    {
        Console.ForegroundColor = InfoForeground;
        Console.WriteLine($"Skipped external resource: {request.Address}");
        Console.WriteLine($"   Found on {request.ParentAddress}");
    }

    public void OnStart()
    {
        Console.ForegroundColor = TitleForeground;
        Console.Title = "Katelyn - Well known for Crawling";
    }

    public virtual void OnEnd()
    {
        Console.WriteLine("");
        Console.Title = "Katelyn - Finished Crawling";
        Console.ForegroundColor = TitleForeground;
        Console.WriteLine($"Finished. {SuccessCount}/{SuccessCount + ErrorCount} succeeded.");
        Console.WriteLine("");
    }

    public virtual CrawlSummary GetCrawlResult()
    {
        return new CrawlSummary
        {
            ErrorCount = ErrorCount,
            SuccessCount = SuccessCount
        };
    }
}
using System;
using System.IO;

namespace Katelyn.Core.Listeners
{
    public class ConsoleListener
        : IListener
    {
        protected readonly ConsoleColor GoodForeground = ConsoleColor.Green;
        protected readonly ConsoleColor BadForeground = ConsoleColor.Red;
        protected int ErrorCount;
        protected int SuccessCount;

        public virtual void OnSuccess(CrawlResult request)
        {
            SuccessCount++;

            var color = Console.ForegroundColor;
            Console.ForegroundColor = GoodForeground;
            Console.WriteLine($"OK {request.Address}");
            Console.ForegroundColor = color;
            Console.WriteLine($"   Found on {request.ParentAddress}");
        }

        public virtual void OnError(CrawlResult request, Exception exception)
        {
            ErrorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            var color = Console.ForegroundColor;
            Console.ForegroundColor = BadForeground;
            TextWriter errorWriter = Console.Error;
            errorWriter.WriteLine($"Exception from {request.Address}");
            Console.ForegroundColor = color;
            errorWriter.WriteLine($"   Found on {request.ParentAddress}");
            errorWriter.WriteLine($"   {exception.Message}");
        }

        public void OnDocumentLoaded(CrawlResult request)
        {
            return;
        }

        public void OnStart()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "Katelyn - Well known for Crawling";
        }

        public virtual void OnEnd()
        {
            Console.Title = "Katelyn - Finished Crawling";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Finished. {SuccessCount}/{SuccessCount + ErrorCount} succeeded.");
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
}
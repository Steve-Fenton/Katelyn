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

        public virtual void OnSuccess(string address, string parent)
        {
            SuccessCount++;

            var color = Console.ForegroundColor;
            Console.ForegroundColor = GoodForeground;
            Console.WriteLine($"OK {address}");
            Console.ForegroundColor = color;
            Console.WriteLine($"   Found on {parent}");
        }

        public virtual void OnError(string address, string parent, Exception exception)
        {
            ErrorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            var color = Console.ForegroundColor;
            Console.ForegroundColor = BadForeground;
            TextWriter errorWriter = Console.Error;
            errorWriter.WriteLine($"Exception from {address}");
            Console.ForegroundColor = color;
            errorWriter.WriteLine($"   Found on {parent}");
            errorWriter.WriteLine($"   {exception.Message}");
        }

        public void OnDocumentLoaded(string address, string parent, string document)
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

        public virtual CrawlResult GetCrawlResult()
        {
            return new CrawlResult
            {
                ErrorCount = ErrorCount,
                SuccessCount = SuccessCount
            };
        }
    }
}
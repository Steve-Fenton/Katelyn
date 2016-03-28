using System;
using System.IO;

namespace Katelyn.Core.Listeners
{
    public class ConsoleListener
        : IListener
    {
        protected readonly ConsoleColor GoodForeground = ConsoleColor.Green;
        protected readonly ConsoleColor BadForeground = ConsoleColor.Red;
        protected int ErrorCount = 0;
        protected int SuccessCount = 0;

        public virtual void OnSuccess(string address)
        {
            SuccessCount++;

            Console.ForegroundColor = GoodForeground;
            Console.WriteLine($"OK {address}");
        }

        public virtual void OnError(string address, Exception exception)
        {
            ErrorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            Console.ForegroundColor = BadForeground;
            TextWriter errorWriter = Console.Error;
            errorWriter.WriteLine($"Exception from {address} {exception.Message}");
        }

        public virtual void OnEnd()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Finished. {SuccessCount}/{SuccessCount + ErrorCount} succeeded.");
        }

        public virtual CrawlResult GetCrawlResult()
        {
            return new CrawlResult
            {
                ErrorCount = ErrorCount,
                SuccessCount = SuccessCount,
            };
        }
    }
}
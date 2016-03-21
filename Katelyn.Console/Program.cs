using System;
using System.Net;
using CLAP;
using Katelyn.Core;
using System.Net.Http;

namespace Katelyn.ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Run<KatelynRunner>(args);
        }
    }

    public class KatelynRunner
    {
        [Verb]
        public static void Crawl(string address)
        {
            var uri = new Uri(address);

            Crawler.Crawl(uri, new ConsoleListener());
        }
    }

    public class ConsoleListener
        : IListener
    {
        private int _successCount = 0;
        private int _errorCount = 0;

        public void OnEnd()
        {
            Console.WriteLine($"Finished. {_successCount}/{_successCount + _errorCount} succeeded.");
        }

        public void OnError(string address, Exception exception)
        {
            _errorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            Console.WriteLine($"Exception from {address} {exception.Message}");
        }

        public void OnSuccess(string address)
        {
            _successCount++;
        }
    }
}

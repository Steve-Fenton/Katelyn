using System;

namespace Katelyn.Core.Listeners
{
    public class ConsoleListener
        : IListener
    {
        private ConsoleColor _good = ConsoleColor.Green;
        private ConsoleColor _bad = ConsoleColor.Red;
        private int _errorCount = 0;
        private int _successCount = 0;

        public void OnEnd()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Finished. {_successCount}/{_successCount + _errorCount} succeeded.");
        }

        public void OnError(string address, Exception exception)
        {
            _errorCount++;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            Console.ForegroundColor = _bad;
            Console.WriteLine($"Exception from {address} {exception.Message}");
        }

        public void OnSuccess(string address)
        {
            _successCount++;

            Console.ForegroundColor = _good;
            Console.WriteLine($"OK {address}");
        }
    }
}
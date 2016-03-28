using CLAP;
using System;

namespace Katelyn.ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var exitCode = Parser.Run<KatelynRunner>(args);

            Environment.Exit(exitCode);
        }
    }
}
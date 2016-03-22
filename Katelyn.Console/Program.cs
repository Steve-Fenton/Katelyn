using CLAP;

namespace Katelyn.ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Run<KatelynRunner>(args);
        }
    }
}
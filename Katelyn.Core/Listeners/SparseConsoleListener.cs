namespace Katelyn.Core.Listeners
{
    public class SparseConsoleListener
        : ConsoleListener
    {
        public override void OnSuccess(CrawlRequest request)
        {
            SuccessCount++;
        }
    }
}
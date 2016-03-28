namespace Katelyn.Core
{
    public class CrawlResult
    {
        public int Total
        {
            get
            {
                return ErrorCount + SuccessCount;
            }
        }

        public int ErrorCount { get; set; }

        public int SuccessCount { get; set; }
    }
}
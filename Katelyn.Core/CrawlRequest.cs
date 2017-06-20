namespace Katelyn.Core
{
    public class CrawlRequest
    {
        public string Address { get; internal set; }

        public string ParentAddress { get; internal set; }

        public string ContentType { get; internal set; }

        public string Document { get; internal set; }

        public long Duration { get; internal set; }

        public int StatusCode { get; internal set; }
    }
}

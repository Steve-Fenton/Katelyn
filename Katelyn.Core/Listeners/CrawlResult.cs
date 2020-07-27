using System.ComponentModel;

namespace Katelyn.Core
{
    public class CrawlResult 
    {
        public string Address { get; internal set; }

        public string ParentAddress { get; internal set; }

        public string ContentType { get; internal set; }

        [Browsable(false)]
        public string Document { get; internal set; }

        public long Duration { get; internal set; }

        public long ContentLengthKB { get; internal set; }

        public int StatusCode { get; internal set; }

        public string Error { get; internal set; }
    }
}
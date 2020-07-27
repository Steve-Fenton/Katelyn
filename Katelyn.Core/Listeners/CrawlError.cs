namespace Katelyn.Core
{
    public class CrawlError 
        : CrawlResult
    {
        public CrawlError(CrawlResult request, string error)
        {
            Address = request.Address;
            ContentType = request.ContentType;
            Document = request.Document;
            Duration = request.Duration;
            ParentAddress = request.ParentAddress;
            StatusCode = request.StatusCode;
            Error = error;
        }
    }
}
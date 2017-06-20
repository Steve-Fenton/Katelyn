using System;

namespace Katelyn.Core
{
    public interface IListener
    {
        void OnStart();

        void OnEnd();

        void OnError(CrawlRequest request, Exception exception);

        void OnSuccess(CrawlRequest request);

        void OnDocumentLoaded(CrawlRequest request);

        CrawlResult GetCrawlResult();
    }
}
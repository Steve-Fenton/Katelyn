using System;

namespace Katelyn.Core
{
    public interface IListener
    {
        void OnEnd();

        void OnError(string address, Exception exception);

        void OnSuccess(string address);

        CrawlResult GetCrawlResult();
    }
}
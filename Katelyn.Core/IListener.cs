using System;

namespace Katelyn.Core
{
    public interface IListener
    {
        void OnStart();

        void OnEnd();

        void OnError(string address, string parent, Exception exception);

        void OnSuccess(string address, string parent);

        CrawlResult GetCrawlResult();
    }
}
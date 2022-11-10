using System;

namespace Katelyn.Core;

public interface IListener
{
    void OnStart();

    void OnEnd();

    void OnError(CrawlResult request, Exception exception);

    void OnSuccess(CrawlResult request);

    void OnDocumentLoaded(CrawlResult request);

    void OnThirdPartyAddress(CrawlResult request);

    CrawlSummary GetCrawlResult();
}
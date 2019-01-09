using Katelyn.Core.LinkParsers;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Katelyn.Core
{
    public class Crawler
    {
        private CrawlerConfig _config;
        private IListener _event;
        private bool _continue = true;
        private readonly RequestQueue _requestQueue = new RequestQueue();

        public bool IsRunning { get; set; }

        public static Crawler Crawl(CrawlerConfig config)
        {
            var crawler = new Crawler(config);
            crawler.Start();
            return crawler;
        }

        private Crawler(CrawlerConfig config)
        {
            _config = config;
            _event = config.Listener;
        }

        public void Start()
        {
            try
            {
                IsRunning = true;
                _event.OnStart();
                CrawlAddresses();
            }
            catch (Exception ex)
            {
                _event.OnError(new CrawlResult { Address = "Crawl Error", ParentAddress = "Start" }, ex);
            }
            finally
            {
                IsRunning = false;
                _event.OnEnd();
            }
        }

        public void Stop()
        {
            _continue = false;
            _event.OnEnd();
        }

        private void CrawlAddresses()
        {
            foreach (var address in AddressProvider.GetAddressProvider(_config))
            {
                _config.RootAddress = address;
                _requestQueue.AddRootAddress(address);
                CrawlAddress(new QueueItem { Address = address, ParentAddress = null });
            }
        }

        private void CrawlAddress(QueueItem queueItem, int currentDepth = 0)
        {
            if (!_continue)
            {
                // The kill switch has been flipped, stop processing
                return;
            }

            if (_config.CrawlDelay.TotalMilliseconds > 0)
            {
                // Throttling is enabled, so delay making the next request
                Thread.Sleep((int)_config.CrawlDelay.TotalMilliseconds);
            }

            // If we get this far, we can actually make the request
            MakeRequest(queueItem, currentDepth);
        }

        private void MakeRequest(QueueItem queueItem, int currentDepth)
        {
            var crawl = new CrawlResult
            {
                Address = queueItem.Address.AbsoluteUri,
                ParentAddress = queueItem.ParentAddress?.AbsoluteUri ?? string.Empty
            };

            try
            {
                AddRobotListsToQueue(currentDepth);
                AddLinksToQueueFor(queueItem);
            }
            catch (Exception ex)
            {
                _event.OnError(crawl, ex);
            }

            ProcessChildRequests(currentDepth);
        }

        private void ProcessChildRequests(int currentDepth)
        {
            var nextDepth = currentDepth + 1;

            if (nextDepth >= _config.MaxDepth)
            {
                // We have followed the hierarchy deep enough, go no deeper
                return;
            }

            QueueItem queueItem = _requestQueue.Pop();
            while (queueItem != default(QueueItem))
            {
                try
                {
                    CrawlAddress(queueItem, nextDepth);
                }
                finally
                {
                    queueItem = _requestQueue.Pop();
                }
            }
        }

        private void AddRobotListsToQueue(int currentDepth)
        {
            if (currentDepth == 0)
            {
                if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeRobots))
                {
                    var robotAddress = new Uri($"{_config.RootAddress.GetLeftPart(UriPartial.Authority)}/robots.txt");
                    _requestQueue.Add(robotAddress, _config.RootAddress);
                }
            }
        }

        private void AddLinksToQueueFor(QueueItem queueItem)
        {
            var request = new CrawlResult
            {
                Address = queueItem.Address.AbsoluteUri,
                ParentAddress = queueItem.ParentAddress?.AbsoluteUri ?? string.Empty
            };

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };

            using (var httpClient = new HttpClient(handler))
            {
                var timer = CrawlTimer.Start();

                var response = httpClient.GetAsync(request.Address).Result;

                request.ContentType = response.Content.Headers.ContentType?.MediaType ?? "Unknown";
                request.StatusCode = (int)response.StatusCode;

                if (request.StatusCode >= 400)
                {
                    request.Duration = timer.Stop();
                    _event.OnError(request, new Exception($"{response.StatusCode} ${response.ReasonPhrase}"));
                    return;
                }

                var content = GetContent(response);
                request.Duration = timer.Stop();

                ContentParser<Uri> contentParser = ParserFactory.GetLinkParser(_config, queueItem.ParentAddress, content, request.ContentType);
                request.Document = contentParser.Content;
                _event.OnDocumentLoaded(request);
                _requestQueue.AddRange(contentParser, queueItem.Address);

                var isError = false;
                ContentParser<string> searchParser = ParserFactory.GetSearchParser(_config, queueItem.ParentAddress, content, request.ContentType);

                foreach (string message in searchParser)
                {
                    isError = true;
                    _event.OnError(request, new Exception(message));
                }

                if (isError)
                {
                    return;
                }
            }

            _event.OnSuccess(request);
        }

        private static string GetContent(HttpResponseMessage response)
        {
            string content = string.Empty;

            try
            {
                content = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                var responseBytes = response.Content.ReadAsByteArrayAsync().Result;
                content = Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length - 1);
            }

            return content;
        }
    }
}
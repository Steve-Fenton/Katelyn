using HtmlAgilityPack;
using Katelyn.Core.LinkParsers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Katelyn.Core
{
    public class Crawler
    {
        private CrawlerConfig _config;
        private IListener _event;
        private bool _continue = true;
        private readonly IDictionary<string, int> _crawled = new Dictionary<string, int>();

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
                CrawlAddress(_config.RootAddress);
            }
        }

        private void CrawlAddress(Uri address, int currentDepth = 0, Uri parent = null)
        {
            if (!_continue)
            {
                // The kill switch has been flipped, stop processing
                return;
            }

            if (_crawled.ContainsKey(address.AbsoluteUri))
            {
                // Already crawled - don't make another request
                _crawled[address.AbsoluteUri]++;
                return;
            }

            if (_config.CrawlDelay.TotalMilliseconds > 0)
            {
                // Throttling is enabled, so delay making the next request
                Thread.Sleep((int)_config.CrawlDelay.TotalMilliseconds);
            }

            // If we get this far, we can actually make the request
            MakeRequest(address, currentDepth, parent);
        }

        private void MakeRequest(Uri address, int currentDepth, Uri parent)
        {
            IDictionary<string, Uri> queue = new Dictionary<string, Uri>();

            var crawl = new CrawlResult
            {
                Address = address.AbsoluteUri,
                ParentAddress = parent?.AbsoluteUri ?? string.Empty
            };

            try
            {
                AddLinksToQueueFor(queue, crawl.Address, parent);
                AddRobotListsToQueue(queue, currentDepth);

                _crawled.Add(crawl.Address, 1);
            }
            catch (Exception ex)
            {
                _event.OnError(crawl, ex);
            }

            ProcessChildRequests(queue, address, currentDepth);
        }

        private void ProcessChildRequests(IDictionary<string, Uri> queue, Uri parentAddress, int currentDepth)
        {
            var nextDepth = currentDepth + 1;

            if (nextDepth >= _config.MaxDepth)
            {
                // We have followed the hierarchy far enough, go no deeper
                return;
            }

            foreach (var key in queue.Keys)
            {
                CrawlAddress(queue[key], nextDepth, parentAddress);
            }
        }

        private void AddRobotListsToQueue(IDictionary<string, Uri> queue, int currentDepth)
        {
            if (currentDepth == 0)
            {
                if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeRobots))
                {
                    var robotAddress = new Uri($"{_config.RootAddress.GetLeftPart(UriPartial.Authority)}/robots.txt");
                    QueueIfNew(queue, robotAddress);
                }
            }
        }

        private bool IsOffSiteResource(string linkText, Uri parent)
        {
            bool isOffSiteResource = LinkParser.IsOffSiteResource(linkText, _config.RootAddress, parent);

            if (isOffSiteResource)
            {
                _event.OnThirdPartyAddress(new CrawlResult { ParentAddress = parent?.AbsoluteUri ?? string.Empty, Address = linkText });
            }

            return isOffSiteResource;
        }

        private void AddLinksToQueueFor(IDictionary<string, Uri> queue, string address, Uri parent)
        {
            var request = new CrawlResult
            {
                Address = address,
                ParentAddress = parent?.AbsoluteUri ?? string.Empty
            };

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };

            using (var client = new HttpClient(handler))
            {
                var timer = CrawlTimer.Start();

                var response = client.GetAsync(request.Address).Result;

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

                switch (request.ContentType)
                {
                    case "text/html":
                        request.Document = content;
                        _event.OnDocumentLoaded(request);

                        var parser1 = new HtmlLinkParser(_config.RootAddress, parent, content, _config);

                        foreach (Uri resource in parser1)
                        {
                            QueueIfNew(queue, resource);
                        }
                        //

                        var htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(content);

                        var isError = false;

                        if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeFailureCheck))
                        {
                            var regex = new Regex(@"KATELYN:ERRORS\([0-9]+\)", RegexOptions.IgnoreCase);

                            foreach (Match match in regex.Matches(content))
                            {
                                _event.OnError(request, new Exception($"At {match.Index} - {match.Value}"));
                                isError = true;
                            }
                        }

                        if (_config.HtmlContentExpression != null)
                        {
                            foreach (Match match in _config.HtmlContentExpression.Matches(htmlDocument.DocumentNode.OuterHtml))
                            {
                                _event.OnError(request, new Exception($"At {match.Index} - {match.Value}"));
                                isError = true;
                            }
                        }

                        if (isError)
                        {
                            return;
                        }
                        break;
                    case "text/plain":
                        // robots.txt
                        request.Document = content;
                        _event.OnDocumentLoaded(request);

                        var parser = new RobotLinkParser(_config.RootAddress, parent, content, _config);

                        foreach (Uri resource in parser)
                        {
                            QueueIfNew(queue, resource);
                        }
                        break;
                    case "text/xml":
                        // sitemap.xml
                        request.Document = content;
                        _event.OnDocumentLoaded(request);

                        var parser2 = new SitemapLinkParser(_config.RootAddress, parent, content, _config);

                        foreach (Uri resource in parser2)
                        {
                            QueueIfNew(queue, resource);
                        }
                        break;
                    default:
                        // Unsupported content type - we still load and measure, but don't look for links
                        _event.OnDocumentLoaded(request);
                        break;
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

        private bool IsAbsoluteUri(string linkText)
        {
            return linkText.StartsWith(_config.RootAddress.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase);
        }

        private void QueueIfNew(IDictionary<string, Uri> queue, Uri uri)
        {
            // Only queue it up if it hasn't been queued or crawled
            if (!_crawled.ContainsKey(uri.AbsoluteUri)
                && !queue.ContainsKey(uri.AbsoluteUri))
            {
                queue.Add(uri.AbsoluteUri, uri);
            }
        }
    }
}
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Katelyn.Core
{
    public class Crawler
    {
        private CrawlerConfig _config;
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
        }

        public void Start()
        {
            IsRunning = true;

            try
            {
                _config.Listener.OnStart();

                if (!string.IsNullOrWhiteSpace(_config.FilePath))
                {
                    // Run the crawler for all websites found in the file
                    foreach (var line in File.ReadLines(_config.FilePath))
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        _config.RootAddress = new Uri(line);
                        StartRoot();
                    }
                }
                else
                {
                    // Run the crawler for a single address
                    StartRoot();
                }
            }
            catch (Exception ex)
            {
                _config.Listener.OnError(new CrawlResult { Address = "Crawl Error", ParentAddress = "Start" }, ex);
            }

            _config.Listener.OnEnd();

            IsRunning = false;
        }

        private void StartRoot()
        {
            CrawlAddress(_config.RootAddress, 0);
        }

        public void Stop()
        {
            _continue = false;

            _config.Listener.OnEnd();
        }

        private void CrawlAddress(Uri address, int currentDepth, Uri parent = null)
        {
            var request = new CrawlResult
            {
                Address = address.AbsoluteUri,
                ParentAddress = parent?.AbsoluteUri ?? string.Empty
            };

            if (!_continue)
            {
                return;
            }

            if (_config.CrawlDelay.TotalMilliseconds > 0)
            {
                Thread.Sleep((int)_config.CrawlDelay.TotalMilliseconds);
            }

            IDictionary<string, Uri> queue = new Dictionary<string, Uri>();

            if (_crawled.ContainsKey(request.Address))
            {
                _crawled[request.Address]++;
            }
            else
            {
                try
                {
                    queue = AddLinksToQueueFor(request.Address, parent);

                    if (currentDepth == 0)
                    {
                        if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeRobots))
                        {
                            var robotAddress = new Uri($"{_config.RootAddress.GetLeftPart(UriPartial.Authority)}/robots.txt");
                            QueueIfNew(queue, robotAddress);
                        }
                    }

                    _crawled.Add(request.Address, 1);
                }
                catch (Exception ex)
                {
                    _config.Listener.OnError(request, ex);
                }
            }

            var nextDepth = currentDepth + 1;

            if (nextDepth >= _config.MaxDepth)
            {
                return;
            }

            foreach (var key in queue.Keys)
            {
                CrawlAddress(queue[key], nextDepth, address);
            }
        }

        private bool IsOffSiteResource(string linkText, Uri parent)
        {
            bool isOffSiteResource = linkText.StartsWith("tel:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("fax:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("javascript:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("//", StringComparison.InvariantCultureIgnoreCase)
                || (linkText.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !linkText.StartsWith("http://" + _config.RootAddress.Host, StringComparison.InvariantCultureIgnoreCase))
                || (linkText.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase) && !linkText.StartsWith("https://" + _config.RootAddress.Host, StringComparison.InvariantCultureIgnoreCase));

            if (isOffSiteResource)
            {
                _config.Listener.OnThirdPartyAddress(new CrawlResult { ParentAddress = parent?.AbsoluteUri ?? string.Empty, Address = linkText });
            }

            return isOffSiteResource;
        }

        private IDictionary<string, Uri> AddLinksToQueueFor(string address, Uri parent)
        {
            var request = new CrawlResult
            {
                Address = address,
                ParentAddress = parent?.AbsoluteUri ?? string.Empty
            };

            IDictionary<string, Uri> queue = new Dictionary<string, Uri>();

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
                    _config.Listener.OnError(request, new Exception($"{response.StatusCode} ${response.ReasonPhrase}"));
                    return queue;
                }

                var content = GetContent(response);
                request.Duration = timer.Stop();

                switch (request.ContentType) {
                    case "text/html":
                        var htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(content);

                        request.Document = htmlDocument.DocumentNode.OuterHtml;

                        _config.Listener.OnDocumentLoaded(request);

                        if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeLinks))
                        {
                            queue = QueueHyperlinks(queue, htmlDocument, parent);
                        }

                        if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeScripts))
                        {
                            queue = QueueScripts(queue, htmlDocument, parent);
                        }

                        if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeStyles))
                        {
                            queue = QueueStyles(queue, htmlDocument, parent);
                        }

                        if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeImages))
                        {
                            queue = QueueImages(queue, htmlDocument, parent);
                        }

                        var isError = false;

                        if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeFailureCheck))
                        {
                            var regex = new Regex(@"KATELYN:ERRORS\([0-9]+\)", RegexOptions.IgnoreCase);

                            foreach (Match match in regex.Matches(htmlDocument.DocumentNode.OuterHtml))
                            {
                                _config.Listener.OnError(request, new Exception($"At {match.Index} - {match.Value}"));
                                isError = true;
                            }
                        }

                        if (_config.HtmlContentExpression != null)
                        {
                            foreach (Match match in _config.HtmlContentExpression.Matches(htmlDocument.DocumentNode.OuterHtml))
                            {
                                _config.Listener.OnError(request, new Exception($"At {match.Index} - {match.Value}"));
                                isError = true;
                            }
                        }

                        if (isError)
                        {
                            return queue;
                        }
                        break;
                    case "text/plain":
                        // robots.txt
                        _config.Listener.OnDocumentLoaded(request);

                        foreach (Match item in Regex.Matches(content, @"(http|https):\/\/(.*)\S"))
                        {
                            QueueIfNew(queue, new Uri(item.Value));
                        }
                        break;
                    case "text/xml":
                        // sitemap.xml
                        var sitemap = new HtmlDocument();
                        sitemap.LoadHtml(content);

                        request.Document = sitemap.DocumentNode.OuterHtml;

                        queue = QueueSitemaplinks(queue, sitemap, parent);

                        _config.Listener.OnDocumentLoaded(request);
                        break;
                    default:
                        // Unsupported content type - we still load and measure, but don't look for links
                        _config.Listener.OnDocumentLoaded(request);
                        break;
                }
            }

            _config.Listener.OnSuccess(request);

            return queue;
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

        private IDictionary<string, Uri> QueueHyperlinks(IDictionary<string, Uri> queue, HtmlDocument htmlDocument, Uri parent)
        {
            var linkNodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes == null || linkNodes.Count == 0)
            {
                // No links on this page
                return queue;
            }

            foreach (HtmlNode link in linkNodes)
            {
                var linkText = link.Attributes["href"].Value;

                if (linkText.Contains("#"))
                {
                    linkText = linkText.Substring(0, linkText.IndexOf('#'));
                }

                if (IsOffSiteResource(linkText, parent))
                {
                    continue;
                }

                var uri = (IsAbsoluteUri(linkText))
                    ? new Uri(linkText)
                    : new Uri(_config.RootAddress, linkText);

                QueueIfNew(queue, uri);
            }

            return queue;
        }

        private IDictionary<string, Uri> QueueSitemaplinks(IDictionary<string, Uri> queue, HtmlDocument htmlDocument, Uri parent)
        {
            var linkNodes = htmlDocument.DocumentNode.SelectNodes("//loc");

            if (linkNodes == null || linkNodes.Count == 0)
            {
                // No links on this page
                return queue;
            }

            foreach (HtmlNode link in linkNodes)
            {
                var linkText = link.InnerText;

                if (linkText.Contains("#"))
                {
                    linkText = linkText.Substring(0, linkText.IndexOf('#'));
                }

                if (IsOffSiteResource(linkText, parent))
                {
                    continue;
                }

                var uri = (IsAbsoluteUri(linkText))
                    ? new Uri(linkText)
                    : new Uri(_config.RootAddress, linkText);

                QueueIfNew(queue, uri);
            }

            return queue;
        }

        private IDictionary<string, Uri> QueueScripts(IDictionary<string, Uri> queue, HtmlDocument htmlDocument, Uri parent)
        {
            var scriptNodes = htmlDocument.DocumentNode.SelectNodes("//script[@src]");

            if (scriptNodes == null || scriptNodes.Count == 0)
            {
                // No external scripts on this page
                return queue;
            }

            foreach (HtmlNode link in scriptNodes)
            {
                var linkText = link.Attributes["src"].Value;

                if (IsOffSiteResource(linkText, parent))
                {
                    continue;
                }

                var uri = (IsAbsoluteUri(linkText))
                    ? new Uri(linkText)
                    : new Uri(_config.RootAddress, linkText);

                QueueIfNew(queue, uri);
            }

            return queue;
        }

        private IDictionary<string, Uri> QueueStyles(IDictionary<string, Uri> queue, HtmlDocument htmlDocument, Uri parent)
        {
            var styleNodes = htmlDocument.DocumentNode.SelectNodes("//link[@href]");

            if (styleNodes == null || styleNodes.Count == 0)
            {
                // No external scripts on this page
                return queue;
            }

            foreach (HtmlNode link in styleNodes)
            {
                var linkText = link.Attributes["href"].Value;

                if (IsOffSiteResource(linkText, parent))
                {
                    continue;
                }

                var uri = (IsAbsoluteUri(linkText))
                    ? new Uri(linkText)
                    : new Uri(_config.RootAddress, linkText);

                QueueIfNew(queue, uri);
            }

            return queue;
        }

        private IDictionary<string, Uri> QueueImages(IDictionary<string, Uri> queue, HtmlDocument htmlDocument, Uri parent)
        {
            var imageNodes = htmlDocument.DocumentNode.SelectNodes("//img[@src]");

            if (imageNodes == null || imageNodes.Count == 0)
            {
                // No external scripts on this page
                return queue;
            }

            foreach (HtmlNode link in imageNodes)
            {
                var linkText = link.Attributes["src"].Value;

                if (IsOffSiteResource(linkText, parent))
                {
                    continue;
                }

                var uri = (IsAbsoluteUri(linkText))
                    ? new Uri(linkText)
                    : new Uri(_config.RootAddress, linkText);

                QueueIfNew(queue, uri);
            }

            return queue;
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
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;

namespace Katelyn.Core
{
    public class Crawler
    {
        private CrawlerConfig _config;
        private readonly IDictionary<string, int> _crawled = new Dictionary<string, int>();

        public static void Crawl(CrawlerConfig config)
        {
            var crawler = new Crawler(config);
            crawler.Start();
        }

        private Crawler(CrawlerConfig config)
        {
            _config = config;
        }

        private void Start()
        {
            _config.Listener.OnStart();

            CrawlAddress(_config.RootAddress, 0);

            _config.Listener.OnEnd();
        }

        private void CrawlAddress(Uri address, int currentDepth, Uri parent = null)
        {
            if (_config.CrawlDelay.TotalMilliseconds > 0)
            {
                Thread.Sleep((int)_config.CrawlDelay.TotalMilliseconds);
            }

            var addressString = address.AbsoluteUri;
            IDictionary<string, Uri> queue = new Dictionary<string, Uri>();

            if (_crawled.ContainsKey(addressString))
            {
                _crawled[addressString]++;
            }
            else
            {
                try
                {
                    queue = AddLinksToQueueFor(addressString, parent);
                    _crawled.Add(addressString, 1);
                }
                catch (Exception ex)
                {
                    _config.Listener.OnError(addressString, parent?.AbsoluteUri, ex);
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

        private bool IsOffSiteResource(string linkText)
        {
            return linkText.StartsWith("tel:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("fax:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("//", StringComparison.InvariantCultureIgnoreCase)
                || (linkText.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !linkText.StartsWith(_config.RootAddress.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase))
                || (linkText.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase) && !linkText.StartsWith(_config.RootAddress.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase));
        }

        private IDictionary<string, Uri> AddLinksToQueueFor(string address, Uri parent)
        {
            IDictionary<string, Uri> queue = new Dictionary<string, Uri>();

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };

            using (var client = new HttpClient(handler))
            {
                var response = client.GetAsync(address).Result;

                var statusCode = (int)response.StatusCode;

                if (statusCode >= 400)
                {
                    _config.Listener.OnError(address, parent?.AbsoluteUri, new Exception($"{response.StatusCode} ${response.ReasonPhrase}"));
                    return queue;
                }

                if (response.Content.Headers.ContentType.MediaType != "text/html")
                {
                    // Not an HTML page
                    _config.Listener.OnSuccess(address, parent?.AbsoluteUri);
                    return queue;
                }

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response.Content.ReadAsStringAsync().Result);

                if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeFailureCheck))
                {
                    var regex = new Regex(@"KATELYN:ERRORS\([0-9]+\)", RegexOptions.IgnoreCase);

                    foreach (Match match in regex.Matches(htmlDocument.DocumentNode.OuterHtml))
                    {
                        _config.Listener.OnError(address, parent?.AbsoluteUri, new Exception($"At {match.Index} - {match.Value}"));
                        return queue;
                    }
                }

                if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeLinks))
                {
                    queue = QueueHyperlinks(queue, htmlDocument);
                }

                if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeScripts))
                {
                    queue = QueueScripts(queue, htmlDocument);
                }

                if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeStyles))
                {
                    queue = QueueStyles(queue, htmlDocument);
                }

                if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeImages))
                {
                    queue = QueueImages(queue, htmlDocument);
                }
            }

            _config.Listener.OnSuccess(address, parent?.AbsoluteUri);

            return queue;
        }

        private IDictionary<string, Uri> QueueHyperlinks(IDictionary<string, Uri> queue, HtmlDocument htmlDocument)
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

                if (IsOffSiteResource(linkText))
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

        private IDictionary<string, Uri> QueueScripts(IDictionary<string, Uri> queue, HtmlDocument htmlDocument)
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

                if (IsOffSiteResource(linkText))
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

        private IDictionary<string, Uri> QueueStyles(IDictionary<string, Uri> queue, HtmlDocument htmlDocument)
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

                if (IsOffSiteResource(linkText))
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

        private IDictionary<string, Uri> QueueImages(IDictionary<string, Uri> queue, HtmlDocument htmlDocument)
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

                if (IsOffSiteResource(linkText))
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
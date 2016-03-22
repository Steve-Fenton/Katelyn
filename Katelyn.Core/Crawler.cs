using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Katelyn.Core
{
    public class Crawler
    {
        public static void Crawl(CrawlerConfig config)
        {
            var crawler = new Crawler(config);
            crawler.Start();
        }

        private CrawlerConfig _config;
        private IDictionary<string, int> _crawled = new Dictionary<string, int>();

        private Crawler(CrawlerConfig config)
        {
            _config = config;
        }

        private static bool IsOffSiteResource(string linkText)
        {
            return linkText.StartsWith("tel:")
                || linkText.StartsWith("fax:")
                || linkText.StartsWith("mailto:")
                || linkText.StartsWith("http://")
                || linkText.StartsWith("https://");
        }

        private IDictionary<string, Uri> AddLinksToQueueFor(string key)
        {
            var queue = new Dictionary<string, Uri>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(key).Result;

                if (!response.IsSuccessStatusCode)
                {
                    _config.Listener.OnError(key, new Exception($"{response.StatusCode} ${response.ReasonPhrase}"));
                    return queue;
                }

                if (response.Content.Headers.ContentType.MediaType != "text/html")
                {
                    // Not an HTML page
                    _config.Listener.OnSuccess(key);
                    return queue;
                }

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response.Content.ReadAsStringAsync().Result);

                var linkNodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");

                if (linkNodes == null || linkNodes.Count == 0)
                {
                    // No links on this page
                    _config.Listener.OnSuccess(key);
                    return queue;
                }

                foreach (HtmlNode link in linkNodes)
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
            }

            _config.Listener.OnSuccess(key);
            return queue;
        }

        private void CrawlAddress(Uri address, int currentDepth)
        {
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
                    queue = AddLinksToQueueFor(addressString);
                    _crawled.Add(addressString, 1);
                }
                catch (Exception ex)
                {
                    _config.Listener.OnError(addressString, ex);
                }
            }

            var nextDepth = currentDepth + 1;

            if (nextDepth >= _config.MaxDepth)
            {
                return;
            }

            foreach (var key in queue.Keys)
            {
                CrawlAddress(queue[key], nextDepth);
            }
        }

        private bool IsAbsoluteUri(string linkText)
        {
            return linkText.StartsWith(_config.RootAddress.AbsoluteUri);
        }

        private void QueueIfNew(IDictionary<string, Uri> queue, Uri uri)
        {
            // Only queue it up if it hasn't been crawled,
            // and isn't already queued
            if (!_crawled.ContainsKey(uri.AbsoluteUri)
                && !queue.ContainsKey(uri.AbsoluteUri))
            {
                queue.Add(uri.AbsoluteUri, uri);
            }
        }

        private void Start()
        {
            CrawlAddress(_config.RootAddress, 0);

            _config.Listener.OnEnd();
        }
    }

    public class CrawlerConfig
    {
        public IListener Listener { get; set; }
        public int MaxDepth { get; set; } = 5;
        public Uri RootAddress { get; set; }
    }
}
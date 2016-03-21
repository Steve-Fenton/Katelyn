using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Katelyn.Core
{
    public class Crawler
    {
        public static void Crawl(Uri rootAddress, IListener listener)
        {
            var crawler = new Crawler(rootAddress, listener);
            crawler.Start();
        }

        private Uri _rootAddress;
        private IListener _listener;
        private IDictionary<string, int> _crawled = new Dictionary<string, int>();

        private int maxDepth = 5;

        private Crawler(Uri rootAddress, IListener listener)
        {
            _rootAddress = rootAddress;
            _listener = listener;
        }

        private void Start()
        {
            CrawlAddress(_rootAddress, 0);

            _listener.OnEnd();
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
                    _listener.OnError(addressString, ex);
                }
            }

            var nextDepth = currentDepth + 1;

            if (nextDepth > maxDepth)
            {
                return;
            }

            foreach (var key in queue.Keys)
            {
                CrawlAddress(queue[key], nextDepth);
            }
        }

        private IDictionary<string, Uri> AddLinksToQueueFor(string key)
        {
            var queue = new Dictionary<string, Uri>();


            using (var client = new HttpClient())
            {
                var response = client.GetAsync(key).Result;

                if (!response.IsSuccessStatusCode)
                {
                    _listener.OnError(key, new Exception($"{response.StatusCode} ${response.ReasonPhrase}"));
                    return queue;
                }

                var mediaType = response.Content.Headers.ContentType.MediaType;

                if (mediaType != "text/html")
                {
                    _listener.OnSuccess(key);
                    return queue;
                }

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response.Content.ReadAsStringAsync().Result);

                var linkNodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");

                if (linkNodes == null || linkNodes.Count == 0)
                {
                    _listener.OnSuccess(key);
                    return queue;
                }

                foreach (HtmlNode link in linkNodes)
                {
                    var linkText = link.Attributes["href"].Value;
                    if (linkText.StartsWith(_rootAddress.AbsoluteUri))
                    {
                        // Absolute on-site URL
                        var uri = new Uri(linkText);
                        QueueIfNew(queue, uri);
                    }
                    else if (
                        linkText.StartsWith("tel:")
                        || linkText.StartsWith("fax:")
                        || linkText.StartsWith("mailto:")
                        || linkText.StartsWith("http://")
                        || linkText.StartsWith("https://"))
                    {
                        // Not an on-site URL
                    }
                    else
                    {
                        // Relative URL
                        var uri = new Uri(_rootAddress, linkText);
                        QueueIfNew(queue, uri);
                    }
                }

            }

            _listener.OnSuccess(key);
            return queue;
        }

        private void QueueIfNew(IDictionary<string, Uri> queue, Uri uri)
        {
            // Only queue it up if it hasn't been crawled, and isn't already queued
            if (!_crawled.ContainsKey(uri.AbsoluteUri)
                && !queue.ContainsKey(uri.AbsoluteUri))
            {
                queue.Add(uri.AbsoluteUri, uri);
            }
        }
    }
}

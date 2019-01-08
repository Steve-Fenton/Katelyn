﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Katelyn.Core.LinkParsers
{
    public class SitemapLinkParser
        : LinkParser
    {
        private readonly Uri _root;
        private readonly Uri _parent;
        private readonly string _content;
        private readonly CrawlerConfig _config;

        public SitemapLinkParser(Uri root, Uri parent, string content, CrawlerConfig config)
        {
            _root = root;
            _parent = parent;
            _content = content;
            _config = config;
        }

        public override IEnumerator<Uri> GetEnumerator()
        {
            var sitemap = new HtmlDocument();
            sitemap.LoadHtml(_content);
            var linkNodes = sitemap.DocumentNode.SelectNodes("//loc");

            if (linkNodes == null || linkNodes.Count == 0)
            {
                // No links on this page
                yield break;
            }

            foreach (HtmlNode link in linkNodes)
            {
                var linkText = link.InnerText;

                if (linkText.Contains("#"))
                {
                    linkText = linkText.Substring(0, linkText.IndexOf('#'));
                }

                if (IsOffSiteResource(linkText, _root, _parent))
                {
                    _config.Listener.OnThirdPartyAddress(new CrawlResult { ParentAddress = _parent?.AbsoluteUri ?? string.Empty, Address = linkText });
                    continue;
                }

                var uri = (IsAbsoluteUri(_root, linkText))
                    ? new Uri(linkText)
                    : new Uri(_root, linkText);

                yield return uri;
            }
        }


    }
}
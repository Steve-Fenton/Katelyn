using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Katelyn.Core.LinkParsers
{
    public class HtmlLinkParser
        : ContentParser<Uri>
    {
        private readonly Uri _root;
        private readonly Uri _parent;
        private readonly string _content;
        private readonly CrawlerConfig _config;

        public override string Content => _content;

        public HtmlLinkParser(Uri root, Uri parent, string content, CrawlerConfig config)
        {
            _root = root;
            _parent = parent;
            _content = content;
            _config = config;
        }

        private IEnumerable<string> GetNodeAttributes(HtmlDocument document, string nodeSelector, string attributeSelector)
        {
            var nodes = document.DocumentNode.SelectNodes(nodeSelector);

            if (nodes == null || nodes.Count == 0)
            {
                return new List<string>();
            }

            return nodes.Select(n =>
            {
                var linkText = n.Attributes[attributeSelector].Value;

                if (linkText.Contains("#"))
                {
                    linkText = linkText.Substring(0, linkText.IndexOf('#'));
                }

                return linkText;
            });
        }

        public override IEnumerator<Uri> GetEnumerator()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_content);

            // Add range for each flag... then loop once we have them all
            List<string> links = new List<string>();


            if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeLinks))
            {
                links.AddRange(GetNodeAttributes(htmlDocument, "//a[@href]", "href"));
            }

            if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeScripts))
            {
                links.AddRange(GetNodeAttributes(htmlDocument, "//script[@src]", "src"));
            }

            if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeStyles))
            {
                links.AddRange(GetNodeAttributes(htmlDocument, "//link[@href]", "href"));
            }

            if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeImages))
            {
                links.AddRange(GetNodeAttributes(htmlDocument, "//img[@src]", "src"));
            }

            foreach (var linkText in links)
            {
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
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Katelyn.Core.LinkParsers
{
    public class RobotLinkParser
        : ContentParser<Uri>
    {
        private readonly Uri _root;
        private readonly Uri _parent;
        private readonly string _content;
        private readonly CrawlerConfig _config;

        public override string Content => _content;

        public RobotLinkParser(Uri root, Uri parent, string content, CrawlerConfig config)
        {
            _root = root;
            _parent = parent;
            _content = content;
            _config = config;
        }

        public override IEnumerator<Uri> GetEnumerator()
        {
            foreach (Match item in Regex.Matches(_content, @"(http|https):\/\/(.*)\S"))
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                {
                    continue;
                }

                if (IsOffSiteResource(item.Value, _root, _parent, _config.PartnerSites))
                {
                    _config.Listener.OnThirdPartyAddress(new CrawlResult { ParentAddress = _parent?.AbsoluteUri ?? string.Empty, Address = item.Value });
                    continue;
                }
                else
                {
                    yield return new Uri(item.Value);
                }
            }
        }
    }
}
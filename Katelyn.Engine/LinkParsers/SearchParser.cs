using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Katelyn.Core.LinkParsers
{
    public class SearchParser
        : ContentParser<string>
    {
        private readonly Uri _root;
        private readonly Uri _parent;
        private readonly string _content;
        private readonly CrawlerConfig _config;

        public override string Content => _content;

        public SearchParser(Uri root, Uri parent, string content, CrawlerConfig config)
        {
            _root = root;
            _parent = parent;
            _content = content;
            _config = config;
        }

        public override IEnumerator<string> GetEnumerator()
        {
            if (_config.CrawlerFlags.HasFlag(CrawlerFlags.IncludeFailureCheck))
            {
                var regex = new Regex(@"KATELYN:ERRORS\([0-9]+\)", RegexOptions.IgnoreCase);

                foreach (Match match in regex.Matches(_content))
                {
                    yield return $"At {match.Index} - {match.Value}";
                }
            }

            if (_config.HtmlContentExpression != null)
            {
                foreach (Match match in _config.HtmlContentExpression.Matches(_content))
                {
                    yield return $"At {match.Index} - {match.Value}";
                }
            }


        }
    }
}
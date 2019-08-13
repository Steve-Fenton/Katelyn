using System;

namespace Katelyn.Core.LinkParsers
{
    public static class ParserFactory
    {
        public static ContentParser<Uri> GetLinkParser(CrawlerConfig config, Uri parent, string content, string contentType)
        {
            switch (contentType)
            {
                case "text/html":
                    return new HtmlLinkParser(config.RootAddress, parent, content, config);
                case "text/plain":
                    // robots.txt
                    return new RobotLinkParser(config.RootAddress, parent, content, config);
                case "text/xml":
                    // sitemap.xml
                    return new SitemapLinkParser(config.RootAddress, parent, content, config);
                default:
                    // Unsupported content type - we still load and measure, but don't look for links
                    return new EmptyLinkParser<Uri>();
            }
        }

        public static ContentParser<string> GetSearchParser(CrawlerConfig config, Uri parent, string content, string contentType)
        {
            switch (contentType)
            {
                case "text/html":
                    return new SearchParser(config.RootAddress, parent, content, config);
                case "text/plain":
                    // robots.txt
                    return new EmptyLinkParser<string>();
                case "text/xml":
                    // sitemap.xml
                    return new EmptyLinkParser<string>();
                default:
                    // Unsupported content type - we still load and measure, but don't look for links
                    return new EmptyLinkParser<string>();
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace Katelyn.Core.LinkParsers
{
    public abstract class LinkParser
    {
        public static bool IsOffSiteResource(string linkText, Uri rootAddress, Uri parent)
        {
            bool isOffSiteResource = linkText.StartsWith("tel:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("fax:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("javascript:", StringComparison.InvariantCultureIgnoreCase)
                || linkText.StartsWith("//", StringComparison.InvariantCultureIgnoreCase)
                || (linkText.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !linkText.StartsWith("http://" + rootAddress.Host, StringComparison.InvariantCultureIgnoreCase))
                || (linkText.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase) && !linkText.StartsWith("https://" + rootAddress.Host, StringComparison.InvariantCultureIgnoreCase));

            return isOffSiteResource;
        }

        public static bool IsAbsoluteUri(Uri rootAddress, string linkText)
        {
            return linkText.StartsWith(rootAddress.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase);
        }

        public abstract IEnumerator<Uri> GetEnumerator();
    }
}

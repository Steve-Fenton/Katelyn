namespace Katelyn.Core.LinkParsers;

public abstract class ContentParser<T>
{
    private const StringComparison NC = StringComparison.InvariantCultureIgnoreCase;

    public static bool IsOffSiteResource(string linkText, Uri rootAddress, Uri parent, IEnumerable<Uri> partnerSites)
    {
        bool isOffSiteResource = linkText.StartsWith("tel:", NC)
            || linkText.StartsWith("fax:", NC)
            || linkText.StartsWith("mailto:", NC)
            || linkText.StartsWith("javascript:", NC)
            || (linkText.StartsWith("//", NC) || linkText.StartsWith("http://", NC) || linkText.StartsWith("https://", NC)) && !IsCrawlableDomain(rootAddress, partnerSites, linkText);

        return isOffSiteResource;
    }

    public static bool IsAbsoluteUri(Uri rootAddress, string linkText)
    {
        return linkText.StartsWith(rootAddress.AbsoluteUri, NC);
    }

    private static bool IsCrawlableDomain(Uri rootAddress, IEnumerable<Uri> partnerSites, string linkText)
    {
        return linkText.StartsWith("//" + rootAddress.Host, NC)
            || linkText.StartsWith("http://" + rootAddress.Host, NC)
            || linkText.StartsWith("https://" + rootAddress.Host, NC)
            || IsPartnerSite(partnerSites, linkText);
    }

    private static bool IsPartnerSite(IEnumerable<Uri> partnerSites, string linkText)
    {
        foreach (var partnerSite in partnerSites)
        {
            if (linkText.StartsWith("//" + partnerSite.Host, NC)
                || linkText.StartsWith("http://" + partnerSite.Host, NC)
                || linkText.StartsWith("https://" + partnerSite.Host, NC))
            {
                return true;
            }
        }

        return false;
    }

    public abstract IEnumerator<T> GetEnumerator();

    public abstract string Content { get; }
}
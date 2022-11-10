namespace Katelyn.Core;

public abstract class AddressProvider
{
    public static AddressProvider GetAddressProvider(CrawlerConfig config)
    {
        if (!string.IsNullOrWhiteSpace(config.FilePath))
        {
            return new FileAddressProvider(config.FilePath);
        }

        return new DefaultAddressProvider(config.RootAddress);
    }

    public abstract IEnumerator<Uri> GetEnumerator();
}
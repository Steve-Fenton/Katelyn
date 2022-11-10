namespace Katelyn.Core;

public class DefaultAddressProvider
    :AddressProvider
{
    private readonly Uri _address;

    public DefaultAddressProvider(Uri address)
    {
        _address = address;
    }

    public override IEnumerator<Uri> GetEnumerator()
    {
        yield return _address;
    }
}
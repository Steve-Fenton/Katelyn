namespace Katelyn.Core.LinkParsers;

public class EmptyLinkParser<T>
    : ContentParser<T>
{
    public override string Content => string.Empty;

    public override IEnumerator<T> GetEnumerator()
    {
        yield break;
    }
}
namespace Katelyn.Core.Listeners
{
    public class SparseConsoleListener
        : ConsoleListener
    {
        public override void OnSuccess(string address, string parent)
        {
            SuccessCount++;
        }
    }
}
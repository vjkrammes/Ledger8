namespace Ledger8.Common;

public sealed class NotFailureException : Exception
{
    public NotFailureException(string? message = null) :
        base(string.IsNullOrWhiteSpace(message) ? "Unable to retrieve a failure payload from a success result" : message)
    {

    }
}

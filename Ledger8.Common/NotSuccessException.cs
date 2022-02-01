namespace Ledger8.Common;

public sealed class NotSuccessException : Exception
{
    public NotSuccessException(string? message = null) :
        base(string.IsNullOrWhiteSpace(message) ? "Unable to retrieve a success payload from a failure result" : message)
    {
    }
}

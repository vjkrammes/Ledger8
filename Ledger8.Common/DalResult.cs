using Ledger8.Common.Enumerations;

namespace Ledger8.Common;

public sealed class DalResult
{
    public DalErrorCode ErrorCode { get; init; }
    public Exception? Exception { get; init; }

    public bool Successful => Exception is null && ErrorCode == DalErrorCode.NoError;

    public DalResult(DalErrorCode errorCode = DalErrorCode.NoError, Exception? exception = null)
    {
        ErrorCode = errorCode;
        Exception = exception;
    }

    public static DalResult Duplicate => new(DalErrorCode.Duplicate);

    public static DalResult NotAuthorized => new(DalErrorCode.NotAuthorized);

    public static DalResult NotFound => new(DalErrorCode.NotFound);

    public static DalResult Success => new();

    public static DalResult FromException(Exception exception) => new(DalErrorCode.Exception, exception);

    public string ErrorMessage => Exception is not null ? Exception.Innermost() : ErrorCode.GetDescriptionFromEnumValue();
}

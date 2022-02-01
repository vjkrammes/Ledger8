namespace Ledger8.Common;

public sealed class ApiResult<TSuccess, TFailure>
{
    private bool _success { get; }
    private TSuccess _successResult { get; }
    private TFailure _failureResult { get; }

    public ApiResult(TSuccess success)
    {
        _success = true;
        _successResult = success;
        _failureResult = default!;
    }

    public ApiResult(TFailure failure)
    {
        _success = false;
        _successResult = default!;
        _failureResult = failure;
    }

    public bool IsSuccessResult => _success;

    public TSuccess SuccessPayload
    {
        get
        {
            if (IsSuccessResult)
            {
                return _successResult;
            }
            throw new NotSuccessException();
        }
    }

    public TFailure FailurePayload
    {
        get
        {
            if (IsSuccessResult)
            {
                throw new NotFailureException();
            }
            return _failureResult;
        }
    }

    public T Transform<T>(Func<TSuccess, T> successMethod, Func<TFailure, T> failureMethod) =>
        IsSuccessResult ? successMethod(SuccessPayload) : failureMethod(FailurePayload);
}

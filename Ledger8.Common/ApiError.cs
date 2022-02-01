using System.Text;

namespace Ledger8.Common;

[Serializable]
public sealed class ApiError
{
    public int Code { get; set; }
    public string Message { get; set; }
    public string[] Messages { get; set; }

    public ApiError() : this(0, string.Empty, Array.Empty<string>()) { }

    public ApiError(int code, string? message = null, string[]? messages = null)
    {
        Code = code;
        Message = message ?? string.Empty;
        Messages = messages ?? Array.Empty<string>();
    }

    public ApiError(string? message) : this(0) => Message = message ?? string.Empty;

    public ApiError(string[]? messages) : this(0) => Messages = messages ?? Array.Empty<string>();

    public ApiError(string? message, string[]? messages) : this(0)
    {
        Message = message ?? string.Empty;
        Messages = messages ?? Array.Empty<string>();
    }

    public static ApiError FromDalResult(DalResult result) => new((int)result.ErrorCode, result.Exception?.Innermost() ?? string.Empty);

    public static ApiError FromException(Exception exception) => new(message: exception?.Innermost() ?? string.Empty);

    public bool Successful => Code == 0 && string.IsNullOrWhiteSpace(Message) && (Messages is null || !Messages.Any());

    public static ApiError Success => new();

    public string[] Errors()
    {
        List<string> ret = new();
        if (!string.IsNullOrWhiteSpace(Message))
        {
            ret.Add(Message);
        }
        if (Messages is not null && Messages.Any())
        {
            Messages.ForEach(x => ret.Add(x));
        }
        return ret.ToArray();
    }

    public string ErrorMessage()
    {
        var sb = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Message))
        {
            sb.AppendLine(Message);
        }
        if (Messages is not null && Messages.Any())
        {
            Messages.ForEach(x => sb.AppendLine(x));
        }
        return sb.ToString().TrimEnd(new char[] { '\r', '\n' });
    }
}

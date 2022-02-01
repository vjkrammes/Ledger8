using Ledger8.Common.Interfaces;

using System.Text;

namespace Ledger8.Common;

public class UriHelper : IUriHelper
{
    private int _version { get; set; } = 1;
    private string? _base { get; set; }

    public void SetVersion(int version) => _version = version;

    public void SetBase(string path)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            _base = path;
            if (!_base.EndsWith('/'))
            {
                _base += "/";
            }
        }
    }

    public Uri Create(string controller, string action, params object[] parms)
    {
        if (string.IsNullOrWhiteSpace(_base))
        {
            throw new InvalidOperationException("The base URL has not been set");
        }
        var sb = new StringBuilder(_base);
        sb.Append("api/");
        if (_version > 0)
        {
            sb.Append($"v{_version}/");
        }
        if (!string.IsNullOrWhiteSpace(controller))
        {
            sb.Append($"{controller}/");
        }
        if (!string.IsNullOrWhiteSpace(action))
        {
            sb.Append($"{action}/");
        }
        if (parms is not null && parms.Any())
        {
            parms.ForEach(x =>
            {
                if (x is not null)
                {
                    sb.Append(x);
                    sb.Append('/');
                }
            });
        }
        return new(sb.ToString());
    }
}

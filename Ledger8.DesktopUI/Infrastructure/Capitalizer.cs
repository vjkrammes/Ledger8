using Humanizer;

using Ledger8.Common;

using System;
using System.Text;

namespace Ledger8.DesktopUI.Infrastructure;

public class Capitalizer : IStringTransformer
{
    public string Transform(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }
        var parts = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        foreach (var part in parts)
        {
            sb.Append(part.Capitalize());
            sb.Append(' ');
        }
        return sb.ToString().TrimEnd(' ');
    }
}

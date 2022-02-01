using Ledger8.Common;
using Ledger8.DataAccess.EFCore;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;

namespace Ledger8.DesktopUI.Infrastructure;

public static class ExtensionMethods
{
    public static LedgerContext GetContext(this IServiceProvider provider) => provider.GetRequiredService<LedgerContext>();

    private readonly static Capitalizer _capitalizer;

    static ExtensionMethods() => _capitalizer = new Capitalizer();

    private readonly static (string, string)[] _months = new (string, string)[]
    {
        ("", ""),
        ("January", "Jan"),
        ("February", "Feb"),
        ("March", "Mar"),
        ("April", "Apr"),
        ("May", "May"),
        ("June", "Jun"),
        ("July", "Jul"),
        ("August", "Aug"),
        ("September", "Sep"),
        ("October", "Oct"),
        ("November", "Nov"),
        ("December", "Dec")
    };

    public static string ToMonth(this int month, bool abbreviate = false)
    {
        if (month is <= 0 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be a number between 1 and 12");
        }
        return abbreviate ? _months[month].Item2 : _months[month].Item1;
    }

    public static string Caseify(this string value) => _capitalizer.Transform(value);

    public static Uri? GetIconFromEnumValue<T>(this T value) where T : Enum =>
        typeof(T)
            .GetField(value.ToString())?
            .GetCustomAttributes(typeof(ExplorerIconAttribute), false)
            .SingleOrDefault() is not ExplorerIconAttribute icon ? null : new(icon.ExplorerIcon, UriKind.Relative);

    public static SolidColorBrush GetBrush(this long argb)
    {
        (var a, var r, var g, var b) = argb.Octets();
        return new SolidColorBrush(Color.FromArgb(a, r, g, b));
    }

    public static bool Contains(this string source, string pattern, StringComparison comp) => source.Contains(pattern, comp);

    public static bool Matches(this string s1, string s2, StringComparison comp) => s1.Equals(s2, comp);

    public static List<string> ToList(this StringCollection collection)
    {
        var ret = new List<string>();
        if (collection is null)
        {
            return ret;
        }
        foreach (var c in collection)
        {
            if (c is not null)
            {
                ret.Add(c);
            }
        }
        return ret;
    }

    public static void Resort<T, U>(this List<T> source, Func<T, U> pred) where U : IComparable<U> => source.Sort((x, y) =>
        pred.Invoke(x).CompareTo(pred.Invoke(y)));
}

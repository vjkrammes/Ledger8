using Ledger8.Common.Enumerations;
using Ledger8.Common.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Ledger8.Common;

public static class ExtensionMethods
{
    public static bool IsTracked<TContext, TEntity>(this TContext context, TEntity entity) where TContext : DbContext where TEntity : class, IIdEntity, new()
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return context.Set<TEntity>().Local.Any(x => x.Id == entity.Id);
    }

    public static string Operation(this bool operation) => operation ? "reloading" : "loading";

    public static bool IsStrongPassword(this PasswordStrength strength) =>
        strength is PasswordStrength.Strong or PasswordStrength.VeryStrong;

    public static string Ordinalize(this int value)
    {
        if (value <= 0)
        {
            return value.ToString();
        }
        return (value % 100) switch
        {
            11 or 12 or 13 => value.ToString() + "th",
            _ => (value % 10) switch
            {
                1 => value.ToString() + "st",
                2 => value.ToString() + "nd",
                3 => value.ToString() + "rd",
                _ => value.ToString() + "th"
            }
        };
    }

    private readonly static Dictionary<string, string> _months = new()
    {
        ["01"] = "January",
        ["02"] = "February",
        ["03"] = "March",
        ["04"] = "April",
        ["05"] = "May",
        ["06"] = "June",
        ["07"] = "July",
        ["08"] = "August",
        ["09"] = "September",
        ["10"] = "October",
        ["11"] = "November",
        ["12"] = "December"
    };

    public static string FormatDate(this string date)
    {
        if (date.Length != 8)
        {
            return date;
        }
        var year = date[..4];
        var month = date[4..6];
        var day = date[6..];
        var sb = new StringBuilder();
        if (!_months.ContainsKey(month))
        {
            return date;
        }
        sb.Append(_months[month]);
        sb.Append(' ');
        sb.Append(day);
        sb.Append(", ");
        sb.Append(year);
        return sb.ToString();
    }

    public static string FirstWord(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }
        return value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).First();
    }

    public static string Host(this string api)
    {
        if (string.IsNullOrWhiteSpace(api))
        {
            return string.Empty;
        }
        try
        {
            var uri = new Uri(api);
            return uri.Host;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static int Sign<T>(this T value) where T : IComparable<T>
    {
        if (value.CompareTo(default) < 0)
        {
            return -1;
        }
        if (value.CompareTo(default) > 0)
        {
            return 1;
        }
        return 0;
    }

    public static string StripPadding(this string value) => value.TrimEnd('=');

    public static string AddPadding(this string value) => (value.Length % 4) switch
    {
        0 => value,
        1 => value + "===",
        2 => value + "==",
        _ => value + "="
    };

    public static byte[] ComputeHash(this byte[] value, string key, string hash = "SHA512") =>
        value.ComputeHash(Encoding.UTF8.GetBytes(key), hash);

    public static byte[] ComputeHash(this byte[] value, byte[] key, string hash = "SHA512")
    {
        byte[] signature;
        switch (hash.ToLower())
        {
            case "sha256":
                using (var hmac = new HMACSHA256(key))
                {
                    signature = hmac.ComputeHash(value);
                }
                break;
            case "sha384":
                using (var hmac = new HMACSHA384(key))
                {
                    signature = hmac.ComputeHash(value);
                }
                break;
            case "sha512":
                using (var hmac = new HMACSHA512(key))
                {
                    signature = hmac.ComputeHash(value);
                }
                break;
            default:
                throw new ArgumentException($"Only SHA256, SHA384 and SHA512 are supported, found '{hash}'");
        }
        return signature;
    }

    public static (byte o0, byte o1, byte o2, byte o3) Octets(this ulong value)
    {
        var o0 = (byte)((value >> 24) & 0xff);
        var o1 = (byte)((value >> 16) & 0xff);
        var o2 = (byte)((value >> 8) & 0xff);
        var o3 = (byte)(value & 0xff);
        return (o0, o1, o2, o3);
    }

    public static (byte o0, byte o1, byte o2, byte o3) Octets(this long value) => ((ulong)value).Octets();

    public static (byte o0, byte o1, byte o2, byte o3) Octets(this uint value) => ((ulong)value).Octets();

    public static (byte o0, byte o1, byte o2, byte o3) Octets(this int value) => ((ulong)value).Octets();

    public static string Hexify(this ulong argb)
    {
        StringBuilder sb = new("0x");
        var (o0, o1, o2, o3) = argb.Octets();
        sb.Append(o0.ToString("x2"));
        sb.Append(o1.ToString("x2"));
        sb.Append(o2.ToString("x2"));
        sb.Append(o3.ToString("x2"));
        return sb.ToString();
    }

    public static string Hexify(this long argb) => ((ulong)argb).Hexify();

    public static string Hexify(this uint argb) => ((ulong)argb).Hexify();

    public static string Hexify(this int argb) => ((ulong)argb).Hexify();

    public static string Hexify(this byte[] value, bool addHeader = true)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (!value.Any())
        {
            return string.Empty;
        }
        var sb = new StringBuilder();
        if (addHeader)
        {
            sb.Append("0x");
        }
        foreach (var b in value)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    public static IEnumerable<TModel> ToModels<TModel, TEntity>(this IEnumerable<TEntity> entities, string methodName = "FromEntity")
        where TModel : class where TEntity : class
    {
        var ret = new List<TModel>();
        var method = typeof(TModel).GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, null,
            new[] { typeof(TEntity) }, Array.Empty<ParameterModifier>());
        if (method is not null)
        {
            if (entities is not null && entities.Any())
            {
                entities.ForEach(x => ret.Add((method.Invoke(null, new[] { x }) as TModel)!));
            }
        }
        return ret;
    }

    public static string TrimEnd(this string value, string suffix, StringComparison comparer = StringComparison.OrdinalIgnoreCase)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (!string.IsNullOrWhiteSpace(suffix) && value.EndsWith(suffix, comparer))
        {
            return value[..^suffix.Length];
        }
        return value;
    }

    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }
        foreach (var item in list)
        {
            action(item);
        }
    }

    public static string Beginning(this string value, int length, char ellipsis = '.')
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }
        return value.Length <= length ? value : value[..length] + new string(ellipsis, 3);
    }

    public static bool IsDescending(this char value) => value is 'd' or 'D';

    public static T[] ArrayCopy<T>(this T[] source)
    {
        if (source is null || !source.Any())
        {
            return Array.Empty<T>();
        }
        var ret = new T[source.Length];
        Array.Copy(source, ret, source.Length);
        return ret;
    }

    public static bool ArrayEquals<T>(this T[] left, T[] right, bool wholeLength = false) where T : IEquatable<T>
    {
        if (left is null)
        {
            if (right is null)
            {
                return true;
            }
            return false;
        }
        if (right is null)
        {
            return false;
        }
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left.Length != right.Length)
        {
            return false;
        }
        var comparer = EqualityComparer<T>.Default;
        var ret = true;
        for (var i = 0; i < left.Length; i++)
        {
            if (!comparer.Equals(left[i], right[i]))
            {
                ret = false;
                if (!wholeLength)
                {
                    break;
                }
            }
        }
        return ret;
    }

    public static T[] Append<T>(this T[] left, T[] right)
    {
        if (left is null || !left.Any())
        {
            return right ?? Array.Empty<T>();
        }
        if (right is null || !right.Any())
        {
            return left;
        }
        var ret = new T[left.Length + right.Length];
        Array.Copy(left, 0, ret, 0, left.Length);
        Array.Copy(right, 0, ret, left.Length, right.Length);
        return ret;
    }

    public static string Capitalize(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }
        return value.First().ToString().ToUpper(CultureInfo.CurrentCulture) + string.Join(string.Empty, value.Skip(1));
    }

    public static string GetDescriptionFromEnumValue<T>(this T value) where T : Enum =>
        typeof(T)
            .GetField(value.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;

    public static string Innermost(this Exception exception)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }
        if (exception.InnerException is null)
        {
            return exception.Message;
        }
        return exception.InnerException.Innermost();
    }

    public static bool IsBetween(this DateTime date, DateTime start, DateTime end) => date >= start && date <= end;

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string order, bool descending)
    {
        //
        // this code from StackOverflow.com answer to this question: 
        //   https://stackoverflow.com/questions/7265186/how-do-i-specify-the-linq-orderby-argument-dynamically
        //

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        var command = descending ? "OrderByDescending" : "OrderBy";
        var type = typeof(T);
        var property = type.GetProperty(order);
        if (property is null)
        {
            return source;
        }
        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(typeof(Queryable), command,
            new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
        return source.Provider.CreateQuery<T>(resultExpression);
    }
}

using Ledger8.Common;
using Ledger8.Common.Enumerations;
using Ledger8.DesktopUI.Models;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Ledger8.DesktopUI.Infrastructure;

// convert from 2 strings (identity tag, current account tag) to solidcolorbrush based on parm which is format "matchingcolor|nonmatchingcolor"

public sealed class TagsToBorderConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type t, object parm, CultureInfo lang)
    {
        if (values is null || values.Length != 2 || values.Any(x => x == DependencyProperty.UnsetValue))
        {
            return Brushes.Black;
        }
        if (parm is not string colors)
        {
            return Brushes.Black;
        }
        if (values[0] is not string identityTag || values[1] is not string currentAccountTag)
        {
            return Brushes.Black;
        }
        var (matchColor, mismatchColor) = Tools.GetColors(colors);
        if (string.Equals(identityTag, currentAccountTag, StringComparison.OrdinalIgnoreCase))
        {
            return matchColor;
        }
        return mismatchColor;
    }

    public object[] ConvertBack(object value, Type[] t, object parm, CultureInfo lang) => throw new NotImplementedException();
}

// similar to above but for thickness, where parm is "matchthickness|mismatchthickness"

public sealed class TagsToThicknessConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type t, object parm, CultureInfo lang)
    {
        var def = new Thickness(1.0);
        if (values is null || values.Length != 2 || values.Any(x => x == DependencyProperty.UnsetValue))
        {
            return def;
        }
        if (values[0] is not string identityTag || values[1] is not string currentAccountTag)
        {
            return def;
        }
        if (parm is not string thicknesses)
        {
            return def;
        }
        var parts = thicknesses.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || !double.TryParse(parts[0], out var matchThickness) || !double.TryParse(parts[1], out var mismatchThickness))
        {
            return def;
        }
        return string.Equals(identityTag, currentAccountTag, StringComparison.OrdinalIgnoreCase)
            ? new Thickness(matchThickness)
            : new Thickness(mismatchThickness);
    }

    public object[] ConvertBack(object value, Type[] t, object parm, CultureInfo lang) => throw new NotSupportedException();
}

// convert from bool to color where true = red else black

[ValueConversion(typeof(bool), typeof(SolidColorBrush))]
public sealed class BoolToForegroundConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not bool val)
        {
            return Brushes.Black;
        }
        return val ? Brushes.Red : Brushes.Black;
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert to/from int and string where 0 = blank

[ValueConversion(typeof(int), typeof(string))]
public sealed class IdToNumberConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not int id)
        {
            return string.Empty;
        }
        return id == 0 ? string.Empty : id.ToString();
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
    {
        var idstring = value as string;
        if (string.IsNullOrWhiteSpace(idstring) || !int.TryParse(idstring, out var id))
        {
            return 0;
        }
        return id;
    }
}

// convert from bool to its inverse

[ValueConversion(typeof(bool), typeof(bool))]
public sealed class BoolToInverseConverter : IValueConverter
{
    public object Convert(object value, Type t, object parm, CultureInfo lang) => !(bool)value;

    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => !(bool)value;
}

// convert from DueDateType, Month and Day to a human readable format like, "Monthly on the 12th"

public sealed class AccountToDueDateConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type t, object parm, CultureInfo lang)
    {
        //
        // parameters are
        // 1. DueDateType
        // 2. Month
        // 3. Day
        // 4. true for abbreviated month name, else false

        if (values is null || values.Length != 4 || values.Any(x => x == DependencyProperty.UnsetValue))
        {
            return string.Empty;
        }
        if (values[0] is not DueDateType ddt || values[1] is not int month || values[2] is not int day || values[3] is not bool abbreviate)
        {
            return string.Empty;
        }
        if (ddt is DueDateType.ServiceRelated or DueDateType.Unspecified or DueDateType.NA)
        {
            return ddt.GetDescriptionFromEnumValue();
        }
        var sb = new StringBuilder(ddt.GetDescriptionFromEnumValue());
        sb.Append(" on the ");
        sb.Append(day.Ordinalize());
        switch (ddt)
        {
            case DueDateType.Annnually:
                sb.Append(" of ");
                sb.Append(month.ToMonth(abbreviate));
                break;
            case DueDateType.Quarterly:
                sb.Append(" of ");
                for (var i = 0; i < 12; i += 3)
                {
                    var m = month + i;
                    if (m > 12)
                    {
                        m -= 12;
                    }
                    sb.Append(m.ToMonth(abbreviate));
                    if (i is 0 or 3)
                    {
                        sb.Append(", ");
                    }
                    else if (i == 6)
                    {
                        sb.Append(", and ");
                    }
                }
                break;
            case DueDateType.SemiAnnual:
                sb.Append(" of ");
                sb.Append(month.ToMonth(abbreviate));
                sb.Append(" and ");
                var m2 = month + 6;
                if (m2 > 12)
                {
                    m2 -= 12;
                }
                sb.Append(m2.ToMonth(abbreviate));
                break;
        }
        return sb.ToString();
    }

    public object[] ConvertBack(object value, Type[] t, object parm, CultureInfo lang) => throw new NotImplementedException();
}

// convert from duedatetype to display

[ValueConversion(typeof(DueDateType), typeof(string))]
public sealed class DueDateTypeConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not DueDateType ddt)
        {
            return string.Empty;
        }
        return ddt.GetDescriptionFromEnumValue();
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from object to uri where if null, return null else return checkmark

[ValueConversion(typeof(object), typeof(Uri))]
public sealed class ObjectToCheckmarkConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang) =>
        value is null ? null! : new Uri(Constants.Checkmark, UriKind.Relative);
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from DateTime to either "Never" if default, or a compressed date/time string

[ValueConversion(typeof(DateTime?), typeof(string))]
public sealed class LastCopyDateConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        var date = (DateTime?)value;
        if (!date.HasValue || date.Value == default)
        {
            return "Never";
        }
        var d = date.Value;
        return $"{d.ToShortDateString()} {d.Hour:00}:{d.Minute:00}:{d.Second:00}.{d.Millisecond:0000}";
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from visibility to its inverse (hidden => vis, vis => hidden)

[ValueConversion(typeof(Visibility), typeof(Visibility))]
public sealed class VisibilityToInverseConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not Visibility visibility)
        {
            return Visibility.Visible;
        }
        return visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from bool (ShowPassword) to string for button text

[ValueConversion(typeof(bool), typeof(string))]
public sealed class ShowPasswordToButtonTextConverter : IValueConverter
{
    private readonly static Dictionary<bool, string> _texts = new()
    {
        [true] = "Hide",
        [false] = "Show"
    };

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not bool v)
        {
            return string.Empty;
        }
        return _texts[v];
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from visibility to icon uri for context menu

[ValueConversion(typeof(Visibility), typeof(Uri))]
public sealed class VisibilityToMenuIconConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not Visibility v)
        {
            return null!;
        }
        return v switch
        {
            Visibility.Visible => new Uri("/resources/cancel-32.png", UriKind.Relative),
            _ => new Uri("/resources/view-32.png", UriKind.Relative)
        };
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from bool to checkmark uri or null

[ValueConversion(typeof(bool), typeof(Uri))]
public sealed class BoolToCheckmarkConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not bool v || !v)
        {
            return null!;
        }
        return new Uri(Constants.Checkmark, UriKind.Relative);
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert to/from string and decimal

[ValueConversion(typeof(decimal), typeof(string))]
public sealed class DecimalConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not decimal d || d == 0M)
        {
            return string.Empty;
        }
        return d.ToString();
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not string v || string.IsNullOrEmpty(v))
        {
            return 0M;
        }
        var val = v;
        if (v.EndsWith("."))
        {
            val = v.TrimEnd('.');
        }
        if (!decimal.TryParse(val, out var d))
        {
            return 0M;
        }
        return d;
    }
}

// convert from datetime to display where default = "initial" maxdate = "current" else toshortdatestring()

[ValueConversion(typeof(DateTime), typeof(string))]
public sealed class DateToDisplayConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not DateTime d)
        {
            return string.Empty;
        }
        if (d == default)
        {
            return "Initial";
        }
        if (d == DateTime.MaxValue)
        {
            return "Current";
        }
        return d.ToShortDateString();
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from datetime to display where default = blank else toshortdatestring

[ValueConversion(typeof(DateTime), typeof(string))]
public sealed class DateToStringConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not DateTime date)
        {
            return string.Empty;
        }
        return date == default ? string.Empty : date.ToShortDateString();
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from decimal to display where 0M = string.empty else c2

[ValueConversion(typeof(decimal), typeof(string))]
public sealed class DecimalToDisplayConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not decimal amount)
        {
            return string.Empty;
        }
        return amount == 0M ? string.Empty : amount.ToString("c2");
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from phone number to display format. if length != 10, return value else return formatted sting based on parm:
//
// parm = 1         .. (xxx) xxx-xxxx
// parm = 2         .. xxx-xxx-xxxxx
// else             .. return value
//

[ValueConversion(typeof(string), typeof(string), ParameterType = typeof(int))]
public sealed class PhoneNumberConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not string phone || phone.Length != 10)
        {
            return value;
        }
        var which = 0;
        if (parm is int w)
        {
            which = w;
        }
        var ac = phone[0..3];
        var exch = phone[3..6];
        var last4 = phone[6..];
        return which switch
        {
            1 => $"({ac}) {exch}-{last4}",
            2 => $"{ac} {exch}-{last4}",
            _ => value
        };
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not string phone)
        {
            return value;
        }
        var ret = phone.Replace("-", "").Replace("(", "").Replace(")", "").TrimStart('1');
        return ret.Length == 10 ? ret : phone;
    }
}

// convert from string to tooltip where string.isnullorempty => null to prevent empty tooltip popup

[ValueConversion(typeof(string), typeof(string))]
public sealed class StringToTooltipConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not string v || string.IsNullOrWhiteSpace(v))
        {
            return null!;
        }
        return v;
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from string with CF/LF to string without CR/LF

[ValueConversion(typeof(string), typeof(string))]
public sealed class LongStringToStringConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not string v)
        {
            return value;
        }
        return v.Replace("\r\n", " ");
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from decimal to foreground foreground color where <= = red, 0 = black, > 0 = green

[ValueConversion(typeof(decimal), typeof(SolidColorBrush))]
public sealed class MoneyToForegroundConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not decimal v || v == 0M)
        {
            return Brushes.Black;
        }
        return v < 0M ? Brushes.Red : Brushes.Green;
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from string to Uri where Urikind is in parm 'r' (default), 'a' or 'ra'

[ValueConversion(typeof(string), typeof(Uri), ParameterType = typeof(string))]
public sealed class StringToUriConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        var kind = UriKind.Relative;
        if (parm is string k)
        {
            kind = k.ToLower() switch
            {
                "a" => UriKind.Absolute,
                "ra" => UriKind.RelativeOrAbsolute,
                _ => UriKind.Relative
            };
        }
        if (value is not string uri || string.IsNullOrWhiteSpace(uri))
        {
            return null!;
        }
        return new Uri(uri, kind);
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from full path to header and back

[ValueConversion(typeof(string), typeof(string))]
public sealed class UriToHeaderConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not string uri)
        {
            return value;
        }
        return uri.Replace("resources/", "").Replace("-32.png", "").TrimStart('/').Capitalize();
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not string header)
        {
            return value;
        }
        return $"/resources/{header.ToLower()}-32.png";
    }
}

// convert from passwordstrength to description

[ValueConversion(typeof(PasswordStrength), typeof(string))]
public sealed class PasswordStrengthConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not PasswordStrength ps)
        {
            return null!;
        }
        return ps.GetDescriptionFromEnumValue();
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from passwordstrength to foreground color

[ValueConversion(typeof(PasswordStrength), typeof(SolidColorBrush))]
public sealed class PasswordStrengthToForegroundConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        if (value is not PasswordStrength ps)
        {
            return Brushes.Black;
        }
        return ps switch
        {
            PasswordStrength.VeryWeak => new SolidColorBrush(Color.FromArgb(255, 192, 0, 0)),
            PasswordStrength.Weak => new SolidColorBrush(Color.FromArgb(255, 128, 0, 0)),
            PasswordStrength.Medium => Brushes.Gray,
            PasswordStrength.Strong => new SolidColorBrush(Color.FromArgb(255, 0, 128, 0)),
            PasswordStrength.VeryStrong => new SolidColorBrush(Color.FromArgb(255, 0, 192, 0)),
            _ => Brushes.Black
        };
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from bool to visibility where true == visible, inverted with parm

[ValueConversion(typeof(bool), typeof(Visibility), ParameterType = typeof(bool))]
public sealed class BoolToVisibilityConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        var invert = false;
        if (parm is bool i)
        {
            invert = i;
        }
        if (value is not bool v)
        {
            return Visibility.Visible;
        }
        if (invert)
        {
            return v ? Visibility.Collapsed : Visibility.Visible;
        }
        return v ? Visibility.Visible : Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from int (count) to visibility where 0 == visible else collapsed, inverted with parm

[ValueConversion(typeof(int), typeof(Visibility), ParameterType = typeof(bool))]
public sealed class CountToVisibilityConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        var invert = false;
        if (parm is bool i)
        {
            invert = i;
        }
        if (value is not int v)
        {
            return Visibility.Visible;
        }
        if (invert)
        {
            return v == 0 ? Visibility.Collapsed : Visibility.Visible;
        }
        return v == 0 ? Visibility.Visible : Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from int (count) to enabled where 0 = false else true, inverted with parm

[ValueConversion(typeof(int), typeof(bool), ParameterType = typeof(bool))]
public sealed class CountToEnabledConverter : IValueConverter
{

    public object Convert(object value, Type t, object parm, CultureInfo lang)
    {
        var invert = false;
        if (parm is bool i)
        {
            invert = i;
        }
        if (value is not int v)
        {
            return false;
        }
        if (invert)
        {
            return v == 0;
        }
        return v != 0;
    }
    public object ConvertBack(object value, Type t, object parm, CultureInfo lang) => DependencyProperty.UnsetValue;
}

// convert from DisplayedAccountModel and bool (IsClosed) to visibility where id model is null || !IsClosed, return collapsed else visible

public sealed class AccountAndClosedToVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type t, object parm, CultureInfo lang)
    {
        if (values is null || values.Length != 2 || values.Any(x => x == DependencyProperty.UnsetValue))
        {
            return Visibility.Collapsed;
        }
        if (values[0] is null || values[0] is not DisplayedAccountModel model)
        {
            return Visibility.Collapsed;
        }
        if (values[1] is not bool isClosed)
        {
            return Visibility.Collapsed;
        }
        // ok account is not null, return depends only on isClosed
        return isClosed ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] t, object parm, CultureInfo lang) => throw new NotImplementedException();
}
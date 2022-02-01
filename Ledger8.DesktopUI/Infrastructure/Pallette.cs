using Ledger8.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Ledger8.DesktopUI.Infrastructure;

public static class Pallette
{
    private static Dictionary<string, Color> _colors { get; set; }

    static Pallette()
    {
        _colors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
        foreach (var info in typeof(Colors).GetProperties())
        {
            if (info.PropertyType == typeof(Color))
            {
                _colors.Add(info.Name, (Color)info.GetValue(null)!);
            }
        }
    }

    public static uint Value(this Color c) => (uint)((c.A << 24) | (c.R << 16) | (c.G << 8) | c.B);

    public static Color Get(string name)
    {
        if (!_colors.ContainsKey(name))
        {
            return Colors.Transparent;
        }
        return _colors[name];
    }

    public static SolidColorBrush GetBrush(string name) => new(Get(name));

    public static List<string> Names() => _colors.Select(x => x.Key).Distinct().ToList();

    public static Color ToColor(this uint value)
    {
        (var a, var r, var g, var b) = value.Octets();
        return Color.FromArgb(a, r, g, b);
    }

    public static SolidColorBrush ToBrush(this uint value) => new(value.ToColor());

    public static bool HasColor(string name) => _colors.ContainsKey(name);

    public static float GetBrightness(this Color color)
    {
        var num1 = color.R / 255f;
        var num2 = color.G / 255f;
        var num3 = color.B / 255f;
        var num4 = num1;
        var num5 = num1;
        if (num2 > num4)
        {
            num4 = num2;
        }
        if (num3 > num4)
        {
            num4 = num3;
        }
        if (num2 < num5)
        {
            num5 = num2;
        }
        if (num3 < num5)
        {
            num5 = num3;
        }
        return (num4 + num5) / 2f;
    }

    public static float GetHue(this Color color)
    {
        if ((color.R == color.G) && (color.G == color.B))
        {
            return 0f;
        }
        var num1 = color.R / 255f;
        var num2 = color.G / 255f;
        var num3 = color.B / 255f;
        var num7 = 0f;
        var num4 = num1;
        var num5 = num1;
        if (num2 > num4)
        {
            num4 = num2;
        }

        if (num3 > num4)
        {
            num4 = num3;
        }

        if (num2 < num5)
        {
            num5 = num2;
        }

        if (num3 < num5)
        {
            num5 = num3;
        }

        var num6 = num4 - num5;
        if (num1 == num4)
        {
            num7 = (num2 - num3) / num6;
        }
        else if (num2 == num4)
        {
            num7 = 2f + ((num3 - num1) / num6);
        }
        else if (num3 == num4)
        {
            num7 = 4f + ((num1 - num2) / num6);
        }

        num7 *= 60f;
        if (num7 < 0f)
        {
            num7 += 360f;
        }

        return num7;
    }

    public static float GetSaturation(this Color color)
    {
        var num1 = color.R / 255f;
        var num2 = color.G / 255f;
        var num3 = color.B / 255f;
        var num7 = 0f;
        var num4 = num1;
        var num5 = num1;
        if (num2 > num4)
        {
            num4 = num2;
        }

        if (num3 > num4)
        {
            num4 = num3;
        }

        if (num2 < num5)
        {
            num5 = num2;
        }

        if (num3 < num5)
        {
            num5 = num3;
        }

        if (num4 == num5)
        {
            return num7;
        }

        var num6 = (num4 + num5) / 2f;
        if (num6 <= 0.5)
        {
            return (num4 - num5) / (num4 + num5);
        }

        return (num4 - num5) / (2f - num4 - num5);
    }
}

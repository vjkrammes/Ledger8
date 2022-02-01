using Ledger8.Common;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ledger8.DesktopUI.Infrastructure;

public static class Tools
{

    public static (SolidColorBrush matchColor, SolidColorBrush mismatchColor) GetColors(string spec)
    {
        var def = (Brushes.Black, Brushes.Black);
        if (string.IsNullOrEmpty(spec))
        {
            return def;
        }
        var parts = spec.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return def;
        }
        if (!Pallette.HasColor(parts[0]) || !Pallette.HasColor(parts[1]))
        {
            return def;
        }
        return (Pallette.GetBrush(parts[0]), Pallette.GetBrush(parts[1]));
    }

    public static bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2) =>
        start1.IsBetween(start2, end2) ||
        end1.IsBetween(start2, end2) ||
        start2.IsBetween(start1, end1) ||
        end2.IsBetween(start1, end1);

    public static IEnumerable<T> GetValues<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();

    public static BitmapImage CachedImage(string uri) => CachedImage(new Uri(uri));

    public static BitmapImage CachedImage(Uri uri)
    {
        var ret = new BitmapImage();
        ret.BeginInit();
        ret.UriSource = uri;
        ret.CacheOption = BitmapCacheOption.OnLoad;
        ret.EndInit();
        return ret;
    }

    public static Uri PackedUri(string resource) => new($"pack://application:,,,/Ledger8.DesktopUI;component/resources/{resource}");

    public static string Normalize(double d)
    {
        if (d < 1_000)
        {
            return d.ToString("n0") + " bytes";
        }

        if (d < 1_000_000)
        {
            return Math.Round(d / 1_000, 2).ToString("n2") + " KB";
        }

        if (d < 1_000_000_000)
        {
            return Math.Round(d / 1_000_000, 2).ToString("n2") + " MB";
        }

        return Math.Round(d / 1_000_000_000, 2).ToString("n2") + " GB";
    }

    public static IEnumerable<string> GetImages(Assembly a)
    {
        var ret = new List<string>();
        var resources = a.GetManifestResourceNames();
        foreach (var resource in resources)
        {
            if (!resource.Contains("g.resources"))
            {
                continue;
            }
            ResourceSet? rs = null;
            try
            {
                rs = new(a.GetManifestResourceStream(resource) ?? throw new InvalidOperationException("No stream"));
            }
            catch
            {
                rs?.Dispose();
                continue;
            }
            var hashes = rs.Cast<DictionaryEntry>().ToList();
            rs.Dispose();
            var keys = new List<string>();
            foreach (var hash in hashes)
            {
                if (!hash.Key.ToString()?.EndsWith("-32.png") ?? true)
                {
                    continue;
                }
                keys.Add(hash.Key.ToString()!);
            }
            var uris = keys.OrderBy(x => x).ToList();
            foreach (var uri in uris)
            {
                var u = uri.ToLower();
                if (!u.StartsWith("/"))
                {
                    u = $"/{u}";
                }
                if (u.Contains("database") || !u.Contains("resources/"))
                {
                    continue;
                }
                ret.Add(u);
            }
        }
        ret.Insert(0, "");
        return ret;
    }
}

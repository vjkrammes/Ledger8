using Ledger8.Common;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Models;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ledger8.DesktopUI.Services;

public class ThemeService : IThemeService
{
    private readonly static Dictionary<string, string> _defaultColors = new()
    {
        [Constants.Alt0] = "AliceBlue",
        [Constants.Alt1] = "FloralWhite",
        [Constants.Background] = "DarkSlateGray",
        [Constants.Border] = "Black",
        [Constants.Foreground] = "White"
    };

    private readonly Dictionary<string, string> _colors = new(_defaultColors);

    private static void SetResourcce(string key, string value) => Application.Current.Resources[key] = Pallette.GetBrush(value);

    private static void SetResources(Dictionary<string, string> colors)
    {
        if (colors is not null && colors.Any())
        {
            foreach (var kvp in colors)
            {
                SetResourcce(kvp.Key, kvp.Value);
            }
        }
    }

    public void LoadDefaultTheme() => SetResources(_defaultColors);

    public void LoadTheme(IConfiguration configuration)
    {
        var section = configuration.GetSection("Theme");
        var theme = section.Get<ThemeModel>();
        if (theme is not null)
        {
            _colors[Constants.Alt0] = theme.Alt0 ?? _defaultColors[Constants.Alt0];
            _colors[Constants.Alt1] = theme.Alt1 ?? _defaultColors[Constants.Alt1];
            _colors[Constants.Background] = theme.Background ?? _defaultColors[Constants.Background];
            _colors[Constants.Border] = theme.Border ?? _defaultColors[Constants.Border];
            _colors[Constants.Foreground] = theme.Foreground ?? _defaultColors[Constants.Foreground];
            SetResources(_colors);
        }
    }

    public void SetColor(string? key, string value)
    {
        if (!string.IsNullOrWhiteSpace(key))
        {
            _colors[key] = value;
            SetResourcce(key, value);
        }
    }

    public string GetColor(string? key)
    {
        if (string.IsNullOrWhiteSpace(key) || !_colors.ContainsKey(key))
        {
            return string.Empty;
        }
        return _colors[key];
    }
}

using Ledger8.Common;
using Ledger8.DesktopUI.Controls;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;

using System.Linq;
using System.Reflection;

namespace Ledger8.DesktopUI.ViewModels;

public class AboutViewModel : ViewModelBase
{
    private ObservableDictionary<string, string>? _credits;
    public ObservableDictionary<string, string>? Credits
    {
        get => _credits!;
        set => SetProperty(ref _credits, value);
    }

    private ISettings? _settings;
    public ISettings Settings
    {
        get => _settings!;
        set => SetProperty(ref _settings, value);
    }

    private string? _shortTitle;
    public string ShortTitle
    {
        get => _shortTitle!;
        set => SetProperty(ref _shortTitle, value);
    }

    private string GetCopyrightFromAssembly()
    {
        var assembly = GetType().Assembly;
        var attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
        if (attributes is not null && attributes.Any())
        {
            return ((AssemblyCopyrightAttribute)attributes.First()).Copyright;
        }
        return "Copyright information unavailable";
    }

    private string GetCompanyFromAssembly()
    {
        var assembly = GetType().Assembly;
        var attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
        if (attributes is not null && attributes.Any())
        {
            return ((AssemblyCompanyAttribute)attributes.First()).Company;
        }
        return "Company information unavailable";
    }

    public AboutViewModel() : this(null) { }

    public AboutViewModel(ISettings? settings = null)
    {
        _settings = settings;
        ShortTitle = $"{Constants.ProductName} {Constants.ProductVersion:0.00}";
        Credits = new()
        {
            ["SystemId"] = _settings?.SystemId.ToString() ?? "Unavailable",
            ["Product"] = Constants.ProductName,
            ["Version"] = Constants.ProductVersion.ToString("n2"),
            ["Author"] = "V. James Krammes",
            ["Company"] = GetCompanyFromAssembly(),
            ["Copyright"] = GetCopyrightFromAssembly(),
            ["Platform"] = "Windows desktop",
            ["Architecture"] = "Model - View - ViewModel (MVVM)",
            [".NET Version"] = ".NET 6",
            ["Presentation"] = "Microsoft Windows Presentation Foundation (WPF)",
            ["Database"] = "Microsoft SQL",
            ["Database Access"] = "Microsoft Entity Framework Core",
            ["Text Handling"] = "Humanizer by Mehdi Khalili, Oren Novotny",
            ["Repository"] = "https://github.com/vjkrammes/Ledger8"
        };
    }
}

using Ledger8.Common;
using Ledger8.Common.Interfaces;
using Ledger8.Models;

using System.Collections.Generic;
using System.Linq;

namespace Ledger8.DesktopUI.Models;

public class DisplayedIdentityModel : NotifyBase
{
    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string? _companyName;
    public string CompanyName
    {
        get => _companyName!;
        set => SetProperty(ref _companyName, value);
    }

    private string? _url;
    public string URL
    {
        get => _url!;
        set => SetProperty(ref _url, value);
    }

    private string? _userId;
    public string UserId
    {
        get => _userId!;
        set => SetProperty(ref _userId, value);
    }

    private string? _password;
    public string Password
    {
        get => _password!;
        set => SetProperty(ref _password, value);
    }

    private string? _tag;
    public string Tag
    {
        get => _tag!;
        set => SetProperty(ref _tag, value);
    }

    public DisplayedIdentityModel()
    {
        Id = 0;
        CompanyName = string.Empty;
        URL = string.Empty;
        UserId = string.Empty;
        Password = string.Empty;
        Tag = string.Empty;
    }

    public DisplayedIdentityModel(IdentityModel model, string password, IStringCypherService cypher)
    {
        Id = model.Id;
        CompanyName = model.Company?.Name ?? "Unknown";
        URL = model.URL;
        UserId = cypher.Decrypt(model.UserId, password, model.UserSalt);
        Password = cypher.Decrypt(model.Password, password, model.PasswordSalt);
        Tag = model.Tag ?? string.Empty;
    }

    public static IEnumerable<DisplayedIdentityModel> FromModels(IEnumerable<IdentityModel> models, string password, IStringCypherService cypher) =>
        models.Select(x => new DisplayedIdentityModel(x, password, cypher)).ToList();
}

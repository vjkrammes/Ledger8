using Ledger8.Common;
using Ledger8.Common.Enumerations;
using Ledger8.Common.Interfaces;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Windows;
using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public class IdentityViewModel : ViewModelBase
{
    private readonly IStringCypherService? _stringCypherService;
    private readonly IPasswordManager? _passwordManager;
    private readonly ISalter _salter;
    private readonly IPasswordChecker _passwordChecker;
    private readonly IServiceFactory _serviceFactory;

    private string? _savedUserId = null;
    public bool _editing = false;

    private CompanyModel? _company;
    public CompanyModel Company
    {
        get => _company!;
        set
        {
            SetProperty(ref _company, value);
            if (Company is null)
            {
                Identity.CompanyId = 0;
                Identity.Company = null;
                URL = Identity?.URL ?? string.Empty;
            }
            else
            {
                Identity.CompanyId = Company.Id;
                Identity.Company = Company.Clone();
                URL = Company.URL ?? string.Empty;
            }
        }
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

    private string? _password1;
    public string Password1
    {
        get => _password1!;
        set
        {
            SetProperty(ref _password1, value);
            PasswordStrength = _passwordChecker.GetPasswordStrengthZxcvbn(Password1, out _);
        }
    }

    private string? _password2;
    public string Password2
    {
        get => _password2!;
        set => SetProperty(ref _password2, value);
    }

    private bool _showPassword;
    public bool ShowPassword
    {
        get => _showPassword;
        set
        {
            SetProperty(ref _showPassword, value);
            Password2Visibility = ShowPassword ? Visibility.Hidden : Visibility.Visible;
        }
    }

    private PasswordStrength _passwordStrength;
    public PasswordStrength PasswordStrength
    {
        get => _passwordStrength;
        set => SetProperty(ref _passwordStrength, value);
    }

    private Visibility _password2Visibility;
    public Visibility Password2Visibility
    {
        get => _password2Visibility;
        set => SetProperty(ref _password2Visibility, value);
    }

    private string? _tag;
    public string Tag
    {
        get => _tag!;
        set => SetProperty(ref _tag, value);
    }

    private IdentityModel? _identity;
    public IdentityModel Identity
    {
        get => _identity!;
        set
        {
            SetProperty(ref _identity, value);
            Company = Identity.Company!;
            URL = string.IsNullOrWhiteSpace(Identity.URL) ? Company?.URL ?? string.Empty : Identity.URL;
            UserId = _stringCypherService!.Decrypt(Identity.UserId, _passwordManager!.Get(), Identity.UserSalt);
            _savedUserId = UserId;
            Password1 = _stringCypherService.Decrypt(Identity.Password, _passwordManager.Get(), Identity.PasswordSalt);
            Password2 = Password1;
            Tag = Identity.Tag;
            _editing = Identity.Id > 0;
        }
    }

    private RelayCommand? _togglePasswordCommand;
    public ICommand TogglePasswordCommand
    {
        get
        {
            if (_togglePasswordCommand is null)
            {
                _togglePasswordCommand = new(parm => TogglePasswordClick(), parm => AlwaysCanExecute());
            }
            return _togglePasswordCommand;
        }
    }

    public override bool OkCanExecute() => !string.IsNullOrWhiteSpace(UserId) && (ShowPassword || Password1 == Password2);

    public override void OK()
    {
        var identityService = _serviceFactory.Create<IIdentityService>()!;
        if (_editing && UserId != _savedUserId && identityService.ReadForUserId(Company.Id, Identity.UserId) is not null)
        {
            Duplicate();
            return;
        }
        else if (!_editing && identityService.ReadForUserId(Company.Id, Identity.UserId) is not null)
        {
            Duplicate();
            return;
        }
        Identity.Company = Company;
        Identity.CompanyId = Company.Id;
        Identity.URL = URL;
        Identity.UserSalt = _salter.GenerateSalt(Constants.SaltLength);
        Identity.UserId = _stringCypherService!.Encrypt(UserId, _passwordManager!.Get(), Identity.UserSalt);
        Identity.PasswordSalt = _salter.GenerateSalt(Constants.SaltLength);
        Identity.Password = _stringCypherService!.Encrypt(Password1, _passwordManager!.Get(), Identity.PasswordSalt);
        Identity.Tag = Tag;
        base.OK();
    }

    public void TogglePasswordClick() => ShowPassword = !ShowPassword;

    private void Duplicate() => PopupManager.Popup($"An identity with the user id '{UserId}' already exists for this company", "Duplicate Identity",
        PopupButtons.OkCancel, PopupImage.Stop);

    public IdentityViewModel(IServiceFactory serviceFactory, IStringCypherService stringCypherService, IPasswordManager passwordManager, ISalter salter, 
        IPasswordChecker passwordChecker)
    {
        _serviceFactory = serviceFactory;
        _stringCypherService = stringCypherService;
        _passwordManager = passwordManager;
        _salter = salter;
        _passwordChecker = passwordChecker;
        Identity = new();
    }
}

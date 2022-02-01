using Ledger8.Common;
using Ledger8.Common.Enumerations;
using Ledger8.Common.Interfaces;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System.Windows;

namespace Ledger8.DesktopUI.ViewModels;

public class PasswordViewModel : ViewModelBase
{
    #region Properties

    private bool _password2Required;
    private readonly IPasswordChecker _passwordChecker;
    private readonly IHasher _hasher;

    public byte[]? Salt { get; set; }
    public byte[]? Hash { get; set; }

    private string? _password1;
    public string Password1
    {
        get => _password1!;
        set
        {
            SetProperty(ref _password1, value);
            PasswordStrength = _passwordChecker.GetPasswordStrengthZxcvbn(Password1, out var _);
        }
    }

    private string? _password2;
    public string Password2
    {
        get => _password2!;
        set => SetProperty(ref _password2, value);
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
        set
        {
            SetProperty(ref _password2Visibility, value);
            _password2Required = Password2Visibility == Visibility.Visible;
        }
    }

    private string? _shortHeader;
    public string ShortHeader
    {
        get => _shortHeader!;
        set => SetProperty(ref _shortHeader, value);
    }

    #endregion

    #region Command Methods

    public override bool OkCanExecute()
    {
        if ((Salt is null || Hash is null) && !_password2Required)
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(Password1))
        {
            return false;
        }
        if (_password2Required && string.IsNullOrWhiteSpace(Password2))
        {
            return false;
        }
        if (_password2Required && Password1 != Password2)
        {
            return false;
        }
        if (!_password2Required)
        {
            var hash = _hasher.GenerateHash(Password1, Salt!, Constants.Iterations, Constants.HashLength);
            if (!hash.ArrayEquals(Hash!))
            {
                return false;
            }
        }
        return true;
    }

    public override void OK()
    {
        var passwordManager = (Application.Current as App)?.ServiceProvider?.GetRequiredService<IPasswordManager>();
        if (passwordManager is not null)
        {
            passwordManager.Set(Password1);
        }
        base.OK();
    }

    #endregion

    public PasswordViewModel(IHasher hasher, IPasswordChecker passwordChecker)
    {
        _hasher = hasher;
        _passwordChecker = passwordChecker;
    }
}

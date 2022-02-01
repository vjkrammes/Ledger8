using Ledger8.Common.Interfaces;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;

using System;

namespace Ledger8.DesktopUI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
#nullable disable
    public MainViewModel(IViewModelFactory viewModelFactory, IServiceFactory serviceFactory, IStringCypherService stringCypherService,
        IPasswordManager passwordManager, IHasher hasher, ISalter salter)
    {
        _viewModelFactory = viewModelFactory;
        _serviceFactory = serviceFactory;
        _stringCypherService = stringCypherService;
        _passwordManager = passwordManager;
        _hasher = hasher;
        _salter = salter;
        WindowTitle = string.Empty; // null title will throw an exception
        PopupManager.SetMaxWidth(400);
        PopupManager.SetWindowIcon(new Uri("/resources/book-32.png", UriKind.Relative));
        OrphanedAccountNumbers = new();
        Companies = new();
        Identities = new();
        Accounts = new();
        Transactions = new();
    }
#nullable enable
}

using Ledger8.Common.Interfaces;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Models;
using Ledger8.Models;

using System;
using System.Collections.ObjectModel;

namespace Ledger8.DesktopUI.ViewModels;

public partial class MainViewModel
{
    private readonly IHasher _hasher;
    private readonly IPasswordManager _passwordManager;
    private readonly ISalter _salter;
    private readonly IServiceFactory _serviceFactory;
    private readonly IStringCypherService _stringCypherService;
    private readonly IViewModelFactory _viewModelFactory;

    #region Window-related properties

    private string? _windowTitle;
    public string WindowTitle
    {
        get => _windowTitle!;
        set => SetProperty(ref _windowTitle, value);
    }

    private string? _banner;
    public string Banner
    {
        get => _banner!;
        set => SetProperty(ref _banner, value);
    }

    #endregion

    private string? _shortTitle;
    public string ShortTitle
    {
        get => _shortTitle!;
        set => SetProperty(ref _shortTitle, value);
    }

    private ObservableCollection<int> _orphanedAccountNumbers;
    public ObservableCollection<int> OrphanedAccountNumbers
    {
        get => _orphanedAccountNumbers;
        set => SetProperty(ref _orphanedAccountNumbers, value);
    }

    private ObservableCollection<CompanyModel> _companies;
    public ObservableCollection<CompanyModel> Companies
    {
        get => _companies;
        set => SetProperty(ref _companies, value);
    }

    private CompanyModel? _selectedCompany;
    public CompanyModel? SelectedCompany
    {
        get => _selectedCompany;
        set
        {
            SetProperty(ref _selectedCompany, value);
            LoadAccounts();
            LoadIdentities();
            UpdateTotals();
        }
    }

    private ObservableCollection<DisplayedIdentityModel> _identities;
    public ObservableCollection<DisplayedIdentityModel> Identities
    {
        get => _identities;
        set => SetProperty(ref _identities, value);
    }

    private DisplayedIdentityModel? _selectedIdentity;
    public DisplayedIdentityModel? SelectedIdentity
    {
        get => _selectedIdentity;
        set => SetProperty(ref _selectedIdentity, value);
    }

    private ObservableCollection<DisplayedAccountModel> _accounts;
    public ObservableCollection<DisplayedAccountModel> Accounts
    {
        get => _accounts;
        set => SetProperty(ref _accounts, value);
    }

    private DisplayedAccountModel? _selectedAccount;
    public DisplayedAccountModel? SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            SetProperty(ref _selectedAccount, value);
            CurrentTag = SelectedAccount?.Tag!;
            SetToggle(SelectedAccount?.IsClosed ?? false);
            LoadTransactions();
            UpdateTotals();
        } 
    }

    private string? _currentTag;
    public string CurrentTag
    {
        get => _currentTag!;
        set => SetProperty(ref _currentTag, value);
    }

    private string? _toggleTooltip;
    public string ToggleTooltip
    {
        get => _toggleTooltip!;
        set => SetProperty(ref _toggleTooltip, value);
    }

    private string? _toggleIcon;
    public string ToggleIcon
    {
        get => _toggleIcon!;
        set => SetProperty(ref _toggleIcon, value);
    }

    private string? _accountNumber;
    public string AccountNumber
    {
        get => _accountNumber!;
        set => SetProperty(ref _accountNumber, value);
    }

    private string? _selectedAccountNumber;
    public string SelectedAccountNumber
    {
        get => _selectedAccountNumber!;
        set => SetProperty(ref _selectedAccountNumber, value);
    }

    private ObservableCollection<TransactionModel> _transactions;
    public ObservableCollection<TransactionModel> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    private TransactionModel? _selectedTransaction;
    public TransactionModel? SelectedTransaction
    {
        get => _selectedTransaction;
        set => SetProperty(ref _selectedTransaction, value);
    }

    private decimal _accountTotal;
    public decimal AccountTotal
    {
        get => _accountTotal;
        set => SetProperty(ref _accountTotal, value);
    }

    private decimal _grandTotal;
    public decimal GrandTotal
    {
        get => _grandTotal;
        set => SetProperty(ref _grandTotal, value);
    }

    private DateTime? _lastPasswordCopy;
    public DateTime? LastPasswordCopy
    {
        get => _lastPasswordCopy;
        set => SetProperty(ref _lastPasswordCopy, value);
    }
}

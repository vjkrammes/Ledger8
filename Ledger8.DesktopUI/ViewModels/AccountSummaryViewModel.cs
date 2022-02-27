using Ledger8.Common.Interfaces;
using Ledger8.DataAccess;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Models;

using Microsoft.EntityFrameworkCore;

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public class AccountSummaryViewModel : ViewModelBase
{
    private readonly LedgerContext _context;
    private readonly IStringCypherService _stringCypherService;
    private readonly IPasswordManager _passwordManager;

    private ObservableCollection<AccountSummaryModel>? _accounts;
    public ObservableCollection<AccountSummaryModel> Accounts
    {
        get => _accounts!;
        set => SetProperty(ref _accounts, value);
    }

    private AccountSummaryModel? _selectedAccount;

    public AccountSummaryModel SelectedAccount
    {
        get => _selectedAccount!;
        set => SetProperty(ref _selectedAccount, value);
    }

    private string? _buttonContent;
    public string ButtonContent
    {
        get => _buttonContent!;
        set => SetProperty(ref _buttonContent, value);
    }

    private string? _buttonIcon;
    public string ButtonIcon
    {
        get => _buttonIcon!;
        set => SetProperty(ref _buttonIcon, value);
    }

    private bool _hideClosed = false;

    private RelayCommand? _toggleHiddenCommand;
    public ICommand ToggleHiddenCommand
    {
        get
        {
            if (_toggleHiddenCommand is null)
            {
                _toggleHiddenCommand = new(parm => ToggleHiddenClick(), parm => AlwaysCanExecute());
            }
            return _toggleHiddenCommand;
        }
    }

    private void ToggleHiddenClick()
    {
        _hideClosed = !_hideClosed;
        ButtonContent = _hideClosed ? "Show Closed Accounts" : "Hide Closed Accounts";
        ButtonIcon = _hideClosed ? "/resources/view-32.png" : "/resources/x-32.png";
        LoadAccounts();
    }

    private void LoadAccounts()
    {
        Accounts = new();
        var companies = _context.Companies.AsNoTracking().OrderBy(x => x.Name).ToList();
        foreach (var company in companies)
        {
            var accounts = _hideClosed
                ?
                    _context.Accounts
                    .AsNoTracking()
                    .Include(x => x.AccountType)
                    .Where(x => x.CompanyId == company.Id && !x.IsClosed)
                    .ToList()
                :
                    _context.Accounts
                    .AsNoTracking()
                    .Include(x => x.AccountType)
                    .Where(x => x.CompanyId == company.Id)
                    .ToList();
            foreach (var account in accounts)
            {
                account.AccountNumber = _context.AccountNumbers
                    .AsNoTracking()
                    .Where(x => x.AccountId == account.Id)
                    .OrderByDescending(x => x.StopDate)
                    .Take(1)
                    .SingleOrDefault();
                var transaction = _context.Transactions
                    .AsNoTracking()
                    .Where(x => x.AccountId == account.Id)
                    .OrderByDescending(x => x.Date)
                    .Take(1)
                    .SingleOrDefault();
                var summary = new AccountSummaryModel(company!, account!, transaction!, _stringCypherService, _passwordManager);
                if (summary is not null)
                {
                    Accounts.Add(summary);
                }
            }
        }
    }

    public AccountSummaryViewModel(LedgerContext context, IStringCypherService stringCypherService, IPasswordManager passwordManager)
    {
        _context = context;
        _stringCypherService = stringCypherService;
        _passwordManager = passwordManager;
        _hideClosed = false;
        ButtonContent = "Hide Closed Accounts";
        ButtonIcon = "/resources/x-32.png";
        LoadAccounts();
    }
}

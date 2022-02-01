using Ledger8.Common;
using Ledger8.Common.Interfaces;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Models;
using Ledger8.DesktopUI.Views;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Ledger8.DesktopUI.ViewModels;

public partial class MainViewModel
{
    public override void Cancel() => Application.Current.MainWindow.Close();

    #region Company Methods

    private void AddCompanyClick()
    {
        var vm = _viewModelFactory.Create<CompanyViewModel>()!;
        var companyService = _serviceFactory.Create<ICompanyService>()!;
        if (DialogSupport.ShowDialog<CompanyWindow>(vm, Application.Current.MainWindow) != true)
        {
            return;
        }
        var company = vm.Company.Clone();
        company.Name = company.Name.Caseify();
        var result = companyService.Insert(company);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to create Company", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        var ix = 0;
        while (ix < Companies.Count && Companies[ix] < company)
        {
            ix++;
        }
        Companies.Insert(ix, company);
        SelectedCompany = company;
    }

    private bool CompanySelected() => SelectedCompany is not null;

    private void EditCompanyClick()
    {
        if (SelectedCompany is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<CompanyViewModel>()!;
        vm.Company = SelectedCompany.Clone();
        if (DialogSupport.ShowDialog<CompanyWindow>(vm, Application.Current.MainWindow) != true)
        {
            return;
        }
        var companyService = _serviceFactory.Create<ICompanyService>()!;
        var company = vm.Company.Clone();
        company.Name = company.Name.Caseify();
        var result = companyService.Update(company);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update Company", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        Companies.Remove(SelectedCompany);
        SelectedCompany = null;
        var ix = 0;
        while (ix < Companies.Count && Companies[ix] < company)
        {
            ix++;
        }
        Companies.Insert(ix, company);
        SelectedCompany = company;
    }

    private bool DeleteCompanyCanClick() => SelectedCompany is not null && SelectedCompany.CanDelete;

    private void DeleteCompanyClick()
    {
        if (SelectedCompany is null || !SelectedCompany.CanDelete)
        {
            return;
        }
        var msg = $"Delete Company '{SelectedCompany.Name}'? This action cannot be undone.";
        if (PopupManager.Popup(msg, "Delete Company?", PopupButtons.YesNo, PopupImage.Question) != PopupResult.Yes)
        {
            return;
        }
        var companyService = _serviceFactory.Create<ICompanyService>()!;
        var result = companyService.Delete(SelectedCompany);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to delete Company", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        Companies.Remove(SelectedCompany);
        if (Companies.Any())
        {
            SelectedCompany = Companies.FirstOrDefault();
        }
    }

    #endregion

    #region Account Methods

    private void AddAccountClick()
    {
        if (SelectedCompany is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<AccountViewModel>()!;
        vm.Company = SelectedCompany;
        if (DialogSupport.ShowDialog<AccountWindow>(vm, Application.Current.MainWindow) != true)
        {
            return;
        }
        var account = new AccountModel
        {
            Id = 0,
            CompanyId = SelectedCompany.Id,
            AccountTypeId = vm.SelectedAccountType.Id,
            DueDateType = vm.SelectedDueDateType,
            Month = vm.Month,
            Day = vm.Day,
            IsPayable = vm.IsPayable,
            Comments = vm.Comments ?? string.Empty,
            Tag = vm.Tag ?? string.Empty,
            AccountType = vm.SelectedAccountType
        };
        var accountService = _serviceFactory.Create<IAccountService>()!;
        AccountModel newaccount;
        try
        {
            newaccount = accountService.Create(account, vm.Number, _salter.GenerateSalt(Constants.SaltLength),
                _passwordManager.Get(), _stringCypherService)!;
        }
        catch (Exception ex)
        {
            PopupManager.Popup("Failed to create new account", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        LoadAccounts(false);
        SelectedAccount = Accounts.SingleOrDefault(x => x.Id == newaccount.Id);
    }

    private bool AccountSelected() => SelectedAccount is not null;

    private void EditAccountClick()
    {
        if (SelectedAccount is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<AccountViewModel>()!;
        vm.Company = SelectedCompany!;
        var accountService = _serviceFactory.Create<IAccountService>()!;
        var account = accountService.Read(SelectedAccount.Id);
        if (account is null)
        {
            PopupManager.Popup("Cannot find account", Constants.DBE, $"Cannot find the account with the Id {SelectedAccount.Id}", PopupButtons.Ok,
                PopupImage.Error);
            return;
        }
        vm.Account = account;
        if (DialogSupport.ShowDialog<AccountWindow>(vm, Application.Current.MainWindow) != true)
        {
            LoadAccounts(); // account number may have changed
            return;
        }
        account.AccountTypeId = vm.SelectedAccountType.Id;
        account.AccountType = vm.SelectedAccountType;
        account.DueDateType = vm.SelectedDueDateType;
        account.Month = vm.Month;
        account.Day = vm.Day;
        account.Comments = vm.Comments ?? string.Empty;
        account.IsPayable = vm.IsPayable;
        account.Tag = vm.Tag ?? string.Empty;
        var result = accountService.Update(account);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update account", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        LoadAccounts();
        SelectedAccount = Accounts.SingleOrDefault(x => x.Id == account.Id);
    }

    private bool AccountsExist()
    {
        var service = _serviceFactory.Create<IAccountService>();
        return (service?.Count ?? 0) > 0;
    }

    private void AccountSummaryClick()
    {
        var vm = _viewModelFactory.Create<AccountSummaryViewModel>()!;
        DialogSupport.ShowDialog<AccountSummaryWindow>(vm, Application.Current.MainWindow);
    }

    private void ToggleAccountClick()
    {
        if (SelectedAccount is null)
        {
            return;
        }
        var accountService = _serviceFactory.Create<IAccountService>()!;
        var account = accountService.Read(SelectedAccount.Id);
        if (account is null)
        {
            PopupManager.Popup("Failed to retrieve Account", Constants.DBE, $"Unable to retrieve the account with the Id {SelectedAccount.Id}",
                PopupButtons.Ok, PopupImage.Error);
            return;
        }
        account.IsClosed = !account.IsClosed;
        account.ClosedDate = account.IsClosed ? DateTime.UtcNow : default;
        var result = accountService.Update(account);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update account", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        SelectedAccount.IsClosed = account.IsClosed;
        SelectedAccount.ClosedDate = account.ClosedDate;
        SetToggle(SelectedAccount.IsClosed);
    }

    private bool DeleteAccountCanClick() => SelectedAccount is not null && SelectedAccount.CanDelete;

    private void DeleteAccountClick()
    {
        if (SelectedAccount is null || !SelectedAccount.CanDelete)
        {
            return;
        }
        var accountService = _serviceFactory.Create<IAccountService>()!;
        var account = accountService.Read(SelectedAccount.Id);
        if (account is null)
        {
            PopupManager.Popup("Failed to retrieve Account", Constants.DBE, $"Unable to retrieve the account wit the Id {SelectedAccount.Id}",
                PopupButtons.Ok, PopupImage.Error);
            return;
        }
        var result = accountService.Delete(account);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to delete Account", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        Accounts.Remove(SelectedAccount);
        SelectedAccount = null;
        if (Accounts.Any())
        {
            SelectedAccount = Accounts.FirstOrDefault(x => !x.IsClosed);
            if (SelectedAccount is null)
            {
                SelectedAccount = Accounts.FirstOrDefault();
            }
        }
    }

    #endregion

    #region Transaction Methods

    private bool AddTransactionCanClick() => SelectedAccount is not null && SelectedAccount.IsPayable;

    private void AddTransactionClick()
    {
        if (!AddTransactionCanClick())
        {
            return;
        }
        var vm = _viewModelFactory.Create<TransactionViewModel>()!;
        if (DialogSupport.ShowDialog<TransactionWindow>(vm, Application.Current.MainWindow) != true)
        {
            SelectedTransaction = null;
            return;
        }
        var transaction = new TransactionModel
        {
            Id = 0,
            AccountId = SelectedAccount!.Id,
            Date = vm.Date ?? DateTime.Now,
            Balance = vm.Balance,
            Payment = vm.Payment,
            Reference = vm.Reference ?? string.Empty
        };
        var transactionService = _serviceFactory.Create<ITransactionService>()!;
        var result = transactionService.Insert(transaction);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to insert new transaction", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedTransaction = null;
            return;
        }
        AddTransaction(transaction);
        SelectedAccount.CanDelete = false;
        UpdateTotals();
    }

    private bool TransactionSelected() => SelectedTransaction is not null;

    private void ChangeDateClick()
    {
        if (SelectedTransaction is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<DateViewModel>()!;
        vm.Prompt = "Transaction Date:";
        vm.Date = SelectedTransaction.Date;
        vm.BorderBrush = (Application.Current.Resources[Constants.Border] as SolidColorBrush) ?? Brushes.Black;
        vm.Validator = ValidateDate;
        if (DialogSupport.ShowDialog<DateWindow>(vm, Application.Current.MainWindow) != true || !vm.Date.HasValue)
        {
            SelectedTransaction = null;
            return;
        }
        var transaction = SelectedTransaction.Clone();
        transaction.Date = vm.Date.Value;
        var transactionService = _serviceFactory.Create<ITransactionService>()!;
        var result = transactionService.Update(transaction);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update Transaction", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedTransaction = null;
            return;
        }
        Transactions.Remove(transaction);
        AddTransaction(transaction);
        SelectedTransaction = null;
    }

    private void ChangeBalanceClick()
    {
        if (SelectedTransaction is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<QAViewModel>()!;
        vm.Question = "Balance:";
        vm.Answer = SelectedTransaction.Balance.ToString();
        vm.AnswerRequired = true;
        vm.BorderBrush = (Application.Current.Resources[Constants.Border] as SolidColorBrush) ?? Brushes.Black;
        vm.Validator = ValidateMoney;
        if (DialogSupport.ShowDialog<QAWindow>(vm, Application.Current.MainWindow) != true || string.IsNullOrWhiteSpace(vm.Answer))
        {
            SelectedTransaction = null;
            return;
        }
        if (!decimal.TryParse(vm.Answer, out var amount))
        {
            SelectedTransaction = null;
            return;
        }
        var transaction = SelectedTransaction.Clone();
        transaction.Balance = amount;
        var transactionService = _serviceFactory.Create<ITransactionService>()!;
        var result = transactionService.Update(transaction);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update Transaction", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedTransaction = null;
            return;
        }
        Transactions.Remove(SelectedTransaction);
        AddTransaction(transaction);
        SelectedTransaction = null;
    }

    private void ChangePaymentClick()
    {
        if (SelectedTransaction is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<QAViewModel>()!;
        vm.Question = "Payment:";
        vm.Answer = SelectedTransaction.Payment.ToString();
        vm.AnswerRequired = true;
        vm.BorderBrush = (Application.Current.Resources[Constants.Border] as SolidColorBrush) ?? Brushes.Black;
        vm.Validator = ValidateMoney;
        if (DialogSupport.ShowDialog<QAWindow>(vm, Application.Current.MainWindow) != true || string.IsNullOrWhiteSpace(vm.Answer))
        {
            SelectedTransaction = null;
            return;
        }
        if (!decimal.TryParse(vm.Answer, out var amount))
        {
            SelectedTransaction = null;
            return;
        }
        var transaction = SelectedTransaction.Clone();
        transaction.Payment = amount;
        var transactionService = _serviceFactory.Create<ITransactionService>()!;
        var result = transactionService.Update(transaction);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update Transaction", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedTransaction = null;
            return;
        }
        Transactions.Remove(SelectedTransaction);
        AddTransaction(transaction);
        SelectedTransaction = null;
        UpdateTotals();
    }

    private void ChangeReferenceClick()
    {
        if (SelectedTransaction is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<QAViewModel>()!;
        vm.Question = "Reference:";
        vm.Answer = SelectedTransaction.Reference;
        vm.AnswerRequired = false;
        vm.BorderBrush = (Application.Current.Resources[Constants.Border] as SolidColorBrush) ?? Brushes.Black;
        if (DialogSupport.ShowDialog<QAWindow>(vm, Application.Current.MainWindow) != true)
        {
            SelectedTransaction = null;
            return;
        }
        var transaction = SelectedTransaction.Clone();
        transaction.Reference = vm.Answer ?? string.Empty;
        var transactionService = _serviceFactory.Create<ITransactionService>()!;
        var result = transactionService.Update(transaction);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update Transaction", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedTransaction = null;
            return;
        }
        Transactions.Remove(SelectedTransaction);
        AddTransaction(transaction);
        SelectedTransaction = null;
    }

    private void DeselectTransactionClick() => SelectedTransaction = null;

    private void DeleteTransactionClick()
    {
        if (SelectedTransaction is null || !SelectedTransaction.CanDelete)
        {
            SelectedTransaction = null;
            return;
        }
        var transactionService = _serviceFactory.Create<ITransactionService>()!;
        var result = transactionService.Delete(SelectedTransaction);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to delete Transaction", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedTransaction = null;
            return;
        }
        Transactions.Remove(SelectedTransaction);
        SelectedTransaction = null;
        UpdateTotals();
    }

    #endregion

    #region Identity Methods

    private bool IdentitySelected() => SelectedIdentity is not null;

    private void CopyURLClick()
    {
        if (SelectedIdentity is not null)
        {
            Clipboard.SetText(SelectedIdentity.URL);
        }
    }

    private bool CopyPasswordCanClick() => SelectedIdentity is not null && !string.IsNullOrWhiteSpace(SelectedIdentity.Password);

    private void CopyPasswordClick()
    {
        if (SelectedIdentity is not null)
        {
            Clipboard.SetText(SelectedIdentity.Password);
            LastPasswordCopy = DateTime.Now;
        }
    }

    private static void ClearClipboardClick() => Clipboard.Clear();

    private void AddIdentityClick()
    {
        if (SelectedCompany is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<IdentityViewModel>()!;
        vm.Company = SelectedCompany;
        if (DialogSupport.ShowDialog<IdentityWindow>(vm, Application.Current.MainWindow) != true)
        {
            return;
        }
        var identity = vm.Identity.Clone();
        var identityService = _serviceFactory.Create<IIdentityService>()!;
        var result = identityService.Insert(identity);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to insert new Identity", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        var didentity = new DisplayedIdentityModel(identity, _passwordManager.Get(), _stringCypherService);
        Identities.Insert(0, didentity);
        SelectedIdentity = didentity;
        SelectedIdentity = null;
    }

    private void EditIdentityClick()
    {
        if (SelectedCompany is null || SelectedIdentity is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<IdentityViewModel>()!;
        var identityService = _serviceFactory.Create<IIdentityService>()!;
        var identity = identityService.Read(SelectedIdentity.Id);
        if (identity is null)
        {
            PopupManager.Popup("Failed to retrieve Identity", Constants.DBE, $"Failed to retrieve the identity with the Id {SelectedIdentity.Id}", PopupButtons.Ok,
                PopupImage.Error);
            SelectedIdentity = null;
            return;
        }
        vm.Identity = identity.Clone();
        vm.Company = SelectedCompany.Clone();
        if (DialogSupport.ShowDialog<IdentityWindow>(vm, Application.Current.MainWindow) != true)
        {
            SelectedIdentity = null;
            return;
        }
        identity = vm.Identity.Clone();
        var result = identityService.Update(identity);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update Identity", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedIdentity = null;
            return;
        }
        var didentity = new DisplayedIdentityModel(identity, _passwordManager.Get(), _stringCypherService);
        Identities.Remove(SelectedIdentity);
        Identities.Insert(0, didentity);
        SelectedIdentity = didentity;
        SelectedIdentity = null;
    }

    private void DeselectIdentityClick() => SelectedIdentity = null;

    private void DeleteIdentityClick()
    {
        if (SelectedIdentity is null)
        {
            return;
        }
        var identityService = _serviceFactory.Create<IIdentityService>()!;
        var identity = identityService.Read(SelectedIdentity.Id);
        if (identity is null)
        {
            PopupManager.Popup("Failed to retrieve Identity", Constants.DBE, $"Failed to retrieve the identity with the id {SelectedIdentity.Id}", PopupButtons.Ok,
                PopupImage.Error);
            SelectedIdentity = null;
            return;
        }
        var msg = $"Delete Identity '{SelectedIdentity.UserId}' from Company '{SelectedCompany!.Name}'?";
        if (PopupManager.Popup(msg, "Delete Identity?", PopupButtons.YesNo, PopupImage.Question) != PopupResult.Yes)
        {
            SelectedIdentity = null;
            return;
        }
        var result = identityService.Delete(identity);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to delete Identity", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedIdentity = null;
            return;
        }
        Identities.Remove(SelectedIdentity);
        SelectedIdentity = null;
    }

    #endregion

    #region Manage Methods

    private void ManageAccountTypesClick()
    {
        var vm = _viewModelFactory.Create<AccountTypeViewModel>()!;
        DialogSupport.ShowDialog<AccountTypeWindow>(vm, Application.Current.MainWindow);
    }

    private void ManagePoolsClick()
    {
        var vm = _viewModelFactory.Create<PoolViewModel>()!;
        DialogSupport.ShowDialog<PoolWindow>(vm, Application.Current.MainWindow);
    }

    private bool CleanUpOrphanedAccountNumbersCanClick() => OrphanedAccountNumbers is not null && OrphanedAccountNumbers.Any();

    private void CleanUpOrphanedAccountNumbersClick()
    {
        if (!OrphanedAccountNumbers.Any())
        {
            return;
        }
        using var wc = new WaitCursor();
        var acccountNumberService = _serviceFactory.Create<IAccountNumberService>()!;
        foreach (var id in OrphanedAccountNumbers)
        {
            try
            {
                var accountNumber = acccountNumberService.Read(id);
                if (accountNumber is not null)
                {
                    acccountNumberService.Delete(accountNumber);
                }
            }
            catch (Exception ex)
            {
                PopupManager.Popup($"Failed to delete account number {id}", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Warning);
            }
        }
        OrphanedAccountNumbers = new(GetOrphanedAccountNumbers());
        wc.Dispose();
        if (OrphanedAccountNumbers.Any())
        {
            PopupManager.Popup("Some orphaned account numbers still exist", "Not All deleted", PopupButtons.Ok, PopupImage.Information);
        }
        else
        {
            PopupManager.Popup("All orphaned account numbers have been deleted", "All Deleted", PopupButtons.Ok, PopupImage.Information);
        }
    }

    #endregion

    #region Miscellaneous Methods

    private void BackupClick()
    {
        var vm = _viewModelFactory.Create<BackupViewModel>()!;
        DialogSupport.ShowDialog<BackupWindow>(vm, Application.Current.MainWindow);
    }

    private void GettingStartedClick()
    {
        var vm = _viewModelFactory.Create<GettingStartedViewModel>()!;
        DialogSupport.ShowDialog<GettingStartedWindow>(vm, Application.Current.MainWindow);
    }

    private void AboutClick()
    {
        var vm = _viewModelFactory.Create<AboutViewModel>()!;
        DialogSupport.ShowDialog<AboutWindow>(vm, Application.Current.MainWindow);
    }

    #endregion

    #region Window Loaded

    private void WindowLoaded()
    {
        var app = (Application.Current as App);
        var settings = app?.ServiceProvider?.GetRequiredService<ISettings>();
        if (settings is null)
        {
            Environment.Exit(Constants.NoSettings);
        }
        var themeService = app?.ServiceProvider?.GetRequiredService<IThemeService>();
        if (themeService is not null)
        {
            var configurationFactory = app?.ServiceProvider?.GetRequiredService<IConfigurationFactory>();
            if (configurationFactory is not null)
            {
                var configuration = configurationFactory.Create(Constants.ConfigurationFilename);
                if (configuration is not null)
                {
                    themeService.LoadTheme(configuration);
                }
                else
                {
                    themeService.LoadDefaultTheme();
                }
            }
            else
            {
                themeService.LoadDefaultTheme();
            }
        }
        WindowTitle = $"{Constants.ProductName} Version {Constants.ProductVersion:0.00}";
        Banner = $"{Constants.ProductName} {Constants.ProductVersion:0.00} - Manage your Accounts and Identities";
        ShortTitle = $"{Constants.ProductName} {Constants.ProductVersion:0.00}";
        Authenticate(settings);
        LoadCompanies(false);
        if (Companies.Count == 0)
        {
            GettingStartedClick();
        }
        OrphanedAccountNumbers = new(GetOrphanedAccountNumbers());
        if (OrphanedAccountNumbers.Any())
        {
            var msg = "Orphaned account numbers can be removed by using the button on the tool bar.";
            PopupManager.Popup("Orphaned Account Numbers Exist", "Orphaned Account Numbers", msg, Enumerations.PopupButtons.Ok,
                Enumerations.PopupImage.Information);
        }
        UpdateTotals();
    }

    #endregion
}

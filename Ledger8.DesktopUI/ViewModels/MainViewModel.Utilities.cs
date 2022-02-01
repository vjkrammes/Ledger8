using Ledger8.Common;
using Ledger8.DataAccess.EFCore;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Models;
using Ledger8.DesktopUI.Views;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ledger8.DesktopUI.ViewModels;

public partial class MainViewModel
{
    private void Authenticate(ISettings settings)
    {
        if (settings.Salt is null && settings.Hash is null)
        {
            CreateNewPassword(settings);
        }
        else
        {
            VerifyPassword(settings);
        }
    }

    private void CreateNewPassword(ISettings settings)
    {
        var vm = _viewModelFactory.Create<PasswordViewModel>()!;
        vm.Password2Visibility = Visibility.Visible;
        vm.ShortHeader = ShortTitle;
        if (DialogSupport.ShowDialog<PasswordWindow>(vm, Application.Current.MainWindow) != true)
        {
            Environment.Exit(Constants.NoPasswordEntered);
        }
        var salt = _salter.GenerateSalt(Constants.SaltLength)!;
        var hash = _hasher.GenerateHash(vm.Password1, salt, Constants.Iterations, Constants.HashLength);
        settings.SetSaltAndHash(salt, hash);
    }

    private void VerifyPassword(ISettings settings)
    {
        var vm = _viewModelFactory.Create<PasswordViewModel>()!;
        vm.Password2Visibility = Visibility.Collapsed;
        vm.Salt = settings.Salt?.ArrayCopy();
        vm.Hash = settings.Hash?.ArrayCopy();
        vm.ShortHeader = ShortTitle;
        if (DialogSupport.ShowDialog<PasswordWindow>(vm, Application.Current.MainWindow) != true)
        {
            Environment.Exit(Constants.NoPasswordEntered);
        }
        var hash = _hasher.GenerateHash(vm.Password1, settings.Salt!, Constants.Iterations, Constants.HashLength);
        if (!hash.ArrayEquals(settings.Hash!))
        {
            PopupManager.Popup("Incorrect password entered", "Incorrect Password", PopupButtons.Ok, PopupImage.Stop);
            VerifyPassword(settings);
        }
    }

    private void LoadCompanies(bool reload = false)
    {
        SelectedCompany = null;
        var companyService = _serviceFactory.Create<ICompanyService>()!;
        try
        {
            Companies = new(companyService.Get(null, "Name"));
        }
        catch (Exception ex)
        {
            PopupManager.Popup($"Failed {reload.Operation()} Companies", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            Environment.Exit(Constants.CompaniesLoadFailed);
        }
        SelectedCompany = Companies.FirstOrDefault();
    }

    private void LoadIdentities(bool reload = false)
    {
        SelectedIdentity = null;
        if (SelectedCompany is null)
        {
            Identities = new();
        }
        else
        {
            try
            {
                var identityService = _serviceFactory.Create<IIdentityService>()!;
                Identities = new(DisplayedIdentityModel.FromModels(identityService.GetForCompany(SelectedCompany.Id),
                    _passwordManager.Get(), _stringCypherService));
            }
            catch (Exception ex)
            {
                PopupManager.Popup($"Failed {reload.Operation()} Identities", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Environment.Exit(Constants.IdentitiesLoadFailed);
            }
        }
    }

    private void LoadAccounts(bool reload = false)
    {
        SelectedAccount = null;
        if (SelectedCompany is null)
        {
            Accounts = new();
        }
        else
        {
            try
            {
                var accountService = _serviceFactory.Create<IAccountService>()!;
                Accounts = new(DisplayedAccountModel.FromModels(accountService.GetForCompany(SelectedCompany.Id),
                    _passwordManager.Get(), _stringCypherService));
                SelectedAccount = Accounts.FirstOrDefault(x => !x.IsClosed);
                if (SelectedAccount is null)
                {
                    SelectedAccount = Accounts.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                PopupManager.Popup($"Failed to {reload.Operation()} Accounts", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Environment.Exit(Constants.AccountsLoadFailed);
            }
        }
    }
    
    private void SetToggle(bool isClosed)
    {
        ToggleTooltip = isClosed ? "Reopen the Selected Account" : "Close the Selected Account";
        ToggleIcon = isClosed ? "/resources/checkmark-32.png" : "/resources/x-32.png";
    }

    private void LoadTransactions(bool reload = false)
    {
        SelectedTransaction = null;
        if (SelectedAccount is null)
        {
            Transactions = new();
        }
        else
        {
            try
            {
                var transactionService = _serviceFactory.Create<ITransactionService>()!;
                Transactions = new(transactionService.GetForAccount(SelectedAccount.Id));
            }
            catch (Exception ex)
            {
                PopupManager.Popup($"Failed {reload.Operation()} Transactions", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Environment.Exit(Constants.TransactionsLoadFailed);
            }
        }
    }

    private IEnumerable<int> GetOrphanedAccountNumbers()
    {
        var ret = new List<int>();
        var accountService = _serviceFactory.Create<IAccountService>()!;
        var context = (Application.Current as App)?.ServiceProvider?.GetRequiredService<LedgerContext>()!;
        var accountIds = context.AccountNumbers.Select(x => x.AccountId).Distinct().ToList();
        // accountIds is now the distinct list of all account id's in the account number table. If any of the id's don't exist in the accounts table, then
        // those account numbers are orphaned
        foreach (var accountId in accountIds)
        {
            if (accountService.Read(accountId) is null)
            {
                // account 'accountId' does not exist, any account numbers with that account id are orphans
                var orphans = context.AccountNumbers.Where(x => x.AccountId == accountId).Select(x => x.Id);
                ret.AddRange(orphans);
            }
        }
        return ret;
    }

    private void AddTransaction(TransactionModel transaction)
    {
        var ix = 0;
        while (ix < Transactions.Count && Transactions[ix] > transaction)
        {
            ix++;
        }
        Transactions.Insert(ix, transaction);
        SelectedTransaction = transaction;          // scroll into view
        SelectedTransaction = null;
    }

    private bool ValidateMoney(string value) => decimal.TryParse(value, out var _);

    private bool ValidateDate(DateTime? value) => value.HasValue && value.Value.Year >= 2020;

    private void UpdateTotals()
    {
        var transactionService = _serviceFactory.Create<ITransactionService>()!;
        AccountTotal = SelectedAccount is null ? 0M : transactionService.TotalForAccount(SelectedAccount.Id);
        GrandTotal = transactionService.Total();
    }
}

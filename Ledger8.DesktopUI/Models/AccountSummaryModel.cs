using Ledger8.Common;
using Ledger8.Common.Interfaces;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.Models;

using System;

namespace Ledger8.DesktopUI.Models;

public class AccountSummaryModel : NotifyBase
{
    private string? _company;
    public string Company
    {
        get => _company!;
        set => SetProperty(ref _company, value);
    }

    private string? _accountType;
    public string AccountType
    {
        get => _accountType!;
        set => SetProperty(ref _accountType, value);
    }

    private string? _accountNumber;
    public string AccountNumber
    {
        get => _accountNumber!;
        set => SetProperty(ref _accountNumber, value);
    }

    private string? _dueDate;
    public string DueDate
    {
        get => _dueDate!;
        set => SetProperty(ref _dueDate, value);
    }

    private DateTime _lastTransaction;
    public DateTime LastTransaction
    {
        get => _lastTransaction;
        set => SetProperty(ref _lastTransaction, value);
    }

    private decimal _lastBalance;
    public decimal LastBalance
    {
        get => _lastBalance;
        set => SetProperty(ref _lastBalance, value);
    }

    private decimal _lastPayment;
    public decimal LastPayment
    {
        get => _lastPayment;
        set => SetProperty(ref _lastPayment, value);
    }

    private string? _lastReference;
    public string LastReference
    {
        get => _lastReference!;
        set => SetProperty(ref _lastReference, value);
    }

    private string? _tag;
    public string Tag
    {
        get => _tag!;
        set => SetProperty(ref _tag, value);
    }

    public AccountSummaryModel()
    {
        Company = string.Empty;
        AccountType = string.Empty;
        AccountNumber = string.Empty;
        DueDate = string.Empty;
        LastTransaction = default;
        LastBalance = 0M;
        LastPayment = 0M;
        LastReference = string.Empty;
        Tag = string.Empty;
    }

    public AccountSummaryModel(CompanyModel company, AccountModel account, TransactionModel lastTransaction, IStringCypherService cypher, IPasswordManager manager)
    {
        Company = company.Name ?? string.Empty;
        AccountType = account.AccountType!.Description ?? string.Empty;
        AccountNumber = cypher.Decrypt(account.AccountNumber!.Number, manager.Get(), account.AccountNumber.Salt);
        DueDate = account.DueDate();
        if (lastTransaction is not null)
        {
            LastTransaction = lastTransaction.Date;
            LastBalance = lastTransaction.Balance;
            LastPayment = lastTransaction.Payment;
            LastReference = lastTransaction.Reference;
        }
        else
        {
            LastTransaction = default;
            LastBalance = 0M;
            LastPayment = 0M;
            LastReference = string.Empty;
        }
        Tag = account.Tag ?? string.Empty;
    }
}

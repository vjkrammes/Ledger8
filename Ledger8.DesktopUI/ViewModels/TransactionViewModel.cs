using Ledger8.DesktopUI.Infrastructure;

using System;

namespace Ledger8.DesktopUI.ViewModels;

public class TransactionViewModel : ViewModelBase
{
    private DateTime? _date;
    public DateTime? Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set => SetProperty(ref _balance, value);
    }

    private decimal _payment;
    public decimal Payment
    {
        get => _payment;
        set => SetProperty(ref _payment, value);
    }

    private string? _reference;
    public string Reference
    {
        get => _reference!;
        set => SetProperty(ref _reference, value);
    }

    public override bool OkCanExecute() => Balance != 0 && Payment != 0 && Payment <= Balance && Date.HasValue && Date.Value != default;

    public TransactionViewModel() => Date = DateTime.Now;
}

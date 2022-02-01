using Ledger8.DesktopUI.Infrastructure;

using System;
using System.Windows.Media;

namespace Ledger8.DesktopUI.ViewModels;

public class DateViewModel : ViewModelBase
{
    private string? _prompt;
    public string Prompt
    {
        get => _prompt!;
        set => SetProperty(ref _prompt, value);
    }

    private DateTime? _date;
    public DateTime? Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private SolidColorBrush? _borderBrush;
    public SolidColorBrush BorderBrush
    {
        get => _borderBrush!;
        set => SetProperty(ref _borderBrush, value);
    }

    public Func<DateTime?, bool>? Validator { get; set; } = null;

    public override bool OkCanExecute() => Validator is null || Validator(Date);

    public DateViewModel() => Date = DateTime.Now;
}

using Ledger8.Common;
using Ledger8.DesktopUI.Infrastructure;

using System;
using System.Windows;
using System.Windows.Media;

namespace Ledger8.DesktopUI.ViewModels;

public class QAViewModel : ViewModelBase
{
    private string? _question;
    public string? Question
    {
        get => _question!;
        set => SetProperty(ref _question, value);
    }

    private string? _answer;
    public string Answer
    {
        get => _answer!;
        set => SetProperty(ref _answer, value);
    }

    private bool _answerRequired;
    public bool AnswerRequired
    {
        get => _answerRequired;
        set => SetProperty(ref _answerRequired, value);
    }

    private SolidColorBrush? _borderBrush;
    public SolidColorBrush BorderBrush
    {
        get => _borderBrush!;
        set => SetProperty(ref _borderBrush, value);
    }

    public Func<string, bool>? Validator { get; set; }

    public override bool OkCanExecute()
    {
        if (AnswerRequired && string.IsNullOrWhiteSpace(Answer))
        {
            return false;
        }
        if (!AnswerRequired && string.IsNullOrWhiteSpace(Answer) && Validator is null)
        {
            return true;
        }
        if (Validator is null)
        {
            return true;
        }
        return Validator(Answer);
    }

    public QAViewModel()
    {
        BorderBrush = Application.Current.Resources[Constants.Border] as SolidColorBrush ?? Brushes.Black;
        Validator = null;
    }
}

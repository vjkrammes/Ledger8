using Ledger8.Common;

using System.Windows.Input;

namespace Ledger8.DesktopUI.Infrastructure;

public abstract class ViewModelBase : NotifyBase
{
    private RelayCommand? _cancelCommand;
    public ICommand CancelCommand
    {
        get
        {
            if (_cancelCommand is null)
            {
                _cancelCommand = new(param => Cancel(), param => AlwaysCanExecute());
            }
            return _cancelCommand;
        }
    }

    private RelayCommand? _okCommand;
    public ICommand OkCommand
    {
        get
        {
            if (_okCommand is null)
            {
                _okCommand = new(param => OK(), param => OkCanExecute());
            }
            return _okCommand;
        }
    }

    public static bool AlwaysCanExecute() => true;

    public virtual void Cancel() => DialogResult = false;

    public virtual bool OkCanExecute() => true;

    public virtual void OK() => DialogResult = true;

    public bool? _dialogResult;
    public bool? DialogResult
    {
        get => _dialogResult;
        set => SetProperty(ref _dialogResult, value);
    }
}

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Ledger8.DesktopUI.Infrastructure;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _action;
    private readonly Predicate<object?>? _predicate;

    public RelayCommand(Action<object?> action) : this(action, null) { }

    public RelayCommand(Action<object?> action, Predicate<object?>? predicate)
    {
        _action = action;
        _predicate = predicate;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    [DebuggerStepThrough]
    public bool CanExecute(object? parameter) => _predicate is null || _predicate(parameter);

    public void Execute(object? parameter) => _action(parameter);
}

public class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _action;
    private readonly Predicate<T?>? _predicate;

    public RelayCommand(Action<T?> action) : this(action, null) { }

    public RelayCommand(Action<T?> action, Predicate<T?>? predicate)
    {
        _action = action;
        _predicate = predicate;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    [DebuggerStepThrough]
    public bool CanExecute(T? parameter) => _predicate is null || _predicate(parameter);

    [DebuggerStepThrough]
    public bool CanExecute(object? parameter)
    {
        if (_predicate is null)
        {
            return true;
        }
        if (parameter is T parm)
        {
            return _predicate(parm);
        }
        return false;
    }

    public void Execute(T? parameter) => _action(parameter);

    public void Execute(object? parameter)
    {
        if (parameter is T parm)
        {
            _action(parm);
        }
    }
}

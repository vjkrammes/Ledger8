using System.Windows.Input;

namespace Ledger8.Common;

public class MenuInfo : NotifyBase
{
    private string? _header;
    public string? Header
    {
        get => _header;
        set => SetProperty(ref _header, value);
    }

    private object? _tag;
    public object? Tag
    {
        get => _tag;
        set => SetProperty(ref _tag, value);
    }

    private string? _icon;
    public string? Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    private ICommand? _command;
    public ICommand? Command
    {
        get => _command;
        set => SetProperty(ref _command, value);
    }

    private object? _commandParameter;
    public object? CommandParameter
    {
        get => _commandParameter;
        set => SetProperty(ref _commandParameter, value);
    }

    public override string ToString() => Header ?? "Unknown";
}

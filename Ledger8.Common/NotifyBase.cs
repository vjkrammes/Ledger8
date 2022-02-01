using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ledger8.Common;

public abstract class NotifyBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? property = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string property = "")
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }
        storage = value;
        OnPropertyChanged(property);
        return true;
    }
}

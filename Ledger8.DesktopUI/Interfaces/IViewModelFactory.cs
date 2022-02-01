using Ledger8.DesktopUI.Infrastructure;

namespace Ledger8.DesktopUI.Interfaces;

public interface IViewModelFactory
{
    T? Create<T>() where T : ViewModelBase;
}
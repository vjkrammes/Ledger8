using Ledger8.Services.Interfaces;

namespace Ledger8.DesktopUI.Interfaces;

public interface IServiceFactory
{
    T? Create<T>() where T : class, IDataServiceTag;
}

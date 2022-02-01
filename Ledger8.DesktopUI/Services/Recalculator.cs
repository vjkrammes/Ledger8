using Ledger8.DesktopUI.Interfaces;
using Ledger8.Services.Interfaces;

namespace Ledger8.DesktopUI.Services;

public class Recalculator : IRecalculator
{
    private readonly IServiceFactory _serviceFactory;

    public Recalculator(IServiceFactory serviceFactory) => _serviceFactory = serviceFactory;

    public void Recalculate()
    {
        var poolService = _serviceFactory.Create<IPoolService>()!;
        if (poolService is not null)
        {
            poolService.Recalculate();
        }
    }
}

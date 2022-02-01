using Ledger8.DesktopUI.Interfaces;
using Ledger8.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

namespace Ledger8.DesktopUI.Infrastructure;

public class ServiceFactory : IServiceFactory
{
    private readonly IServiceProvider? _serviceProvider;

    public ServiceFactory()
    {
        var app = Application.Current as App;
        _serviceProvider = app?.ServiceProvider;
    }

    public T? Create<T>() where T : class, IDataServiceTag => _serviceProvider?.GetRequiredService<T>();
}

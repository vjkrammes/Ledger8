using Ledger8.DesktopUI.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

namespace Ledger8.DesktopUI.Infrastructure;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IServiceProvider? _serviceProvider;
    public ViewModelFactory()
    {
        var app = Application.Current as App;
        _serviceProvider = app?.ServiceProvider;
    }
    public T? Create<T>() where T : ViewModelBase => _serviceProvider?.GetRequiredService<T>();
}

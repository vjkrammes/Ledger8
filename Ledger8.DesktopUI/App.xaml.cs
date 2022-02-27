using Ledger8.Common;
using Ledger8.Common.Interfaces;
using Ledger8.DataAccess;
using Ledger8.DataAccess.EFCore;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Services;
using Ledger8.DesktopUI.ViewModels;
using Ledger8.DesktopUI.Views;
using Ledger8.Services;
using Ledger8.Services.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.Windows;

namespace Ledger8.DesktopUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; }

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        services.AddSingleton(x => x);
        ServiceProvider = services.BuildServiceProvider();
        UpdateDatabase();
    }

    private void UpdateDatabase()
    {
        var context = ServiceProvider.GetRequiredService<LedgerContext>();
        if (context is not null)
        {
            try
            {
                context.Database.Migrate();
                if (!context.Settings.Any())
                {
                    context.Settings.Add(SettingsEntity.Default);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                PopupManager.Popup(ex.Innermost(), "Migration Failed");
                Environment.Exit(Constants.MigrationFailed);
            }
        }
    }

    private void ApplicationStartup(object sender, StartupEventArgs e)
    {
        var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
        var mainWindow = new MainWindow
        {
            DataContext = mainViewModel
        };
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<LedgerContext>(ServiceLifetime.Transient);

        // miscellaneous services

        services.AddTransient<IColorService, ColorService>();
        services.AddTransient<IConfigurationFactory, ConfigurationFactory>();
        services.AddTransient<IHasher, Hasher>();
        services.AddTransient<IPasswordChecker, PasswordChecker>();
        services.AddSingleton<IPasswordManager, PasswordManager>();
        services.AddTransient<IRecalculator, Recalculator>();
        services.AddTransient<ISalter, Salter>();
        services.AddSingleton<IServiceFactory, ServiceFactory>();
        services.AddTransient<ISettings, Settings>();
        services.AddTransient<IStringCypherService, StringCypherService>();
        services.AddTransient<IThemeService, ThemeService>();
        services.AddTransient<ITimeSpanConverter, TimeSpanConverter>();
        services.AddSingleton<IViewModelFactory, ViewModelFactory>();

        // data access services

        services.AddTransient<IAccountDal, AccountDal>();
        services.AddTransient<IAccountNumberDal, AccountNumberDal>();
        services.AddTransient<IAccountTypeDal, AccountTypeDal>();
        services.AddTransient<IAllotmentDal, AllotmentDal>();
        services.AddTransient<ICompanyDal, CompanyDal>();
        services.AddTransient<IIdentityDal, IdentityDal>();
        services.AddTransient<IPoolDal, PoolDal>();
        services.AddTransient<ISettingsDal, SettingsDal>();
        services.AddTransient<ITransactionDal, TransactionDal>();

        // DTO services

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IAccountNumberService, AccountNumberService>();
        services.AddTransient<IAccountTypeService, AccountTypeService>();
        services.AddTransient<IAllotmentService, AllotmentService>();
        services.AddTransient<ICompanyService, CompanyService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IPoolService, PoolService>();
        services.AddTransient<ISettingsService, SettingsService>();
        services.AddTransient<ITransactionService, TransactionService>();

        // view models

        services.AddTransient<AboutViewModel>();
        services.AddTransient<AccountSummaryViewModel>();
        services.AddTransient<AccountTypeViewModel>();
        services.AddTransient<AccountViewModel>();
        services.AddTransient<AllotmentViewModel>();
        services.AddTransient<BackupViewModel>();
        services.AddTransient<CompanyViewModel>();
        services.AddTransient<DateViewModel>();
        services.AddTransient<GettingStartedViewModel>();
        services.AddTransient<IdentityViewModel>();
        services.AddTransient<ImportAccountTypeViewModel>();
        services.AddSingleton<MainViewModel>();
        services.AddTransient<PasswordViewModel>();
        services.AddTransient<PoolViewModel>();
        services.AddTransient<PopupViewModel>();
        services.AddTransient<QAViewModel>();
        services.AddTransient<TransactionViewModel>();
    }
}

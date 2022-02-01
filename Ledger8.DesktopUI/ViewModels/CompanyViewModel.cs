using Ledger8.DesktopUI.Infrastructure;
using Ledger8.Models;

using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public class CompanyViewModel : ViewModelBase
{
    private CompanyModel? _company;
    public CompanyModel Company
    {
        get => _company!;
        set => SetProperty(ref _company, value);
    }

    private RelayCommand? _windowLoadedCommand;
    public ICommand WindowLoadedCommand
    {
        get
        {
            if (_windowLoadedCommand is null)
            {
                _windowLoadedCommand = new(parm => WindowLoaded(), parm => AlwaysCanExecute());
            }
            return _windowLoadedCommand;
        }
    }

    public override bool OkCanExecute() => !string.IsNullOrWhiteSpace(Company?.Name);

    private void WindowLoaded()
    {
        if (Company is null)
        {
            Company = new();
        }
    }
}

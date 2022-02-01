using Ledger8.DesktopUI.Infrastructure;

using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public partial class MainViewModel
{
    private RelayCommand? _manageAccountTypesCommand;
    public ICommand ManageAccountTypesCommand
    {
        get
        {
            if (_manageAccountTypesCommand is null)
            {
                _manageAccountTypesCommand = new(parm => ManageAccountTypesClick(), parm => AlwaysCanExecute());
            }
            return _manageAccountTypesCommand;
        }
    }

    private RelayCommand? _managePoolsCommand;
    public ICommand ManagePoolsCommand
    {
        get
        {
            if (_managePoolsCommand is null)
            {
                _managePoolsCommand = new(parm => ManagePoolsClick(), parm => AlwaysCanExecute());
            }
            return _managePoolsCommand;
        }
    }

    private RelayCommand? _cleanUpOrphanedAccountNumbersCommand;
    public ICommand CleanUpOrphanedAccountNumbersCommand
    {
        get
        {
            if (_cleanUpOrphanedAccountNumbersCommand is null)
            {
                _cleanUpOrphanedAccountNumbersCommand =
                    new(parm => CleanUpOrphanedAccountNumbersClick(), parm => CleanUpOrphanedAccountNumbersCanClick());
            }
            return _cleanUpOrphanedAccountNumbersCommand;
        }
    }

    private RelayCommand? _addCompanyCommand;
    public ICommand AddCompanyCommand
    {
        get
        {
            if (_addCompanyCommand is null)
            {
                _addCompanyCommand = new(parm => AddCompanyClick(), parm => AlwaysCanExecute());
            }
            return _addCompanyCommand;
        }
    }

    private RelayCommand? _editCompanyCommand;
    public ICommand EditCompanyCommand
    {
        get
        {
            if (_editCompanyCommand is null)
            {
                _editCompanyCommand = new(parm => EditCompanyClick(), parm => CompanySelected());
            }
            return _editCompanyCommand;
        }
    }

    private RelayCommand? _deleteCompanyCommand;
    public ICommand DeleteCompanyCommand
    {
        get
        {
            if (_deleteCompanyCommand is null)
            {
                _deleteCompanyCommand = new(parm => DeleteCompanyClick(), parm => DeleteCompanyCanClick());
            }
            return _deleteCompanyCommand;
        }
    }

    private RelayCommand? _addAccountCommand;
    public ICommand AddAccountCommand
    {
        get
        {
            if (_addAccountCommand is null)
            {
                _addAccountCommand = new(parm => AddAccountClick(), parm => CompanySelected());
            }
            return _addAccountCommand;
        }
    }

    private RelayCommand? _editAccountCommand;
    public ICommand EditAccountCommand
    {
        get
        {
            if (_editAccountCommand is null)
            {
                _editAccountCommand = new(parm => EditAccountClick(), parm => AccountSelected());
            }
            return _editAccountCommand;
        }
    }

    private RelayCommand? _accountSummaryCommand;
    public ICommand AccountSummaryCommand
    {
        get
        {
            if (_accountSummaryCommand is null)
            {
                _accountSummaryCommand = new(parm => AccountSummaryClick(), parm => AccountsExist());
            }
            return _accountSummaryCommand;
        }
    }

    private RelayCommand? _toggleAccountCommand;
    public ICommand ToggleAccountCommmand
    {
        get
        {
            if (_toggleAccountCommand is null)
            {
                _toggleAccountCommand = new(parm => ToggleAccountClick(), parm => AlwaysCanExecute());
            }
            return _toggleAccountCommand;
        }
    }

    private RelayCommand? _deleteAccountCommand;
    public ICommand DeleteAccountCommand
    {
        get
        {
            if (_deleteAccountCommand is null)
            {
                _deleteAccountCommand = new(parm => DeleteAccountClick(), parm => DeleteAccountCanClick());
            }
            return _deleteAccountCommand;
        }
    }

    private RelayCommand? _addTransactionCommand;
    public ICommand AddTransactionCommand
    {
        get
        {
            if (_addTransactionCommand is null)
            {
                _addTransactionCommand = new(parm => AddTransactionClick(), parm => AddTransactionCanClick());
            }
            return _addTransactionCommand;
        }
    }

    private RelayCommand? _changeDateCommand;
    public ICommand ChangeDateCommand
    {
        get
        {
            if (_changeDateCommand is null)
            {
                _changeDateCommand = new(parm => ChangeDateClick(), parm => TransactionSelected());
            }
            return _changeDateCommand;
        }
    }

    private RelayCommand? _changeBalanceCommand;
    public ICommand ChangeBalanceCommand
    {
        get
        {
            if (_changeBalanceCommand is null)
            {
                _changeBalanceCommand = new(parm => ChangeBalanceClick(), parm => TransactionSelected());
            }
            return _changeBalanceCommand;
        }
    }

    private RelayCommand? _changePaymentCommand;
    public ICommand ChangePaymentCommand
    {
        get
        {
            if (_changePaymentCommand is null)
            {
                _changePaymentCommand = new(parm => ChangePaymentClick(), parm => TransactionSelected());
            }
            return _changePaymentCommand;
        }
    }

    private RelayCommand? _changeReferenceCommand;
    public ICommand ChangeReferenceCommand
    {
        get
        {
            if (_changeReferenceCommand is null)
            {
                _changeReferenceCommand = new(parm => ChangeReferenceClick(), parm => TransactionSelected());
            }
            return _changeReferenceCommand;
        }
    }

    private RelayCommand? _deselectTransactionCommand;
    public ICommand DeselectTransactionCommand
    {
        get
        {
            if (_deselectTransactionCommand is null)
            {
                _deselectTransactionCommand = new(parm => DeselectTransactionClick(), parm => TransactionSelected());
            }
            return _deselectTransactionCommand;
        }
    }

    private RelayCommand? _deleteTransactionCommand;
    public ICommand DeleteTransactionCommand
    {
        get
        {
            if (_deleteTransactionCommand is null)
            {
                _deleteTransactionCommand = new(parm => DeleteTransactionClick(), parm => TransactionSelected());
            }
            return _deleteTransactionCommand;
        }
    }

    private RelayCommand? _copyURLCommand;
    public ICommand CopyURLCommand
    {
        get
        {
            if (_copyURLCommand is null)
            {
                _copyURLCommand = new(parm => CopyURLClick(), parm => IdentitySelected());
            }
            return _copyURLCommand;
        }
    }

    private RelayCommand? _copyPasswordCommand;
    public ICommand CopyPasswordCommand
    {
        get
        {
            if (_copyPasswordCommand is null)
            {
                _copyPasswordCommand = new(parm => CopyPasswordClick(), parm => CopyPasswordCanClick());
            }
            return _copyPasswordCommand;
        }
    }

    private RelayCommand? _clearClipboardCommand;
    public ICommand ClearClipboardCommand
    {
        get
        {
            if (_clearClipboardCommand is null)
            {
                _clearClipboardCommand = new(parm => ClearClipboardClick(), parm => AlwaysCanExecute());
            }
            return _clearClipboardCommand;
        }
    }

    private RelayCommand? _addIdentityCommand;
    public ICommand AddIdentityCommand
    {
        get
        {
            if (_addIdentityCommand is null)
            {
                _addIdentityCommand = new(parm => AddIdentityClick(), parm => CompanySelected());
            }
            return _addIdentityCommand;
        }
    }

    private RelayCommand? _editIdentityCommand;
    public ICommand EditIdentityCommand
    {
        get
        {
            if (_editIdentityCommand is null)
            {
                _editIdentityCommand = new(parm => EditIdentityClick(), parm => IdentitySelected());
            }
            return _editIdentityCommand;
        }
    }

    private RelayCommand? _deselectIdentityCommand;
    public ICommand DeselectIdentityCommand
    {
        get
        {
            if (_deselectIdentityCommand is null)
            {
                _deselectIdentityCommand = new(parm => DeselectIdentityClick(), parm => IdentitySelected());
            }
            return _deselectIdentityCommand;
        }
    }

    private RelayCommand? _deleteIdentityCommand;
    public ICommand DeleteIdentityCommand
    {
        get
        {
            if (_deleteIdentityCommand is null)
            {
                _deleteIdentityCommand = new(parm => DeleteIdentityClick(), parm => IdentitySelected());
            }
            return _deleteIdentityCommand;
        }
    }

    private RelayCommand? _backupCommand;
    public ICommand BackupCommand
    {
        get
        {
            if (_backupCommand is null)
            {
                _backupCommand = new(parm => BackupClick(), parm => AlwaysCanExecute());
            }
            return _backupCommand;
        }
    }

    private RelayCommand? _gettingStartedCommand;
    public ICommand GettingStartedCommand
    {
        get
        {
            if (_gettingStartedCommand is null)
            {
                _gettingStartedCommand = new(parm => GettingStartedClick(), parm => AlwaysCanExecute());
            }
            return _gettingStartedCommand;
        }
    }

    private RelayCommand? _aboutCommand;
    public ICommand AboutCommand
    {
        get
        {
            if (_aboutCommand is null)
            {
                _aboutCommand = new(parm => AboutClick(), parm => AlwaysCanExecute());
            }
            return _aboutCommand;
        }
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
}

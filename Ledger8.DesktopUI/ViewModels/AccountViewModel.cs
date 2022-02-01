using Ledger8.Common;
using Ledger8.Common.Enumerations;
using Ledger8.Common.Interfaces;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Views;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ledger8.DesktopUI.ViewModels;

public class AccountViewModel : ViewModelBase
{
    private CompanyModel? _company;
    public CompanyModel Company
    {
        get => _company!;
        set
        {
            SetProperty(ref _company, value);
            IsPayable = Company is null || Company.IsPayee;
        }
    }

    private ObservableCollection<AccountTypeModel>? _accountTypes;
    public ObservableCollection<AccountTypeModel> AccountTypes
    {
        get => _accountTypes!;
        set => SetProperty(ref _accountTypes, value);
    }

    private AccountTypeModel? _selectedAccountType;
    public AccountTypeModel SelectedAccountType
    {
        get => _selectedAccountType!;
        set => SetProperty(ref _selectedAccountType, value);
    }

    private ObservableCollection<DueDateType>? _dueDateTypes;
    public ObservableCollection<DueDateType> DueDateTypes
    {
        get => _dueDateTypes!;
        set => SetProperty(ref _dueDateTypes, value);
    }

    private DueDateType _selectedDueDateType;
    public DueDateType SelectedDueDateType
    {
        get => _selectedDueDateType;
        set => SetProperty(ref _selectedDueDateType, value);
    }

    private string? _number;
    public string Number
    {
        get => _number!;
        set => SetProperty(ref _number, value);
    }

    private int _month;
    public int Month
    {
        get => _month;
        set => SetProperty(ref _month, value);
    }

    private int _day;
    public int Day
    {
        get => _day;
        set => SetProperty(ref _day, value);
    }

    private bool _isPayable;
    public bool IsPayable
    {
        get => _isPayable;
        set
        {
            SetProperty(ref _isPayable, value);
            if (IsPayable)
            {
                SelectedDueDateType = Account?.DueDateType ?? DueDateType.Unspecified;
                Month = Account?.Month ?? 0;
                Day = Account?.Day ?? 0;
            }
            else
            {
                SelectedDueDateType = DueDateType.NA;
                Month = 0;
                Day = 0;
            }
        }
    }

    private string? _comments;
    public string Comments
    {
        get => _comments!;
        set => SetProperty(ref _comments, value);
    }

    private string? _tag;
    public string Tag
    {
        get => _tag!;
        set => SetProperty(ref _tag, value);
    }

    private AccountModel? _account;
    public AccountModel Account
    {
        get => _account!;
        set
        {
            SetProperty(ref _account, value);
            if (Account is not null && Account.Id > 0)
            {
                SelectedAccountType = Account.AccountType!;
                SelectedDueDateType = Account.DueDateType;
                Number = _stringCypherService.Decrypt(Account!.AccountNumber!.Number, _passwordManager.Get(), Account.AccountNumber.Salt);
                Month = Account.Month;
                Day = Account.Day;
                IsPayable = Account.IsPayable;
                Comments = Account.Comments;
                Tag = Account.Tag;
                IsEditing = true;
            }
            else
            {
                SelectedAccountType = null!;
                SelectedDueDateType = DueDateType.Unspecified;
                Number = string.Empty;
                Month = 0;
                Day = 0;
                IsPayable = Company?.IsPayee ?? true;
                Comments = string.Empty;
                Tag = string.Empty;
                IsEditing = false;
            }
        }
    }

    private bool _isEditing;
    public bool IsEditing
    {
        get => _isEditing;
        set
        {
            SetProperty(ref _isEditing, value);
            NANButtonVisibility = IsEditing ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private Visibility _nanButtonVisibility;
    public Visibility NANButtonVisibility
    {
        get => _nanButtonVisibility;
        set => SetProperty(ref _nanButtonVisibility, value);
    }

    private readonly IStringCypherService _stringCypherService;
    private readonly IPasswordManager _passwordManager;
    private readonly ISalter _salter;
    private readonly IServiceFactory _serviceFactory;
    private readonly QAViewModel _qAViewModel;
    private readonly AccountTypeViewModel _accountTypeViewModel;

    private RelayCommand? _changeNumberCommand;
    public ICommand ChangeNumberCommand
    {
        get
        {
            if (_changeNumberCommand is null)
            {
                _changeNumberCommand = new(parm => ChangeNumberClick(), parm => AlwaysCanExecute());
            }
            return _changeNumberCommand;
        }
    }

    private RelayCommand? _manageTypesCommand;
    public ICommand ManageTypesCommand
    {
        get
        {
            if (_manageTypesCommand is null)
            {
                _manageTypesCommand = new(parm => ManageTypesClick(), parm => AlwaysCanExecute());
            }
            return _manageTypesCommand;
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

    public override bool OkCanExecute() => SelectedAccountType is not null && SelectedDueDateType != DueDateType.Unspecified && !string.IsNullOrWhiteSpace(Number);

    public override void OK()
    {
        switch (SelectedDueDateType)
        {
            case DueDateType.NA:
            case DueDateType.ServiceRelated:
                break;
            case DueDateType.Unspecified:
                return;
            case DueDateType.Monthly:
                if (Day == 0)
                {
                    PopupManager.Popup("Day is required for monthly accounts", "Missing Day", PopupButtons.Ok, PopupImage.Stop);
                    return;
                }
                break;
            case DueDateType.Quarterly:
            case DueDateType.SemiAnnual:
            case DueDateType.Annnually:
                if (Day == 0 || Month == 0)
                {
                    PopupManager.Popup("Month and Day are required for Quarterly, Semi-Annual and Annual accounts", "Missing Date", PopupButtons.Ok,
                        PopupImage.Stop);
                    return;
                }
                break;
        }
        base.OK();
    }

    private void ChangeNumberClick()
    {
        var vm = _qAViewModel;
        vm.Question = "New Account Number:";
        vm.Answer = _stringCypherService.Decrypt(Account.AccountNumber!.Number, _passwordManager.Get(), Account.AccountNumber.Salt);
        vm.AnswerRequired = true;
        vm.BorderBrush = (Application.Current.Resources[Constants.Border] as SolidColorBrush) ?? Brushes.Black;
        if (DialogSupport.ShowDialog<QAWindow>(vm, Application.Current.MainWindow) != true)
        {
            return;
        }
        try
        {
            var accountService = _serviceFactory.Create<IAccountService>()!;
            var newaccount = accountService.Create(Account, vm.Answer, _salter.GenerateSalt(Constants.SaltLength),
                _passwordManager.Get(), _stringCypherService);
            Account = null!;
            Account = newaccount!;
        }
        catch (Exception ex)
        {
            PopupManager.Popup("Failed to create new account number", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
        }
    }

    private void ManageTypesClick()
    {
        DialogSupport.ShowDialog<AccountTypeWindow>(_accountTypeViewModel, Application.Current.MainWindow);
        LoadAccountTypes();
    }

    private void WindowLoaded()
    {
        if (Company is null)
        {
            PopupManager.Popup("Company was not selected", "No Company", PopupButtons.Ok, PopupImage.Error);
            Cancel();
            return;
        }
    }

    private void LoadAccountTypes()
    {
        try
        {
            var accountTypeService = _serviceFactory.Create<IAccountTypeService>()!;
            AccountTypes = new(accountTypeService.Get(null, "Description", 'a'));
        }
        catch (Exception ex)
        {
            PopupManager.Popup("Failed to retrieve Account Types", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            Cancel();
            return;
        }
    }

    public AccountViewModel(IStringCypherService stringCypherService, IPasswordManager passwordManager, ISalter salter, IServiceFactory serviceFactory,
        QAViewModel qAViewModel, AccountTypeViewModel accountTypeViewModel)
    {
        _stringCypherService = stringCypherService;
        _passwordManager = passwordManager;
        _salter = salter;
        _serviceFactory = serviceFactory;
        _qAViewModel = qAViewModel;
        _accountTypeViewModel = accountTypeViewModel;
        LoadAccountTypes();
        DueDateTypes = new(Infrastructure.Tools.GetValues<DueDateType>());
        Account = new();
    }
}

using Ledger8.Common;
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

public class AccountTypeViewModel : ViewModelBase
{
    private readonly IServiceFactory _serviceFactory;
    private readonly IViewModelFactory _viewModelFactory;

    private string? _description;
    public string Description
    {
        get => _description!;
        set => SetProperty(ref _description, value);
    }

    private ObservableCollection<AccountTypeModel>? _accountTypes;
    public ObservableCollection<AccountTypeModel> AccountTypes
    {
        get => _accountTypes!;
        set => SetProperty(ref _accountTypes, value);
    }

    private AccountTypeModel? _selectedAccountType;
    public AccountTypeModel? SelectedAccountType
    {
        get => _selectedAccountType;
        set => SetProperty(ref _selectedAccountType, value);
    }

    private RelayCommand? _addCommand;
    public ICommand AddCommand
    {
        get
        {
            if (_addCommand is null)
            {
                _addCommand = new(parm => AddClick(), parm => AddCanClick());
            }
            return _addCommand;
        }
    }

    private RelayCommand? _renameCommand;
    public ICommand RenameCommand
    {
        get
        {
            if (_renameCommand is null)
            {
                _renameCommand = new(parm => RenameClick(), parm => AccountTypeSelected());
            }
            return _renameCommand;
        }
    }

    private RelayCommand? _deleteCommand;
    public ICommand DeleteCommand
    {
        get
        {
            if (_deleteCommand is null)
            {
                _deleteCommand = new(parm => DeleteClick(), parm => DeleteCanClick());
            }
            return _deleteCommand;
        }
    }

    private RelayCommand? _importCommand;
    public ICommand ImportCommand
    {
        get
        {
            if (_importCommand is null)
            {
                _importCommand = new(parm => ImportClick(), parm => AlwaysCanExecute());
            }
            return _importCommand;
        }
    }

    private bool AddCanClick() => !string.IsNullOrWhiteSpace(Description);

    private void AddClick()
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            return;
        }
        var a = new AccountTypeModel
        {
            Id = 0,
            Description = Description.Caseify(),
            CanDelete = true
        };
        var accountTypeService = _serviceFactory.Create<IAccountTypeService>()!;
        var result = accountTypeService.Insert(a);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to add new account type", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            FocusRequested?.Invoke(this, EventArgs.Empty);
            return;
        }
        var ix = 0;
        while (ix < AccountTypes.Count && AccountTypes[ix] < a)
        {
            ix++;
        }
        AccountTypes.Insert(ix, a);
        SelectedAccountType = a;
        SelectedAccountType = null;
        Description = string.Empty;
        FocusRequested?.Invoke(this, EventArgs.Empty);
    }

    private bool AccountTypeSelected() => SelectedAccountType is not null;

    private void RenameClick()
    {
        if (SelectedAccountType is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<QAViewModel>()!;
        vm.Question = "Description";
        vm.Answer = SelectedAccountType.Description;
        vm.AnswerRequired = true;
        vm.BorderBrush = (Application.Current.Resources[Constants.Border] as SolidColorBrush)!;
        if (DialogSupport.ShowDialog<QAWindow>(vm, Application.Current.MainWindow) != true)
        {
            SelectedAccountType = null;
            FocusRequested?.Invoke(this, EventArgs.Empty);
            return;
        }
        if (vm.Answer.Equals(SelectedAccountType.Description, StringComparison.OrdinalIgnoreCase))
        {
            SelectedAccountType = null;
            FocusRequested?.Invoke(this, EventArgs.Empty);
            return;
        }
        var at = SelectedAccountType.Clone();
        at.Description = vm.Answer.Caseify();
        var accountTypeService = _serviceFactory.Create<IAccountTypeService>()!;
        var result = accountTypeService.Update(at);
        if (!result.Successful)
        {
            SelectedAccountType = null;
            PopupManager.Popup("Failed to update account type", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            FocusRequested?.Invoke(this, EventArgs.Empty);
            return;
        }
        AccountTypes.Remove(SelectedAccountType);
        var ix = 0;
        while (ix < AccountTypes.Count && AccountTypes[ix] < at)
        {
            ix++;
        }
        AccountTypes.Insert(ix, at);
        SelectedAccountType = at;
        SelectedAccountType = null;
        Description = string.Empty;
        FocusRequested?.Invoke(this, EventArgs.Empty);
    }

    private bool DeleteCanClick() => SelectedAccountType is not null && SelectedAccountType.CanDelete;

    private void DeleteClick()
    {
        if (SelectedAccountType is null || !SelectedAccountType.CanDelete)
        {
            return;
        }
        var accountTypeService = _serviceFactory.Create<IAccountTypeService>()!;
        var result = accountTypeService.Delete(SelectedAccountType);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to delete account type", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        AccountTypes.Remove(SelectedAccountType);
        SelectedAccountType = null;
        FocusRequested?.Invoke(this, EventArgs.Empty);
    }

    private void ImportClick()
    {
        var vm = _viewModelFactory.Create<ImportAccountTypeViewModel>()!;
        DialogSupport.ShowDialog<ImportAccountTypeWindow>(vm, Application.Current.MainWindow);
        LoadAccountTypes(true);
    }

    private void LoadAccountTypes(bool reload = false)
    {
        try
        {
            var accountTypeService = _serviceFactory.Create<IAccountTypeService>()!;
            AccountTypes = new(accountTypeService.Get(null, "Description", 'a'));
        }
        catch (Exception ex)
        {
            PopupManager.Popup($"Failed {reload.Operation()} Account Types", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            Cancel();
            return;
        }
    }

    public event EventHandler? FocusRequested;

    public AccountTypeViewModel(IServiceFactory serviceFactory, IViewModelFactory viewModelFactory)
    {
        _serviceFactory = serviceFactory;
        _viewModelFactory = viewModelFactory;
        LoadAccountTypes(false);
    }
}

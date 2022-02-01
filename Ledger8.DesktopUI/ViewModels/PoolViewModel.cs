using Ledger8.Common;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Views;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public class PoolViewModel : ViewModelBase
{
    private string? _name;
    public string Name
    {
        get => _name!;
        set => SetProperty(ref _name, value);
    }

    private DateTime? _date;
    public DateTime? Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private string? _description;
    public string Description
    {
        get => _description!;
        set => SetProperty(ref _description, value);
    }

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }

    private ObservableCollection<PoolModel>? _pools;
    public ObservableCollection<PoolModel> Pools
    {
        get => _pools!;
        set => SetProperty(ref _pools, value);
    }

    private PoolModel? _selectedPool;
    public PoolModel? SelectedPool
    {
        get => _selectedPool!;
        set
        {
            SetProperty(ref _selectedPool, value);
            if (SelectedPool is not null)
            {
                _editing = true;
                Name = SelectedPool.Name ?? string.Empty;
                Date = SelectedPool.Date == default ? null! : SelectedPool.Date;
                Description = SelectedPool.Description ?? string.Empty;
                Amount = SelectedPool.Amount;
                FocusRequested?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private bool _editing;
    private readonly IViewModelFactory _viewModelFactory;
    private readonly IServiceFactory _serviceFactory;

    public event EventHandler? FocusRequested;
    public event EventHandler? AmountFocusRequested;

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

    private RelayCommand? _saveChangesCommand;
    public ICommand SaveChangesCommand
    {
        get
        {
            if (_saveChangesCommand is null)
            {
                _saveChangesCommand = new(parm => SaveChangesClick(), parm => SaveChangesCanClick());
            }
            return _saveChangesCommand;
        }
    }

    private RelayCommand? _cancelChangesCommand;
    public ICommand CancelChangesCommand
    {
        get
        {
            if (_cancelChangesCommand is null)
            {
                _cancelChangesCommand = new(parm => CancelChangesClick(), parm => IsEditing());
            }
            return _cancelChangesCommand;
        }
    }

    private RelayCommand? _recalculateCommand;
    public ICommand RecalculateCommand
    {
        get
        {
            if (_recalculateCommand is null)
            {
                _recalculateCommand = new(parm => RecalculateClick(), parm => PoolsExist());
            }
            return _recalculateCommand;
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

    private RelayCommand? _manageAllotmentsCommand;
    public ICommand ManageAllotmentsCommand
    {
        get
        {
            if (_manageAllotmentsCommand is null)
            {
                _manageAllotmentsCommand = new(parm => ManageAllotmentsClick(), parm => PoolSelected());
            }
            return _manageAllotmentsCommand;
        }
    }

    private bool AddCanClick() => !_editing && !string.IsNullOrWhiteSpace(Name) && Amount > 0M;

    private void AddClick()
    {
        if (string.IsNullOrWhiteSpace(Name) || Amount <= 0M)
        {
            FocusRequested?.Invoke(this, EventArgs.Empty);
            return;
        }
        var pool = new PoolModel
        {
            Id = 0,
            Name = Name.Caseify(),
            Date = Date ?? default,
            Description = Description ?? string.Empty,
            Amount = Amount,
            Balance = Amount,
            CanDelete = true
        };
        var poolService = _serviceFactory.Create<IPoolService>()!;
        var result = poolService.Insert(pool);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to add new Pool", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            FocusRequested?.Invoke(this, EventArgs.Empty);
            return;
        }
        var ix = 0;
        while (ix < Pools.Count && Pools[ix] < pool)
        {
            ix++;
        }
        Pools.Insert(ix, pool);
        SelectedPool = pool;
        SelectedPool = null;
        Clear();
    }

    private bool SaveChangesCanClick() => _editing && !string.IsNullOrWhiteSpace(Name) && Amount > 0M;

    private void SaveChangesClick()
    {
        var difference = Amount - SelectedPool!.Amount;
        if (SelectedPool!.Balance + difference < 0M && SelectedPool!.Balance >= 0M)
        {
            var msg = $"Adjusting the amount to {Amount:c2} will create a negative balance. Continue?";
            if (PopupManager.Popup("Balance will be negative", "Negative Balance", msg, PopupButtons.YesNo, PopupImage.Question) != PopupResult.Yes)
            {
                AmountFocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        var pool = SelectedPool.Clone();
        pool.Name = Name.Caseify();
        pool.Date = Date ?? default;
        pool.Amount = Amount;
        pool.Balance += difference;
        pool.Description = Description ?? string.Empty;
        var poolService = _serviceFactory.Create<IPoolService>()!;
        var result = poolService.Update(pool);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update Pool", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            FocusRequested?.Invoke(this, EventArgs.Empty);
            return;
        }
        Pools.Remove(SelectedPool);
        SelectedPool = null;
        var ix = 0;
        while (ix < Pools.Count && Pools[ix] < pool)
        {
            ix++;
        }
        Pools.Insert(ix, pool);
        SelectedPool = pool;
        SelectedPool = null;
        Clear();
    }

    private bool IsEditing() => _editing;

    private void CancelChangesClick() => Clear();

    private bool PoolsExist() => Pools.Any();

    private void RecalculateClick()
    {
        var recalculator = (Application.Current as App)?.ServiceProvider?.GetRequiredService<IRecalculator>();
        if (recalculator is not null)
        {
            try
            {
                recalculator.Recalculate();
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Recalculation Failed", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            }
        }
        LoadPools(true);
        Clear();
    }

    private bool DeleteCanClick() => SelectedPool is not null && SelectedPool.CanDelete;

    private void DeleteClick()
    {
        if (SelectedPool is null || !SelectedPool.CanDelete)
        {
            return;
        }
        var poolService = _serviceFactory.Create<IPoolService>()!;
        var result = poolService.Delete(SelectedPool);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to delete Pool", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedPool = null;
            return;
        }
        Pools.Remove(SelectedPool);
        Clear();
    }

    private bool PoolSelected() => SelectedPool is not null;

    private void ManageAllotmentsClick()
    {
        if (SelectedPool is null)
        {
            return;
        }
        var vm = _viewModelFactory.Create<AllotmentViewModel>()!;
        vm.Pool = SelectedPool!;
        DialogSupport.ShowDialog<AllotmentWindow>(vm, Application.Current.MainWindow);
        LoadPools(true);
        Clear();
    }

    private void LoadPools(bool reload = false)
    {
        var poolService = _serviceFactory.Create<IPoolService>()!;
        try
        {
            Pools = new(poolService.Get(null, "Name", 'a'));
        }
        catch (Exception ex)
        {
            PopupManager.Popup($"Failed {reload.Operation()} Pools", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            Cancel();
            return;
        }
    }

    private void Clear()
    {
        Name = string.Empty;
        Date = DateTime.Now;
        Description = string.Empty;
        Amount = 0M;
        SelectedPool = null;
        _editing = false;
        FocusRequested?.Invoke(this, EventArgs.Empty);
    }

    public PoolViewModel(IViewModelFactory viewModelFactory, IServiceFactory serviceFactory)
    {
        _viewModelFactory = viewModelFactory;
        _serviceFactory = serviceFactory;
        LoadPools();
        Date = DateTime.Now;
    }
}

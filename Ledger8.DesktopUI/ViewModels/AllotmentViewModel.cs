using Ledger8.Common;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public class AllotmentViewModel : ViewModelBase
{
    private PoolModel? _pool;
    public PoolModel Pool
    {
        get => _pool!;
        set => SetProperty(ref _pool, value);
    }

    private ObservableCollection<CompanyModel>? _companies;
    public ObservableCollection<CompanyModel> Companies
    {
        get => _companies!;
        set => SetProperty(ref _companies, value);
    }

    private CompanyModel? _selectedCompany;
    public CompanyModel SelectedCompany
    {
        get => _selectedCompany!;
        set => SetProperty(ref _selectedCompany, value);
    }

    private DateTime? _date;
    public DateTime? Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private string? _description;
    public string? Description
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

    private ObservableCollection<AllotmentModel>? _allotments;
    public ObservableCollection<AllotmentModel> Allotments
    {
        get => _allotments!;
        set => SetProperty(ref _allotments, value);
    }

    private AllotmentModel? _selectedAllotment;
    public AllotmentModel SelectedAllotment
    {
        get => _selectedAllotment!;
        set
        {
            SetProperty(ref _selectedAllotment, value);
            if (SelectedAllotment is not null)
            {
                SelectedCompany = Companies.SingleOrDefault(x => x.Id == SelectedAllotment.CompanyId)!;
                Date = SelectedAllotment.Date;
                Description = SelectedAllotment.Description ?? string.Empty;
                Amount = SelectedAllotment.Amount;
                _editing = true;
            }
            else
            {
                Clear();
            }
        }
    }

    private bool _editing = false;
    private readonly IServiceFactory _serviceFactory;

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

    private RelayCommand? _deleteCommand;
    public ICommand DeleteCommand
    {
        get
        {
            if (_deleteCommand is null)
            {
                _deleteCommand = new(parm => DeleteClick(), parm => AllotmentSelected());
            }
            return _deleteCommand;
        }
    }

    private RelayCommand? _deleteAllCommand;
    public ICommand DeleteAllCommand
    {
        get
        {
            if (_deleteAllCommand is null)
            {
                _deleteAllCommand = new(parm => DeleteAllClick(), parm => AllotmentsExist());
            }
            return _deleteAllCommand;
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

    private bool AddCanClick() => SelectedCompany is not null && Amount > 0M;

    private void AddClick()
    {
        if (SelectedCompany is null || Amount <= 0M)
        {
            return;
        }
        if (Pool.Balance - Amount < 0M)
        {
            var msg = "Adding this allotment will reduce the balance of the pool to below zero. Continue?";
            if (PopupManager.Popup("Continue with negative balance?", "Negative Balance", msg, PopupButtons.YesNo, PopupImage.Question) != PopupResult.Yes)
            {
                return;
            }
        }
        var allotment = new AllotmentModel
        {
            Id = 0,
            PoolId = Pool.Id,
            CompanyId = SelectedCompany.Id,
            Company = SelectedCompany,
            Date = Date ?? default,
            Amount = Amount,
            Description = Description ?? string.Empty
        };
        var poolService = _serviceFactory.Create<IPoolService>()!;
        var result = poolService.AddAllotment(Pool.Id, allotment);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to create new Allotment", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        var ix = 0;
        while (ix < Allotments.Count && Allotments[ix] > allotment)
        {
            ix++;
        }
        Allotments.Insert(ix, allotment);
        SelectedAllotment = allotment;
        SelectedAllotment = null!;
        Clear();
        ReloadPool();
    }

    private bool SaveChangesCanClick() => _editing && SelectedCompany is not null && Amount > 0M;

    private void SaveChangesClick()
    {
        if (SelectedAllotment is null || SelectedCompany is null || Amount <= 0M)
        {
            return;
        }
        var allotment = SelectedAllotment.Clone();
        allotment.CompanyId = SelectedCompany.Id;
        allotment.Company = SelectedCompany;
        allotment.Date = Date ?? default;
        allotment.Amount = Amount;
        allotment.Description = Description ?? string.Empty;
        var poolService = _serviceFactory.Create<IPoolService>()!;
        var result = poolService.UpdateAllotment(allotment);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update Allotment", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedAllotment = null!;
            Clear();
            return;
        }
        Allotments.Remove(SelectedAllotment);
        SelectedAllotment = null!;
        var ix = 0;
        while (ix < Allotments.Count && Allotments[ix] > allotment)
        {
            ix++;
        }
        Allotments.Insert(ix, allotment);
        SelectedAllotment = allotment;
        SelectedAllotment = null!;
        Clear();
        ReloadPool();
    }

    private bool IsEditing() => _editing;

    private void CancelChangesClick() => Clear();

    private bool AllotmentSelected() => SelectedAllotment is not null;

    private void DeleteClick()
    {
        if (SelectedAllotment is null)
        {
            return;
        }
        var poolService = _serviceFactory.Create<IPoolService>()!;
        var result = poolService.RemoveAllotment(SelectedAllotment);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to delete Allotment", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedAllotment = null!;
            Clear();
            return;
        }
        Allotments.Remove(SelectedAllotment);
        SelectedAllotment = null!;
        Clear();
        ReloadPool();
    }

    private bool AllotmentsExist() => Allotments is not null && Allotments.Any();

    private void DeleteAllClick()
    {
        if (Pool is null || Pool.Id <= 0)
        {
            return;
        }
        var poolService = _serviceFactory.Create<IPoolService>()!;
        var result = poolService.RemoveAllAllotments(Pool.Id);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to delete all Allotments", Constants.DBE, result.ErrorMessage(), PopupButtons.Ok, PopupImage.Error);
            SelectedAllotment = null!;
            Clear();
            return;
        }
        SelectedAllotment = null!;
        Clear();
        LoadAllotments();
        ReloadPool();
    }

    private void WindowLoaded()
    {
        if (Pool is null)
        {
            PopupManager.Popup("No Pool was selected", "Application Error", PopupButtons.Ok, PopupImage.Stop);
            Cancel();
            return;
        }
        LoadCompanies();
        LoadAllotments();
    }

    private void LoadCompanies()
    {
        try
        {
            var companyService = _serviceFactory.Create<ICompanyService>()!;
            Companies = new(companyService.Get(null, "Name", 'a'));
        }
        catch (Exception ex)
        {
            PopupManager.Popup("Failed to load Companies", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            Cancel();
            return;
        }
    }

    private void LoadAllotments()
    {
        try
        {
            var allotmentService = _serviceFactory.Create<IAllotmentService>()!;
            Allotments = new(allotmentService.Get(null, "Date", 'd'));
        }
        catch (Exception ex)
        {
            PopupManager.Popup("Failed to load Allotments", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            Cancel();
            return;
        }
    }

    private void ReloadPool()
    {
        if (Pool is not null)
        {
            var poolService = _serviceFactory.Create<IPoolService>()!;
            var id = Pool.Id;
            Pool = null!;
            Pool = poolService?.Read(id)!;
        }
    }

    private void Clear()
    {
        SelectedCompany = null!;
        Date = DateTime.Now;
        Description = string.Empty;
        Amount = 0M;
        _editing = false;
    }

    public AllotmentViewModel(IServiceFactory serviceFactory)
    {
        Date = DateTime.Now;
        _serviceFactory = serviceFactory;
    }
}

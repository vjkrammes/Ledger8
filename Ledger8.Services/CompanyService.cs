using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyDal _companyDal;
    private readonly IAccountDal _accountDal;
    private readonly IAllotmentDal _allotmentDal;
    private readonly IIdentityDal _identityDal;

    public CompanyService(ICompanyDal companyDal, IAccountDal accountDal, IAllotmentDal allotmentDal, IIdentityDal identityDal)
    {
        _companyDal = companyDal;
        _accountDal = accountDal;
        _allotmentDal = allotmentDal;
        _identityDal = identityDal;
    }

    public int Count => _companyDal.Count;

    private bool CompanyCanBeDeleted(int companyId) =>
        !(_accountDal.CompanyHasAccounts(companyId) || _allotmentDal.CompanyHasAllotments(companyId) || _identityDal.CompanyHasIdentities(companyId));

    private ApiError ValidateModel(CompanyModel model, bool checkid = false, bool update = false)
    {
        if (model is null || string.IsNullOrWhiteSpace(model.Name))
        {
            return new(Strings.InvalidModel);
        }
        if (model.Id < 0)
        {
            model.Id = 0;
        }
        if (checkid && model.Id <= 0)
        {
            return new(string.Format(Strings.Invalid, "id"));
        }
        var existing = _companyDal.Read(model.Name);
        if (update)
        {
            if (existing is not null && existing.Id != model.Id)
            {
                return new(string.Format(Strings.DuplicateA, "company", "name", model.Name));
            }
        }
        else if (existing is not null)
        {
            return new(string.Format(Strings.DuplicateA, "company", "name", model.Name));
        }
        return ApiError.Success;
    }

    public ApiError Insert(CompanyModel model)
    {
        var checkresult = ValidateModel(model);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        CompanyEntity entity = model!;
        try
        {
            var result = _companyDal.Insert(entity);
            if (result.Successful)
            {
                model.Id = entity.Id;
            }
            return ApiError.FromDalResult(result);
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Update(CompanyModel model)
    {
        var checkresult = ValidateModel(model, true, true);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        CompanyEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_companyDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Delete(CompanyModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        if (!CompanyCanBeDeleted(model.Id))
        {
            return new(string.Format(Strings.CantDelete, "company", "items"));
        }
        try
        {
            return ApiError.FromDalResult(_companyDal.Delete(model.Id));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public IEnumerable<CompanyModel> Get(Expression<Func<CompanyEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = _companyDal.Get(pred, order, direction);
        var models = entities.ToModels<CompanyModel, CompanyEntity>();
        models.ForEach(x => x.CanDelete = CompanyCanBeDeleted(x.Id));
        return models;
    }

    public IEnumerable<CompanyModel> GetPayees() => Get(x => x.IsPayee);

    public CompanyModel? Read(int id) => _companyDal.Read(id)!;

    public CompanyModel? Read(string name) => _companyDal.Read(name)!;
}

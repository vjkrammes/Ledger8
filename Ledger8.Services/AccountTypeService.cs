using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.Services;

public class AccountTypeService : IAccountTypeService
{
    private readonly IAccountTypeDal _accountTypeDal;
    private readonly IAccountDal _accountDal;

    public AccountTypeService(IAccountTypeDal accountTypeDal, IAccountDal accountDal)
    {
        _accountTypeDal = accountTypeDal;
        _accountDal = accountDal;
    }

    public int Count => _accountTypeDal.Count;

    private ApiError ValidateModel(AccountTypeModel model, bool checkid = false, bool update = false)
    {
        if (model is null || string.IsNullOrWhiteSpace(model.Description))
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
        var existing = _accountTypeDal.Read(model.Description);
        if (update)
        {
            if (existing is not null && existing.Id != model.Id)
            {
                return new(string.Format(Strings.DuplicateAn, "account type", "description", model.Description));
            }
        }
        else
        {
            if (existing is not null)
            {
                return new(string.Format(Strings.DuplicateAn, "account type", "description", model.Description));
            }
        }
        return ApiError.Success;
    }

    public ApiError Insert(AccountTypeModel model)
    {
        var checkresult = ValidateModel(model);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        AccountTypeEntity entity = model!;
        try
        {
            var result = _accountTypeDal.Insert(entity);
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

    public ApiError Update(AccountTypeModel model)
    {
        var checkresult = ValidateModel(model, true, true);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        AccountTypeEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_accountTypeDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Delete(AccountTypeModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        if (_accountDal.AccountTypeHasAccounts(model.Id))
        {
            return new(string.Format(Strings.CantDelete, "account type", "accounts"));
        }
        try
        {
            return ApiError.FromDalResult(_accountTypeDal.Delete(model.Id));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public IEnumerable<AccountTypeModel> Get(Expression<Func<AccountTypeEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = _accountTypeDal.Get(pred, order, direction);
        var models = entities.ToModels<AccountTypeModel, AccountTypeEntity>();
        models.ForEach(x => x.CanDelete = !_accountDal.AccountTypeHasAccounts(x.Id));
        return models;
    }

    public AccountTypeModel Read(int id) => _accountTypeDal.Read(id)!;

    public AccountTypeModel Read(string description) => _accountTypeDal.Read(description)!;
}

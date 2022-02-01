using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.Services;

public class AccountNumberService : IAccountNumberService
{
    private readonly IAccountNumberDal _accountNumberDal;

    public AccountNumberService(IAccountNumberDal accountNumberDal) => _accountNumberDal = accountNumberDal;

    public int Count => _accountNumberDal.Count;

    private static ApiError ValidateModel(AccountNumberModel model, bool checkid = false)
    {
        if (model is null || model.AccountId <= 0 || string.IsNullOrWhiteSpace(model.Number))
        {
            return new(Strings.InvalidModel);
        }
        if (model.Salt is null || model.Salt.Length != Constants.SaltLength)
        {
            return new(Strings.InvalidSalt);
        }
        if (model.Id < 0)
        {
            model.Id = 0;
        }
        if (checkid && model.Id <= 0)
        {
            return new(string.Format(Strings.Invalid, "id"));
        }
        return ApiError.Success;
    }

    public ApiError Insert(AccountNumberModel model)
    {
        var checkresult = ValidateModel(model);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        AccountNumberEntity entity = model!;
        try
        {
            var result = _accountNumberDal.Insert(entity);
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

    public ApiError Update(AccountNumberModel model)
    {
        var checkresult = ValidateModel(model, true);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        AccountNumberEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_accountNumberDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Delete(AccountNumberModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        try
        {
            return ApiError.FromDalResult(_accountNumberDal.Delete(model.Id));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError DeleteForAccount(int accountId)
    {
        try
        {
            _accountNumberDal.DeleteForAccount(accountId);
            return ApiError.Success;
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public IEnumerable<AccountNumberModel> Get(Expression<Func<AccountNumberEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = _accountNumberDal.Get(pred, order, direction);
        var models = entities.ToModels<AccountNumberModel, AccountNumberEntity>();
        models.ForEach(x => x.CanDelete = true);
        return models;
    }

    public IEnumerable<AccountNumberModel> GetForAccount(int accountId) => Get(x => x.AccountId == accountId);

    public AccountNumberModel? Read(int id) => _accountNumberDal.Read(id)!;

    public AccountNumberModel? Current(int accountId) => _accountNumberDal.Current(accountId)!;

    public bool AccountHasAccountNumbers(int accountId) => _accountNumberDal.AccountHasAccountNumbers(accountId);
}

using Ledger8.Common;
using Ledger8.Common.Enumerations;
using Ledger8.Common.Interfaces;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.Services;

public class AccountService : IAccountService
{
    private readonly IAccountDal _accountDal;
    private readonly ITransactionDal _transactionDal;

    public AccountService(IAccountDal accountDal, ITransactionDal transactionDal)
    {
        _accountDal = accountDal;
        _transactionDal = transactionDal;
    }

    public int Count => _accountDal.Count;

    private ApiError ValidateModel(AccountModel model, bool checkid = false, bool update = false)
    {
        if (model is null || model.CompanyId <= 0 || model.AccountTypeId <= 0 || model.DueDateType == DueDateType.Unspecified)
        {
            return new(Strings.InvalidModel);
        }
        switch (model.DueDateType)
        {
            case DueDateType.Monthly:
                if (model.Day == 0)
                {
                    return new(Strings.InvalidDueMonth);
                }
                model.Month = 0;
                break;
            case DueDateType.Quarterly:
            case DueDateType.SemiAnnual:
            case DueDateType.Annnually:
                if (model.Day == 0 || model.Month == 0)
                {
                    return new(Strings.InvalidDueQuarterlySemiAnnual);
                }
                break;
            case DueDateType.NA:
            case DueDateType.ServiceRelated:
                model.Month = 0;
                model.Day = 0;
                break;
        }
        if (model.Id < 0)
        {
            model.Id = 0;
        }
        if (checkid && model.Id <= 0)
        {
            return new(string.Format(Strings.Invalid, "id"));
        }
        if (!string.IsNullOrWhiteSpace(model.Tag))
        {
            var existing = _accountDal.Read(model.CompanyId, model.Tag);
            if (update || model.Id != 0)
            {
                if (existing is not null && model.Id != existing.Id)
                {
                    return new(string.Format(Strings.DuplicateAn, "account", "tag", model.Tag));
                }
            }
            else if (existing is not null)
            {
                return new(string.Format(Strings.DuplicateAn, "account", "tag", model.Tag));
            }
        }
        return ApiError.Success;
    }

    public ApiError Insert(AccountModel model)
    {
        var checkresult = ValidateModel(model);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        AccountEntity entity = model!;
        try
        {
            var result = _accountDal.Insert(entity);
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

    public ApiError Update(AccountModel model)
    {
        var checkresult = ValidateModel(model, true, true);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        AccountEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_accountDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Delete(AccountModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        if (_transactionDal.AccountHasTransactions(model.Id))
        {
            return new(string.Format(Strings.CantDelete, "account", "transactions"));
        }
        try
        {
            return ApiError.FromDalResult(_accountDal.Delete(model.Id));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public IEnumerable<AccountModel> Get(Expression<Func<AccountEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = _accountDal.Get(pred, order, direction);
        var models = entities.ToModels<AccountModel, AccountEntity>();
        models.ForEach(x => x.CanDelete = !_transactionDal.AccountHasTransactions(x.Id));
        return models;
    }

    public IEnumerable<AccountModel> GetForCompany(int companyId) => Get(x => x.CompanyId == companyId);

    public IEnumerable<AccountModel> GetForAccountType(int accountTypeId) => Get(x => x.AccountTypeId == accountTypeId);

    public AccountModel? Read(int id) => _accountDal.Read(id)!;

    public AccountModel? Read(int companyId, string tag) => _accountDal.Read(companyId, tag)!;

    public bool CompanyHasAccounts(int companyId) => _accountDal.CompanyHasAccounts(companyId);

    public bool AccountTypeHasAccounts(int accountTypeId) => _accountDal.AccountTypeHasAccounts(accountTypeId);

    public AccountModel? Create(AccountModel account, string number, byte[] salt, string password, IStringCypherService stringCypherService)
    {
        if (account is null || salt is null || salt.Length != Constants.SaltLength || string.IsNullOrWhiteSpace(number))
        {
            return null;
        }
        var result = ValidateModel(account);
        if (!result.Successful)
        {
            throw new Exception(result.ErrorMessage());
        }
        return _accountDal.Create(account!, number, salt, password, stringCypherService)!;
    }
}

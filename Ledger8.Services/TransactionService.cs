using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionDal _transactionDal;

    public TransactionService(ITransactionDal transactionDal) => _transactionDal = transactionDal;

    public int Count => _transactionDal.Count;

    private static ApiError ValidateModel(TransactionModel model, bool checkid = false)
    {
        if (model is null || model.AccountId <= 0 || model.Date == default || model.Balance < 0M || model.Payment < 0M)
        {
            return new(Strings.InvalidModel);
        }
        if (model.Payment > model.Balance)
        {
            return new(Strings.PaymentInvalid);
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

    public ApiError Insert(TransactionModel model)
    {
        var checkresult = ValidateModel(model);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        TransactionEntity entity = model!;
        try
        {
            var result = _transactionDal.Insert(entity);
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

    public ApiError Update(TransactionModel model)
    {
        var checkresult = ValidateModel(model, true);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        TransactionEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_transactionDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Delete(TransactionModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        try
        {
            return ApiError.FromDalResult(_transactionDal.Delete(model.Id));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public IEnumerable<TransactionModel> Get(Expression<Func<TransactionEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = _transactionDal.Get(pred, order, direction);
        var models = entities.ToModels<TransactionModel, TransactionEntity>();
        models.ForEach(x => x.CanDelete = true);
        return models;
    }

    public IEnumerable<TransactionModel> GetForAccount(int accountId) => Get(x => x.AccountId == accountId);

    public TransactionModel? Read(int id) => _transactionDal.Read(id)!;

    public decimal Total() => _transactionDal.Total();

    public decimal TotalForAccount(int accountId) => _transactionDal.TotalForAccount(accountId);

    public bool AccountHasTransactions(int accountId) => _transactionDal.AccountHasTransactions(accountId);
}

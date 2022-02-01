using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.Services;

public class PoolService : IPoolService
{
    private readonly IPoolDal _poolDal;
    private readonly IAllotmentDal _allotmentDal;

    public PoolService(IPoolDal poolDal, IAllotmentDal allotmentDal)
    {
        _poolDal = poolDal;
        _allotmentDal = allotmentDal;
    }

    public int Count => _poolDal.Count;

    private ApiError ValidateModel(PoolModel model, bool checkid = false, bool update = false)
    {
        if (model is null || string.IsNullOrEmpty(model.Name) || model.Date == default || model.Amount <= 0M || model.Balance > model.Amount)
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
        var existing = _poolDal.Read(model.Name);
        if (update)
        {
            if (existing is not null && existing.Id != model.Id)
            {
                return new(string.Format(Strings.DuplicateA, "pool", "name", model.Name));
            }
        }
        else if (existing is not null)
        {
            return new(string.Format(Strings.DuplicateA, "pool", "name", model.Name));
        }
        return ApiError.Success;
    }

    public ApiError Insert(PoolModel model)
    {
        var checkresult = ValidateModel(model);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        PoolEntity entity = model!;
        try
        {
            var result = _poolDal.Insert(entity);
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

    public ApiError Update(PoolModel model)
    {
        var checkresult = ValidateModel(model, true, true);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        PoolEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_poolDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Delete(PoolModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        try
        {
            return ApiError.FromDalResult(_poolDal.Delete(model.Id));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public IEnumerable<PoolModel> Get(Expression<Func<PoolEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = _poolDal.Get(pred, order, direction);
        var models = entities.ToModels<PoolModel, PoolEntity>();
        models.ForEach(x => x.CanDelete = !_allotmentDal.PoolHasAllotments(x.Id));
        return models;
    }

    public PoolModel? Read(int id) => _poolDal.Read(id)!;

    public PoolModel? Read(string name) => _poolDal.Read(name)!;

    public void Recalculate() => _poolDal.Recalculate();

    public ApiError AddAllotment(int poolId, AllotmentModel allotment)
    {
        if (poolId <= 0)
        {
            return new(string.Format(Strings.Invalid, "pool id"));
        }
        if (allotment is null)
        {
            return new(string.Format(Strings.Invalid, "allotment"));
        }
        AllotmentEntity entity = allotment!;
        try
        {
            var result = _poolDal.AddAllotment(poolId, entity);
            if (result.Successful)
            {
                allotment.Id = entity.Id;
            }
            return ApiError.FromDalResult(result);
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError UpdateAllotment(AllotmentModel allotment)
    {
        if (allotment is null || allotment.PoolId <= 0)
        {
            return new(Strings.InvalidModel);
        }
        AllotmentEntity entity = allotment!;
        try
        {
            return ApiError.FromDalResult(_poolDal.UpdateAllotment(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError RemoveAllotment(AllotmentModel allotment)
    {
        if (allotment is null || allotment.PoolId <= 0)
        {
            return new(Strings.InvalidModel);
        }
        AllotmentEntity entity = allotment!;
        try
        {
            return ApiError.FromDalResult(_poolDal.RemoveAllotment(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError RemoveAllAllotments(int poolId)
    {
        if (poolId <= 0)
        {
            return new(string.Format(Strings.Invalid, "pool id"));
        }
        try
        {
            return ApiError.FromDalResult(_poolDal.RemoveAllAllotments(poolId));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }
}

using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.Services;

public class AllotmentService : IAllotmentService
{
    private readonly IAllotmentDal _allotmentDal;

    public AllotmentService(IAllotmentDal allotmentDal) => _allotmentDal = allotmentDal;

    public int Count => _allotmentDal.Count;

    private static ApiError ValidateModel(AllotmentModel model, bool checkid = false)
    {
        if (model is null || model.PoolId <= 0 || model.CompanyId <= 0 || model.Date == default)
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
        return ApiError.Success;
    }

    public ApiError Insert(AllotmentModel model)
    {
        var checkresult = ValidateModel(model);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        AllotmentEntity entity = model!;
        try
        {
            var result = _allotmentDal.Insert(entity);
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

    public ApiError Update(AllotmentModel model)
    {
        var checkresult = ValidateModel(model, true);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        AllotmentEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_allotmentDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Delete(AllotmentModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        try
        {
            return ApiError.FromDalResult(_allotmentDal.Delete(model.Id));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError DeleteAll(int poolId)
    {
        try
        {
            _allotmentDal.DeleteAll(poolId);
            return ApiError.Success;
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public IEnumerable<AllotmentModel> Get(Expression<Func<AllotmentEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = _allotmentDal.Get(pred, order, direction);
        var models = entities.ToModels<AllotmentModel, AllotmentEntity>();
        models.ForEach(x => x.CanDelete = true);
        return models;
    }

    public IEnumerable<AllotmentModel> GetForPool(int poolId) => Get(x => x.PoolId == poolId);

    public IEnumerable<AllotmentModel> GetForCompany(int companyId) => Get(x => x.CompanyId == companyId);

    public AllotmentModel? Read(int id) => _allotmentDal.Read(id)!;

    public bool PoolHasAllotments(int poolId) => _allotmentDal.PoolHasAllotments(poolId);

    public bool CompanyHasAllotments(int companyId) => _allotmentDal.CompanyHasAllotments(companyId);
}

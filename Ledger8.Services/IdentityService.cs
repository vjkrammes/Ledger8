using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.Services;

public class IdentityService : IIdentityService
{
    private readonly IIdentityDal _identityDal;

    public IdentityService(IIdentityDal identityDal) => _identityDal = identityDal;

    public int Count => _identityDal.Count;

    private static ApiError ValidateModel(IdentityModel model, bool checkid = false)
    {
        if (model is null || model.CompanyId <= 0 || string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.Password))
        {
            return new(Strings.InvalidModel);
        }
        if (model.UserSalt is null || model.UserSalt.Length != Constants.SaltLength || model.PasswordSalt is null || model.PasswordSalt.Length != Constants.SaltLength)
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

    public ApiError Insert(IdentityModel model)
    {
        var checkresult = ValidateModel(model);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        IdentityEntity entity = model!;
        try
        {
            var result = _identityDal.Insert(entity);
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

    public ApiError Update(IdentityModel model)
    {
        var checkresult = ValidateModel(model, true);
        if (!checkresult.Successful)
        {
            return checkresult;
        }
        IdentityEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_identityDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public ApiError Delete(IdentityModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        try
        {
            return ApiError.FromDalResult(_identityDal.Delete(model.Id));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public IEnumerable<IdentityModel> Get(Expression<Func<IdentityEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = _identityDal.Get(pred, order, direction);
        var models = entities.ToModels<IdentityModel, IdentityEntity>();
        models.ForEach(x => x.CanDelete = true);
        return models;
    }

    public IEnumerable<IdentityModel> GetForCompany(int companyId) => Get(x => x.CompanyId == companyId);

    public IdentityModel? Read(int id) => _identityDal.Read(id)!;

    public IdentityModel? ReadForUrl(int companyId, string url) => _identityDal.ReadForUrl(companyId, url)!;

    public IdentityModel? ReadForTag(int companyId, string tag) => _identityDal.ReadForTag(companyId, tag)!;

    public IdentityModel? ReadForUserId(int companyId, string userId) => _identityDal.ReadForUserId(companyId, userId)!;

    public bool CompanyHasIdentities(int companyId) => _identityDal.CompanyHasIdentities(companyId);
}

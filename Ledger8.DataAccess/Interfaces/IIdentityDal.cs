using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface IIdentityDal : IDalBase<IdentityEntity>
{
    IEnumerable<IdentityEntity> GetForCompany(int companyId);
    IdentityEntity? ReadForUrl(int companyId, string url);
    IdentityEntity? ReadForTag(int companyId, string tag);
    IdentityEntity? ReadForUserId(int companyId, string userId);
    bool CompanyHasIdentities(int companyId);
}

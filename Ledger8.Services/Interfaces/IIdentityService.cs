using Ledger8.DataAccess.Entities;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface IIdentityService : IDataService<IdentityModel, IdentityEntity>
{
    IEnumerable<IdentityModel> GetForCompany(int companyId);
    IdentityModel? ReadForUrl(int companyId, string url);
    IdentityModel? ReadForTag(int companyId, string tag);
    IdentityModel? ReadForUserId(int companyId, string userId);
    bool CompanyHasIdentities(int companyId);
}

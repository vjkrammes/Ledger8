using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface IAllotmentService : IDataService<AllotmentModel, AllotmentEntity>
{
    ApiError DeleteAll(int poolId);
    IEnumerable<AllotmentModel> GetForPool(int poolId);
    IEnumerable<AllotmentModel> GetForCompany(int companyId);
    bool PoolHasAllotments(int poolId);
    bool CompanyHasAllotments(int companyId);
}

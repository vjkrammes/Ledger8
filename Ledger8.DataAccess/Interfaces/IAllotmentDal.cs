using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface IAllotmentDal : IDalBase<AllotmentEntity>
{
    void DeleteAll(int poolId);
    IEnumerable<AllotmentEntity> GetForPool(int poolId);
    IEnumerable<AllotmentEntity> GetForCompany(int companyId);
    bool PoolHasAllotments(int poolId);
    bool CompanyHasAllotments(int companyId);
}

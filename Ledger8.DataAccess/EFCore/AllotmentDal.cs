using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace Ledger8.DataAccess.EFCore;

public class AllotmentDal : DalBase<AllotmentEntity, LedgerContext>, IAllotmentDal
{
    public AllotmentDal(LedgerContext context) : base(context) { }

    public void DeleteAll(int poolId)
    {
        var allotments = Entities.Where(x => x.PoolId == poolId).ToList();
        if (allotments.Any())
        {
            Entities.RemoveRange(allotments);
            Context.SaveChanges();
        }
    }

    public override IEnumerable<AllotmentEntity> Get(Expression<Func<AllotmentEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = (pred, order) switch
        {
            (null, null) => Entities
                                .AsNoTracking()
                                .Include(x => x.Company)
                                .ToList(),
            (null, _) => Entities
                            .AsNoTracking()
                            .Include(x => x.Company)
                            .OrderBy(order, direction.IsDescending())
                            .ToList(),
            (_, null) => Entities
                            .AsNoTracking()
                            .Include(x => x.Company)
                            .Where(pred)
                            .ToList(),
            (_, _) => Entities
                        .AsNoTracking()
                        .Include(x => x.Company)
                        .Where(pred)
                        .OrderBy(order, direction.IsDescending())
                        .ToList()
        };
        return entities;
    }

    public IEnumerable<AllotmentEntity> GetForPool(int poolId) => Get(x => x.PoolId == poolId);

    public IEnumerable<AllotmentEntity> GetForCompany(int companyId) => Get(x => x.CompanyId == companyId);

    public bool PoolHasAllotments(int poolId) => Entities.Any(x => x.PoolId == poolId);

    public bool CompanyHasAllotments(int companyId) => Entities.Any(x => x.CompanyId == companyId);
}

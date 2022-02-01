using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace Ledger8.DataAccess.EFCore;

public class IdentityDal : DalBase<IdentityEntity, LedgerContext>, IIdentityDal
{
    public IdentityDal(LedgerContext context) : base(context) { }

    public override IEnumerable<IdentityEntity> Get(Expression<Func<IdentityEntity, bool>>? pred = null, string? order = null, char direction = 'a')
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

    public IEnumerable<IdentityEntity> GetForCompany(int companyId) => Get(x => x.CompanyId == companyId);

    public IdentityEntity? ReadForUrl(int companyId, string url) =>
        Entities
            .AsNoTracking()
            .Include(x => x.Company)
            .SingleOrDefault(x => x.CompanyId == companyId && x.URL.ToLower() == url.ToLower());

    public IdentityEntity? ReadForTag(int companyId, string tag) =>
        Entities
        .AsNoTracking()
        .Include(x => x.Company)
        .SingleOrDefault(x => x.CompanyId == companyId && x.Tag.ToLower() == tag.ToLower());

    public IdentityEntity? ReadForUserId(int companyId, string userId) =>
        Entities
            .AsNoTracking()
            .Include(x => x.Company)
            .SingleOrDefault(x => x.CompanyId == companyId && x.UserId.ToLower() == userId.ToLower());

    public bool CompanyHasIdentities(int companyId) => Entities.Any(x => x.CompanyId == companyId);
}

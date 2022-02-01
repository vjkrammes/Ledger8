using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

namespace Ledger8.DataAccess.EFCore;

public class CompanyDal : DalBase<CompanyEntity, LedgerContext>, ICompanyDal
{
    public CompanyDal(LedgerContext context) : base(context) { }

    public IEnumerable<CompanyEntity> GetPayees() => Get(x => x.IsPayee);

    public CompanyEntity? Read(string name) => Entities.SingleOrDefault(x => x.Name.ToLower() == name.ToLower());
}

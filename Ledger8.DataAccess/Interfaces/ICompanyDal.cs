using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface ICompanyDal : IDalBase<CompanyEntity>
{
    IEnumerable<CompanyEntity> GetPayees();
    CompanyEntity? Read(string name);
}

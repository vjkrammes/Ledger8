using Ledger8.DataAccess.Entities;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface ICompanyService : IDataService<CompanyModel, CompanyEntity>
{
    IEnumerable<CompanyModel> GetPayees();
    CompanyModel? Read(string name);
}

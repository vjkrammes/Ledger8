using Ledger8.Common.Interfaces;
using Ledger8.DataAccess.Entities;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface IAccountService : IDataService<AccountModel, AccountEntity>
{
    AccountModel? Create(AccountModel account, string number, byte[] salt, string password, IStringCypherService stringCypherService);
    IEnumerable<AccountModel> GetForCompany(int companyId);
    IEnumerable<AccountModel> GetForAccountType(int accountTypeId);
    AccountModel? Read(int companyId, string tag);
    bool CompanyHasAccounts(int companyId);
    bool AccountTypeHasAccounts(int accountTypeId);
}

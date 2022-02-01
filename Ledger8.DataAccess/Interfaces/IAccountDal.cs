using Ledger8.Common.Interfaces;
using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface IAccountDal : IDalBase<AccountEntity>
{
    AccountEntity Create(AccountEntity account, string number, byte[] salt, string password, IStringCypherService stringCypherService);
    IEnumerable<AccountEntity> GetForCompany(int companyId);
    IEnumerable<AccountEntity> GetForAccountType(int accountTypeId);
    AccountEntity? Read(int companyId, string tag);
    bool CompanyHasAccounts(int companyId);
    bool AccountTypeHasAccounts(int accountTypeId);
}

using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface IAccountNumberDal : IDalBase<AccountNumberEntity>
{
    void DeleteForAccount(int accountId);
    IEnumerable<AccountNumberEntity> GetForAccount(int accountId);
    AccountNumberEntity? Current(int accountId);
    bool AccountHasAccountNumbers(int accountId);
}

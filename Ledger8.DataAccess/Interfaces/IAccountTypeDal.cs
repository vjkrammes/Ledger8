using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface IAccountTypeDal : IDalBase<AccountTypeEntity>
{
    AccountTypeEntity? Read(string description);
}

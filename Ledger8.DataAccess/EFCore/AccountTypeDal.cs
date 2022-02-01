using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

namespace Ledger8.DataAccess.EFCore;

public class AccountTypeDal : DalBase<AccountTypeEntity, LedgerContext>, IAccountTypeDal
{
    public AccountTypeDal(LedgerContext context) : base(context) { }

    public AccountTypeEntity? Read(string description) => Entities.SingleOrDefault(x => x.Description.ToLower() == description.ToLower());
}

using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

namespace Ledger8.DataAccess.EFCore;

public class AccountNumberDal : DalBase<AccountNumberEntity, LedgerContext>, IAccountNumberDal
{
    public AccountNumberDal(LedgerContext context) : base(context) { }

    public void DeleteForAccount(int accountId)
    {
        var numbers = Entities.Where(x => x.AccountId == accountId).ToList();
        Entities.RemoveRange(numbers);
        Context.SaveChanges();
    }

    public IEnumerable<AccountNumberEntity> GetForAccount(int accountId) => Get(x => x.AccountId == accountId);

    public AccountNumberEntity? Current(int accountId)
    {
        var now = DateTime.Now;
        return Entities.SingleOrDefault(x => x.AccountId == accountId && x.StartDate <= now && x.StopDate >= now);
    }

    public bool AccountHasAccountNumbers(int accountId) => Entities.Any(x => x.AccountId == accountId);
}

using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

namespace Ledger8.DataAccess.EFCore;

public class TransactionDal : DalBase<TransactionEntity, LedgerContext>, ITransactionDal
{
    public TransactionDal(LedgerContext context) : base(context) { }

    public IEnumerable<TransactionEntity> GetForAccount(int accountId) => Get(x => x.AccountId == accountId);

    public decimal Total() => Entities.Sum(x => x.Payment);

    public decimal TotalForAccount(int accountId) => Entities.Where(x => x.AccountId == accountId).Sum(x => x.Payment);

    public bool AccountHasTransactions(int accountId) => Entities.Any(x => x.AccountId == accountId);
}

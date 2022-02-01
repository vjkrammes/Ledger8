using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface ITransactionDal : IDalBase<TransactionEntity>
{
    IEnumerable<TransactionEntity> GetForAccount(int accountId);
    decimal Total();
    decimal TotalForAccount(int accountId);
    bool AccountHasTransactions(int accountId);
}

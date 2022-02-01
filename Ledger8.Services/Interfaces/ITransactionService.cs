using Ledger8.DataAccess.Entities;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface ITransactionService : IDataService<TransactionModel, TransactionEntity>
{
    IEnumerable<TransactionModel> GetForAccount(int accountId);
    decimal Total();
    decimal TotalForAccount(int accountId);
    bool AccountHasTransactions(int accountId);
}

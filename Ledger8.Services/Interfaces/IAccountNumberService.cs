using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface IAccountNumberService : IDataService<AccountNumberModel, AccountNumberEntity>
{
    ApiError DeleteForAccount(int accountID);
    IEnumerable<AccountNumberModel> GetForAccount(int accountId);
    AccountNumberModel? Current(int accountID);
    bool AccountHasAccountNumbers(int accountID);
}

using Ledger8.DataAccess.Entities;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface IAccountTypeService : IDataService<AccountTypeModel, AccountTypeEntity>
{
    AccountTypeModel? Read(string description);
}

using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface IPoolService : IDataService<PoolModel, PoolEntity>
{
    PoolModel? Read(string name);
    void Recalculate();
    ApiError AddAllotment(int poolId, AllotmentModel allotment);
    ApiError UpdateAllotment(AllotmentModel allotment);
    ApiError RemoveAllotment(AllotmentModel allotment);
    ApiError RemoveAllAllotments(int poolId);
}

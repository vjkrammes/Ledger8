using Ledger8.Common;
using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface IPoolDal : IDalBase<PoolEntity>
{
    PoolEntity? Read(string name);
    void Recalculate();
    DalResult AddAllotment(int poolId, AllotmentEntity allotment);
    DalResult UpdateAllotment(AllotmentEntity allotment);
    DalResult RemoveAllotment(AllotmentEntity allotment);
    DalResult RemoveAllAllotments(int poolId);
}

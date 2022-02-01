using Ledger8.Common;
using Ledger8.Common.Interfaces;

using System.Linq.Expressions;

namespace Ledger8.DataAccess.Interfaces;

public interface IDalBase<TEntity> where TEntity : class, IIdEntity, new()
{
    int Count { get; }
    DalResult Insert(TEntity entity);
    DalResult Update(TEntity entity);
    DalResult Delete(TEntity entity);
    DalResult Delete(int id);
    IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? pred = null, string? order = null, char direction = 'a');
    TEntity? Read(int id);
}

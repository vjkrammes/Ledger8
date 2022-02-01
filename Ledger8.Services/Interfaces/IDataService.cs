using Ledger8.Common;
using Ledger8.Common.Interfaces;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;

using System.Linq.Expressions;

namespace Ledger8.Services.Interfaces;

public interface IDataService<TModel, TEntity> : IDataServiceTag where TModel : ModelBase where TEntity : class, IIdEntity, new()
{
    int Count { get; }
    ApiError Insert(TModel model);
    ApiError Update(TModel model);
    ApiError Delete(TModel model);
    IEnumerable<TModel> Get(Expression<Func<TEntity, bool>>? pred = null, string? order = null, char direction = 'a');
    TModel? Read(int id);
}

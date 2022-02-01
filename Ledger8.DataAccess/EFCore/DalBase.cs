using Ledger8.Common;
using Ledger8.Common.Attributes;
using Ledger8.Common.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using System.Reflection;

namespace Ledger8.DataAccess.EFCore;

public abstract class DalBase<TEntity, TContext> where TEntity : class, IIdEntity, new() where TContext : DbContext
{
    private readonly DbSet<TEntity>? _entities;
    private readonly bool _hasNullableMembers = false;

    protected TContext Context { get; }
    protected DbSet<TEntity> Entities => _entities!;

    public DalBase(TContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        Context = context;
        _entities = null;
        var tabletype = typeof(DbSet<>).MakeGenericType(typeof(TEntity));
        foreach (var property in typeof(TContext).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.PropertyType == tabletype)
            {
                _entities = property.GetValue(Context) as DbSet<TEntity>;
                break;
            }
        }
        if (_entities is null)
        {
            throw new MissingMemberException($"Unable to locate DbSet of type {tabletype}");
        }
        _hasNullableMembers = typeof(TEntity).GetCustomAttribute(typeof(HasNullableMembersAttribute), false) is HasNullableMembersAttribute;
    }

    protected virtual void Nullify(TEntity entity)
    {
        if (_hasNullableMembers)
        {
            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(NullOnInsertOrUpdateAttribute), false) is NullOnInsertOrUpdateAttribute)
                {
                    property.SetValue(entity, null);
                }
            }
        }
    }

    public virtual int Count => Entities.Count();

    public virtual DalResult Insert(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        Nullify(entity);
        try
        {
            Entities.Add(entity);
            Context.SaveChanges();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            return DalResult.FromException(ex);
        }
    }

    public virtual DalResult Update(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        Nullify(entity);
        try
        {
            Context.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            return DalResult.FromException(ex);
        }
    }

    public virtual DalResult Delete(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        try
        {
            Context.Attach(entity);
            Entities.Remove(entity);
            Context.SaveChanges();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            return DalResult.FromException(ex);
        }
    }

    public virtual DalResult Delete(int id)
    {
        var entity = Entities.SingleOrDefault(x => x.Id == id);
        if (entity is null)
        {
            return DalResult.NotFound;
        }
        try
        {
            Entities.Remove(entity);
            Context.SaveChanges();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            return DalResult.FromException(ex);
        }
    }

    public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var ret = (pred, order) switch
        {
            (null, null) => Entities.AsNoTracking().ToList(),
            (null, _) => Entities.AsNoTracking().OrderBy(order, direction.IsDescending()).ToList(),
            (_, null) => Entities.AsNoTracking().Where(pred).ToList(),
            (_, _) => Entities.AsNoTracking().Where(pred).OrderBy(order, direction.IsDescending()).ToList()
        };
        return ret;
    }

    public virtual TEntity? Read(int id) => Entities.AsNoTracking().SingleOrDefault(x => x.Id == id);
}

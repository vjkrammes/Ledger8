using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Ledger8.DataAccess.EFCore;

public class PoolDal : DalBase<PoolEntity, LedgerContext>, IPoolDal
{
    public PoolDal(LedgerContext context) : base(context) { }

    public PoolEntity? Read(string name) => Entities.AsNoTracking().SingleOrDefault(x => x.Name.ToLower() == name.ToLower());

    public void Recalculate()
    {
        using var transaction = Context.Database.BeginTransaction();
        try
        {
            var pools = Entities.ToList();
            foreach (var pool in pools)
            {
                var allotments = Context.Allotments.AsNoTracking().Where(x => x.PoolId == pool.Id).ToList();
                var spent = allotments.Sum(x => x.Amount);
                pool.Balance = pool.Amount - spent;
            }
            Context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public DalResult AddAllotment(int poolId, AllotmentEntity allotment)
    {
        if (allotment is null)
        {
            throw new ArgumentNullException(nameof(allotment));
        }
        using var transaction = Context.Database.BeginTransaction();
        try
        {
            var pool = Entities.SingleOrDefault(x => x.Id == poolId);
            if (pool is null)
            {
                transaction.Rollback();
                return DalResult.NotFound;
            }
            pool.Balance -= allotment.Amount;
            allotment.Company = null;
            Context.Allotments.Add(allotment);
            Context.SaveChanges();
            transaction.Commit();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return DalResult.FromException(ex);
        }
    }

    public DalResult UpdateAllotment(AllotmentEntity allotment)
    {
        if (allotment is null)
        {
            throw new ArgumentNullException(nameof(allotment));
        }
        using var transaction = Context.Database.BeginTransaction();
        try
        {
            var pool = Entities.SingleOrDefault(x => x.Id == allotment.PoolId);
            if (pool is null)
            {
                transaction.Rollback();
                return DalResult.NotFound;
            }
            var oldAllotment = Context.Allotments.SingleOrDefault(x => x.Id == allotment.Id);
            if (oldAllotment is null)
            {
                transaction.Rollback();
                return DalResult.NotFound;
            }
            pool.Balance = pool.Balance + oldAllotment.Amount - allotment.Amount;
            oldAllotment.Description = allotment.Description;
            oldAllotment.Date = allotment.Date;
            oldAllotment.Amount = allotment.Amount;
            Context.SaveChanges();
            transaction.Commit();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return DalResult.FromException(ex);
        }
    }

    public DalResult RemoveAllotment(AllotmentEntity allotment)
    {
        if (allotment is null)
        {
            throw new ArgumentNullException(nameof(allotment));
        }
        using var transaction = Context.Database.BeginTransaction();
        try
        {
            var pool = Entities.SingleOrDefault(x => x.Id == allotment.PoolId);
            if (pool is null)
            {
                transaction.Rollback();
                return DalResult.NotFound;
            }
            pool.Balance += allotment.Amount;
            Context.Attach(allotment);
            Context.Allotments.Remove(allotment);
            Context.SaveChanges();
            transaction.Commit();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return DalResult.FromException(ex);
        }
    }

    public DalResult RemoveAllAllotments(int poolId)
    {
        using var transaction = Context.Database.BeginTransaction();
        try
        {
            var pool = Entities.SingleOrDefault(x => x.Id == poolId);
            if (pool is null)
            {
                transaction.Rollback();
                return DalResult.NotFound;
            }
            pool.Balance = pool.Amount;
            var allotments = Context.Allotments.Where(x => x.PoolId == poolId);
            Context.Allotments.RemoveRange(allotments);
            Context.SaveChanges();
            transaction.Commit();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return DalResult.FromException(ex);
        }
    }
}

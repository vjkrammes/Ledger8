using Ledger8.Common;
using Ledger8.Common.Interfaces;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace Ledger8.DataAccess.EFCore;

public class AccountDal : DalBase<AccountEntity, LedgerContext>, IAccountDal
{
    private readonly IAccountNumberDal _accountNumberDal;

    public AccountDal(LedgerContext context, IAccountNumberDal accountNumberDal) : base(context) => _accountNumberDal = accountNumberDal;

    public override DalResult Delete(AccountEntity entity)
    {
        using var transaction = Context.Database.BeginTransaction();
        try
        {
            var accountNumbers = Context.AccountNumbers.Where(x => x.AccountId == entity.Id);
            Context.AccountNumbers.RemoveRange(accountNumbers);
            Context.Attach(entity);
            Context.Accounts.Remove(entity);
            Context.SaveChanges();
            transaction.Commit();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Context.Entry(entity).State = EntityState.Detached;
            return DalResult.FromException(ex);
        }
    }

    public override DalResult Delete(int id)
    {
        var entity = Entities.AsNoTracking().SingleOrDefault(x => x.Id == id);
        if (entity is null)
        {
            return DalResult.NotFound;
        }
        return Delete(entity);
    }

    public AccountEntity Create(AccountEntity entity, string number, byte[] salt, string password, IStringCypherService stringCypherService)
    {
        using var transaction = Context.Database.BeginTransaction();
        if (entity.Id <= 0)
        {
            // insert new account and create new accountnumber
            var accountType = entity.AccountType;
            entity.AccountType = null;
            Entities.Add(entity);
            Context.SaveChanges();
            entity.AccountType = accountType;
            var accountNumber = new AccountNumberEntity
            {
                AccountId = entity.Id,
                StartDate = default,
                StopDate = DateTime.MaxValue,
                Salt = salt.ArrayCopy(),
                Number = stringCypherService.Encrypt(number, password, salt)
            };
            Context.AccountNumbers.Add(accountNumber);
            Context.SaveChanges();
            entity.AccountNumber = accountNumber;
        }
        else
        {
            // update existing accountnumber and create a new one
            var accountNumber = entity.AccountNumber;
            if (accountNumber is null)
            {
                throw new ArgumentException($"{nameof(accountNumber)} is null");
            }
            accountNumber.StopDate = DateTime.Now;
            Context.AccountNumbers.Update(accountNumber);
            accountNumber = new()
            {
                AccountId = entity.Id,
                StartDate = DateTime.Now,
                StopDate = DateTime.MaxValue,
                Salt = salt.ArrayCopy(),
                Number = stringCypherService.Encrypt(number, password, salt)
            };
            Context.AccountNumbers.Add(accountNumber);
            Context.SaveChanges();
            entity.AccountNumber = accountNumber;
        }
        transaction.Commit();
        return entity;
    }

    public override IEnumerable<AccountEntity> Get(Expression<Func<AccountEntity, bool>>? pred = null, string? order = null, char direction = 'a')
    {
        var entities = (pred, order) switch
        {
            (null, null) => Entities
                                .AsNoTracking()
                                .Include(x => x.AccountType)
                                .ToList(),
            (null, _) => Entities
                            .AsNoTracking()
                            .Include(x => x.AccountType)
                            .OrderBy(order, direction.IsDescending())
                            .ToList(),
            (_, null) => Entities
                            .AsNoTracking()
                            .Include(x => x.AccountType)
                            .Where(pred)
                            .ToList(),
            (_, _) => Entities
                        .AsNoTracking()
                        .Include(x => x.AccountType)
                        .Where(pred)
                        .OrderBy(order, direction.IsDescending())
                        .ToList()
        };
        entities.ForEach(x => x.AccountNumber = _accountNumberDal.Current(x.Id));
        return entities;
    }

    public IEnumerable<AccountEntity> GetForCompany(int companyId) => Get(x => x.CompanyId == companyId);

    public IEnumerable<AccountEntity> GetForAccountType(int accountTypeId) => Get(x => x.AccountTypeId == accountTypeId);

    public override AccountEntity? Read(int id)
    {
        var ret = Entities
            .AsNoTracking()
            .Include(x => x.AccountType)
            .SingleOrDefault(x => x.Id == id);
        if (ret is not null)
        {
            ret.AccountNumber = _accountNumberDal.Current(id);
        }
        return ret;
    }

    public AccountEntity? Read(int companyId, string tag)
    {
        var ret = Entities
            .AsNoTracking()
            .Include(x => x.AccountType)
            .SingleOrDefault(x => x.CompanyId == companyId && x.Tag.ToLower() == tag.ToLower());
        if (ret is not null)
        {
            ret.AccountNumber = _accountNumberDal.Current(ret.Id);
        }
        return ret;
    }

    public bool CompanyHasAccounts(int companyId) => Entities.Any(x => x.CompanyId == companyId);

    public bool AccountTypeHasAccounts(int accountTypeId) => Entities.Any(x => x.AccountTypeId == accountTypeId);
}

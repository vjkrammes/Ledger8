using Ledger8.DataAccess.Entities;

namespace Ledger8.Models;

public class PoolModel : ModelBase, IEquatable<PoolModel>, IComparable<PoolModel>
{
    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string? _name;
    public string Name
    {
        get => _name!;
        set => SetProperty(ref _name, value);
    }

    private DateTime _date;
    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private string? _description;
    public string Description
    {
        get => _description!;
        set => SetProperty(ref _description, value);
    }

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }

    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set => SetProperty(ref _balance, value);
    }

    public PoolModel() : base()
    {
        Id = 0;
        Name = string.Empty;
        Date = DateTime.Now;
        Description = string.Empty;
        Amount = 0M;
        Balance = 0M;
    }

    public static PoolModel? FromEntity(PoolEntity entity) => entity is null ? null : new()
    {
        Id = entity.Id,
        Name = entity.Name ?? string.Empty,
        Date = entity.Date,
        Description = entity.Description ?? string.Empty,
        Amount = entity.Amount,
        Balance = entity.Balance,
        CanDelete = true
    };

    public static PoolEntity? FromModel(PoolModel model) => model is null ? null : new()
    {
        Id = model.Id,
        Name = model.Name ?? string.Empty,
        Date = model.Date,
        Description = model.Description ?? string.Empty,
        Amount = model.Amount,
        Balance = model.Balance
    };

    public PoolModel Clone() => new()
    {
        Id = Id,
        Name = Name ?? string.Empty,
        Date = Date,
        Description = Description ?? string.Empty,
        Amount = Amount,
        Balance = Balance,
        CanDelete = CanDelete
    };

    public override string ToString() => Name;

    public override bool Equals(object? obj) => obj is PoolModel model && model.Id == Id;

    public bool Equals(PoolModel? model) => model is not null && model.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(PoolModel left, PoolModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(PoolModel left, PoolModel right) => !(left == right);

    public int CompareTo(PoolModel? other) => Name.CompareTo(other?.Name);

    public static bool operator >(PoolModel left, PoolModel right) => left.CompareTo(right) > 0;

    public static bool operator <(PoolModel left, PoolModel right) => left.CompareTo(right) < 0;

    public static bool operator >=(PoolModel left, PoolModel right) => left.CompareTo(right) >= 0;

    public static bool operator <=(PoolModel left, PoolModel right) => left.CompareTo(right) <= 0;

    public static implicit operator PoolModel?(PoolEntity entity) => FromEntity(entity);

    public static implicit operator PoolEntity?(PoolModel model) => FromModel(model);
}

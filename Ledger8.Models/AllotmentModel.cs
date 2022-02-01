using Ledger8.DataAccess.Entities;

namespace Ledger8.Models;

public class AllotmentModel : ModelBase, IEquatable<AllotmentModel>, IComparable<AllotmentModel>
{
    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private int _poolId;
    public int PoolId
    {
        get => _poolId;
        set => SetProperty(ref _poolId, value);
    }

    private int _companyId;
    public int CompanyId
    {
        get => _companyId;
        set => SetProperty(ref _companyId, value);
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

    public CompanyModel? Company { get; set; }

    public AllotmentModel()
    {
        Id = 0;
        PoolId = 0;
        CompanyId = 0;
        Date = default;
        Description = string.Empty;
        Amount = 0M;
        Company = null;
    }

    public static AllotmentModel? FromEntity(AllotmentEntity entity) => entity is null ? null : new()
    {
        Id = entity.Id,
        PoolId = entity.PoolId,
        CompanyId = entity.CompanyId,
        Date = entity.Date,
        Description = entity.Description ?? string.Empty,
        Amount = entity.Amount,
        Company = entity.Company!,
        CanDelete = true
    };

    public static AllotmentEntity? FromModel(AllotmentModel model) => model is null ? null : new()
    {
        Id = model.Id,
        PoolId = model.PoolId,
        CompanyId = model.CompanyId,
        Date = model.Date,
        Description = model.Description ?? string.Empty,
        Amount = model.Amount,
        Company = model.Company!
    };

    public AllotmentModel Clone() => new()
    {
        Id = Id,
        PoolId = PoolId,
        CompanyId = CompanyId,
        Date = Date,
        Description = Description ?? string.Empty,
        Amount = Amount,
        Company = Company?.Clone(),
        CanDelete = CanDelete
    };

    public override string ToString() => $"{Date.ToShortDateString()}: ${Amount:c2}";

    public override bool Equals(object? obj) => obj is AllotmentModel model && model.Id == Id;

    public bool Equals(AllotmentModel? model) => model is not null && model.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(AllotmentModel left, AllotmentModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(AllotmentModel left, AllotmentModel right) => !(left == right);

    public int CompareTo(AllotmentModel? other) => Date.CompareTo(other?.Date);

    public static bool operator >(AllotmentModel left, AllotmentModel right) => left.CompareTo(right) > 0;

    public static bool operator <(AllotmentModel left, AllotmentModel right) => left.CompareTo(right) < 0;

    public static bool operator >=(AllotmentModel left, AllotmentModel right) => left.CompareTo(right) >= 0;

    public static bool operator <=(AllotmentModel left, AllotmentModel right) => left.CompareTo(right) <= 0;

    public static implicit operator AllotmentModel?(AllotmentEntity entity) => FromEntity(entity);

    public static implicit operator AllotmentEntity?(AllotmentModel model) => FromModel(model);
}

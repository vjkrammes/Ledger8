using Ledger8.DataAccess.Entities;

namespace Ledger8.Models;

public class AccountTypeModel : ModelBase, IEquatable<AccountTypeModel>, IComparable<AccountTypeModel>
{
    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string? _description;
    public string Description
    {
        get => _description!;
        set => SetProperty(ref _description, value);
    }

    public AccountTypeModel() : base()
    {
        Id = 0;
        Description = string.Empty;
    }

    public static AccountTypeModel? FromEntity(AccountTypeEntity entity) => entity is null ? null : new()
    {
        Id = entity.Id,
        Description = entity.Description ?? string.Empty,
        CanDelete = true
    };

    public static AccountTypeEntity? FromModel(AccountTypeModel model) => model is null ? null : new()
    {
        Id = model.Id,
        Description = model.Description ?? string.Empty,
    };

    public AccountTypeModel Clone() => new()
    {
        Id = Id,
        Description = Description ?? string.Empty,
        CanDelete = CanDelete
    };

    public override string ToString() => Description;

    public override bool Equals(object? obj) => obj is AccountTypeModel model && model.Id == Id;

    public bool Equals(AccountTypeModel? model) => model is not null && model.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(AccountTypeModel left, AccountTypeModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(AccountTypeModel left, AccountTypeModel right) => !(left == right);

    public int CompareTo(AccountTypeModel? other) => Description.CompareTo(other?.Description);

    public static bool operator >(AccountTypeModel left, AccountTypeModel right) => left.CompareTo(right) > 0;

    public static bool operator <(AccountTypeModel left, AccountTypeModel right) => left.CompareTo(right) < 0;

    public static bool operator >=(AccountTypeModel left, AccountTypeModel right) => left.CompareTo(right) >= 0;

    public static bool operator <=(AccountTypeModel left, AccountTypeModel right) => left.CompareTo(right) <= 0;

    public static implicit operator AccountTypeModel?(AccountTypeEntity entity) => FromEntity(entity);

    public static implicit operator AccountTypeEntity?(AccountTypeModel model) => FromModel(model);
}

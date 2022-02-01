using Ledger8.DataAccess.Entities;

namespace Ledger8.Models;

public class CompanyModel : ModelBase, IEquatable<CompanyModel>, IComparable<CompanyModel>
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

    private string? _address1;
    public string Address1
    {
        get => _address1!;
        set => SetProperty(ref _address1, value);
    }

    private string? _address2;
    public string Address2
    {
        get => _address2!;
        set => SetProperty(ref _address2, value);
    }

    private string? _city;
    public string City
    {
        get => _city!;
        set => SetProperty(ref _city, value);
    }

    private string? _state;
    public string State
    {
        get => _state!;
        set => SetProperty(ref _state, value);
    }

    private string? _postalCode;
    public string PostalCode
    {
        get => _postalCode!;
        set => SetProperty(ref _postalCode, value);
    }

    private string? _phone;
    public string Phone
    {
        get => _phone!;
        set => SetProperty(ref _phone, value);
    }

    private string? _url;
    public string URL
    {
        get => _url!;
        set => SetProperty(ref _url, value);
    }

    private bool _isPayee;
    public bool IsPayee
    {
        get => _isPayee;
        set => SetProperty(ref _isPayee, value);
    }

    private string? _comments;
    public string Comments
    {
        get => _comments!;
        set => SetProperty(ref _comments, value);
    }

    public CompanyModel() : base()
    {
        Id = 0;
        Name = string.Empty;
        Address1 = string.Empty;
        Address2 = string.Empty;
        City = string.Empty;
        State = string.Empty;
        PostalCode = string.Empty;
        Phone = string.Empty;
        URL = string.Empty;
        IsPayee = true;
        Comments = string.Empty;
        CanDelete = true;
    }

    public static CompanyModel? FromEntity(CompanyEntity entity) => entity is null ? null : new()
    {
        Id = entity.Id,
        Name = entity.Name ?? string.Empty,
        Address1 = entity.Address1 ?? string.Empty,
        Address2 = entity.Address2 ?? string.Empty,
        City = entity.City ?? string.Empty,
        State = entity.State ?? string.Empty,
        PostalCode = entity.PostalCode ?? string.Empty,
        Phone = entity.Phone ?? string.Empty,
        URL = entity.URL ?? string.Empty,
        IsPayee = entity.IsPayee,
        Comments = entity.Comments ?? string.Empty,
        CanDelete = true
    };

    public static CompanyEntity? FromModel(CompanyModel model) => model is null ? null : new()
    {
        Id = model.Id,
        Name = model.Name ?? string.Empty,
        Address1 = model.Address1 ?? string.Empty,
        Address2 = model.Address2 ?? string.Empty,
        City = model.City ?? string.Empty,
        State = model.State ?? string.Empty,
        PostalCode = model.PostalCode ?? string.Empty,
        Phone = model.Phone ?? string.Empty,
        URL = model.URL ?? string.Empty,
        IsPayee = model.IsPayee,
        Comments = model.Comments ?? string.Empty
    };

    public CompanyModel Clone() => new()
    {
        Id = Id,
        Name = Name ?? string.Empty,
        Address1 = Address1 ?? string.Empty,
        Address2 = Address2 ?? string.Empty,
        City = City ?? string.Empty,
        State = State ?? string.Empty,
        PostalCode = PostalCode ?? string.Empty,
        Phone = Phone ?? string.Empty,
        URL = URL ?? string.Empty,
        IsPayee = IsPayee,
        Comments = Comments ?? string.Empty,
        CanDelete = CanDelete
    };

    public override string ToString() => Name;

    public override bool Equals(object? obj) => obj is CompanyModel model && model.Id == Id;

    public bool Equals(CompanyModel? model) => model is not null && model.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(CompanyModel left, CompanyModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(CompanyModel left, CompanyModel right) => !(left == right);

    public int CompareTo(CompanyModel? other) => Name.CompareTo(other?.Name);

    public static bool operator >(CompanyModel left, CompanyModel right) => left.CompareTo(right) > 0;

    public static bool operator <(CompanyModel left, CompanyModel right) => left.CompareTo(right) < 0;

    public static bool operator >=(CompanyModel left, CompanyModel right) => left.CompareTo(right) >= 0;

    public static bool operator <=(CompanyModel left, CompanyModel right) => left.CompareTo(right) <= 0;

    public static implicit operator CompanyModel?(CompanyEntity entity) => FromEntity(entity);

    public static implicit operator CompanyEntity?(CompanyModel model) => FromModel(model);
}

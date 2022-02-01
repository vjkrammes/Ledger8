using Ledger8.Common;
using Ledger8.DataAccess.Entities;

namespace Ledger8.Models;

public class IdentityModel : ModelBase, IEquatable<IdentityModel>
{
    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private int _companyId;
    public int CompanyId
    {
        get => _companyId;
        set => SetProperty(ref _companyId, value);
    }

    private string? _url;
    public string URL
    {
        get => _url!;
        set => SetProperty(ref _url, value);
    }

    private byte[]? _userSalt;
    public byte[]? UserSalt
    {
        get => _userSalt;
        set => SetProperty(ref _userSalt, value);
    }

    private string? _userId;
    public string UserId
    {
        get => _userId!;
        set => SetProperty(ref _userId, value);
    }

    private byte[]? _passwordSalt;
    public byte[]? PasswordSalt
    {
        get => _passwordSalt;
        set => SetProperty(ref _passwordSalt, value);
    }

    private string? _password;
    public string Password
    {
        get => _password!;
        set => SetProperty(ref _password, value);
    }

    private string? _tag;
    public string Tag
    {
        get => _tag!;
        set => SetProperty(ref _tag, value);
    }

    public CompanyModel? Company { get; set; }

    public IdentityModel() : base()
    {
        Id = 0;
        CompanyId = 0;
        URL = string.Empty;
        UserSalt = null;
        UserId = string.Empty;
        PasswordSalt = null;
        Password = string.Empty;
        Tag = string.Empty;
        Company = null;
    }

    public static IdentityModel? FromEntity(IdentityEntity entity) => entity is null ? null : new()
    {
        Id = entity.Id,
        CompanyId = entity.CompanyId,
        URL = entity.URL ?? string.Empty,
        UserSalt = entity.UserSalt?.ArrayCopy(),
        UserId = entity.UserId ?? string.Empty,
        PasswordSalt = entity.PasswordSalt?.ArrayCopy(),
        Password = entity.Password ?? string.Empty,
        Tag = entity.Tag ?? string.Empty,
        Company = entity.Company!,
        CanDelete = true
    };

    public static IdentityEntity? FromModel(IdentityModel model) => model is null ? null : new()
    {
        Id = model.Id,
        CompanyId = model.CompanyId,
        URL = model.URL ?? string.Empty,
        UserSalt = model.UserSalt?.ArrayCopy(),
        UserId = model.UserId ?? string.Empty,
        PasswordSalt = model.PasswordSalt?.ArrayCopy(),
        Password = model.Password ?? string.Empty,
        Tag = model.Tag ?? string.Empty,
        Company = model.Company!
    };

    public IdentityModel Clone() => new()
    {
        Id = Id,
        CompanyId = CompanyId,
        URL = URL ?? string.Empty,
        UserSalt = UserSalt?.ArrayCopy(),
        UserId = UserId ?? string.Empty,
        PasswordSalt = PasswordSalt?.ArrayCopy(),
        Password = Password ?? string.Empty,
        Tag = Tag ?? string.Empty,
        Company = Company?.Clone(),
        CanDelete = CanDelete
    };

    public override bool Equals(object? obj) => obj is IdentityModel model && model.Id == Id;

    public bool Equals(IdentityModel? model) => model is not null && model.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(IdentityModel left, IdentityModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(IdentityModel left, IdentityModel right) => !(left == right);

    public static implicit operator IdentityModel?(IdentityEntity entity) => FromEntity(entity);

    public static implicit operator IdentityEntity?(IdentityModel model) => FromModel(model);
}

using Ledger8.Common;
using Ledger8.DataAccess.Entities;

namespace Ledger8.Models;

public class AccountNumberModel : ModelBase, IEquatable<AccountNumberModel>
{
    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private int _accountId;
    public int AccountId
    {
        get => _accountId;
        set => SetProperty(ref _accountId, value);
    }

    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    private DateTime _stopDate;
    public DateTime StopDate
    {
        get => _stopDate;
        set => SetProperty(ref _stopDate, value);
    }

    private byte[]? _salt;
    public byte[]? Salt
    {
        get => _salt;
        set => SetProperty(ref _salt, value);
    }

    private string? _number;
    public string Number
    {
        get => _number!;
        set => SetProperty(ref _number, value);
    }

    public AccountNumberModel() : base()
    {
        Id = 0;
        AccountId = 0;
        StartDate = default;
        StopDate = DateTime.MaxValue;
        Salt = null;
        Number = string.Empty;
    }

    public static AccountNumberModel? FromEntity(AccountNumberEntity entity) => entity is null ? null : new()
    {
        Id = entity.Id,
        AccountId = entity.AccountId,
        StartDate = entity.StartDate,
        StopDate = entity.StopDate,
        Salt = entity.Salt?.ArrayCopy(),
        Number = entity.Number ?? string.Empty,
        CanDelete = true
    };

    public static AccountNumberEntity? FromModel(AccountNumberModel model) => model is null ? null : new()
    {
        Id = model.Id,
        AccountId = model.AccountId,
        StartDate = model.StartDate,
        StopDate = model.StopDate,
        Salt = model.Salt?.ArrayCopy(),
        Number = model.Number ?? string.Empty
    };

    public AccountNumberModel Clone() => new()
    {
        Id = Id,
        AccountId = AccountId,
        StartDate = StartDate,
        StopDate = StopDate,
        Salt = Salt?.ArrayCopy(),
        Number = Number ?? string.Empty,
        CanDelete = CanDelete
    };

    public override string ToString() => $"{StartDate.ToShortDateString()} - {StopDate.ToShortDateString()}";

    public override bool Equals(object? obj) => obj is AccountNumberModel model && model.Id == Id;

    public bool Equals(AccountNumberModel? model) => model is not null && model.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(AccountNumberModel left, AccountNumberModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(AccountNumberModel left, AccountNumberModel right) => !(left == right);

    public static implicit operator AccountNumberEntity?(AccountNumberModel model) => FromModel(model);

    public static implicit operator AccountNumberModel?(AccountNumberEntity entity) => FromEntity(entity);
}

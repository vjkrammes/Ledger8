using Ledger8.DataAccess.Entities;

namespace Ledger8.Models;

public class TransactionModel : ModelBase, IEquatable<TransactionModel>, IComparable<TransactionModel>
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

    private DateTime _date;
    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set => SetProperty(ref _balance, value);
    }

    private decimal _payment;
    public decimal Payment
    {
        get => _payment;
        set => SetProperty(ref _payment, value);
    }

    private string? _reference;
    public string Reference
    {
        get => _reference!;
        set => SetProperty(ref _reference, value);
    }

    public TransactionModel() : base()
    {
        Id = 0;
        AccountId = 0;
        Date = default;
        Balance = 0M;
        Payment = 0M;
        Reference = string.Empty;
    }

    public static TransactionModel? FromEntity(TransactionEntity entity) => entity is null ? null : new()
    {
        Id = entity.Id,
        AccountId = entity.AccountId,
        Date = entity.Date,
        Balance = entity.Balance,
        Payment = entity.Payment,
        Reference = entity.Reference ?? string.Empty,
        CanDelete = true
    };

    public static TransactionEntity? FromModel(TransactionModel model) => model is null ? null : new()
    {
        Id = model.Id,
        AccountId = model.AccountId,
        Date = model.Date,
        Balance = model.Balance,
        Payment = model.Payment,
        Reference = model.Reference ?? string.Empty
    };

    public TransactionModel Clone() => new()
    {
        Id = Id,
        AccountId = AccountId,
        Date = Date,
        Balance = Balance,
        Payment = Payment,
        Reference = Reference ?? string.Empty,
        CanDelete = CanDelete
    };

    public override string ToString() => $"{Date.ToShortDateString()} {Payment:c2} ({Balance:c2})";

    public override bool Equals(object? obj) => obj is TransactionModel model && model.Id == Id;

    public bool Equals(TransactionModel? model) => model is not null && model.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(TransactionModel left, TransactionModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(TransactionModel left, TransactionModel right) => !(left == right);

    public int CompareTo(TransactionModel? other) => Date.CompareTo(other?.Date);

    public static bool operator >(TransactionModel left, TransactionModel right) => left.CompareTo(right) > 0;

    public static bool operator <(TransactionModel left, TransactionModel right) => left.CompareTo(right) < 0;

    public static bool operator >=(TransactionModel left, TransactionModel right) => left.CompareTo(right) >= 0;

    public static bool operator <=(TransactionModel left, TransactionModel right) => left.CompareTo(right) <= 0;

    public static implicit operator TransactionModel?(TransactionEntity entity) => FromEntity(entity);

    public static implicit operator TransactionEntity?(TransactionModel model) => FromModel(model);
}

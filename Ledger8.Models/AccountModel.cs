using Ledger8.Common;
using Ledger8.Common.Enumerations;
using Ledger8.Common.Interfaces;
using Ledger8.DataAccess.Entities;

using System.Text;

namespace Ledger8.Models;

public class AccountModel : ModelBase
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

    private int _accountTypeId;
    public int AccountTypeId
    {
        get => _accountTypeId;
        set => SetProperty(ref _accountTypeId, value);
    }

    private DueDateType _dueDateType;
    public DueDateType DueDateType
    {
        get => _dueDateType;
        set => SetProperty(ref _dueDateType, value);
    }

    private int _month;
    public int Month
    {
        get => _month;
        set => SetProperty(ref _month, value);
    }

    private int _day;
    public int Day
    {
        get => _day;
        set => SetProperty(ref _day, value);
    }

    private bool _isPayable;
    public bool IsPayable
    {
        get => _isPayable;
        set => SetProperty(ref _isPayable, value);
    }

    private bool _isClosed;
    public bool IsClosed
    {
        get => _isClosed;
        set => SetProperty(ref _isClosed, value);
    }

    private DateTime _closedDate;
    public DateTime ClosedDate
    {
        get => _closedDate;
        set => SetProperty(ref _closedDate, value);
    }

    private bool _isAutoPaid;
    public bool IsAutoPaid
    {
        get => _isAutoPaid;
        set => SetProperty(ref _isAutoPaid, value);
    }

    private string? _comments;
    public string Comments
    {
        get => _comments!;
        set => SetProperty(ref _comments, value);
    }

    private string? _tag;
    public string Tag
    {
        get => _tag!;
        set => SetProperty(ref _tag, value);
    }

    private AccountTypeModel? _accountType;
    public AccountTypeModel? AccountType
    {
        get => _accountType;
        set => SetProperty(ref _accountType, value);
    }

    private AccountNumberModel? _accountNumber;
    public AccountNumberModel? AccountNumber
    {
        get => _accountNumber;
        set => SetProperty(ref _accountNumber, value);
    }

    public AccountModel() : base()
    {
        Id = 0;
        CompanyId = 0;
        AccountTypeId = 0;
        DueDateType = DueDateType.Unspecified;
        Month = 0;
        Day = 0;
        IsPayable = true;
        IsClosed = false;
        ClosedDate = default;
        IsAutoPaid = false;
        Comments = string.Empty;
        Tag = string.Empty;
        AccountType = null;
        AccountNumber = null;
    }

    public static AccountModel? FromEntity(AccountEntity entity) => entity is null ? null : new()
    {
        Id = entity.Id,
        CompanyId = entity.CompanyId,
        AccountTypeId = entity.AccountTypeId,
        DueDateType = entity.DueDateType,
        Month = entity.Month,
        Day = entity.Day,
        IsPayable = entity.IsPayable,
        IsClosed = entity.IsClosed,
        ClosedDate = entity.ClosedDate,
        IsAutoPaid = entity.IsAutoPaid,
        Comments = entity.Comments ?? string.Empty,
        Tag = entity.Tag ?? string.Empty,
        AccountType = entity.AccountType!,
        AccountNumber = entity.AccountNumber!,
        CanDelete = true
    };

    public static AccountEntity? FromModel(AccountModel model) => model is null ? null : new()
    {
        Id = model.Id,
        CompanyId = model.CompanyId,
        AccountTypeId = model.AccountTypeId,
        DueDateType = model.DueDateType,
        Month = model.Month,
        Day = model.Day,
        IsPayable = model.IsPayable,
        IsClosed = model.IsClosed,
        ClosedDate = model.ClosedDate,
        IsAutoPaid = model.IsAutoPaid,
        Comments = model.Comments ?? string.Empty,
        Tag = model.Tag ?? string.Empty,
        AccountType = model.AccountType!,
        AccountNumber = model.AccountNumber!
    };

    public AccountModel Clone() => new()
    {
        Id = Id,
        CompanyId = CompanyId,
        AccountTypeId = AccountTypeId,
        DueDateType = DueDateType,
        Month = Month,
        Day = Day,
        IsPayable = IsPayable,
        IsClosed = IsClosed,
        ClosedDate = ClosedDate,
        IsAutoPaid= IsAutoPaid,
        Comments = Comments ?? string.Empty,
        Tag = Tag ?? string.Empty,
        AccountType = AccountType?.Clone(),
        AccountNumber = AccountNumber?.Clone(),
        CanDelete = CanDelete
    };

    public string Description(string password, IStringCypherService cypher)
    {
        if (string.IsNullOrWhiteSpace(password) || cypher is null || AccountType is null || AccountNumber is null)
        {
            return string.Empty;
        }
        var sb = new StringBuilder(AccountType.Description);
        sb.Append(' ');
        string number;
        try
        {
            number = cypher.Decrypt(AccountNumber.Number, password, AccountNumber.Salt);
        }
        catch
        {
            return string.Empty;
        }
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }
        if (number.Length <= 4)
        {
            sb.Append(number);
        }
        else
        {
            sb.Append(number[^4..]);
        }
        if (!string.IsNullOrWhiteSpace(Tag))
        {
            sb.Append($" ({Tag})");
        }
        return sb.ToString();
    }

    private readonly static List<string> _months = new()
    {
        "",
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    };

    public string DueDate()
    {
        var sb = new StringBuilder(DueDateType.GetDescriptionFromEnumValue());
        switch (DueDateType)
        {
            case DueDateType.Annnually:
                sb.Append(" on the ");
                sb.Append(Day.Ordinalize());
                sb.Append(" of ");
                sb.Append(_months[Month]);
                break;
            case DueDateType.Quarterly:
                sb.Append(" on the ");
                sb.Append(Day.Ordinalize());
                sb.Append(" of ");
                for (var i = 0; i < 12; i += 3)
                {
                    sb.Append(_months[(Month + i) % 12]);
                    if (i == 6)
                    {
                        sb.Append(" and ");
                    }
                    else if (i != 9)
                    {
                        sb.Append(", ");
                    }
                }
                break;
            case DueDateType.Monthly:
                sb.Append(" on the ");
                sb.Append(Day.Ordinalize());
                break;
            case DueDateType.SemiAnnual:
                sb.Append(" on the ");
                sb.Append(Day.Ordinalize());
                sb.Append(" of ");
                sb.Append(_months[Month]);
                sb.Append(" and ");
                sb.Append(_months[(Month + 6) % 12]);
                break;
        }
        return sb.ToString();
    }

    public override bool Equals(object? obj) => obj is AccountModel model && model.Id == Id;

    public bool Equals(AccountModel? model) => model is not null && model.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(AccountModel left, AccountModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.Id == right.Id
    };

    public static bool operator !=(AccountModel left, AccountModel right) => !(left == right);

    public static implicit operator AccountModel?(AccountEntity entity) => FromEntity(entity);

    public static implicit operator AccountEntity?(AccountModel model) => FromModel(model);
}

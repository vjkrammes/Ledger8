using Ledger8.Common;
using Ledger8.Common.Attributes;
using Ledger8.Common.Enumerations;
using Ledger8.Common.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

[HasNullableMembers]
public class AccountEntity : IIdEntity
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public int AccountTypeId { get; set; }
    [Required, NonNegative]
    public DueDateType DueDateType { get; set; }
    [Required, NonNegative]
    public int Month { get; set; }
    [Required, NonNegative]
    public int Day { get; set; }
    [Required]
    public bool IsPayable { get; set; }
    [Required]
    public bool IsClosed { get; set; }
    [Required]
    public DateTime ClosedDate { get; set; }
    [Required]
    public bool IsAutoPaid { get; set; }
    [Required]
    public string Comments { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Tag { get; set; }

    [NullOnInsertOrUpdate]
    public AccountTypeEntity? AccountType { get; set; }
    [NullOnInsertOrUpdate]
    public AccountNumberEntity? AccountNumber { get; set; }

    public AccountEntity()
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
}

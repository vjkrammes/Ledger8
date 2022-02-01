using Ledger8.Common;
using Ledger8.Common.Attributes;
using Ledger8.Common.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

public class PoolEntity : IIdEntity
{
    [Required]
    public int Id { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Name { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string Description { get; set; }
    [Required, Positive]
    public decimal Amount { get; set; }
    [Required, NonNegative]
    public decimal Balance { get; set; }

    public PoolEntity()
    {
        Id = 0;
        Name = string.Empty;
        Date = default;
        Description = string.Empty;
        Amount = 0M;
        Balance = 0M;
    }

    public override string ToString() => Name;
}

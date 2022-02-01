using Ledger8.Common;
using Ledger8.Common.Attributes;
using Ledger8.Common.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

public class TransactionEntity : IIdEntity
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required, Positive]
    public decimal Balance { get; set; }
    [Required, Positive]
    public decimal Payment { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Reference { get; set; }

    public TransactionEntity()
    {
        Id = 0;
        AccountId = 0;
        Date = default;
        Balance = 0M;
        Payment = 0M;
        Reference = string.Empty;
    }

    public override string ToString() => $"{Date.ToShortDateString()} {Balance:c2} paid {Payment:c2}";
}

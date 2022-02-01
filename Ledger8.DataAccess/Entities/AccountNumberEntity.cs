using Ledger8.Common.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

public class AccountNumberEntity : IIdEntity
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime StopDate { get; set; }
    [Required]
    public byte[]? Salt { get; set; }
    [Required]
    public string Number { get; set; }

    public AccountNumberEntity()
    {
        Id = 0;
        AccountId = 0;
        StartDate = DateTime.MinValue;
        StopDate = DateTime.MaxValue;
        Salt = null;
        Number = string.Empty;
    }
}

using Ledger8.Common;
using Ledger8.Common.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

public class AccountTypeEntity : IIdEntity
{
    [Required]
    public int Id { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Description { get; set; }

    public AccountTypeEntity()
    {
        Id = 0;
        Description = string.Empty;
    }

    public override string ToString() => Description;
}

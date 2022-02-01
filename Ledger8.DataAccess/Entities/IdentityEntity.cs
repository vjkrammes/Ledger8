using Ledger8.Common;
using Ledger8.Common.Attributes;
using Ledger8.Common.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

[HasNullableMembers]
public class IdentityEntity : IIdEntity
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required, MaxLength(Constants.UriLength)]
    public string URL { get; set; }
    [Required]
    public byte[]? UserSalt { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public byte[]? PasswordSalt { get; set; }
    [Required]
    public string Password { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Tag { get; set; }

    [NullOnInsertOrUpdate]
    public CompanyEntity? Company { get; set; }

    public IdentityEntity()
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
}

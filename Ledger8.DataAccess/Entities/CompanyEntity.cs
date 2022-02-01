using Ledger8.Common;
using Ledger8.Common.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

public class CompanyEntity : IIdEntity
{
    [Required]
    public int Id { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Name { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Address1 { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Address2 { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string City { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string State { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string PostalCode { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Phone { get; set; }
    [Required, MaxLength(Constants.UriLength)]
    public string URL { get; set; }
    [Required]
    public bool IsPayee { get; set; }
    [Required]
    public string Comments { get; set; }

    public CompanyEntity()
    {
        Id = 0;
        Name = string.Empty;
        Address1 = string.Empty;
        Address2 = string.Empty;
        City = string.Empty;
        State = string.Empty;
        PostalCode = string.Empty;
        Phone = string.Empty;
        URL = string.Empty;
        IsPayee = true;
        Comments = string.Empty;
    }

    public override string ToString() => Name;
}

using Ledger8.Common.Attributes;
using Ledger8.Common.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

[HasNullableMembers]
public class AllotmentEntity : IIdEntity
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int PoolId { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal Amount { get; set; }

    [NullOnInsertOrUpdate]
    public CompanyEntity? Company { get; set; }

    public AllotmentEntity()
    {
        Id = 0;
        PoolId = 0;
        CompanyId = 0;
        Date = default;
        Description = string.Empty;
        Amount = 0M;
        Company = null;
    }
}

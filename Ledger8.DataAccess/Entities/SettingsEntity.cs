using Ledger8.Common;
using Ledger8.DataAccess.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace Ledger8.DataAccess.Entities;

public class SettingsEntity
{ 
    [Required]
    public int Lock { get; set; }
    [Required]
    public Guid SystemId { get; set; }
    [Required, MaxLength(Constants.NameLength)]
    public string Theme { get; set; }
    [Required]
    public string BackupDirectory { get; set; }
    public byte[]? Salt { get; set; }
    public byte[]? Hash { get; set; }

    public SettingsEntity()
    {
        Lock = 0;
        SystemId = Guid.NewGuid();
        Theme = "Classic";
        BackupDirectory = string.Empty;
        Salt = null;
        Hash = null;
    }

    public override string ToString() => SystemId.ToString();

    public static SettingsEntity Default => new()
    {
        Lock = 1,
        SystemId = Guid.NewGuid(),
        Theme = "Classic",
        BackupDirectory = string.Empty,
        Salt = null,
        Hash = null
    };
}

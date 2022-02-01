
using System;

namespace Ledger8.DesktopUI.Interfaces;

public interface ISettings
{
    string BackupDirectory { get; set; }
    byte[]? Hash { get; set; }
    byte[]? Salt { get; set; }
    Guid SystemId { get; }
    void SetSaltAndHash(byte[] salt, byte[] hash);
}
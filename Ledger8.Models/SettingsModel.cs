using Ledger8.Common;
using Ledger8.DataAccess.Entities;

namespace Ledger8.Models;

public class SettingsModel : NotifyBase
{
    private int _lock;
    public int Lock
    {
        get => _lock;
        set => SetProperty(ref _lock, value);
    }


    private Guid _systemId;
    public Guid SystemId
    {
        get => _systemId;
        set => SetProperty(ref _systemId, value);
    }

    private string? _theme;
    public string Theme
    {
        get => _theme!;
        set => SetProperty(ref _theme, value);
    }

    private string? _backupDirectory;
    public string BackupDirectory
    {
        get => _backupDirectory!;
        set => SetProperty(ref _backupDirectory, value);
    }

    private byte[]? _salt;
    public byte[]? Salt
    {
        get => _salt;
        set => SetProperty(ref _salt, value);
    }

    private byte[]? _hash;
    public byte[]? Hash
    {
        get => _hash;
        set => SetProperty(ref _hash, value);
    }

    public SettingsModel()
    {
        Lock = 0;
        SystemId = Guid.Empty;
        BackupDirectory = string.Empty;
        Salt = null;
        Hash = null;
    }

    public static SettingsModel? FromEntity(SettingsEntity entity) => entity is null ? null : new()
    {
        Lock = entity.Lock,
        SystemId = entity.SystemId,
        BackupDirectory = entity.BackupDirectory ?? string.Empty,
        Salt = entity.Salt?.ArrayCopy(),
        Hash = entity.Hash?.ArrayCopy()
    };

    public static SettingsEntity? FromModel(SettingsModel model) => model is null ? null : new()
    {
        Lock = model.Lock,
        SystemId = model.SystemId,
        BackupDirectory = model.BackupDirectory ?? string.Empty,
        Salt = model.Salt?.ArrayCopy(),
        Hash = model.Hash?.ArrayCopy()
    };

    public SettingsModel Clone() => new()
    {
        Lock = Lock,
        SystemId = SystemId,
        BackupDirectory = BackupDirectory ?? string.Empty,
        Salt = Salt?.ArrayCopy(),
        Hash = Hash?.ArrayCopy()
    };

    public override string ToString() => SystemId.ToString();

    public override bool Equals(object? obj) => obj is SettingsModel model && model.SystemId == SystemId;

    public bool Equals(SettingsModel model) => model is not null && model.SystemId == SystemId;

    public override int GetHashCode() => SystemId.GetHashCode();

    public static bool operator ==(SettingsModel left, SettingsModel right) => (left, right) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        (_, _) => left.SystemId == right.SystemId
    };

    public static bool operator !=(SettingsModel left, SettingsModel right) => !(left == right);

    public static implicit operator SettingsModel?(SettingsEntity entity) => FromEntity(entity);

    public static implicit operator SettingsEntity?(SettingsModel model) => FromModel(model);
}

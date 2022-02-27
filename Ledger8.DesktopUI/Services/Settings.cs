using Ledger8.Common;
using Ledger8.DataAccess;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using System;

namespace Ledger8.DesktopUI.Services;

public class Settings : ISettings
{
    private readonly SettingsModel? _settings;
    private readonly ISettingsService _settingsService;

    public Settings(LedgerContext context, ISettingsService settingsService)
    {
        _settings = context?.GetSettings ?? throw new InvalidOperationException("No Settings");
        _settingsService = settingsService;
    }

    private void Persist()
    {
        var result = _settingsService.Update(_settings!);
        if (!result.Successful)
        {
            PopupManager.Popup("Failed to update settings", Constants.DBE, result.Message, PopupButtons.Ok, PopupImage.Error);
        }
    }

    public Guid SystemId => _settings!.SystemId;

    public string BackupDirectory
    {
        get => _settings!.BackupDirectory;
        set
        {
            _settings!.BackupDirectory = value;
            Persist();
        }
    }

    public byte[]? Hash
    {
        get => _settings!.Hash;
        set
        {
            _settings!.Hash = value?.ArrayCopy();
            Persist();
        }
    }

    public byte[]? Salt
    {
        get => _settings!.Salt;
        set
        {
            _settings!.Salt = value?.ArrayCopy();
            Persist();
        }
    }

    public void SetSaltAndHash(byte[] salt, byte[] hash)
    {
        _settings!.Salt = salt;
        _settings!.Hash = hash;
        Persist();
    }
}


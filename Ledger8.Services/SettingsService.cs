using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

namespace Ledger8.Services;

public class SettingsService : ISettingsService, IDataServiceTag
{
    private readonly ISettingsDal _settingsDal;

    public SettingsService(ISettingsDal settingsDal) => _settingsDal = settingsDal;

    public ApiError Update(SettingsModel model)
    {
        if (model is null)
        {
            return new(Strings.InvalidModel);
        }
        SettingsEntity entity = model!;
        try
        {
            return ApiError.FromDalResult(_settingsDal.Update(entity));
        }
        catch (Exception ex)
        {
            return ApiError.FromException(ex);
        }
    }

    public SettingsModel GetSettings()
    {
        var entity = _settingsDal.GetSettings();
        SettingsModel ret = entity!;
        return ret;
    }
}

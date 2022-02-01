using Ledger8.Common;
using Ledger8.DataAccess.Entities;

namespace Ledger8.DataAccess.Interfaces;

public interface ISettingsDal
{
    DalResult Update(SettingsEntity entity);
    SettingsEntity GetSettings();
}

using Ledger8.Common;
using Ledger8.Models;

namespace Ledger8.Services.Interfaces;

public interface ISettingsService
{
    ApiError Update(SettingsModel model);
    SettingsModel GetSettings();
}

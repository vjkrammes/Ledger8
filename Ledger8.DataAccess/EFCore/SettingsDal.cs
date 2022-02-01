using Ledger8.Common;
using Ledger8.DataAccess.Entities;
using Ledger8.DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Ledger8.DataAccess.EFCore;

public class SettingsDal : ISettingsDal
{
    private readonly LedgerContext _context;

    public SettingsDal(LedgerContext context) => _context = context;

    public DalResult Update(SettingsEntity entity)
    {
        try
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return DalResult.Success;
        }
        catch (Exception ex)
        {
            return DalResult.FromException(ex);
        }
    }

    public SettingsEntity GetSettings() => _context.GetSettings!;
}

using Ledger8.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ledger8.DataAccess;

public class LedgerContextFactory : IDesignTimeDbContextFactory<LedgerContext>
{
    public LedgerContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationFactory().Create(Constants.ConfigurationFilename, false);
        var optionsBuilder = new DbContextOptionsBuilder<LedgerContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString(Constants.ConnectionStringName));
        return new LedgerContext(optionsBuilder.Options);
    }
}

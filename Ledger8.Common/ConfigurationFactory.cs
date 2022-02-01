using Ledger8.Common.Interfaces;

using Microsoft.Extensions.Configuration;

namespace Ledger8.Common;

public class ConfigurationFactory : IConfigurationFactory
{
    public IConfiguration Create(string filename, bool isOptional = true, string? directory = null)
    {
        var ret = new ConfigurationBuilder()
            .SetBasePath(string.IsNullOrWhiteSpace(directory) ? Directory.GetCurrentDirectory() : directory)
            .AddJsonFile(filename, optional: isOptional, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        return ret;
    }
}

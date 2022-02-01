using Microsoft.Extensions.Configuration;

namespace Ledger8.Common.Interfaces;

public interface IConfigurationFactory
{
    IConfiguration? Create(string filename, bool isOptional = true, string? directory = null);
}

namespace Ledger8.Common.Interfaces;

public interface ISalter
{
    byte[] GenerateSalt(int length);
}

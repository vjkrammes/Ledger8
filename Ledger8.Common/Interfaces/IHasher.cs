namespace Ledger8.Common.Interfaces;

public interface IHasher
{
    byte[] GenerateHash(byte[] password, byte[] salt, int iterations, int length);
    byte[] GenerateHash(string password, byte[] salt, int iteraitons, int length);
}

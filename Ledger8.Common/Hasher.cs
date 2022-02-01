using Ledger8.Common.Interfaces;

using System.Security.Cryptography;
using System.Text;

namespace Ledger8.Common;

public sealed class Hasher : IHasher
{
    public byte[] GenerateHash(byte[] password, byte[] salt, int iterations, int length)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512);
        return deriveBytes.GetBytes(length);
    }

    public byte[] GenerateHash(string password, byte[] salt, int iterations, int length) =>
        GenerateHash(Encoding.UTF8.GetBytes(password), salt, iterations, length);
}

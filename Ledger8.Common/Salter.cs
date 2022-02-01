using Ledger8.Common.Interfaces;

using System.Security.Cryptography;

namespace Ledger8.Common;

public sealed class Salter : ISalter
{
    public byte[] GenerateSalt(int length)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be a number greater than zero");
        }
        var ret = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(ret);
        return ret;
    }
}

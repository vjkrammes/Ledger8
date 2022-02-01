using Ledger8.Common.Interfaces;

using System.Security.Cryptography;
using System.Text;

namespace Ledger8.Common;

public sealed class StringCypherService : IStringCypherService
{
    private readonly static byte[] _initializationVector = Encoding.UTF8.GetBytes(";'g9q=lGEu4}'P*G");
    private const int _keySize = 256;

    public string Encrypt(string plaintext, byte[] passphrase, byte[]? salt = null)
    {
        if (string.IsNullOrWhiteSpace(plaintext))
        {
            return string.Empty;
        }
        var ptbytes = Encoding.UTF8.GetBytes(plaintext);
        using var password = new PasswordDeriveBytes(passphrase, salt);
        var keybytes = password.GetBytes(_keySize / 8);
        using var symmetricKey = Aes.Create("AesManaged");
        if (symmetricKey is null)
        {
            throw new InvalidOperationException("Unable to create AES instance");
        }
        symmetricKey.Mode = CipherMode.CBC;
        symmetricKey.BlockSize = 128;
        using var encryptor = symmetricKey.CreateEncryptor(keybytes, _initializationVector);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(ptbytes, 0, ptbytes.Length);
        cs.FlushFinalBlock();
        var ctbytes = ms.ToArray();
        return Convert.ToBase64String(ctbytes);
    }

    public string Encrypt(string plaintext, string passphrase, byte[]? salt = null)
    {
        if (string.IsNullOrWhiteSpace(plaintext))
        {
            return string.Empty;
        }
        var ptbytes = Encoding.UTF8.GetBytes(plaintext);
        using var password = new PasswordDeriveBytes(passphrase, salt);
        var keybytes = password.GetBytes(_keySize / 8);
        using var symmetricKey = Aes.Create("AesManaged");
        if (symmetricKey is null)
        {
            throw new InvalidOperationException("Unable to create AES instance");
        }
        symmetricKey.Mode = CipherMode.CBC;
        symmetricKey.BlockSize = 128;
        using var encryptor = symmetricKey.CreateEncryptor(keybytes, _initializationVector);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(ptbytes, 0, ptbytes.Length);
        cs.FlushFinalBlock();
        var ctbytes = ms.ToArray();
        return Convert.ToBase64String(ctbytes);
    }

    public string Decrypt(string cyphertext, byte[] passphrase, byte[]? salt = null)
    {
        if (string.IsNullOrWhiteSpace(cyphertext))
        {
            return string.Empty;
        }
        var ctbytes = Convert.FromBase64String(cyphertext);
        using var password = new PasswordDeriveBytes(passphrase, salt);
        var keybytes = password.GetBytes(_keySize / 8);
        using var symmetricKey = Aes.Create("AesManaged");
        if (symmetricKey is null)
        {
            throw new InvalidOperationException("Unable to create AES instance");
        }
        symmetricKey.Mode = CipherMode.CBC;
        symmetricKey.BlockSize = 128;
        using var decryptor = symmetricKey.CreateDecryptor(keybytes, _initializationVector);
        using var ms = new MemoryStream(ctbytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        var ptbytes = new byte[ctbytes.Length];
        var totalRead = 0;
        while (totalRead < ptbytes.Length)
        {
            var bytesRead = cs.Read(ptbytes, totalRead, ptbytes.Length - totalRead);
            if (bytesRead == 0)
            {
                break;
            }
            totalRead += bytesRead;
        }
        return Encoding.UTF8.GetString(ptbytes, 0, totalRead);
    }

    public string Decrypt(string cyphertext, string passphrase, byte[]? salt = null)
    {
        if (string.IsNullOrWhiteSpace(cyphertext))
        {
            return string.Empty;
        }
        var ctbytes = Convert.FromBase64String(cyphertext);
        using var password = new PasswordDeriveBytes(passphrase, salt);
        var keybytes = password.GetBytes(_keySize / 8);
        using var symmetricKey = Aes.Create("AesManaged");
        if (symmetricKey is null)
        {
            throw new InvalidOperationException("Unable to create AES instance");
        }
        symmetricKey.Mode = CipherMode.CBC;
        symmetricKey.BlockSize = 128;
        using var decryptor = symmetricKey.CreateDecryptor(keybytes, _initializationVector);
        using var ms = new MemoryStream(ctbytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        var ptbytes = new byte[ctbytes.Length];
        var totalRead = 0;
        while (totalRead < ptbytes.Length)
        {
            var bytesRead = cs.Read(ptbytes, totalRead, ptbytes.Length - totalRead);
            if (bytesRead == 0)
            {
                break;
            }
            totalRead += bytesRead;
        }
        return Encoding.UTF8.GetString(ptbytes, 0, totalRead);
    }
}

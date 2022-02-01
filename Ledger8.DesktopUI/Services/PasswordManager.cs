using Ledger8.DesktopUI.Interfaces;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ledger8.DesktopUI.Services;

public class PasswordManager : IPasswordManager
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    private byte[]? _password;

    public PasswordManager()
    {
        _key = new byte[256 / 8];
        _iv = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(_key);
        rng.GetBytes(_iv);
    }

    public void Set(string password)
    {
        var pwbytes = Encoding.UTF8.GetBytes(password);
        using var key = Aes.Create("AesManaged");
        if (key is null)
        {
            throw new InvalidOperationException("Aes create failed");
        }
        key.Mode = CipherMode.CBC;
        key.BlockSize = 128;
        using var encryptor = key.CreateEncryptor(_key, _iv);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(pwbytes, 0, pwbytes.Length);
        cs.FlushFinalBlock();
        _password = ms.ToArray();
    }

    public string Get()
    {
        if (_password is null)
        {
            return string.Empty;
        }
        using var key = Aes.Create("AesManaged");
        if (key is null)
        {
            throw new InvalidOperationException("Aes create failed");
        }
        key.Mode = CipherMode.CBC;
        key.BlockSize = 128;
        using var decryptor = key.CreateDecryptor(_key, _iv);
        using var ms = new MemoryStream(_password);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        var ptbytes = new byte[_password.Length];
        var totalread = 0;
        while (totalread < ptbytes.Length)
        {
            var bytesRead = cs.Read(ptbytes, totalread, ptbytes.Length - totalread);
            if (bytesRead == 0)
            {
                break;
            }
            totalread += bytesRead;
        }
        return Encoding.UTF8.GetString(ptbytes).TrimEnd((char)0);
    }
}

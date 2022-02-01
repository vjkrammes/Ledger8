namespace Ledger8.Common.Interfaces;

public interface IStringCypherService
{
    string Encrypt(string plaintext, byte[] passphrase, byte[]? salt = null);
    string Encrypt(string plaintext, string passphrase, byte[]? salt = null);
    string Decrypt(string cyphertext, byte[] passphrase, byte[]? salt = null);
    string Decrypt(string cyphertext, string passphrase, byte[]? salt = null);
}

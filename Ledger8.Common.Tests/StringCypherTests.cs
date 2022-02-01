using Ledger8.Common.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Security.Cryptography;

namespace Ledger8.Common.Tests;

[TestClass]
public class StringCypherTests
{
    private readonly string _password = "T3$tP4$sw0rd";
    private readonly string _plaintext = "If it weren't for my horse, I never would have spent that year in college.";
    private readonly string _badPassword = "this won't work!";

    [TestMethod]
    [ExpectedException(typeof(CryptographicException))]
    public void TestStringCypherWithoutSalt()
    {
        IStringCypherService service = new StringCypherService();
        var cyphertext = service.Encrypt(_plaintext, _password);
        var decrypted = service.Decrypt(cyphertext, _password);
        Assert.AreEqual(_plaintext, decrypted);

        _ = service.Decrypt(cyphertext, _badPassword);
    }

    [TestMethod]
    [ExpectedException(typeof(CryptographicException))]
    public void TestStringCypherWithSalt()
    {
        IStringCypherService service = new StringCypherService();
        ISalter salter = new Salter();
        var salt = salter.GenerateSalt(64);
        var cyphertext = service.Encrypt(_plaintext, _password, salt);
        var decrypted = service.Decrypt(cyphertext, _password, salt);
        Assert.AreEqual(_plaintext, decrypted);

        salt = salter.GenerateSalt(64);
        _ = service.Decrypt(cyphertext, _password, salt);
    }
}

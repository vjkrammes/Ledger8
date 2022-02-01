using Ledger8.Common.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Ledger8.Common.Tests;

[TestClass]
public class UriHelperTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestUriHelperWithNoBase()
    {
        IUriHelper helper = new UriHelper();
        _ = helper.Create("testcontroller", "testaction");
    }

    [TestMethod]
    public void TestUriHelperWithBase()
    {
        var baseuri = "https://localhost:5001/";
        var version = 3;

        IUriHelper helper = new UriHelper();
        helper.SetBase(baseuri);
        helper.SetVersion(version);
        var uri = helper.Create("Test", "UriTest", 1);
        Assert.AreEqual("https://localhost:5001/api/v3/Test/UriTest/1/", uri.ToString());
    }
}

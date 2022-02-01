using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ledger8.Common.Tests;

[TestClass]
public class ApiErrorTests
{
    [TestMethod]
    public void TestApiError()
    {
        var error = ApiError.FromDalResult(DalResult.Success);
        Assert.IsTrue(error.Successful);
        error = ApiError.FromDalResult(DalResult.NotFound);
        Assert.IsFalse(error.Successful);
    }
}

﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ledger8.Common.Tests;

[TestClass]
public class ApiResultTests
{
    private readonly string _successValue = "This is a success result";
    private readonly int _failureValue = 42;

    [TestMethod]
    [ExpectedException(typeof(NotFailureException))]
    public void TestSuccessApiResult()
    {
        var success = new ApiResult<string, int>(_successValue);

        Assert.IsTrue(success.IsSuccessResult);
        Assert.AreEqual(_successValue, success.SuccessPayload);
        _ = success.FailurePayload;
    }

    [TestMethod]
    [ExpectedException(typeof(NotSuccessException))]
    public void TestFailureApiResult()
    {
        var failure = new ApiResult<string, int>(_failureValue);

        Assert.IsFalse(failure.IsSuccessResult);
        Assert.AreEqual(_failureValue, failure.FailurePayload);
        _ = failure.SuccessPayload;
    }
}

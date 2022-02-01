using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ledger8.Common.Tests;

[TestClass]
public class FormatDateTests
{
    [TestMethod]
    public void TestFormatDate()
    {
        var date1 = "this is not a valid date";
        var date2 = "20210630";
        var date3 = "20211520";

        Assert.AreEqual(date1, date1.FormatDate());
        Assert.AreEqual("June 30, 2021", date2.FormatDate());
        Assert.AreEqual(date3, date3.FormatDate());
    }
}

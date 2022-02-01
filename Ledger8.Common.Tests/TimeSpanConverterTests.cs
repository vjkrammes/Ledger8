using Ledger8.Common.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Ledger8.Common.Tests;

[TestClass]
public class TimeSpanConverterTests
{
    [TestMethod]
    public void TestTimeSpanConverter()
    {
        var msecSpec = "15msec";
        var secSpec = "12sec";
        var minSpec = "18 minutes";
        var hourSpec = "1h";
        var daySpec = "1d";
        var weekSpec = "2 weeks";

        ITimeSpanConverter converter = new TimeSpanConverter();

        var msec = converter.Convert(msecSpec);
        var sec = converter.Convert(secSpec);
        var min = converter.Convert(minSpec);
        var hour = converter.Convert(hourSpec);
        var day = converter.Convert(daySpec);
        var week = converter.Convert(weekSpec);

        Assert.AreEqual(TimeSpan.FromMilliseconds(15), msec);
        Assert.AreEqual(TimeSpan.FromSeconds(12), sec);
        Assert.AreEqual(TimeSpan.FromMinutes(18), min);
        Assert.AreEqual(TimeSpan.FromHours(1), hour);
        Assert.AreEqual(TimeSpan.FromDays(1), day);
        Assert.AreEqual(TimeSpan.FromDays(2 * 7), week);
    }
}

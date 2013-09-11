using NUnit.Framework;
using System;
using System.Threading;

namespace NodaTime.Humanization.Test.SingleUnit
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class DayTests
    {
        [Test]
        public void Can_Get_Relative_Time_For_One_Day()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 2, 0, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("a day", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneAndAHalf_Days()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 2, 12, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("1.5 days", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_Two_Days()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 3, 0, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("2 days", result);
        }
    }
}

using NUnit.Framework;

namespace NodaTime.Humanization.Test.SingleUnit
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class HourTests
    {
        [Test]
        public void GetRelativeTime_OneHour()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 1, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("an hour", result);
        }

        [Test]
        public void GetRelativeTime_OneAndAHalfHours()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 1, 30);

            var result = new Humanizer(PeriodUnits.Hours).GetRelativeTime(start, end);

            Assert.AreEqual("1.5 hours", result);
        }

        [Test]
        public void GetRelativeTime_TwoHours()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 2, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("2 hours", result);
        }
    }
}

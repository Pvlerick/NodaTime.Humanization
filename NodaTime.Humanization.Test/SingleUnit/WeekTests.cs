using NUnit.Framework;

namespace NodaTime.Humanization.Test.SingleUnit
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class WeekTests
    {
        [Test]
        public void GetRelativeTime_OneWeek()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 8, 0, 0);

            var result = new Humanizer(PeriodUnits.AllUnits).GetRelativeTime(start, end);

            Assert.AreEqual("a week", result);
        }

        [Test]
        public void GetRelativeTime_TwoWeeks()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 15, 0, 0);

            var result = new Humanizer(PeriodUnits.AllUnits).GetRelativeTime(start, end);

            Assert.AreEqual("2 weeks", result);
        }

        [Test]
        public void GetRelativeTime_OneAndAHalfWeeks_MaximumUnitsToDisplay_1()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 11, 12, 0); //Three days an 12 hours is half a week

            var result = new Humanizer(PeriodUnits.AllUnits, new HumanizerParameters.Builder().MaxiumumNumberOfUnitsToDisplay(1).Build()).GetRelativeTime(start, end);

            Assert.AreEqual("1.5 weeks", result);
        }

        [Test]
        public void GetRelativeTime_OneWeek_MaximumUnitsToDisplay_2()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 8, 0, 0);

            var result = new Humanizer(PeriodUnits.AllUnits, new HumanizerParameters.Builder().MaxiumumNumberOfUnitsToDisplay(2).Build()).GetRelativeTime(start, end);

            Assert.AreEqual("a week", result);
        }

        [Test]
        public void GetRelativeTime_OneWeek_UnitsToDisplay_Week()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 8, 0, 0);

            var result = new Humanizer(PeriodUnits.Weeks).GetRelativeTime(start, end);

            Assert.AreEqual("a week", result);
        }

        [Test]
        public void GetRelativeTime_OneAndAHalfWeeks_UnitsToDisplay_Week()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 11, 12, 0); //Three days an 12 hours is half a week

            var result = new Humanizer(PeriodUnits.Weeks).GetRelativeTime(start, end);

            Assert.AreEqual("1.5 weeks", result);
        }

        [Test]
        public void GetRelativeTime_TwoWeeks_UnitsToDisplay_Week()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 15, 0, 0);

            var result = new Humanizer(PeriodUnits.Weeks).GetRelativeTime(start, end);

            Assert.AreEqual("2 weeks", result);
        }
    }
}

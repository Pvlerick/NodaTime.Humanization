using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization.Test.FiveUnit
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class YearMonthDayHourMinuteTests
    {
        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months, 5 days, 10 hours and 12 minutes", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_MaximumUnitsToDisplay_1()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(1).GetRelativeTime(start, end);

            Assert.AreEqual("1.2 years", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_MaximumUnitsToDisplay_2()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(2).GetRelativeTime(start, end);

            Assert.AreEqual("a year and 2.2 months", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_MaximumUnitsToDisplay_3()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(3).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months and 5.4 days", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_MaximumUnitsToDisplay_4()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(4).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months, 5 days and 10.2 hours", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_MaximumUnitsToDisplay_5()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(5).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months, 5 days, 10 hours and 12 minutes", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_MaximumUnitsToDisplay_6()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(6).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months, 5 days, 10 hours and 12 minutes", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_UnitsToDisplay_YearMonthDay()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months and 5.4 days", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_UnitsToDisplay_YearDay()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Days).GetRelativeTime(start, end);

            Assert.AreEqual("a year and 64.4 days", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_UnitsToDisplay_MonthDay()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(PeriodUnits.Months | PeriodUnits.Days).GetRelativeTime(start, end);

            Assert.AreEqual("14 months and 5.4 days", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_UnitsToDisplay_Day()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(PeriodUnits.Days).GetRelativeTime(start, end);

            Assert.AreEqual("429.4 days", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_UnitsToDisplay_YearHours()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Hours).GetRelativeTime(start, end);

            Assert.AreEqual("a year and 1546.2 hours", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_UnitsToDisplay_YearSecond()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Seconds).GetRelativeTime(start, end);

            Assert.AreEqual("a year and 5566320 seconds", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_UnitsToDisplay_HourSecond()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(PeriodUnits.Hours | PeriodUnits.Seconds).GetRelativeTime(start, end);

            Assert.AreEqual("10306 hours and 720 seconds", result);
        }
    }
}

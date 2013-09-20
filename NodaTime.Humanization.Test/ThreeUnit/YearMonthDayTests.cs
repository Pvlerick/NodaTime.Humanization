using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization.Test.ThreeUnit
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class YearMonthDayTests
    {
        [Test]
        public void GetRelativeTime_TwoYearOneMonthSixDays()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2015, 2, 7, 0, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("2 years, a month and 6 days", result);
        }

        [Test]
        public void GetRelativeTime_OneYearOneMonthOneDay()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 2, 2, 0, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("a year, a month and a day", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDays_UnitsToDisplay_YearDay()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 0, 0);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Days).GetRelativeTime(start, end);

            Assert.AreEqual("a year and 64 days", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDays_UnitsToDisplay_YearMonth()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 0, 0);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Months).GetRelativeTime(start, end);

            Assert.AreEqual("a year and 2.2 months", result);
        }

        [Test]
        public void GetRelativeTime_OneYearTwoMonthsFiveDays_MaxiumumUnitsToDisplays_2()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 0, 0);

            var result = new Humanizer(2).GetRelativeTime(start, end);

            Assert.AreEqual("a year and 2.2 months", result);
        }

        [Test]
        public void GetRelativeTime_OneYearZeroMonthsFiveDays_UnitsToDisplay_YearMonthDay_DisplaySignificantZeroValueUnits()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 1, 6, 0, 0);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days , new HumanizerParameters.Builder().DisplaySignificantZeroValueUnits(true).Build()).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 0 months and 5 days", result);
        }

        [Test]
        public void GetRelativeTime_OneYearZeroMonthsFiveDays_MaxiumumUnitsToDisplay_2_DisplaySignificantZeroValueUnits()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 1, 6, 0, 0);

            var result = new Humanizer(2, new HumanizerParameters.Builder().DisplaySignificantZeroValueUnits(true).Build()).GetRelativeTime(start, end);

            Assert.AreEqual("a year and 0.2 months", result);
        }

        [Test]
        public void GetRelativeTime_ZeroYearZeroMonthsFiveDays_UnitsToDisplay_Day()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 6, 0, 0);

            var result = new Humanizer(PeriodUnits.Days, new HumanizerParameters.Builder().Build()).GetRelativeTime(start, end);

            Assert.AreEqual("5 days", result);
        }

        [Test]
        public void GetRelativeTime_ZeroYearZeroMonthsFiveDays_UnitsToDisplay_YearMonth()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 6, 0, 0);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Months, new HumanizerParameters.Builder().Build()).GetRelativeTime(start, end);

            Assert.AreEqual("0.4 month", result);
        }
    }
}

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
        public void Can_Get_Relative_Time_For_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months, 5 days, 10 hours and 12 minutes", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_UnitsToDisplay_YearMonthDay()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months and 5.4 days", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_MaximumUnitsToDisplay_3()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(3).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months and 5.4 days", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneYearTwoMonthsFiveDaysTenHoursTwelveMinutes_MaximumUnitsToDisplay_4()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 10, 12);

            var result = new Humanizer(4).GetRelativeTime(start, end);

            Assert.AreEqual("a year, 2 months, 5 days and 10.2 hours", result);
        }
    }
}

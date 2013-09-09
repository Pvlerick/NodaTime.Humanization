using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization.Test.ThreeUnit.Fr
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class YearMonthDayTests
    {
        [Test]
        public void Can_Get_Relative_Time_For_Two_Year_One_Month_Six_Days()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2015, 2, 7, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end, 3);

            Assert.AreEqual("2 years, a month and 6 days", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_One_Year_One_Month_One_Day()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 2, 2, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end, 3);

            Assert.AreEqual("a year, a month and a day", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_One_Year_Two_Months_Five_Days_Only_Display_YearsDays()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 3, 6, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end, PeriodUnits.Years | PeriodUnits.Days, 2);

            Assert.AreEqual("a year and 64 days", result);
        }
    }
}

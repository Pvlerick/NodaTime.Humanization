using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization.Test.ThreeUnit.Fr
{
    [TestFixture]
    [SetCulture("fr-FR")]
    [SetUICulture("fr")]
    public class YearMonthDayTests
    {
        [Test]
        public void GetRelativeTime_TwoYearOneMonthSixDays()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2015, 2, 7, 0, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("2 années et 1,2 mois", result);
        }

        [Test]
        public void GetRelativeTime_TwoYearOneMonthSixDays_MaximumNumberOfUnitsToDisplay_3()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2015, 2, 7, 0, 0);

            var result = new Humanizer(3).GetRelativeTime(start, end);

            Assert.AreEqual("2 années, un mois et 6 jours", result);
        }

        [Test]
        public void GetRelativeTime_OneYearOneMonthOneDay()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 2, 2, 0, 0);

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual("un an et un mois", result);
        }

        [Test]
        public void GetRelativeTime_OneYearOneMonthOneDay_MaximumNumberOfUnitsToDisplay_3()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 2, 2, 0, 0);

            var result = new Humanizer(3).GetRelativeTime(start, end);

            Assert.AreEqual("un an, un mois et un jour", result);
        }

        [Test]
        public void GetRelativeTime_TwoYearsFiveMonthsSixDays_UnitsToDisplay_YearsDays()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2015, 6, 7, 0, 0);

            var result = new Humanizer(PeriodUnits.Years | PeriodUnits.Days).GetRelativeTime(start, end);

            Assert.AreEqual("2 années et 157 jours", result);
        }
    }
}

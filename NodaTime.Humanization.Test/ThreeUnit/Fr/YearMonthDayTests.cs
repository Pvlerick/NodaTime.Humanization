using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization.Test.ThreeUnit
{
    [TestFixture]
    [SetCulture("fr-FR")]
    [SetUICulture("fr")]
    public class YearMonthDayTests
    {
        [Test]
        public void Can_Get_Relative_Time_For_Two_Year_One_Month_Six_Days()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2015, 2, 7, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end, 3);

            Assert.AreEqual("2 années, un mois et 6 jours", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_One_Year_One_Month_One_Day()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 2, 2, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end, 3);

            Assert.AreEqual("un an, un mois et un jour", result);
        }
    }
}

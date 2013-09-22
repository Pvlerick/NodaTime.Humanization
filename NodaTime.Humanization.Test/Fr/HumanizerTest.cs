using NodaTime.Text;
using NUnit.Framework;
using System;

namespace NodaTime.Humanization.Test.Fr
{
    [TestFixture]
    [SetCulture("fr-FR")]
    [SetUICulture("fr")]
    public class HumanizerTest
    {
        [Test]
        [TestCase("2013-01-01T00:00:00", "2013-02-01T00:00:00", "un mois")]
        [TestCase("2013-01-01T00:00:00", "2013-03-01T00:00:00", "2 mois")]
        public void GetRelativeTime(String startLdt, String endLdt, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, "un an et 2 mois")]
        public void GetRelativeTime_UnitsToDisplay(String startLdt, String endLdt, PeriodUnits unitsToDisplay, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(unitsToDisplay).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 1, "un an")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 2, "un an et 2 mois")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 3, "un an, 2 mois et 5 jours")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 4, "un an, 2 mois, 5 jours et 10 heures")]
        public void GetRelativeTime_MaxiumumNumberOfUnitsToDisplay(String startLdt, String endLdt, int maxiumumNumberOfUnitsToDisplay, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(maxiumumNumberOfUnitsToDisplay).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 1, "un an et 2,2 mois")]
        public void GetRelativeTime_DigitsAfterDecimalPoint(String startLdt, String endLdt, int digitsAfterDecimalPoint, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(new HumanizerParameters.Builder().DigitsAfterDecimalPoint(digitsAfterDecimalPoint).Build()).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, 3, "un an, 2 mois et 5 jours")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T10:12:00", PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds, 1, "10 heures")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T10:12:00", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, 1, "")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T10:12:00", PeriodUnits.Months | PeriodUnits.Days, 2, "")]
        public void GetRelativeTime_UnitsToDisplay_MaximumUnitsToDisplay(String startLdt, String endLdt, PeriodUnits unitsToDisplay, int maxiumumNumberOfUnitsToDisplay, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(unitsToDisplay, maxiumumNumberOfUnitsToDisplay).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Days, 1, "un an et 64,4 jours")]
        public void GetRelativeTime_UnitsToDisplay_DigitsAfterDecimalPoint(String startLdt, String endLdt, PeriodUnits unitsToDisplay, int digitsAfterDecimalPoint, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(unitsToDisplay, new HumanizerParameters.Builder().DigitsAfterDecimalPoint(digitsAfterDecimalPoint).Build()).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 1, 1, "1,2 ans")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 2, 1, "un an et 2,2 mois")]
        [TestCase("2013-01-01T00:00:00", "2014-03-14T10:12:00", 2, 2, "un an et 2,42 mois")]
        [TestCase("2013-01-01T00:00:00", "2014-03-14T10:12:00", 3, 1, "un an, 2 mois et 13,4 jours")]
        public void GetRelativeTime_MaximumUnitsToDisplay_DigitsAfterDecimalPoint(String startLdt, String endLdt, int maxiumumNumberOfUnitsToDisplay, int digitsAfterDecimalPoint, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(maxiumumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().DigitsAfterDecimalPoint(digitsAfterDecimalPoint).Build()).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, 3, 1, "un an, 2 mois et 5,4 jours")]
        public void GetRelativeTime_UnitsToDisplay_MaximumUnitsToDisplay_DigitsAfterDecimalPoint(String startLdt, String endLdt, PeriodUnits unitsToDisplay, int maxiumumNumberOfUnitsToDisplay, int digitsAfterDecimalPoint, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(unitsToDisplay, maxiumumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().DigitsAfterDecimalPoint(digitsAfterDecimalPoint).Build()).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }
    }
}

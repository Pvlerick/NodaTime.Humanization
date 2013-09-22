using NodaTime.Text;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization.Test
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class HumanizerTest
    {
        #region Public Members Tests
        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-01-01T00:00:00", "a year")]
        [TestCase("2013-01-01T00:00:00", "2015-01-01T00:00:00", "2 years")]
        [TestCase("2013-01-01T00:00:00", "2013-02-01T00:00:00", "a month")]
        [TestCase("2013-01-01T00:00:00", "2013-03-01T00:00:00", "2 months")]
        [TestCase("2013-01-01T00:00:00", "2013-01-02T00:00:00", "a day")]
        [TestCase("2013-01-01T00:00:00", "2013-01-03T00:00:00", "2 days")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T01:00:00", "an hour")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T02:00:00", "2 hours")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T00:01:00", "a minute")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T00:02:00", "2 minutes")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:01", "a second")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:02", "2 seconds")]
        //[TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:01.499", "a second")] // Should Round Down
        //[TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:01.500", "2 seconds")] // Should Round Up
        //[TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:00.499", "")] // Should Round Down
        //[TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:00.500", "a seconds")] // Should Round Up
        [TestCase("2013-01-01T00:00:00", "2014-02-01T10:12:00", "a year and a month")]
        [TestCase("2013-01-01T00:00:00", "2014-03-01T10:12:00", "a year and 2 months")]
        [TestCase("2013-01-01T00:00:00", "2014-01-03T10:12:00", "a year and 2 days")]
        public void GetRelativeTime(String startLdt, String endLdt, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer().GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, "a year and 2 months")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Days, "a year and 64 days")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Months | PeriodUnits.Days, "14 months and 5 days")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Days, "429 days")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Hours, "a year and 1546 hours")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Seconds, "a year and 5566320 seconds")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Hours | PeriodUnits.Seconds, "10306 hours and 720 seconds")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T10:12:00", PeriodUnits.Years | PeriodUnits.Months, "")]
        [TestCase("2013-01-01T00:00:00", "2013-01-08T00:00:00", PeriodUnits.Weeks, "a week")]
        [TestCase("2013-01-01T00:00:00", "2013-01-15T00:00:00", PeriodUnits.Weeks, "2 weeks")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:00", PeriodUnits.Weeks, "")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:00", PeriodUnits.Weeks, "")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:00", PeriodUnits.Weeks, "")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T00:00:00", PeriodUnits.Weeks, "")]
        public void GetRelativeTime_UnitsToDisplay(String startLdt, String endLdt, PeriodUnits unitsToDisplay, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(unitsToDisplay).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 1, "a year")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 2, "a year and 2 months")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 3, "a year, 2 months and 5 days")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 4, "a year, 2 months, 5 days and 10 hours")]
        public void GetRelativeTime_MaxiumumNumberOfUnitsToDisplay(String startLdt, String endLdt, int maxiumumNumberOfUnitsToDisplay, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(maxiumumNumberOfUnitsToDisplay).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 1, "a year and 2.2 months")]
        public void GetRelativeTime_DigitsAfterDecimalPoint(String startLdt, String endLdt, int digitsAfterDecimalPoint, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(new HumanizerParameters.Builder().DigitsAfterDecimalPoint(digitsAfterDecimalPoint).Build()).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, 3, "a year, 2 months and 5 days")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T10:12:00", PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds, 1, "10 hours")]
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
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Days, 1, "a year and 64.4 days")]
        [TestCase("2013-01-01T00:00:00", "2013-01-11T12:00:00", PeriodUnits.Weeks, 1, "1.5 weeks")] //Three days an 12 hours is half a week
        [TestCase("2013-01-01T00:00:00", "2013-01-11T12:00:00", PeriodUnits.AllUnits, 1, "a week and 3.5 days")]
        public void GetRelativeTime_UnitsToDisplay_DigitsAfterDecimalPoint(String startLdt, String endLdt, PeriodUnits unitsToDisplay, int digitsAfterDecimalPoint, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(unitsToDisplay, new HumanizerParameters.Builder().DigitsAfterDecimalPoint(digitsAfterDecimalPoint).Build()).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 1, 1, "1.2 years")]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", 2, 1, "a year and 2.2 months")]
        [TestCase("2013-01-01T00:00:00", "2014-03-14T10:12:00", 2, 2, "a year and 2.42 months")]
        [TestCase("2013-01-01T00:00:00", "2014-03-14T10:12:00", 3, 1, "a year, 2 months and 13.4 days")]
        [TestCase("2013-01-01T00:00:00", "2014-03-14T10:12:00", 4, 1, "a year, 2 months, 13 days and 10.2 hours")]
        [TestCase("2013-01-01T00:00:00", "2014-03-14T10:12:00", 5, 1, "a year, 2 months, 13 days, 10 hours and 12 minutes")]
        [TestCase("2013-01-01T00:00:00", "2014-03-14T10:12:00", 6, 1, "a year, 2 months, 13 days, 10 hours and 12 minutes")]
        [TestCase("2013-01-01T00:00:00", "2013-01-02T12:00:00", 1, 1, "1.5 days")]
        [TestCase("2013-01-01T00:00:00", "2013-01-03T12:00:00", 1, 1, "2.5 days")]
        [TestCase("2013-01-01T00:00:00", "2013-01-02T06:00:00", 1, 2, "1.25 days")]
        [TestCase("2013-01-01T00:00:00", "2013-01-01T02:30:00", 1, 2, "2.5 hours")]
        public void GetRelativeTime_MaximumUnitsToDisplay_DigitsAfterDecimalPoint(String startLdt, String endLdt, int maxiumumNumberOfUnitsToDisplay, int digitsAfterDecimalPoint, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(maxiumumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().DigitsAfterDecimalPoint(digitsAfterDecimalPoint).Build()).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("2013-01-01T00:00:00", "2014-03-06T10:12:00", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, 3, 1, "a year, 2 months and 5.4 days")]
        public void GetRelativeTime_UnitsToDisplay_MaximumUnitsToDisplay_DigitsAfterDecimalPoint(String startLdt, String endLdt, PeriodUnits unitsToDisplay, int maxiumumNumberOfUnitsToDisplay, int digitsAfterDecimalPoint, String expectedResult)
        {
            var start = LocalDateTimePattern.ExtendedIsoPattern.Parse(startLdt).Value;
            var end = LocalDateTimePattern.ExtendedIsoPattern.Parse(endLdt).Value;

            var result = new Humanizer(unitsToDisplay, maxiumumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().DigitsAfterDecimalPoint(digitsAfterDecimalPoint).Build()).GetRelativeTime(start, end);

            Assert.AreEqual(expectedResult, result);
        }

        #endregion

        #region Internal Members Tests

        [Test]
        [TestCase(PeriodUnits.Years, PeriodUnits.Years | PeriodUnits.Months)]
        [TestCase(PeriodUnits.Years | PeriodUnits.Months, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days)]
        [TestCase(PeriodUnits.Hours | PeriodUnits.Minutes, PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds)]
        [TestCase(PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds, PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds)]
        [TestCase(PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds)]
        [TestCase(PeriodUnits.Years | PeriodUnits.Hours, PeriodUnits.Years | PeriodUnits.Hours | PeriodUnits.Minutes)]
        [TestCase(PeriodUnits.Years | PeriodUnits.Seconds, PeriodUnits.Years | PeriodUnits.Seconds | PeriodUnits.Milliseconds)]
        [TestCase(PeriodUnits.Years | PeriodUnits.Seconds | PeriodUnits.Milliseconds, PeriodUnits.Years | PeriodUnits.Seconds | PeriodUnits.Milliseconds)]
        [TestCase(PeriodUnits.Years | PeriodUnits.Milliseconds, PeriodUnits.Years | PeriodUnits.Milliseconds)]
        public void GetRequiredUnitsForRounding(PeriodUnits parameter, PeriodUnits expectedResult)
        {
            var result = new Humanizer(parameter).GetRequiredUnitsForRounding();

            Assert.AreEqual(expectedResult, result);
        }

        //See http://nodatime.org/1.1.x/userguide/period-patterns.html for patterns
        [Test]
        [TestCase("P1Y", PeriodUnits.Years)]
        [TestCase("P1Y1M", PeriodUnits.Years | PeriodUnits.Months)]
        [TestCase("P1Y1M1W1D", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days)]
        [TestCase("P1Y1M1D", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days)]
        [TestCase("P1Y1D", PeriodUnits.Years | PeriodUnits.Days)]
        [TestCase("P1DT1H", PeriodUnits.Days | PeriodUnits.Hours)]
        [TestCase("P1DT1H1M", PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes)]
        [TestCase("P1DT1H1M1S", PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds)]
        [TestCase("P1YT1S", PeriodUnits.Years | PeriodUnits.Seconds)]
        [TestCase("P1YT1S1s", PeriodUnits.Years | PeriodUnits.Seconds | PeriodUnits.Milliseconds)]
        [TestCase("P1YT1s", PeriodUnits.Years | PeriodUnits.Milliseconds)]
        public void GetSignificantUnitsForPeriod(String periodText, PeriodUnits expectedResult)
        {
            var result = new Humanizer(8).GetSignificantUnitsForPeriod(PeriodPattern.RoundtripPattern.Parse(periodText).Value);

            Assert.AreEqual(expectedResult, result);
        }

        //See http://nodatime.org/1.1.x/userguide/period-patterns.html for patterns
        [Test]
        [TestCase("P1Y", 1, PeriodUnits.Years)]
        [TestCase("P1Y1M", 1, PeriodUnits.Years)]
        [TestCase("P1Y1M1W1D", 2, PeriodUnits.Years | PeriodUnits.Months)]
        [TestCase("P1Y1M1W1D", 3, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks)]
        [TestCase("P1Y1M1W1D", 4, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days)]
        [TestCase("P1Y1M1D", 2, PeriodUnits.Years | PeriodUnits.Months)]
        [TestCase("P1Y1D", 2, PeriodUnits.Years | PeriodUnits.Days)]
        [TestCase("P1DT1H", 1, PeriodUnits.Days)]
        [TestCase("P1DT1H1M", 2, PeriodUnits.Days | PeriodUnits.Hours)]
        [TestCase("P1DT1H1M1S", 3, PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes)]
        [TestCase("P1YT1S", 2, PeriodUnits.Years | PeriodUnits.Seconds)]
        [TestCase("P1YT1S", 1, PeriodUnits.Years)]
        [TestCase("P1YT1S1s", 2, PeriodUnits.Years | PeriodUnits.Seconds)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 1, PeriodUnits.Years)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 2, PeriodUnits.Years | PeriodUnits.Months)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 3, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 4, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 5, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 6, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 7, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 8, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds)]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 125, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds)]
        public void GetSignificantUnitsForPeriod_MaxiumumNumberOfUnitsToDisplay(String periodText, int maximumNumberOfUnitsToDisplay, PeriodUnits expectedResult)
        {
            var result = new Humanizer(maximumNumberOfUnitsToDisplay).GetSignificantUnitsForPeriod(PeriodPattern.RoundtripPattern.Parse(periodText).Value);

            Assert.AreEqual(expectedResult, result);
        }

        //See http://nodatime.org/1.1.x/userguide/period-patterns.html for patterns
        [Test]
        [TestCase("P1Y1MT1H", PeriodUnits.Years | PeriodUnits.Months)]
        [TestCase("P1YT1s", PeriodUnits.Years | PeriodUnits.Months)]
        [TestCase("PT1H1S", PeriodUnits.Hours | PeriodUnits.Minutes)]
        public void GetSignificantUnitsForPeriod_DisplaySignificantZeroValueUnits(String periodText, PeriodUnits expectedResult)
        {
            var result = new Humanizer(new HumanizerParameters.Builder().DisplaySignificantZeroValueUnits(true).Build()).GetSignificantUnitsForPeriod(PeriodPattern.RoundtripPattern.Parse(periodText).Value);

            Assert.AreEqual(expectedResult, result);
        }

        //See http://nodatime.org/1.1.x/userguide/period-patterns.html for patterns
        [Test]
        [TestCase("P1Y1MT1H", 3, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks)]
        [TestCase("P1Y1MT1H", 4, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days)]
        [TestCase("P1Y1MT1H", 5, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours)]
        [TestCase("P1Y1MT1H", 6, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours)]
        [TestCase("P1YT1s", 2, PeriodUnits.Years | PeriodUnits.Months)]
        [TestCase("P1YT1s", 5, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours)]
        [TestCase("P1YT1s", 8, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds)]
        public void GetSignificantUnitsForPeriod_MaxiumumNumberOfUnitsToDisplay_DisplaySignificantZeroValueUnits(String periodText, int maximumNumberOfUnitsToDisplay, PeriodUnits expectedResult)
        {
            var result = new Humanizer(maximumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().DisplaySignificantZeroValueUnits(true).Build()).GetSignificantUnitsForPeriod(PeriodPattern.RoundtripPattern.Parse(periodText).Value);

            Assert.AreEqual(expectedResult, result);
        }

        #endregion
    }
}

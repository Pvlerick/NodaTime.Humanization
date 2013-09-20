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
        [Test]
        [TestCase(PeriodUnits.Years, PeriodUnits.Years | PeriodUnits.Months, Description = "Years -> Years | Months")]
        [TestCase(PeriodUnits.Years | PeriodUnits.Months, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, Description = "Years | Months -> Years | Months | Days")]
        [TestCase(PeriodUnits.Hours | PeriodUnits.Minutes, PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds, Description = "Hours | Minutes -> Hours | Minutes | Seconds")]
        [TestCase(PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds, PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "Hours | Minutes | Seconds -> Hours | Minutes | Seconds | Milliseconds")]
        [TestCase(PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "Hours | Minutes | Seconds | Milliseconds -> Hours | Minutes | Seconds | Milliseconds")]
        [TestCase(PeriodUnits.Years | PeriodUnits.Hours, PeriodUnits.Years | PeriodUnits.Hours | PeriodUnits.Minutes, Description = "Years | Hours -> Years | Hours | Minutes")]
        [TestCase(PeriodUnits.Years | PeriodUnits.Seconds, PeriodUnits.Years | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "Years | Seconds -> Years Seconds | Milliseconds")]
        [TestCase(PeriodUnits.Years | PeriodUnits.Seconds | PeriodUnits.Milliseconds, PeriodUnits.Years | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "Years | Seconds | Milliseconds -> Years | Seconds | Milliseconds")]
        [TestCase(PeriodUnits.Years | PeriodUnits.Milliseconds, PeriodUnits.Years | PeriodUnits.Milliseconds, Description = "Years | Milliseconds -> Years | Milliseconds")]
        public void GetRequiredUnitsForRounding(PeriodUnits parameter, PeriodUnits expectedResult)
        {
            var result = new Humanizer(parameter).GetRequiredUnitsForRounding();

            Assert.AreEqual(expectedResult, result);
        }

        //See http://nodatime.org/1.1.x/userguide/period-patterns.html for patterns
        [Test]
        [TestCase("P1Y", PeriodUnits.Years, Description = "P1Y1M -> Years")]
        [TestCase("P1Y1M", PeriodUnits.Years | PeriodUnits.Months, Description = "P1Y1M -> Years | Months")]
        [TestCase("P1Y1M1W1D", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days, Description = "P1Y1M1D -> Years | Months | Weeks | Days")]
        [TestCase("P1Y1M1D", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days, Description = "P1Y1M1D -> Years | Months | Days")]
        [TestCase("P1Y1D", PeriodUnits.Years | PeriodUnits.Days, Description = "P1Y1D -> Years | Days")]
        [TestCase("P1DT1H", PeriodUnits.Days | PeriodUnits.Hours, Description = "P1DT1H -> Days | Hours")]
        [TestCase("P1DT1H1M", PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes, Description = "P1DT1H1M -> Days | Hours | Minutes")]
        [TestCase("P1DT1H1M1S", PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds, Description = "P1DT1H1M1S -> Days | Hours | Minutes | Seconds")]
        [TestCase("P1YT1S", PeriodUnits.Years | PeriodUnits.Seconds, Description = "P1YT1S -> Years | Seconds")]
        [TestCase("P1YT1S1s", PeriodUnits.Years | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "P1YT1S1s -> Years | Seconds | Milliseconds")]
        [TestCase("P1YT1s", PeriodUnits.Years | PeriodUnits.Milliseconds, Description = "P1YT1s -> Years | Milliseconds")]
        public void GetSignificantUnitsForPeriod(String periodText, PeriodUnits expectedResult)
        {
            var result = new Humanizer(8).GetSignificantUnitsForPeriod(PeriodPattern.RoundtripPattern.Parse(periodText).Value);

            Assert.AreEqual(expectedResult, result);
        }

        //See http://nodatime.org/1.1.x/userguide/period-patterns.html for patterns
        [Test]
        [TestCase("P1Y", 1, PeriodUnits.Years, Description = "P1Y1M & 1 -> Years")]
        [TestCase("P1Y1M", 1, PeriodUnits.Years, Description = "P1Y1M & 1 -> Years")]
        [TestCase("P1Y1M1W1D", 2, PeriodUnits.Years | PeriodUnits.Months, Description = "P1Y1M1D & 2 -> Years | Months")]
        [TestCase("P1Y1M1W1D", 3, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks, Description = "P1Y1M1D & 3 -> Years | Months | Weeks")]
        [TestCase("P1Y1M1W1D", 4, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days, Description = "P1Y1M1D & 4 -> Years | Months | Weeks | Days")]
        [TestCase("P1Y1M1D", 2, PeriodUnits.Years | PeriodUnits.Months, Description = "P1Y1M1D & 2 -> Years | Months")]
        [TestCase("P1Y1D", 2, PeriodUnits.Years | PeriodUnits.Days, Description = "P1Y1D & 2 -> Years | Days")]
        [TestCase("P1DT1H", 1, PeriodUnits.Days, Description = "P1DT1H & 1 -> Days")]
        [TestCase("P1DT1H1M", 2, PeriodUnits.Days | PeriodUnits.Hours, Description = "P1DT1H1M & 2 -> Days | Hours")]
        [TestCase("P1DT1H1M1S", 3, PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes, Description = "P1DT1H1M1S & 3 -> Days | Hours | Minutes")]
        [TestCase("P1YT1S", 2, PeriodUnits.Years | PeriodUnits.Seconds, Description = "P1YT1S -> Years | Seconds")]
        [TestCase("P1YT1S", 1, PeriodUnits.Years, Description = "P1YT1S -> Years")]
        [TestCase("P1YT1S1s", 2, PeriodUnits.Years | PeriodUnits.Seconds, Description = "P1YT1S1s & 2 -> Years | Seconds")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 1, PeriodUnits.Years, Description = "P1Y1M1W1DT1H1M1S1s & 1 -> Years")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 2, PeriodUnits.Years | PeriodUnits.Months, Description = "P1Y1M1W1DT1H1M1S1s & 2 -> Years | Months")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 3, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks, Description = "P1Y1M1W1DT1H1M1S1s & 3 -> Years | Months | Weeks")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 4, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days, Description = "P1Y1M1W1DT1H1M1S1s & 4 -> Years | Months | Weeks | Days")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 5, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours, Description = "P1Y1M1W1DT1H1M1S1s & 5 -> Years | Months | Weeks | Days | Hours")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 6, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes, Description = "P1Y1M1W1DT1H1M1S1s & 6 -> Years | Months | Weeks | Days | Hours | Minutes")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 7, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds, Description = "P1Y1M1W1DT1H1M1S1s & 7 -> Years | Months | Weeks | Days | Hours | Minutes | Seconds")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 8, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "P1Y1M1W1DT1H1M1S1s & 8 -> Years | Months | Weeks | Days | Hours | Minutes | Seconds | Milliseconds")]
        [TestCase("P1Y1M1W1DT1H1M1S1s", 125, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "P1Y1M1W1DT1H1M1S1s & 125 -> Years | Months | Weeks | Days | Hours | Minutes | Seconds | Milliseconds")]
        public void GetSignificantUnitsForPeriod_MaxiumumNumberOfUnitsToDisplay(String periodText, int maximumNumberOfUnitsToDisplay, PeriodUnits expectedResult)
        {
            var result = new Humanizer(maximumNumberOfUnitsToDisplay).GetSignificantUnitsForPeriod(PeriodPattern.RoundtripPattern.Parse(periodText).Value);

            Assert.AreEqual(expectedResult, result);
        }

        //See http://nodatime.org/1.1.x/userguide/period-patterns.html for patterns
        [Test]
        [TestCase("P1Y1MT1H", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days | PeriodUnits.Hours, Description = "P1Y1MT1H -> Years | Months | Days | Hours")]
        [TestCase("P1YT1s", PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "P1YT1s -> Years")]

        //[TestCase("P1Y1D", 2, PeriodUnits.Years | PeriodUnits.Days, Description = "P1Y1D & 2 -> Years | Days")]
        //[TestCase("P1DT1H", 1, PeriodUnits.Days, Description = "P1DT1H & 1 -> Days")]
        //[TestCase("P1DT1H1M", 2, PeriodUnits.Days | PeriodUnits.Hours, Description = "P1DT1H1M & 2 -> Days | Hours")]
        //[TestCase("P1DT1H1M1S", 3, PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes, Description = "P1DT1H1M1S & 3 -> Days | Hours | Minutes")]
        //[TestCase("P1YT1S", 2, PeriodUnits.Years | PeriodUnits.Seconds, Description = "P1YT1S -> Years | Seconds")]
        //[TestCase("P1YT1S", 1, PeriodUnits.Years, Description = "P1YT1S -> Years")]
        //[TestCase("P1YT1S1s", 2, PeriodUnits.Years | PeriodUnits.Seconds, Description = "P1YT1S1s & 2 -> Years | Seconds")]
        //[TestCase("P1Y1M1W1DT1H1M1S1s", 2, PeriodUnits.Years | PeriodUnits.Months, Description = "P1Y1M1W1DT1H1M1S1s & 2 -> Years | Months")]
        //[TestCase("P1Y1M1W1DT1H1M1S1s", 3, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks, Description = "P1Y1M1W1DT1H1M1S1s & 3 -> Years | Months | Weeks")]
        //[TestCase("P1Y1M1W1DT1H1M1S1s", 4, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days, Description = "P1Y1M1W1DT1H1M1S1s & 4 -> Years | Months | Weeks | Days")]
        //[TestCase("P1Y1M1W1DT1H1M1S1s", 5, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours, Description = "P1Y1M1W1DT1H1M1S1s & 5 -> Years | Months | Weeks | Days | Hours")]
        //[TestCase("P1Y1M1W1DT1H1M1S1s", 6, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes, Description = "P1Y1M1W1DT1H1M1S1s & 6 -> Years | Months | Weeks | Days | Hours | Minutes")]
        //[TestCase("P1Y1M1W1DT1H1M1S1s", 7, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds, Description = "P1Y1M1W1DT1H1M1S1s & 7 -> Years | Months | Weeks | Days | Hours | Minutes | Seconds")]
        //[TestCase("P1Y1M1W1DT1H1M1S1s", 8, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "P1Y1M1W1DT1H1M1S1s & 8 -> Years | Months | Weeks | Days | Hours | Minutes | Seconds | Milliseconds")]
        //[TestCase("P1Y1M1W1DT1H1M1S1s", 125, PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Weeks | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds | PeriodUnits.Milliseconds, Description = "P1Y1M1W1DT1H1M1S1s & 125 -> Years | Months | Weeks | Days | Hours | Minutes | Seconds | Milliseconds")]
        public void GetSignificantUnitsForPeriod_DisplaySignificantZeroValueUnits(String periodText, PeriodUnits expectedResult)
        {
            var result = new Humanizer(new HumanizerParameters.Builder().DisplaySignificantZeroValueUnits(true).Build()).GetSignificantUnitsForPeriod(PeriodPattern.RoundtripPattern.Parse(periodText).Value);

            Assert.AreEqual(expectedResult, result);
        }
    }
}

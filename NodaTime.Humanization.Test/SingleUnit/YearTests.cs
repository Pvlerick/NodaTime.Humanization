using NUnit.Framework;

namespace NodaTime.Humanization.Test.SingleUnit
{
    [TestFixture]
    public class YearTests
    {
        [Test]
        public void Can_Get_Relative_Time_For_One_Year()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 1, 1, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("a year", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneAndAHalf_Years()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2014, 7, 1, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("1.5 years", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_Two_Years()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2015, 1, 1, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("2 years", result);
        }
    }
}

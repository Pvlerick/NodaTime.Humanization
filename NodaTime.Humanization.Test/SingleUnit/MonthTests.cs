using NUnit.Framework;

namespace NodaTime.Humanization.Test.SingleUnit
{
    [TestFixture]
    public class MonthTests
    {
        [Test]
        public void Can_Get_Relative_Time_For_One_Month()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 2, 1, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("a month", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneAndAHalf_Months()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 2, 15, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("1.5 months", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_Two_Months()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 3, 1, 0, 0);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("2 months", result);
        }
    }
}

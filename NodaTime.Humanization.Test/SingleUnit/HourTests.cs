using NUnit.Framework;

namespace NodaTime.Humanization.Test.SingleUnit
{
    [TestFixture]
    public class HourTests
    {
        [Test]
        public void Can_Get_Relative_Time_For_One_Hour()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 1, 0);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("an hour", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneAndAHalf_Hours()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 1, 30);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("1.5 hours", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_Two_Hours()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 2, 0);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("2 hours", result);
        }
    }
}

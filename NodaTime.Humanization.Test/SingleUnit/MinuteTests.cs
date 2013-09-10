using NUnit.Framework;

namespace NodaTime.Humanization.Test.SingleUnit
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class MinuteTests
    {
        [Test]
        public void Can_Get_Relative_Time_For_One_Minute()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 0, 1);

            var result = new Humanizer(1).GetRelativeTime(start, end);

            Assert.AreEqual("a minute", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneAndAHalf_Minutes()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 0, 1, 30);

            var result = new Humanizer(1).GetRelativeTime(start, end);

            Assert.AreEqual("1.5 minutes", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_Two_Minutes()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 0, 2);

            var result = new Humanizer(1).GetRelativeTime(start, end);

            Assert.AreEqual("2 minutes", result);
        }
    }
}

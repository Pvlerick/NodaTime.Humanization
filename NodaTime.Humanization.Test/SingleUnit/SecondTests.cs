using NUnit.Framework;

namespace NodaTime.Humanization.Test.SingleUnit
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en")]
    public class SecondTests
    {
        [Test]
        public void Can_Get_Relative_Time_For_One_Second()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 0, 0, 1);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("a second", result);
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneAndAHalf_Seconds_Round_Down()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 0, 0, 1, 499);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("a second", result); // Should Round Down
        }

        [Test]
        public void Can_Get_Relative_Time_For_OneAndAHalf_Seconds_Round_Up()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 0, 0, 1, 500);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("2 seconds", result); // Should Round Up
        }

        [Test]
        public void Can_Get_Relative_Time_For_Two_Seconds()
        {
            var start = new LocalDateTime(2013, 1, 1, 0, 0, 0);
            var end = new LocalDateTime(2013, 1, 1, 0, 0, 2);

            var result = Humanizer.GetRelativeTime(start, end);

            Assert.AreEqual("2 seconds", result);
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="TimeProviderExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Bcl.Tests
{
    public class TimeProviderExtensionsTest
    {
        [Fact]
        public void GetUtcToday()
        {
            var timeProvider = new Mock<TimeProvider>(MockBehavior.Strict);
            timeProvider.Setup(tp => tp.GetUtcNow())
                .Returns(new DateTime(2025, 1, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc));

            var result = timeProvider.Object.GetUtcToday();

            result.Should().Be(new DateOnly(2025, 1, 2));

            timeProvider.VerifyAll();
        }

        [Theory]
        [InlineData(23, 30, 2, 3)]
        [InlineData(0, 30, -2, 1)]
        [InlineData(12, 0, 2, 2)]
        public void GetLocalToday_UsesProviderTimeZone(int hour, int minute, int offsetHours, int expectedDay)
        {
            var timeProvider = new Mock<TimeProvider>(MockBehavior.Strict);
            timeProvider.Setup(tp => tp.GetUtcNow())
                .Returns(new DateTimeOffset(2025, 1, 2, hour, minute, 0, TimeSpan.Zero));
            timeProvider.Setup(tp => tp.LocalTimeZone)
                .Returns(TimeZoneInfo.CreateCustomTimeZone("Test", TimeSpan.FromHours(offsetHours), "Test", "Test"));

            timeProvider.Object.GetLocalToday().Should().Be(new DateOnly(2025, 1, expectedDay));
            timeProvider.Object.GetUtcToday().Should().Be(new DateOnly(2025, 1, 2));
            timeProvider.VerifyAll();
        }

        [Fact]
        public void GetUtcToday_NullTimeProvider()
        {
            Action act = () => TimeProviderExtensions.GetUtcToday(null);

            act.Should().ThrowExactly<ArgumentNullException>().WithParameterName("timeProvider");
        }

        [Fact]
        public void GetLocalToday_NullTimeProvider()
        {
            Action act = () => TimeProviderExtensions.GetLocalToday(null);

            act.Should().ThrowExactly<ArgumentNullException>().WithParameterName("timeProvider");
        }
    }
}
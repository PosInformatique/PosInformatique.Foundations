//-----------------------------------------------------------------------
// <copyright file="StringExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Bcl.Tests
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("Beam Report", 4, "Beam")]
        [InlineData("Beam Report", 11, "Beam Report")]
        [InlineData("Beam Report", 12, "Beam Report")]
        [InlineData("Beam Report", 0, "")]
        public void Truncate(string value, int maxLength, string expected)
        {
            var result = value.Truncate(maxLength);

            result.Should().Be(expected);
        }

        [Fact]
        public void Truncate_NegativeMaxLength()
        {
            Action act = () => "Beam Report".Truncate(-1);

            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithMessage("*maxLength*")
                .WithParameterName("maxLength");
        }

        [Fact]
        public void Truncate_NullValue()
        {
            Action act = () => StringExtensions.Truncate(null, 1);

            act.Should().ThrowExactly<ArgumentNullException>().WithParameterName("value");
        }
    }
}
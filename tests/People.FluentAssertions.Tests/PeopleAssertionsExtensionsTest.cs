//-----------------------------------------------------------------------
// <copyright file="PeopleAssertionsExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.FluentAssertions.Tests
{
    using Xunit.Sdk;

    public class PeopleAssertionsExtensionsTest
    {
        [Fact]
        public void FirstName_Be()
        {
            var firstName = FirstName.Create("john");

            firstName.Should().Be(FirstName.Create("john"))
                .And.NotBeNull();
            firstName.Should().Be("John")
                .And.NotBeNull();
        }

        [Theory]
        [InlineData(null, null, "Expected string to be \"john\", but \"John\" differs near \"Joh\" (index 0).")]
        [InlineData("Because {0}", 10, "Expected string to be \"john\" Because 10, but \"John\" differs near \"Joh\" (index 0).")]
        public void FirstName_BeFailed(string because, object becauseArgs, string expectedMessage)
        {
            var firstName = FirstName.Create("John");

            firstName.Should().Invoking(f => f.Be("john", because, becauseArgs))
                .Should().ThrowExactly<XunitException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public void LastName_Be()
        {
            var lastName = LastName.Create("Doe");

            lastName.Should().Be(LastName.Create("doe"))
                .And.NotBeNull();
            lastName.Should().Be("DOE")
                .And.NotBeNull();
        }

        [Theory]
        [InlineData(null, null, "Expected string to be \"doe\", but \"DOE\" differs near \"DOE\" (index 0).")]
        [InlineData("Because {0}", 10, "Expected string to be \"doe\" Because 10, but \"DOE\" differs near \"DOE\" (index 0).")]
        public void LastName_BeFailed(string because, object becauseArgs, string expectedMessage)
        {
            var lastName = LastName.Create("Doe");

            lastName.Should().Invoking(f => f.Be("doe", because, becauseArgs))
                .Should().ThrowExactly<XunitException>()
                .WithMessage(expectedMessage);
        }
    }
}

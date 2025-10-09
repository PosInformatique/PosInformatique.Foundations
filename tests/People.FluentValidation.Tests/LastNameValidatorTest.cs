//-----------------------------------------------------------------------
// <copyright file="LastNameValidatorTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.FluentValidation.Tests
{
    using global::FluentValidation.Validators;

    public class LastNameValidatorTest
    {
        [Fact]
        public void Constructor()
        {
            var validator = new LastNameValidator<object>();

            validator.Name.Should().Be("LastNameValidator");
        }

        [Fact]
        public void GetDefaultMessageTemplate()
        {
            var validator = new LastNameValidator<object>();

            validator.As<IPropertyValidator>().GetDefaultMessageTemplate(default).Should().Be("'{PropertyName}' must contain a last name that consists only of alphabetic characters, with the [' ', '-'] separators, and is less than 50 characters long.");
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidLastNames), MemberType = typeof(NameTestData))]
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        public void IsValid_True(string lastName, string _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            var validator = new LastNameValidator<object>();

            validator.IsValid(default, lastName).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(NameTestData.InvalidLastNames), MemberType = typeof(NameTestData))]
        public void IsValid_False(string lastName)
        {
            var validator = new LastNameValidator<object>();

            validator.IsValid(default, lastName).Should().BeFalse();
        }
    }
}

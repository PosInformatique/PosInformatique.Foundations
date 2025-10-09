//-----------------------------------------------------------------------
// <copyright file="FirstNameValidatorTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.FluentValidation.Tests
{
    using global::FluentValidation.Validators;

    public class FirstNameValidatorTest
    {
        [Fact]
        public void Constructor()
        {
            var validator = new FirstNameValidator<object>();

            validator.Name.Should().Be("FirstNameValidator");
        }

        [Fact]
        public void GetDefaultMessageTemplate()
        {
            var validator = new FirstNameValidator<object>();

            validator.As<IPropertyValidator>().GetDefaultMessageTemplate(default).Should().Be("'{PropertyName}' must contain a first name that consists only of alphabetic characters, with the [' ', '-'] separators, and is less than 50 characters long.");
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidFirstNames), MemberType = typeof(NameTestData))]
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        public void IsValid_True(string firstName, string _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            var validator = new FirstNameValidator<object>();

            validator.IsValid(default, firstName).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(NameTestData.InvalidFirstNames), MemberType = typeof(NameTestData))]
        public void IsValid_False(string firstName)
        {
            var validator = new FirstNameValidator<object>();

            validator.IsValid(default, firstName).Should().BeFalse();
        }
    }
}

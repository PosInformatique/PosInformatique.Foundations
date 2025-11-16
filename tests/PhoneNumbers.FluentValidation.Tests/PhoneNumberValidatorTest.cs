//-----------------------------------------------------------------------
// <copyright file="PhoneNumberValidatorTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation.Tests
{
    using FluentValidation.Validators;
    using PosInformatique.Foundations.PhoneNumbers;

    public class PhoneNumberValidatorTest
    {
        [Fact]
        public void Constructor()
        {
            var validator = new PhoneNumberValidator<object>();

            validator.Name.Should().Be("PhoneNumberValidator");
        }

        [Fact]
        public void GetDefaultMessageTemplate()
        {
            var validator = new PhoneNumberValidator<object>();

            validator.As<IPropertyValidator>().GetDefaultMessageTemplate(default).Should().Be("'{PropertyName}' must be a valid phone number in E.164 format.");
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.ValidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        public void IsValid_True(string phoneNumber, string _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            var validator = new PhoneNumberValidator<object>();

            validator.IsValid(default, phoneNumber).Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithNull()
        {
            var validator = new PhoneNumberValidator<object>();

            validator.IsValid(default!, null!).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.InvalidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void IsValid_False(string phoneNumber)
        {
            var validator = new PhoneNumberValidator<object>();

            validator.IsValid(default, phoneNumber).Should().BeFalse();
        }
    }
}
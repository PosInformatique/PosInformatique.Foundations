//-----------------------------------------------------------------------
// <copyright file="EmailAddressValidatorTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation.Tests
{
    using FluentValidation.Validators;
    using PosInformatique.Foundations;

    public class EmailAddressValidatorTest
    {
        [Fact]
        public void Constructor()
        {
            var validator = new EmailAddressValidator<object>();

            validator.Name.Should().Be("EmailAddressValidator");
        }

        [Fact]
        public void GetDefaultMessageTemplate()
        {
            var validator = new EmailAddressValidator<object>();

            validator.As<IPropertyValidator>().GetDefaultMessageTemplate(default).Should().Be("'{PropertyName}' must be a valid email address.");
        }

        [Theory]
        [MemberData(nameof(EmailAddressTestData.ValidEmailAddresses), MemberType = typeof(EmailAddressTestData))]
        public void IsValid_True(string emailAddress)
        {
            var validator = new EmailAddressValidator<object>();

            validator.IsValid(default!, emailAddress).Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithNull()
        {
            var validator = new EmailAddressValidator<object>();

            validator.IsValid(default!, null!).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(EmailAddressTestData.InvalidEmailAddresses), MemberType = typeof(EmailAddressTestData))]
        public void IsValid_False(string emailAddress)
        {
            var validator = new EmailAddressValidator<object>();

            validator.IsValid(default!, emailAddress).Should().BeFalse();
        }
    }
}

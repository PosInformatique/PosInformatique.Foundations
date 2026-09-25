//-----------------------------------------------------------------------
// <copyright file="EmailingOptionsValidatorTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using Microsoft.Extensions.Options;
    using PosInformatique.Foundations.EmailAddresses;

    public class EmailingOptionsValidatorTest
    {
        [Fact]
        public void Validate_WithSenderEmailAddress()
        {
            var options = new EmailingOptions
            {
                SenderEmailAddress = EmailAddress.Parse("user@domain.com"),
            };

            var validator = new EmailingOptionsValidator();

            var result = validator.Validate(null, options);

            result.Succeeded.Should().BeTrue();
            result.Failed.Should().BeFalse();
            result.FailureMessage.Should().BeNull();
            result.Failures.Should().BeNull();
        }

        [Fact]
        public void Validate_WithoutSenderEmailAddress()
        {
            var options = new EmailingOptions();

            var validator = new EmailingOptionsValidator();

            var result = validator.Validate(null, options);

            result.Succeeded.Should().BeFalse();
            result.Failed.Should().BeTrue();
            result.FailureMessage.Should().Be("The SenderEmailAddress property must be provided when configuring the emailing feature with EmailingOptions.");
            result.Failures.Should().ContainSingle("The SenderEmailAddress property must be provided when configuring the emailing feature with EmailingOptions.");
        }
    }
}

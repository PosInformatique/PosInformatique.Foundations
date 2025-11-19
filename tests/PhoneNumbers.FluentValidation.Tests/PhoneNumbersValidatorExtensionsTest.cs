//-----------------------------------------------------------------------
// <copyright file="PhoneNumbersValidatorExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation.Tests
{
    public class PhoneNumbersValidatorExtensionsTest
    {
        [Fact]
        public void MustBePhoneNumber()
        {
            var options = Mock.Of<IRuleBuilderOptions<object, string>>(MockBehavior.Strict);

            var ruleBuilder = new Mock<IRuleBuilder<object, string>>(MockBehavior.Strict);
            ruleBuilder.Setup(rb => rb.SetValidator(It.IsNotNull<PhoneNumberValidator<object>>()))
                .Returns(options);

            ruleBuilder.Object.MustBePhoneNumber().Should().BeSameAs(options);

            ruleBuilder.VerifyAll();
        }

        [Fact]
        public void MustBePhoneNumber_NullRuleBuilderArgument()
        {
            var act = () =>
            {
                PhoneNumbersValidatorExtensions.MustBePhoneNumber((IRuleBuilder<int, string>)null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("ruleBuilder");
        }
    }
}
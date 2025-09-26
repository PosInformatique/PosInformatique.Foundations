//-----------------------------------------------------------------------
// <copyright file="EmailAddressValidatorExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation.Tests
{
    public class EmailAddressValidatorExtensionsTest
    {
        [Fact]
        public void MustBeEmailAddress()
        {
            var options = Mock.Of<IRuleBuilderOptions<object, string>>(MockBehavior.Strict);

            var ruleBuilder = new Mock<IRuleBuilder<object, string>>(MockBehavior.Strict);
            ruleBuilder.Setup(rb => rb.SetValidator(It.IsNotNull<EmailAddressValidator<object>>()))
                .Returns(options);

            ruleBuilder.Object.MustBeEmailAddress().Should().BeSameAs(options);

            ruleBuilder.VerifyAll();
        }
    }
}

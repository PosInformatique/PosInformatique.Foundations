//-----------------------------------------------------------------------
// <copyright file="NameValidatorExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation.Tests
{
    using PosInformatique.Foundations.People;

    public class NameValidatorExtensionsTest
    {
        [Fact]
        public void MustBeFirstName()
        {
            var options = Mock.Of<IRuleBuilderOptions<object, string>>(MockBehavior.Strict);

            var ruleBuilder = new Mock<IRuleBuilder<object, string>>(MockBehavior.Strict);
            ruleBuilder.Setup(rb => rb.SetValidator(It.IsNotNull<FirstNameValidator<object>>()))
                .Returns(options);

            ruleBuilder.Object.MustBeFirstName().Should().BeSameAs(options);

            ruleBuilder.VerifyAll();
        }

        [Fact]
        public void MustBeFirstName_NullRuleBuilderArgument()
        {
            var act = () =>
            {
                NameValidatorExtensions.MustBeFirstName((IRuleBuilder<int, string>)null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("ruleBuilder");
        }

        [Fact]
        public void MustBeLastName()
        {
            var options = Mock.Of<IRuleBuilderOptions<object, string>>(MockBehavior.Strict);

            var ruleBuilder = new Mock<IRuleBuilder<object, string>>(MockBehavior.Strict);
            ruleBuilder.Setup(rb => rb.SetValidator(It.IsNotNull<LastNameValidator<object>>()))
                .Returns(options);

            ruleBuilder.Object.MustBeLastName().Should().BeSameAs(options);

            ruleBuilder.VerifyAll();
        }

        [Fact]
        public void MustBeLastName_NullRuleBuilderArgument()
        {
            var act = () =>
            {
                NameValidatorExtensions.MustBeLastName((IRuleBuilder<int, string>)null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("ruleBuilder");
        }
    }
}

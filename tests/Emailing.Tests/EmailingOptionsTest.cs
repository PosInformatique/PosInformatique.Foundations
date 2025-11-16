//-----------------------------------------------------------------------
// <copyright file="EmailingOptionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.EmailAddresses;
    using PosInformatique.Foundations.Text.Templating;

    public class EmailingOptionsTest
    {
        [Fact]
        public void Constructor()
        {
            var options = new EmailingOptions();

            options.SenderEmailAddress.Should().BeNull();
        }

        [Fact]
        public void SenderEmailAddress_ValueChanged()
        {
            var options = new EmailingOptions();

            options.SenderEmailAddress = EmailAddress.Parse("user@domain.com");

            options.SenderEmailAddress.Should().Be(EmailAddress.Parse("user@domain.com"));
        }

        [Fact]
        public void RegisterTemplate()
        {
            var identifier = EmailTemplateIdentifier<Model>.Create();
            var template = new EmailTemplate<Model>(Mock.Of<TextTemplate<Model>>(MockBehavior.Strict), Mock.Of<TextTemplate<Model>>(MockBehavior.Strict));

            var options = new EmailingOptions();

            options.RegisterTemplate(identifier, template);

            options.GetTemplate(identifier).Should().BeSameAs(template);
        }

        [Fact]
        public void RegisterTemplate_NullIdentifier()
        {
            var options = new EmailingOptions();

            options.Invoking(o => o.RegisterTemplate<Model>(null, default))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("identifier");
        }

        [Fact]
        public void RegisterTemplate_AlreadyRegistered()
        {
            var identifier = EmailTemplateIdentifier<Model>.Create();

            var template = new EmailTemplate<Model>(Mock.Of<TextTemplate<Model>>(MockBehavior.Strict), Mock.Of<TextTemplate<Model>>(MockBehavior.Strict));
            var otherTemplate = new EmailTemplate<Model>(Mock.Of<TextTemplate<Model>>(MockBehavior.Strict), Mock.Of<TextTemplate<Model>>(MockBehavior.Strict));

            var options = new EmailingOptions();

            options.RegisterTemplate(identifier, template);

            options.Invoking(opt => opt.RegisterTemplate(identifier, otherTemplate))
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("An e-mail template with the same identifier has already been registered. (Parameter 'identifier')")
                .WithParameterName("identifier");
        }

        [Fact]
        public void RegisterTemplate_NullTemplate()
        {
            var identifier = EmailTemplateIdentifier<Model>.Create();

            var options = new EmailingOptions();

            options.Invoking(o => o.RegisterTemplate(identifier, null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("template");
        }

        [Fact]
        public void GetTemplate_NotRegistered()
        {
            var identifier = EmailTemplateIdentifier<Model>.Create();

            var options = new EmailingOptions();

            options.GetTemplate(identifier).Should().BeNull();
        }

        public sealed class Model
        {
        }
    }
}
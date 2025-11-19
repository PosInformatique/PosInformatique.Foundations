//-----------------------------------------------------------------------
// <copyright file="EmailTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.Text.Templating;

    public class EmailTest
    {
        [Fact]
        public void Constructor()
        {
            var subject = Mock.Of<TextTemplate<Model>>(MockBehavior.Strict);
            var htmlContent = Mock.Of<TextTemplate<Model>>(MockBehavior.Strict);

            var template = new EmailTemplate<Model>(subject, htmlContent);

            var email = new Email<Model>(template);

            email.Importance.Should().Be(EmailImportance.Normal);
            email.Recipients.Should().BeEmpty();
            email.Template.Should().BeSameAs(template);
        }

        [Fact]
        public void Constructor_WithNullTemplate()
        {
            var act = () =>
            {
                new Email<Model>(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("template");
        }

        [Fact]
        public void Importance_ValueChanged()
        {
            var subject = Mock.Of<TextTemplate<Model>>(MockBehavior.Strict);
            var htmlContent = Mock.Of<TextTemplate<Model>>(MockBehavior.Strict);

            var template = new EmailTemplate<Model>(subject, htmlContent);

            var email = new Email<Model>(template);

            email.Importance = EmailImportance.High;

            email.Importance.Should().Be(EmailImportance.High);
        }

        internal sealed class Model
        {
        }
    }
}
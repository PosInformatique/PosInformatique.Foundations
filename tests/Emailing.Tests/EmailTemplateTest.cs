//-----------------------------------------------------------------------
// <copyright file="EmailTemplateTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.Text.Templating;

    public class EmailTemplateTest
    {
        [Fact]
        public void Constructor()
        {
            var subject = Mock.Of<TextTemplate<Model>>(MockBehavior.Strict);
            var htmlBody = Mock.Of<TextTemplate<Model>>(MockBehavior.Strict);

            var template = new EmailTemplate<Model>(subject, htmlBody);

            template.HtmlBody.Should().BeSameAs(htmlBody);
            template.Subject.Should().BeSameAs(subject);
        }

        [Fact]
        public void Constructor_WithNullSubject()
        {
            var act = () =>
            {
                new EmailTemplate<Model>(null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("subject");
        }

        [Fact]
        public void Constructor_WithNullHtmlBody()
        {
            var subject = Mock.Of<TextTemplate<Model>>(MockBehavior.Strict);

            var act = () =>
            {
                new EmailTemplate<Model>(subject, null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("htmlBody");
        }

        internal sealed class Model
        {
        }
    }
}
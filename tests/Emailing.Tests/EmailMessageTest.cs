//-----------------------------------------------------------------------
// <copyright file="EmailMessageTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.EmailAddresses;

    public class EmailMessageTest
    {
        [Fact]
        public void Constructor()
        {
            var from = new EmailContact(EmailAddress.Parse("from@domain.com"), "From");
            var to = new EmailContact(EmailAddress.Parse("to@domain.com"), "To");

            var emailMessage = new EmailMessage(
                from,
                to,
                "The subject",
                "HTML content");

            emailMessage.Attachments.Should().BeEmpty();
            emailMessage.From.Should().Be(from);
            emailMessage.Importance.Should().Be(EmailImportance.Normal);
            emailMessage.HtmlContent.Should().Be("HTML content");
            emailMessage.Subject.Should().Be("The subject");
            emailMessage.To.Should().Be(to);
        }

        [Fact]
        public void Constructor_WithNullFrom()
        {
            var act = () =>
            {
                new EmailMessage(null, default, default, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("from");
        }

        [Fact]
        public void Constructor_WithNullTo()
        {
            var from = new EmailContact(EmailAddress.Parse("from@domain.com"), "From");

            var act = () =>
            {
                new EmailMessage(from, null, default, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("to");
        }

        [Fact]
        public void Constructor_WithNullSubject()
        {
            var from = new EmailContact(EmailAddress.Parse("from@domain.com"), "From");
            var to = new EmailContact(EmailAddress.Parse("to@domain.com"), "To");

            var act = () =>
            {
                new EmailMessage(from, to, null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("subject");
        }

        [Fact]
        public void Constructor_WithNullHtmlContent()
        {
            var from = new EmailContact(EmailAddress.Parse("from@domain.com"), "From");
            var to = new EmailContact(EmailAddress.Parse("to@domain.com"), "To");

            var act = () =>
            {
                new EmailMessage(from, to, "The subject", null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("htmlContent");
        }

        [Fact]
        public void Importance_ValueChanged()
        {
            var from = new EmailContact(EmailAddress.Parse("from@domain.com"), "From");
            var to = new EmailContact(EmailAddress.Parse("to@domain.com"), "To");

            var emailMessage = new EmailMessage(
                from,
                to,
                "The subject",
                "HTML content");

            emailMessage.Importance = EmailImportance.High;

            emailMessage.Importance.Should().Be(EmailImportance.High);
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="MailjetEmailingIntegrationTests.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Mailjet.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using PosInformatique.Foundations.EmailAddresses;

    public class MailjetEmailingIntegrationTests
    {
        private readonly EmailingIntegrationTests tests;

        public MailjetEmailingIntegrationTests()
        {
            var configuration = new EmailingIntegrationTestsConfiguration("MailjetEmailingIntegrationTests.local.settings.json");

            this.tests = new EmailingIntegrationTests(
                emailingBuilder => emailingBuilder.UseMailjet(
                    configuration["EMAILING_MAILJET_API_KEY"],
                    configuration["EMAILING_MAILJET_API_SECRET"]),
                EmailAddress.Parse(configuration["EMAILING_MAILJET_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(configuration["EMAILING_MAILJET_RECIPIENT_EMAIL_ADDRESS"]));
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public async Task SendEmailAsync()
#pragma warning restore S2699 // Tests should include assertions
        {
            await this.tests.SendEmailAsync();
        }
    }
}
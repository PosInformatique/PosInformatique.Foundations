//-----------------------------------------------------------------------
// <copyright file="GraphEmailingIntegrationTests.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Graph.Tests
{
    using Azure.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PosInformatique.Foundations.EmailAddresses;

    public class GraphEmailingIntegrationTests
    {
        private readonly EmailingIntegrationTests tests;

        public GraphEmailingIntegrationTests()
        {
            if (!File.Exists("GraphEmailingIntegrationTests.local.settings.json"))
            {
                return;
            }

            var configuration = new EmailingIntegrationTestsConfiguration("GraphEmailingIntegrationTests.local.settings.json");

            var credentials = new ClientSecretCredential(
                configuration["EMAILING_GRAPH_TENANT_ID"],
                configuration["EMAILING_GRAPH_CLIENT_ID"],
                configuration["EMAILING_GRAPH_CLIENT_SECRET"]);

            this.tests = new EmailingIntegrationTests(
                emailingBuilder => emailingBuilder.UseGraph(credentials),
                EmailAddress.Parse(configuration["EMAILING_GRAPH_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(configuration["EMAILING_GRAPH_RECIPIENT_EMAIL_ADDRESS"]));
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public async Task SendEmailAsync()
#pragma warning restore S2699 // Tests should include assertions
        {
            if (this.tests is null)
            {
                return;
            }

            await this.tests.SendEmailAsync();
        }
    }
}
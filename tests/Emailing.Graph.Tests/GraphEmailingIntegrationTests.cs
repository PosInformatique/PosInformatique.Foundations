//-----------------------------------------------------------------------
// <copyright file="GraphEmailingIntegrationTests.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Graph.Tests
{
    using Azure.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Graph.Models.ODataErrors;
    using PosInformatique.Foundations.EmailAddresses;

    public class GraphEmailingIntegrationTests
    {
        private readonly EmailingIntegrationTestsConfiguration configuration;

        public GraphEmailingIntegrationTests()
        {
            if (!File.Exists("GraphEmailingIntegrationTests.local.settings.json"))
            {
                return;
            }

            this.configuration = new EmailingIntegrationTestsConfiguration("GraphEmailingIntegrationTests.local.settings.json");
        }

        [Fact]
        public async Task SendEmailAsync()
        {
            if (this.configuration is null)
            {
                return;
            }

            var credentials = new ClientSecretCredential(
                this.configuration["EMAILING_GRAPH_TENANT_ID"],
                this.configuration["EMAILING_GRAPH_CLIENT_ID"],
                this.configuration["EMAILING_GRAPH_CLIENT_SECRET"]);

            await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseGraph(credentials),
                EmailAddress.Parse(this.configuration["EMAILING_GRAPH_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(this.configuration["EMAILING_GRAPH_RECIPIENT_EMAIL_ADDRESS"]),
                manuallyRunOnly: true);
        }

        [Fact]
        public async Task SendEmailAsync_WithWrongCredentials()
        {
            if (this.configuration is null)
            {
                return;
            }

            var credentials = new ClientSecretCredential(
                "00000000-0000-0000-0000-000000000000",
                this.configuration["EMAILING_GRAPH_CLIENT_ID"],
                this.configuration["EMAILING_GRAPH_CLIENT_SECRET"]);

            var action = async () => await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseGraph(credentials),
                EmailAddress.Parse(this.configuration["EMAILING_GRAPH_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(this.configuration["EMAILING_GRAPH_RECIPIENT_EMAIL_ADDRESS"]));

            var exception = await action.Should().ThrowExactlyAsync<EmailProviderException>()
                .WithMessage("ClientSecretCredential authentication failed: ");

            exception
                .WithInnerExceptionExactly<AuthenticationFailedException>()
                .WithMessage("ClientSecretCredential authentication failed: ");
        }

        [Fact]
        public async Task SendEmailAsync_WithWrongSender()
        {
            if (this.configuration is null)
            {
                return;
            }

            var credentials = new ClientSecretCredential(
                this.configuration["EMAILING_GRAPH_TENANT_ID"],
                this.configuration["EMAILING_GRAPH_CLIENT_ID"],
                this.configuration["EMAILING_GRAPH_CLIENT_SECRET"]);

            var action = async () => await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseGraph(credentials),
                "invalid_sender@domain.com",
                EmailAddress.Parse(this.configuration["EMAILING_GRAPH_RECIPIENT_EMAIL_ADDRESS"]));

            var exception = await action.Should().ThrowExactlyAsync<EmailProviderException>()
                .WithMessage("The requested user 'invalid_sender@domain.com' is invalid.");

            exception
                .WithInnerExceptionExactly<ODataError>()
                .WithMessage("The requested user 'invalid_sender@domain.com' is invalid.");
        }
    }
}
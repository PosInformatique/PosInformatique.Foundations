//-----------------------------------------------------------------------
// <copyright file="AzureEmailingIntegrationTests.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Azure.Tests
{
    using global::Azure;
    using global::Azure.Identity;
    using Microsoft.Extensions.Azure;
    using Microsoft.Extensions.DependencyInjection;
    using PosInformatique.Foundations.EmailAddresses;

    public class AzureEmailingIntegrationTests
    {
        private readonly EmailingIntegrationTestsConfiguration configuration;

        public AzureEmailingIntegrationTests()
        {
            if (!File.Exists("AzureEmailingIntegrationTests.local.settings.json"))
            {
                return;
            }

            this.configuration = new EmailingIntegrationTestsConfiguration("AzureEmailingIntegrationTests.local.settings.json");
        }

        [Fact]
        public async Task SendEmailAsync()
        {
            if (this.configuration is null)
            {
                return;
            }

            await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseAzureCommunicationService(
                    new Uri(this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_ENDPOINT_URL"]),
                    clientBuilder =>
                    {
                        var credentialsOptions = new DefaultAzureCredentialOptions()
                        {
                            ExcludeEnvironmentCredential = true,
                            ExcludeWorkloadIdentityCredential = true,
                            ExcludeManagedIdentityCredential = true,
                            TenantId = this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_TENANT_ID"],
                        };

                        clientBuilder.WithCredential(new DefaultAzureCredential(credentialsOptions));
                    }),
                EmailAddress.Parse(this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_RECIPIENT_EMAIL_ADDRESS"]),
                manuallyRunOnly: true);
        }

        [Fact]
        public async Task SendEmailAsync_WithInvalidCredentials()
        {
            if (this.configuration is null)
            {
                return;
            }

            var action = async () => await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseAzureCommunicationService(
                    new Uri(this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_ENDPOINT_URL"]),
                    clientBuilder =>
                    {
                        var credentials = new ClientSecretCredential(
                            "00000000-0000-0000-0000-000000000000",
                            "00000000-0000-0000-0000-000000000000",
                            "Wrong secret");

                        clientBuilder.WithCredential(credentials);
                    }),
                EmailAddress.Parse(this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_RECIPIENT_EMAIL_ADDRESS"]));

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

            var action = async () => await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseAzureCommunicationService(
                    new Uri(this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_ENDPOINT_URL"]),
                    clientBuilder =>
                    {
                        var credentialsOptions = new DefaultAzureCredentialOptions()
                        {
                            ExcludeEnvironmentCredential = true,
                            ExcludeWorkloadIdentityCredential = true,
                            ExcludeManagedIdentityCredential = true,
                            TenantId = this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_TENANT_ID"],
                        };

                        clientBuilder.WithCredential(new DefaultAzureCredential(credentialsOptions));
                    }),
                "invalid_sender@domain.com",
                EmailAddress.Parse(this.configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_RECIPIENT_EMAIL_ADDRESS"]));

            var exception = await action.Should().ThrowExactlyAsync<EmailProviderException>()
                .WithMessage("The specified sender domain has not been linked.*");

            exception
                .WithInnerExceptionExactly<RequestFailedException>()
                .WithMessage("The specified sender domain has not been linked.*");
        }
    }
}
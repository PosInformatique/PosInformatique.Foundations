//-----------------------------------------------------------------------
// <copyright file="AzureEmailingIntegrationTests.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Azure.Tests
{
    using global::Azure.Identity;
    using Microsoft.Extensions.Azure;
    using Microsoft.Extensions.DependencyInjection;
    using PosInformatique.Foundations.EmailAddresses;

    public class AzureEmailingIntegrationTests
    {
        private readonly EmailingIntegrationTests tests;

        public AzureEmailingIntegrationTests()
        {
            var configuration = new EmailingIntegrationTestsConfiguration("AzureEmailingIntegrationTests.local.settings.json");

            this.tests = new EmailingIntegrationTests(
                emailingBuilder => emailingBuilder.UseAzureCommunicationService(
                    new Uri(configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_ENDPOINT_URL"]),
                    clientBuilder =>
                    {
                        var credentialsOptions = new DefaultAzureCredentialOptions()
                        {
                            ExcludeEnvironmentCredential = true,
                            ExcludeWorkloadIdentityCredential = true,
                            ExcludeManagedIdentityCredential = true,
                            TenantId = configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_TENANT_ID"],
                        };

                        clientBuilder.WithCredential(new DefaultAzureCredential(credentialsOptions));
                    }),
                EmailAddress.Parse(configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(configuration["EMAILING_AZURE_COMMUNICATION_SERVICES_RECIPIENT_EMAIL_ADDRESS"]));
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
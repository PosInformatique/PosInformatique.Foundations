//-----------------------------------------------------------------------
// <copyright file="MailjetEmailingIntegrationTests.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Mailjet.Tests
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using global::Mailjet.Client;
    using global::Mailjet.Client.Exceptions;
    using Microsoft.Extensions.DependencyInjection;
    using PosInformatique.Foundations.EmailAddresses;

    public class MailjetEmailingIntegrationTests
    {
        private readonly EmailingIntegrationTestsConfiguration configuration;

        public MailjetEmailingIntegrationTests()
        {
            if (!File.Exists("MailjetEmailingIntegrationTests.local.settings.json"))
            {
                return;
            }

            this.configuration = new EmailingIntegrationTestsConfiguration("MailjetEmailingIntegrationTests.local.settings.json");
        }

        [Fact]
        public async Task SendEmailAsync()
        {
            if (this.configuration is null)
            {
                return;
            }

            await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseMailjet(
                    this.configuration["EMAILING_MAILJET_API_KEY"],
                    this.configuration["EMAILING_MAILJET_API_SECRET"]),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_RECIPIENT_EMAIL_ADDRESS"]),
                manuallyRunOnly: true);
        }

        [Fact]
        public async Task SendEmailAsync_WithWrongApiKey()
        {
            if (this.configuration is null)
            {
                return;
            }

            var action = async () => await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseMailjet(
                    "wrong-api-key",
                    this.configuration["EMAILING_MAILJET_API_SECRET"]),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_RECIPIENT_EMAIL_ADDRESS"]));

            var exception = await action.Should().ThrowExactlyAsync<EmailProviderException>();

            var response = JsonSerializer.Deserialize<MailjetErrorResponse>(exception.Which.Message);

            response.ErrorMessage.Should().Be("API key authentication/authorization failure. You may be unauthorized to access the API or your API key may be expired. Visit API keys management section to check your keys.");
            response.ErrorInfo.Should().Be("Unauthorized");
            response.StatusCode.Should().Be(401);

            var mailjetException = exception.WithInnerExceptionExactly<MailjetServerException>().Which;

            response = JsonSerializer.Deserialize<MailjetErrorResponse>(mailjetException.Message);

            response.ErrorMessage.Should().Be("API key authentication/authorization failure. You may be unauthorized to access the API or your API key may be expired. Visit API keys management section to check your keys.");
            response.ErrorInfo.Should().Be("Unauthorized");
            response.StatusCode.Should().Be(401);
        }

        [Fact]
        public async Task SendEmailAsync_WithWrongApiSecret()
        {
            if (this.configuration is null)
            {
                return;
            }

            var action = async () => await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseMailjet(
                    this.configuration["EMAILING_MAILJET_API_KEY"],
                    "wrong-api-secret"),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_RECIPIENT_EMAIL_ADDRESS"]));

            var exception = await action.Should().ThrowExactlyAsync<EmailProviderException>();

            var response = JsonSerializer.Deserialize<MailjetErrorResponse>(exception.Which.Message);

            response.ErrorMessage.Should().Be("API key authentication/authorization failure. You may be unauthorized to access the API or your API key may be expired. Visit API keys management section to check your keys.");
            response.ErrorInfo.Should().Be("Unauthorized");
            response.StatusCode.Should().Be(401);

            var mailjetException = exception.WithInnerExceptionExactly<MailjetServerException>().Which;

            response = JsonSerializer.Deserialize<MailjetErrorResponse>(mailjetException.Message);

            response.ErrorMessage.Should().Be("API key authentication/authorization failure. You may be unauthorized to access the API or your API key may be expired. Visit API keys management section to check your keys.");
            response.ErrorInfo.Should().Be("Unauthorized");
            response.StatusCode.Should().Be(401);
        }

        [Fact]
        public async Task SendEmailAsync_WithWrongSender()
        {
            if (this.configuration is null)
            {
                return;
            }

            var action = async () => await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseMailjet(
                    this.configuration["EMAILING_MAILJET_API_KEY"],
                    this.configuration["EMAILING_MAILJET_API_SECRET"]),
                EmailAddress.Parse("wrong-sender-email@example.com"),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_RECIPIENT_EMAIL_ADDRESS"]));

            await action.Should().ThrowExactlyAsync<EmailProviderException>()
                .WithMessage("\"wrong-sender-email@example.com\" is not an authorized sender email address for your account.");
        }

        [Fact]
        public async Task SendEmailAsync_WithTooLargeAttachment()
        {
            if (this.configuration is null)
            {
                return;
            }

            // Mailjet limits the total size of a message (15 MB). The expected failure mode
            // (per-message "error" status or HTTP error) has to be confirmed by running this test manually.
            var action = async () => await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseMailjet(
                    this.configuration["EMAILING_MAILJET_API_KEY"],
                    this.configuration["EMAILING_MAILJET_API_SECRET"]),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_RECIPIENT_EMAIL_ADDRESS"]),
                attachmentContent: new byte[20 * 1024 * 1024]);

            var innerException = await action.Should().ThrowAsync<EmailProviderException>();

            innerException.WithInnerExceptionExactly<MailjetServerException>()
                .WithMessage("{\r\n  \"StatusCode\": 413,\r\n  \"ErrorInfo\": \"Request Entity Too Large\"\r\n}");
        }

        [Fact]
        public async Task SendEmailAsync_WithEmptyContent()
        {
            if (this.configuration is null)
            {
                return;
            }

            // Calls the provider directly because the e-mail manager always renders a non-empty content.
            // Mailjet answers HTTP 200 with a message "Status" = "error" (no HTTP error).
            using var httpClient = new HttpClient();
            httpClient.SetDefaultSettings();
            httpClient.UseBasicAuthentication(
                this.configuration["EMAILING_MAILJET_API_KEY"],
                this.configuration["EMAILING_MAILJET_API_SECRET"]);

            var provider = new MailjetEmailProvider(new MailjetClient(httpClient));

            var message = new EmailMessage(
                new EmailContact(EmailAddress.Parse(this.configuration["EMAILING_MAILJET_SENDER_EMAIL_ADDRESS"]), "Sender"),
                new EmailContact(EmailAddress.Parse(this.configuration["EMAILING_MAILJET_RECIPIENT_EMAIL_ADDRESS"]), "Recipient"),
                "The subject",
                string.Empty);

            var exception = await provider.Invoking(p => p.SendAsync(message, TestContext.Current.CancellationToken))
                .Should().ThrowExactlyAsync<EmailProviderException>()
                .WithMessage("At least \"HTMLPart\", \"TextPart\" or \"TemplateID\" must be provided.");

            exception.Which.InnerException.Should().BeNull();
        }

        [Fact]
        public async Task SendEmailAsync_WithWrongRecipient()
        {
            if (this.configuration is null)
            {
                return;
            }

            await EmailingIntegrationTests.SendEmailAsync(
                emailingBuilder => emailingBuilder.UseMailjet(
                    this.configuration["EMAILING_MAILJET_API_KEY"],
                    this.configuration["EMAILING_MAILJET_API_SECRET"]),
                EmailAddress.Parse(this.configuration["EMAILING_MAILJET_SENDER_EMAIL_ADDRESS"]),
                EmailAddress.Parse("wrong@cfdkjdfsjksdgfjksdfgjhnsdghjbsdgjbhsdgjbnsdg.com"),
                manuallyRunOnly: true);
        }

        private sealed class MailjetErrorResponse
        {
            [JsonPropertyName("ErrorInfo")]
            public required string ErrorInfo { get; init; }

            [JsonPropertyName("ErrorMessage")]
            public required string ErrorMessage { get; init; }

            [JsonPropertyName("StatusCode")]
            public required int StatusCode { get; init; }
        }
    }
}
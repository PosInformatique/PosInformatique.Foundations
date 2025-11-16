//-----------------------------------------------------------------------
// <copyright file="AzureEmailProviderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Azure.Tests
{
    using PosInformatique.Foundations.EmailAddresses;

    public class AzureEmailProviderTest
    {
        [Fact]
        public void Constructor_WithClientArgumentNull()
        {
            var act = () =>
            {
                _ = new AzureEmailProvider(null!);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("client");
        }

        [Fact]
        public async Task SendSync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var from = new EmailContact(EmailAddress.Parse("sender@domain.com"), "Ignored");
            var to = new EmailContact(EmailAddress.Parse("recipient@domain.com"), "The recipient");

            var message = new EmailMessage(from, to, "The subject", "The HTML content");

            var azureClient = new Mock<global::Azure.Communication.Email.EmailClient>(MockBehavior.Strict);
            azureClient.Setup(c => c.SendAsync(global::Azure.WaitUntil.Started, It.IsAny<global::Azure.Communication.Email.EmailMessage>(), cancellationToken))
                .Callback((global::Azure.WaitUntil _, global::Azure.Communication.Email.EmailMessage m, CancellationToken _) =>
                {
                    m.Attachments.Should().BeEmpty();
                    m.Content.Html.Should().Be("The HTML content");
                    m.Content.PlainText.Should().BeNull();
                    m.Content.Subject.Should().Be("The subject");
                    m.SenderAddress.Should().Be("sender@domain.com");
                    m.Recipients.BCC.Should().BeEmpty();
                    m.Recipients.CC.Should().BeEmpty();
                    m.Recipients.To.Should().HaveCount(1);
                    m.Recipients.To[0].Address.Should().Be("recipient@domain.com");
                    m.Recipients.To[0].DisplayName.Should().Be("The recipient");
                })
                .ReturnsAsync(new global::Azure.Communication.Email.EmailSendOperation("The id", azureClient.Object));

            var provider = new AzureEmailProvider(azureClient.Object);

            await provider.SendAsync(message, cancellationToken);

            azureClient.VerifyAll();
        }

        [Fact]
        public async Task SendSync_WithMessageArgumentNull()
        {
            var azureClient = new Mock<global::Azure.Communication.Email.EmailClient>(MockBehavior.Strict);

            var provider = new AzureEmailProvider(azureClient.Object);

            await provider.Invoking(p => p.SendAsync(null, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("message");

            azureClient.VerifyAll();
        }
    }
}
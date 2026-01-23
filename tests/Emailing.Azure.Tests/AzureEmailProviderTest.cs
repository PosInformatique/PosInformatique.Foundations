//-----------------------------------------------------------------------
// <copyright file="AzureEmailProviderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Azure.Tests
{
    using System.Reflection;
    using PosInformatique.Foundations.EmailAddresses;
    using PosInformatique.Foundations.MediaTypes;

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

        [Theory]
        [InlineData(EmailImportance.Low, "5", "Low")]
        [InlineData(EmailImportance.Normal, "3", "Normal")]
        [InlineData(EmailImportance.High, "1", "High")]
        public async Task SendSync(EmailImportance importance, string expectedXPriority, string expectedImportance)
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var from = new EmailContact(EmailAddress.Parse("sender@domain.com"), "Ignored");
            var to = new EmailContact(EmailAddress.Parse("recipient@domain.com"), "The recipient");

            var attachment1 = new EmailAttachment("Attachment1", MimeTypes.Application.Pdf, new MemoryStream([1, 2]));
            var attachment2 = new EmailAttachment("Attachment2", MimeTypes.Application.Docx, new MemoryStream([3, 4]));

            var message = new EmailMessage(from, to, "The subject", "The HTML content")
            {
                Attachments =
                {
                    attachment1,
                    attachment2,
                },
                Importance = importance,
            };

            var azureClient = new Mock<global::Azure.Communication.Email.EmailClient>(MockBehavior.Strict);
            azureClient.Setup(c => c.SendAsync(global::Azure.WaitUntil.Started, It.IsAny<global::Azure.Communication.Email.EmailMessage>(), cancellationToken))
                .Callback((global::Azure.WaitUntil _, global::Azure.Communication.Email.EmailMessage m, CancellationToken _) =>
                {
                    m.Attachments.Should().HaveCount(2);
                    m.Attachments[0].Content.ToArray().Should().BeEquivalentTo(new byte[] { 1, 2 });
                    m.Attachments[0].ContentType.Should().Be("application/pdf");
                    m.Attachments[0].Name.Should().Be("Attachment1");
                    m.Attachments[1].Content.ToArray().Should().BeEquivalentTo(new byte[] { 3, 4 });
                    m.Attachments[1].ContentType.Should().Be("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    m.Attachments[1].Name.Should().Be("Attachment2");
                    m.Headers.Should().HaveCount(2);
                    m.Headers["X-Priority"].Should().Be(expectedXPriority);
                    m.Headers["Importance"].Should().Be(expectedImportance);
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

            IsOpen(attachment1.Content).Should().BeTrue();
            IsOpen(attachment2.Content).Should().BeTrue();
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

        private static bool IsOpen(Stream stream)
        {
            var fieldIsOpen = typeof(MemoryStream).GetField("_isOpen", BindingFlags.NonPublic | BindingFlags.Instance);

            return (bool)fieldIsOpen.GetValue(stream);
        }
    }
}
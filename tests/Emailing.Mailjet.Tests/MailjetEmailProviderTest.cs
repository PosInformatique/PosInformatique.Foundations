//-----------------------------------------------------------------------
// <copyright file="MailjetEmailProviderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Mailjet.Tests
{
    using System.Reflection;
    using global::Mailjet.Client;
    using global::Mailjet.Client.Exceptions;
    using global::Mailjet.Client.TransactionalEmails;
    using global::Mailjet.Client.TransactionalEmails.Response;
    using PosInformatique.Foundations.EmailAddresses;
    using PosInformatique.Foundations.MediaTypes;

    public class MailjetEmailProviderTest
    {
        [Fact]
        public void Constructor_WithClientArgumentNull()
        {
            var act = () =>
            {
                _ = new MailjetEmailProvider(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("client");
        }

        [Theory]
        [InlineData(EmailImportance.Low, "5")]
        [InlineData(EmailImportance.Normal, "3")]
        [InlineData(EmailImportance.High, "1")]
        public async Task SendSync(EmailImportance importance, string expectedXPriority)
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

            var mailjetClient = new Mock<IMailjetClient>(MockBehavior.Strict);
            mailjetClient.Setup(c => c.SendTransactionalEmailAsync(It.IsAny<TransactionalEmail>(), false, true))
                .Callback((TransactionalEmail m, bool _, bool _) =>
                {
                    m.Subject.Should().Be("The subject");
                    m.HTMLPart.Should().Be("The HTML content");
                    m.From.Email.Should().Be("sender@domain.com");
                    m.From.Name.Should().Be("Ignored");
                    m.To.Should().HaveCount(1);
                    m.To[0].Email.Should().Be("recipient@domain.com");
                    m.To[0].Name.Should().Be("The recipient");
                    m.Headers.Should().HaveCount(1);
                    m.Headers["X-Priority"].Should().Be(expectedXPriority);
                    m.Attachments.Should().HaveCount(2);
                    m.Attachments[0].Filename.Should().Be("Attachment1");
                    m.Attachments[0].ContentType.Should().Be("application/pdf");
                    m.Attachments[0].Base64Content.Should().Be(Convert.ToBase64String([1, 2]));
                    m.Attachments[1].Filename.Should().Be("Attachment2");
                    m.Attachments[1].ContentType.Should().Be("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    m.Attachments[1].Base64Content.Should().Be(Convert.ToBase64String([3, 4]));
                })
                .ReturnsAsync(new TransactionalEmailResponse()
                {
                    Messages =
                    [
                        new MessageResult
                        {
                            Status = "not an error",
                        },
                    ],
                });

            var provider = new MailjetEmailProvider(mailjetClient.Object);

            await provider.SendAsync(message, cancellationToken);

            mailjetClient.VerifyAll();

            IsOpen(attachment1.Content).Should().BeTrue();
            IsOpen(attachment2.Content).Should().BeTrue();
        }

        [Fact]
        public async Task SendSync_WithMessageArgumentNull()
        {
            var client = Mock.Of<IMailjetClient>(MockBehavior.Strict);

            var provider = new MailjetEmailProvider(client);

            await provider.Invoking(p => p.SendAsync(null, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("message");
        }

        [Fact]
        public async Task SendSync_WithInvalidResponse()
        {
            var from = new EmailContact(EmailAddress.Parse("sender@domain.com"), "Ignored");
            var to = new EmailContact(EmailAddress.Parse("recipient@domain.com"), "The recipient");

            var message = new EmailMessage(from, to, "The subject", "The HTML content");

            var mailjetClient = new Mock<IMailjetClient>(MockBehavior.Strict);
            mailjetClient.Setup(c => c.SendTransactionalEmailAsync(It.IsAny<TransactionalEmail>(), false, true))
                .Callback((TransactionalEmail m, bool _, bool _) =>
                {
                    m.Subject.Should().Be("The subject");
                    m.HTMLPart.Should().Be("The HTML content");
                    m.From.Email.Should().Be("sender@domain.com");
                    m.From.Name.Should().Be("Ignored");
                    m.To.Should().HaveCount(1);
                    m.To[0].Email.Should().Be("recipient@domain.com");
                    m.To[0].Name.Should().Be("The recipient");
                })
                .ReturnsAsync(new TransactionalEmailResponse
                {
                    Messages = [],
                });

            var provider = new MailjetEmailProvider(mailjetClient.Object);

            await provider.Invoking(p => p.SendAsync(message, default))
                .Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage("The Mailjet emailing provider did not return a proper response.");

            mailjetClient.VerifyAll();
        }

        [Fact]
        public async Task SendSync_WithErrorStatusCode403()
        {
            var from = new EmailContact(EmailAddress.Parse("sender@domain.com"), "Ignored");
            var to = new EmailContact(EmailAddress.Parse("recipient@domain.com"), "The recipient");

            var message = new EmailMessage(from, to, "The subject", "The HTML content");

            var mailjetClient = new Mock<IMailjetClient>(MockBehavior.Strict);
            mailjetClient.Setup(c => c.SendTransactionalEmailAsync(It.IsAny<TransactionalEmail>(), false, true))
                .Callback((TransactionalEmail m, bool _, bool _) =>
                {
                    m.Subject.Should().Be("The subject");
                    m.HTMLPart.Should().Be("The HTML content");
                    m.From.Email.Should().Be("sender@domain.com");
                    m.From.Name.Should().Be("Ignored");
                    m.To.Should().HaveCount(1);
                    m.To[0].Email.Should().Be("recipient@domain.com");
                    m.To[0].Name.Should().Be("The recipient");
                })
                .ReturnsAsync(new TransactionalEmailResponse
                {
                    Messages =
                    [
                        new MessageResult
                        {
                            Status = "error",
                            Errors =
                            [
                                new SendEmailError
                                {
                                    StatusCode = 403,
                                    ErrorMessage = "The error message",
                                },
                            ],
                        },
                    ],
                });

            var provider = new MailjetEmailProvider(mailjetClient.Object);

            await provider.Invoking(p => p.SendAsync(message, default))
                .Should().ThrowExactlyAsync<EmailProviderException>()
                .WithMessage("The error message");

            mailjetClient.VerifyAll();
        }

        [Fact]
        public async Task SendSync_WithErrorStatusCodeOtherThan403()
        {
            var from = new EmailContact(EmailAddress.Parse("sender@domain.com"), "Ignored");
            var to = new EmailContact(EmailAddress.Parse("recipient@domain.com"), "The recipient");

            var message = new EmailMessage(from, to, "The subject", "The HTML content");

            var mailjetClient = new Mock<IMailjetClient>(MockBehavior.Strict);
            mailjetClient.Setup(c => c.SendTransactionalEmailAsync(It.IsAny<TransactionalEmail>(), false, true))
                .Callback((TransactionalEmail m, bool _, bool _) =>
                {
                    m.Subject.Should().Be("The subject");
                    m.HTMLPart.Should().Be("The HTML content");
                    m.From.Email.Should().Be("sender@domain.com");
                    m.From.Name.Should().Be("Ignored");
                    m.To.Should().HaveCount(1);
                    m.To[0].Email.Should().Be("recipient@domain.com");
                    m.To[0].Name.Should().Be("The recipient");
                })
                .ReturnsAsync(new TransactionalEmailResponse
                {
                    Messages =
                    [
                        new MessageResult
                        {
                            Status = "error",
                            Errors =
                            [
                                new SendEmailError
                                {
                                    StatusCode = 500,
                                    ErrorMessage = "The error message",
                                },
                            ],
                        },
                    ],
                });

            var provider = new MailjetEmailProvider(mailjetClient.Object);

            await provider.SendAsync(message, TestContext.Current.CancellationToken);

            mailjetClient.VerifyAll();
        }

        [Fact]
        public async Task SendSync_WithMailjetException()
        {
            var from = new EmailContact(EmailAddress.Parse("sender@domain.com"), "Ignored");
            var to = new EmailContact(EmailAddress.Parse("recipient@domain.com"), "The recipient");

            var message = new EmailMessage(from, to, "The subject", "The HTML content");

            var mailjetException = new MailjetException("The server error message");

            var mailjetClient = new Mock<IMailjetClient>(MockBehavior.Strict);
            mailjetClient.Setup(c => c.SendTransactionalEmailAsync(It.IsAny<TransactionalEmail>(), false, true))
                .Callback((TransactionalEmail m, bool _, bool _) =>
                {
                    m.Subject.Should().Be("The subject");
                    m.HTMLPart.Should().Be("The HTML content");
                    m.From.Email.Should().Be("sender@domain.com");
                    m.From.Name.Should().Be("Ignored");
                    m.To.Should().HaveCount(1);
                    m.To[0].Email.Should().Be("recipient@domain.com");
                    m.To[0].Name.Should().Be("The recipient");
                })
                .ThrowsAsync(mailjetException);

            var provider = new MailjetEmailProvider(mailjetClient.Object);

            await provider.Invoking(p => p.SendAsync(message, default))
                .Should().ThrowExactlyAsync<EmailProviderException>()
                .WithMessage("The server error message")
                .Where(e => e.InnerException == mailjetException);

            mailjetClient.VerifyAll();
        }

        private static bool IsOpen(Stream stream)
        {
            var fieldIsOpen = typeof(MemoryStream).GetField("_isOpen", BindingFlags.NonPublic | BindingFlags.Instance);

            return (bool)fieldIsOpen.GetValue(stream);
        }
    }
}
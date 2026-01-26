//-----------------------------------------------------------------------
// <copyright file="EmailManagerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using System.Reflection;
    using Microsoft.Extensions.Options;
    using PosInformatique.Foundations.EmailAddresses;
    using PosInformatique.Foundations.MediaTypes;
    using PosInformatique.Foundations.Text.Templating;

    public class EmailManagerTest
    {
        [Fact]
        public void Constructeur_WithNoSenderEmailAddress()
        {
            var options = new EmailingOptions();

            options.SenderEmailAddress = null;

            Action act = () =>
            {
                new EmailManager(Options.Create(options), default, default);
            };

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("Sender email address is required. (Parameter 'options')")
                .WithParameterName("options");
        }

        [Fact]
        public void Create()
        {
            var identifier = EmailTemplateIdentifier<Model>.Create();

            var template = new EmailTemplate<Model>(Mock.Of<TextTemplate<Model>>(MockBehavior.Strict), Mock.Of<TextTemplate<Model>>(MockBehavior.Strict));

            var options = new EmailingOptions();
            options.RegisterTemplate(identifier, template);
            options.SenderEmailAddress = EmailAddress.Parse("sender@domain.com");

            var manager = new EmailManager(Options.Create(options), default, default);

            var email = manager.Create(identifier);

            email.Importance.Should().Be(EmailImportance.Normal);
            email.Recipients.Should().BeEmpty();
            email.Template.Should().BeSameAs(template);
        }

        [Fact]
        public void Create_WithNoRegisteredTemplate()
        {
            var identifier = EmailTemplateIdentifier<Model>.Create();

            var options = new EmailingOptions();
            options.SenderEmailAddress = EmailAddress.Parse("sender@domain.com");

            var manager = new EmailManager(Options.Create(options), default, default);

            manager.Invoking(m => m.Create(identifier))
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("Unable to find a template for the specified identifier. (Parameter 'identifier')")
                .WithParameterName("identifier");
        }

        [Fact]
        public void Create_WithNullIdentifier()
        {
            var options = new EmailingOptions();
            options.SenderEmailAddress = EmailAddress.Parse("sender@domain.com");

            var manager = new EmailManager(Options.Create(options), default, default);

            manager.Invoking(m => m.Create<Model>(null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("identifier");
        }

        [Fact]
        public async Task SendAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var serviceProvider = Mock.Of<IServiceProvider>(MockBehavior.Strict);

            var model1 = new Model();
            var model2 = new Model();

            var subject = new Mock<TextTemplate<Model>>(MockBehavior.Strict);
            subject.Setup(s => s.RenderAsync(model1, It.IsAny<TextWriter>(), It.IsAny<ITextTemplateRenderContext>(), cancellationToken))
                .Callback((Model _, TextWriter writer, ITextTemplateRenderContext context, CancellationToken _) =>
                {
                    context.ServiceProvider.Should().BeSameAs(serviceProvider);

                    writer.Write("Subject 1");
                })
                .Returns(Task.CompletedTask);
            subject.Setup(s => s.RenderAsync(model2, It.IsAny<TextWriter>(), It.IsAny<ITextTemplateRenderContext>(), cancellationToken))
                .Callback((Model _, TextWriter writer, ITextTemplateRenderContext context, CancellationToken _) =>
                {
                    context.ServiceProvider.Should().BeSameAs(serviceProvider);

                    writer.Write("Subject 2");
                })
                .Returns(Task.CompletedTask);

            var htmlBody = new Mock<TextTemplate<Model>>(MockBehavior.Strict);
            htmlBody.Setup(s => s.RenderAsync(model1, It.IsAny<TextWriter>(), It.IsAny<ITextTemplateRenderContext>(), cancellationToken))
                .Callback((Model _, TextWriter writer, ITextTemplateRenderContext context, CancellationToken _) =>
                {
                    context.ServiceProvider.Should().BeSameAs(serviceProvider);

                    writer.Write("HTML Content 1");
                })
                .Returns(Task.CompletedTask);
            htmlBody.Setup(s => s.RenderAsync(model2, It.IsAny<TextWriter>(), It.IsAny<ITextTemplateRenderContext>(), cancellationToken))
                .Callback((Model _, TextWriter writer, ITextTemplateRenderContext context, CancellationToken _) =>
                {
                    context.ServiceProvider.Should().BeSameAs(serviceProvider);

                    writer.Write("HTML Content 2");
                })
                .Returns(Task.CompletedTask);

            var template = new EmailTemplate<Model>(subject.Object, htmlBody.Object);

            var emailAddressRecipient1 = EmailAddress.Parse("email1@domain.com");
            var emailAddressRecipient2 = EmailAddress.Parse("email2@domain.com");

            var content1 = new MemoryStream([1, 2]);
            var content2 = new MemoryStream([3, 4]);

            var attachment1 = new EmailAttachment("Attachment1", MimeTypes.Application.Pdf, content1);
            var attachment2 = new EmailAttachment("Attachment2", MimeTypes.Application.Docx, content2);

            var email = new Email<Model>(template)
            {
                Attachments =
                {
                    attachment1,
                    attachment2,
                },
                Importance = EmailImportance.High,
                Recipients =
                {
                    new EmailRecipient<Model>(emailAddressRecipient1, "The display name 1", model1),
                    new EmailRecipient<Model>(emailAddressRecipient2, "The display name 2", model2),
                },
            };

            var sender = EmailAddress.Parse("sender@domain.com");

            var options = new EmailingOptions();
            options.SenderEmailAddress = sender;

            var contentStreams = new List<Stream>();

            var provider = new Mock<IEmailProvider>(MockBehavior.Strict);
            provider.Setup(p => p.SendAsync(It.Is<EmailMessage>(m => m.To.Email == emailAddressRecipient1), cancellationToken))
                .Callback((EmailMessage m, CancellationToken _) =>
                {
                    m.Attachments[0].Content.As<MemoryStream>().Position.Should().Be(0);
                    m.Attachments[0].Content.As<MemoryStream>().ToArray().Should().Equal(1, 2);
                    m.Attachments[0].ContentType.Should().Be(MimeTypes.Application.Pdf);
                    m.Attachments[0].FileName.Should().Be("Attachment1");
                    m.Attachments[1].Content.As<MemoryStream>().Position.Should().Be(0);
                    m.Attachments[1].Content.As<MemoryStream>().ToArray().Should().Equal(3, 4);
                    m.Attachments[1].ContentType.Should().Be(MimeTypes.Application.Docx);
                    m.Attachments[1].FileName.Should().Be("Attachment2");
                    m.From.Email.Should().BeSameAs(sender);
                    m.From.DisplayName.Should().BeEmpty();
                    m.Importance.Should().Be(EmailImportance.High);
                    m.Subject.Should().Be("Subject 1");
                    m.HtmlContent.Should().Be("HTML Content 1");
                    m.To.DisplayName.Should().Be("The display name 1");

                    contentStreams.Add(m.Attachments[0].Content);
                    contentStreams.Add(m.Attachments[1].Content);
                })
                .Returns(Task.CompletedTask);
            provider.Setup(p => p.SendAsync(It.Is<EmailMessage>(m => m.To.Email == emailAddressRecipient2), cancellationToken))
                .Callback((EmailMessage m, CancellationToken _) =>
                {
                    m.Attachments[0].Content.As<MemoryStream>().Position.Should().Be(0);
                    m.Attachments[0].Content.As<MemoryStream>().ToArray().Should().Equal(1, 2);
                    m.Attachments[0].ContentType.Should().Be(MimeTypes.Application.Pdf);
                    m.Attachments[0].FileName.Should().Be("Attachment1");
                    m.Attachments[1].Content.As<MemoryStream>().Position.Should().Be(0);
                    m.Attachments[1].Content.As<MemoryStream>().ToArray().Should().Equal(3, 4);
                    m.Attachments[1].ContentType.Should().Be(MimeTypes.Application.Docx);
                    m.Attachments[1].FileName.Should().Be("Attachment2");
                    m.From.Email.Should().BeSameAs(sender);
                    m.From.DisplayName.Should().BeEmpty();
                    m.Importance.Should().Be(EmailImportance.High);
                    m.Subject.Should().Be("Subject 2");
                    m.HtmlContent.Should().Be("HTML Content 2");
                    m.To.DisplayName.Should().Be("The display name 2");

                    contentStreams.Add(m.Attachments[0].Content);
                    contentStreams.Add(m.Attachments[1].Content);
                })
                .Returns(Task.CompletedTask);

            var manager = new EmailManager(Options.Create(options), provider.Object, serviceProvider);

            await manager.SendAsync(email, cancellationToken);

            foreach (var contentStream in contentStreams)
            {
                IsOpen(contentStream).Should().BeFalse();
            }

            IsOpen(email.Attachments[0].Content).Should().BeTrue();
            IsOpen(email.Attachments[1].Content).Should().BeTrue();

            htmlBody.VerifyAll();
            provider.VerifyAll();
            subject.VerifyAll();
        }

        [Fact]
        public async Task SendAsync_WithNullIdentifier()
        {
            var options = new EmailingOptions();
            options.SenderEmailAddress = EmailAddress.Parse("sender@domain.com");

            var manager = new EmailManager(Options.Create(options), default, default);

            await manager.Invoking(m => m.SendAsync<Model>(null, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("email");
        }

        private static bool IsOpen(Stream stream)
        {
            var fieldIsOpen = typeof(MemoryStream).GetField("_isOpen", BindingFlags.NonPublic | BindingFlags.Instance);

            return (bool)fieldIsOpen.GetValue(stream);
        }

        internal sealed class Model
        {
        }
    }
}
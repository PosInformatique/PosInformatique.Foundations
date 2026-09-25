//-----------------------------------------------------------------------
// <copyright file="EmailingIntegrationTests.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using System.Diagnostics;
    using System.Text;
    using Microsoft.Extensions.DependencyInjection;
    using PosInformatique.Foundations.EmailAddresses;
    using PosInformatique.Foundations.MediaTypes;
    using PosInformatique.Foundations.Text.Templating;

    public class EmailingIntegrationTests
    {
        private readonly Action<EmailingBuilder> configure;

        private readonly EmailAddress senderEmailAddress;

        private readonly EmailAddress recipientEmailAddress;

        public EmailingIntegrationTests(
            Action<EmailingBuilder> configure,
            EmailAddress senderEmailAddress,
            EmailAddress recipientEmailAddress)
        {
            this.configure = configure;
            this.senderEmailAddress = senderEmailAddress;
            this.recipientEmailAddress = recipientEmailAddress;
        }

        public async Task SendEmailAsync()
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            var serviceCollection = new ServiceCollection();

            var emailingBuilder = serviceCollection.AddEmailing(opt =>
            {
                opt.SenderDisplayName = "P.O.S Informatique - Foundations";
                opt.SenderEmailAddress = this.senderEmailAddress;
            });

            this.configure(emailingBuilder);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var emailManager = serviceProvider.GetRequiredService<IEmailManager>();

            var template = new Template(new StringTextTemplate<TemplateData>("The subject"), new StringTextTemplate<TemplateData>("<html><body><h1>The title</h1><p>The content</p></body></html>"));

            var email = new Email<TemplateData>(template)
            {
                Attachments =
                {
                    new EmailAttachment("Attachment.txt", MimeType.Parse("text/plain"), new MemoryStream(Encoding.UTF8.GetBytes("This is the content of the attachment."))),
                },
                Importance = EmailImportance.High,
                Recipients =
                {
                    new EmailRecipient<TemplateData>(
                        this.recipientEmailAddress,
                        "John DOE",
                        new TemplateData()),
                },
            };

            await emailManager.SendAsync(email);
        }

        private sealed class Template : EmailTemplate<TemplateData>
        {
            public Template(TextTemplate<TemplateData> subject, TextTemplate<TemplateData> htmlBody)
                : base(subject, htmlBody)
            {
            }
        }

        private sealed class TemplateData
        {
        }

        private sealed class StringTextTemplate<TModel> : TextTemplate<TModel>
        {
            private readonly string content;

            public StringTextTemplate(string content)
            {
                this.content = content;
            }

            public override async Task RenderAsync(TModel model, TextWriter output, ITextTemplateRenderContext context, CancellationToken cancellationToken = default)
            {
                await output.WriteLineAsync(this.content);
            }
        }
    }
}
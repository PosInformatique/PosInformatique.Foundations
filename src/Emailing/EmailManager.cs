//-----------------------------------------------------------------------
// <copyright file="EmailManager.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using Microsoft.Extensions.Options;
    using PosInformatique.Foundations.Text.Templating;

    internal sealed class EmailManager : IEmailManager
    {
        private readonly IOptions<EmailingOptions> options;

        private readonly IEmailProvider provider;

        private readonly IServiceProvider serviceProvider;

        public EmailManager(IOptions<EmailingOptions> options, IEmailProvider provider, IServiceProvider serviceProvider)
        {
            if (options.Value.SenderEmailAddress is null)
            {
                throw new ArgumentException("Sender email address is required.", nameof(options));
            }

            this.options = options;
            this.provider = provider;
            this.serviceProvider = serviceProvider;
        }

        public Email<TModel> Create<TModel>(EmailTemplateIdentifier<TModel> identifier)
        {
            ArgumentNullException.ThrowIfNull(identifier);

            var template = this.options.Value.GetTemplate(identifier);

            if (template is null)
            {
                throw new ArgumentException("Unable to find a template for the specified identifier.", nameof(identifier));
            }

            return new Email<TModel>(template);
        }

        public async Task SendAsync<TModel>(Email<TModel> email, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(email);

            var senderEmailAddress = this.options.Value.SenderEmailAddress!;

            foreach (var recipient in email.Recipients)
            {
                // Render the subject
                using var subjectOutputWriter = new StringWriter();

                var textTemplateRenderContext = new TextTemplateRenderContext(this.serviceProvider);

                await email.Template.Subject.RenderAsync(recipient.Model, subjectOutputWriter, textTemplateRenderContext, cancellationToken);

                var subject = subjectOutputWriter.ToString();

                // Render the HTML content
                using var htmlContentOutputWriter = new StringWriter();

                textTemplateRenderContext = new TextTemplateRenderContext(this.serviceProvider);

                await email.Template.HtmlBody.RenderAsync(recipient.Model, htmlContentOutputWriter, textTemplateRenderContext, cancellationToken);

                var htmlContent = htmlContentOutputWriter.ToString();

                var message = new EmailMessage(
                    new EmailContact(senderEmailAddress, string.Empty),
                    new EmailContact(recipient.Address, recipient.DisplayName),
                    subject,
                    htmlContent)
                {
                    Importance = email.Importance,
                };

                foreach (var attachment in email.Attachments)
                {
                    message.Attachments.Add(attachment);
                }

                await this.provider.SendAsync(message, cancellationToken);
            }
        }

        private sealed class TextTemplateRenderContext : ITextTemplateRenderContext
        {
            public TextTemplateRenderContext(IServiceProvider serviceProvider)
            {
                this.ServiceProvider = serviceProvider;
            }

            public IServiceProvider ServiceProvider { get; }
        }
    }
}
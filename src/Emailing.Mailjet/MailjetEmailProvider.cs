//-----------------------------------------------------------------------
// <copyright file="MailjetEmailProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Mailjet
{
    using global::Mailjet.Client;
    using global::Mailjet.Client.TransactionalEmails;

    /// <summary>
    /// Implementation of the <see cref="IEmailProvider"/> to send the e-mail using
    /// <c>Mailjet</c>.
    /// </summary>
    public sealed class MailjetEmailProvider : IEmailProvider
    {
        private readonly IMailjetClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailjetEmailProvider"/> class
        /// using the <see cref="IMailjetClient"/>.
        /// </summary>
        /// <param name="client"><see cref="IMailjetClient"/> used to call the <c>Mailjet</c> API.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="client"/> argument is <see langword="null"/>.</exception>
        public MailjetEmailProvider(IMailjetClient client)
        {
            ArgumentNullException.ThrowIfNull(client);

            this.client = client;
        }

        /// <inheritdoc />
        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            var importance = message.Importance switch
            {
                EmailImportance.Low => "5",
                EmailImportance.High => "1",
                _ => "3",
            };

            var mailjetMessage = new TransactionalEmail
            {
                Headers = new Dictionary<string, string>()
                {
                    { "X-Priority", importance },
                },
                HTMLPart = message.HtmlContent,
                From = new SendContact(message.From.Email.ToString(), message.From.DisplayName),
                Subject = message.Subject,
                To = new List<SendContact>
                {
                    new SendContact(message.To.Email.ToString(), message.To.DisplayName),
                },
            };

            if (message.Attachments.Count > 0)
            {
                mailjetMessage.Attachments = new List<Attachment>();

                foreach (var attachment in message.Attachments)
                {
                    using var attachmentContent = new MemoryStream();

                    await attachment.Content.CopyToAsync(attachmentContent, cancellationToken);

                    var mailjetAttachment = new Attachment(
                        attachment.FileName,
                        attachment.ContentType.ToString(),
                        Convert.ToBase64String(attachmentContent.ToArray()));

                    mailjetMessage.Attachments.Add(mailjetAttachment);
                }
            }

            await this.client.SendTransactionalEmailAsync(mailjetMessage);
        }
    }
}
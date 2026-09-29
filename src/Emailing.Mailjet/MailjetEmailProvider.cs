//-----------------------------------------------------------------------
// <copyright file="MailjetEmailProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Mailjet
{
    using global::Mailjet.Client;
    using global::Mailjet.Client.Exceptions;
    using global::Mailjet.Client.TransactionalEmails;
    using global::Mailjet.Client.TransactionalEmails.Response;

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
                TrackClicks = TrackClicks.disabled,
                TrackOpens = TrackOpens.disabled,
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

            try
            {
                var response = await this.client.SendTransactionalEmailAsync(mailjetMessage);

                if (response.Messages.Length != 1)
                {
                    throw new InvalidOperationException("The Mailjet emailing provider did not return a proper response.");
                }

                var responseMessage = response.Messages[0];

                if (responseMessage.Status == "error")
                {
                    var exception = ProcessError(responseMessage);

                    if (exception is not null)
                    {
                        throw exception;
                    }
                }
            }
            catch (MailjetException exception)
            {
                throw new EmailProviderException(exception.Message, exception);
            }
        }

        private static EmailProviderException? ProcessError(MessageResult result)
        {
            foreach (var error in result.Errors)
            {
                var exception = ProcessError(error);

                if (exception is not null)
                {
                    return exception;
                }
            }

            return null;
        }

        private static EmailProviderException? ProcessError(SendEmailError error)
        {
            if (error.StatusCode == 403)
            {
                return new EmailProviderException(error.ErrorMessage);
            }

            return null;
        }
    }
}
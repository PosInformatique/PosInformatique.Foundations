//-----------------------------------------------------------------------
// <copyright file="GraphEmailProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Graph
{
    using Microsoft.Graph;
    using Microsoft.Graph.Models;
    using Microsoft.Graph.Users.Item.SendMail;

    /// <summary>
    /// Implementation of the <see cref="IEmailProvider"/> to send the e-mail using
    /// the <c>Graph</c> API.
    /// </summary>
    public sealed class GraphEmailProvider : IEmailProvider
    {
        private readonly GraphServiceClient serviceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphEmailProvider"/> class
        /// using the Microsoft <see cref="GraphServiceClient"/>.
        /// </summary>
        /// <param name="serviceClient"><see cref="GraphServiceClient"/>
        /// used to call the <c>Azure Communication Service</c> API.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="serviceClient"/> argument is <see langword="null"/>.</exception>
        public GraphEmailProvider(GraphServiceClient serviceClient)
        {
            ArgumentNullException.ThrowIfNull(serviceClient);

            this.serviceClient = serviceClient;
        }

        /// <inheritdoc />
        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            var importance = message.Importance switch
            {
                EmailImportance.Low => Importance.Low,
                EmailImportance.High => Importance.High,
                _ => Importance.Normal,
            };

            var graphMessage = new Message()
            {
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = message.HtmlContent,
                },
                Importance = importance,
                Subject = message.Subject,
                ToRecipients =
                [
                    new()
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = message.To.Email.ToString(),
                            Name = message.To.DisplayName,
                        },
                    },
                ],
            };

            if (message.Attachments.Count > 0)
            {
                graphMessage.Attachments = new List<Attachment>();

                foreach (var attachment in message.Attachments)
                {
                    using var attachmentContent = new MemoryStream();

                    await attachment.Content.CopyToAsync(attachmentContent, cancellationToken);

                    var graphAttachment = new FileAttachment()
                    {
                        Name = attachment.FileName,
                        ContentBytes = attachmentContent.ToArray(),
                        ContentType = attachment.ContentType.ToString(),
                    };

                    graphMessage.Attachments.Add(graphAttachment);
                }
            }

            var body = new SendMailPostRequestBody()
            {
                Message = graphMessage,
                SaveToSentItems = false,
            };

            await this.serviceClient.Users[message.From.Email.ToString()].SendMail.PostAsync(body, cancellationToken: cancellationToken);
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="AzureEmailProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Azure
{
    using System.Globalization;

    /// <summary>
    /// Implementation of the <see cref="IEmailProvider"/> to send the e-mail using
    /// <c>Azure Communication Service</c>.
    /// </summary>
    public sealed class AzureEmailProvider : IEmailProvider
    {
        private readonly global::Azure.Communication.Email.EmailClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEmailProvider"/> class
        /// using the Microsoft <see cref="global::Azure.Communication.Email.EmailClient"/>.
        /// </summary>
        /// <param name="client"><see cref="global::Azure.Communication.Email.EmailClient"/>
        /// used to call the <c>Azure Communication Service</c> API.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="client"/> argument is <see langword="null"/>.</exception>
        public AzureEmailProvider(global::Azure.Communication.Email.EmailClient client)
        {
            ArgumentNullException.ThrowIfNull(client);

            this.client = client;
        }

        /// <inheritdoc />
        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            var receipients = new global::Azure.Communication.Email.EmailRecipients()
            {
                To =
                {
                    new global::Azure.Communication.Email.EmailAddress(message.To.Email, message.To.DisplayName),
                },
            };

            var content = new global::Azure.Communication.Email.EmailContent(message.Subject)
            {
                Html = message.HtmlContent,
            };

            var azureMessage = new global::Azure.Communication.Email.EmailMessage(message.From.Email, receipients, content)
            {
                Headers =
                {
                    { "X-Priority", Convert.ToString(Convert.ToInt32(message.Importance, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) },
                    { "Importance", Convert.ToString(message.Importance, CultureInfo.InvariantCulture) },
                },
            };

            await this.client.SendAsync(global::Azure.WaitUntil.Started, azureMessage, cancellationToken);
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="EmailMessage.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Represents an e-mail generated an can be send to the <see cref="IEmailProvider"/>.
    /// </summary>
    public sealed class EmailMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="from">The sender of the e-mail message.</param>
        /// <param name="to">The recipient of the e-mail message.</param>
        /// <param name="subject">The subject of the e-mail message.</param>
        /// <param name="htmlContent">The HTML content of the e-mail message.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="from"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="to"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="subject"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="htmlContent"/> argument is <see langword="null"/>.</exception>
        public EmailMessage(EmailContact from, EmailContact to, string subject, string htmlContent)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(htmlContent);

            this.From = from;
            this.To = to;
            this.Subject = subject;
            this.HtmlContent = htmlContent;
        }

        /// <summary>
        /// Gets the sender of the e-mail message.
        /// </summary>
        public EmailContact From { get; }

        /// <summary>
        /// Gets the recipient of the e-mail message.
        /// </summary>
        public EmailContact To { get; }

        /// <summary>
        /// Gets the subject of the e-mail message.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Gets the HTML content of the e-mail message.
        /// </summary>
        public string HtmlContent { get; }

        /// <summary>
        /// Gets or sets the importance of the e-mail message.
        /// </summary>
        public EmailImportance Importance { get; set; }
    }
}
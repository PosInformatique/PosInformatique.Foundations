//-----------------------------------------------------------------------
// <copyright file="EmailTemplate.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using PosInformatique.Foundations.Text.Templating;

    /// <summary>
    /// Represents an e-mail template used to generate e-mail to send to the <see cref="EmailRecipient{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel">Type of data model injected to the <see cref="Subject"/> and <see cref="HtmlBody"/> to generate the e-mail.</typeparam>
    public class EmailTemplate<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplate{TModel}"/> class.
        /// </summary>
        /// <param name="subject">The text template used to generate the subject of the e-mail to send.</param>
        /// <param name="htmlBody">The text template used to generate the HTML content of the e-mail to send.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="subject"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="htmlBody"/> argument is <see langword="null"/>.</exception>
        public EmailTemplate(TextTemplate<TModel> subject, TextTemplate<TModel> htmlBody)
        {
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(htmlBody);

            this.Subject = subject;
            this.HtmlBody = htmlBody;
        }

        /// <summary>
        /// Gets the text template used to generate the subject of the e-mail to send.
        /// </summary>
        public TextTemplate<TModel> Subject { get; }

        /// <summary>
        /// Gets the text template used to generate the HTML content of the e-mail to send.
        /// </summary>
        public TextTemplate<TModel> HtmlBody { get; }
    }
}
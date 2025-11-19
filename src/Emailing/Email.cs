//-----------------------------------------------------------------------
// <copyright file="Email.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Represents a templated e-mail to send.
    /// </summary>
    /// <typeparam name="TModel">Type of data model injected to the <see cref="Template"/> to generate the e-mail.</typeparam>
    public sealed class Email<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Email{TModel}"/> class
        /// with the specified <paramref name="template"/>.
        /// </summary>
        /// <param name="template">The e-mail template used to generate the e-mail to send.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="template"/> argument is <see langword="null"/>.</exception>
        public Email(EmailTemplate<TModel> template)
        {
            ArgumentNullException.ThrowIfNull(template);

            this.Template = template;

            this.Importance = EmailImportance.Normal;
            this.Recipients = [];
        }

        /// <summary>
        /// Gets or sets the importance of the e-mail.
        /// </summary>
        public EmailImportance Importance { get; set; }

        /// <summary>
        /// Gets the collection of the recipients which the e-mail have to be send.
        /// </summary>
        public EmailRecipientCollection<TModel> Recipients { get; }

        /// <summary>
        /// Gets the e-mail template used to generate the e-mail to send.
        /// </summary>
        public EmailTemplate<TModel> Template { get; }
    }
}
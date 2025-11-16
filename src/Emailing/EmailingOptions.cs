//-----------------------------------------------------------------------
// <copyright file="EmailingOptions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using PosInformatique.Foundations.EmailAddresses;

    /// <summary>
    /// Represents the e-mailing feature options.
    /// </summary>
    public class EmailingOptions
    {
        private readonly Dictionary<object, object> templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailingOptions"/> class.
        /// </summary>
        public EmailingOptions()
        {
            this.templates = [];
        }

        /// <summary>
        /// Gets or sets the e-mail address of the sender used for the e-mails.
        /// </summary>
        public EmailAddress? SenderEmailAddress { get; set; }

        /// <summary>
        /// Registers a <paramref name="template"/> instance with the specified <paramref name="identifier"/>.
        /// </summary>
        /// <typeparam name="TModel">Type of the data model to inject in the <see cref="Email{TModel}.Template"/>.</typeparam>
        /// <param name="identifier">Unique identifier of the <paramref name="template"/>.</param>
        /// <param name="template"><see cref="EmailTemplate{TModel}"/> to register.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="identifier"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="template"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If a <see cref="EmailTemplate{TModel}"/> has already been registered with the specified <paramref name="identifier"/>.</exception>
        public void RegisterTemplate<TModel>(EmailTemplateIdentifier<TModel> identifier, EmailTemplate<TModel> template)
        {
            ArgumentNullException.ThrowIfNull(identifier);
            ArgumentNullException.ThrowIfNull(template);

            if (this.templates.ContainsKey(identifier))
            {
                throw new ArgumentException("An e-mail template with the same identifier has already been registered.", nameof(identifier));
            }

            this.templates.Add(identifier, template);
        }

        internal EmailTemplate<TModel>? GetTemplate<TModel>(EmailTemplateIdentifier<TModel> identifier)
        {
            if (!this.templates.TryGetValue(identifier, out var templateFound))
            {
                return null;
            }

            return (EmailTemplate<TModel>)templateFound;
        }
    }
}
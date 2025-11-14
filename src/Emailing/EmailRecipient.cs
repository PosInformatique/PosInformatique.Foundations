//-----------------------------------------------------------------------
// <copyright file="EmailRecipient.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using PosInformatique.Foundations.EmailAddresses;

    /// <summary>
    /// Represents a recipient of a <see cref="Email{TModel}"/> to send.
    /// </summary>
    /// <typeparam name="TModel">Data model injected to the <see cref="EmailTemplate{TModel}"/> to generate the e-mail.</typeparam>
    public sealed class EmailRecipient<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailRecipient{TModel}"/> class.
        /// </summary>
        /// <param name="address">E-mail address of the recipient.</param>
        /// <param name="displayName">Display name of the recipient (can be empty).</param>
        /// <param name="model">Data model to apply on the <see cref="Email{TModel}.Template"/> to generate the e-mail for the recipient.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="address"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="displayName"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> argument is <see langword="null"/>.</exception>
        public EmailRecipient(EmailAddress address, string displayName, TModel model)
        {
            ArgumentNullException.ThrowIfNull(address);
            ArgumentNullException.ThrowIfNull(displayName);
            ArgumentNullException.ThrowIfNull(model);

            this.Address = address;
            this.DisplayName = displayName;
            this.Model = model;
        }

        /// <summary>
        /// Gets the e-mail address of the recipient.
        /// </summary>
        public EmailAddress Address { get; }

        /// <summary>
        /// Gets the display name of the recipient.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the data model to apply on the <see cref="Email{TModel}.Template"/> to generate the e-mail for the recipient.
        /// </summary>
        public TModel Model { get; }
    }
}
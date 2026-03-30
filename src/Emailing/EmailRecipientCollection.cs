//-----------------------------------------------------------------------
// <copyright file="EmailRecipientCollection.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using PosInformatique.Foundations.EmailAddresses;

    /// <summary>
    /// Represents a collection of <see cref="EmailRecipient{TModel}"/> to send
    /// the <see cref="Email{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel">Data model injected to the <see cref="EmailTemplate{TModel}"/> to generate the e-mail for each recipient.</typeparam>
    public class EmailRecipientCollection<TModel> : Collection<EmailRecipient<TModel>>
    {
        /// <summary>
        /// Creates and add new <see cref="EmailRecipient{TModel}"/> in the <see cref="EmailRecipientCollection{TModel}"/>.
        /// </summary>
        /// <param name="address">E-mail address of the recipient.</param>
        /// <param name="model">Data model to apply on the <see cref="Email{TModel}.Template"/> to generate the e-mail for the recipient.</param>
        /// <returns>The <see cref="EmailRecipientCollection{TModel}"/> created and added.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="address"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> argument is <see langword="null"/>.</exception>
        public EmailRecipient<TModel> Add(EmailAddress address, TModel model)
        {
            return this.Add(address, string.Empty, model);
        }

        /// <summary>
        /// Creates and add new <see cref="EmailRecipient{TModel}"/> in the <see cref="EmailRecipientCollection{TModel}"/>.
        /// </summary>
        /// <param name="address">E-mail address of the recipient.</param>
        /// <param name="displayName">Display name of the recipient (can be empty).</param>
        /// <param name="model">Data model to apply on the <see cref="Email{TModel}.Template"/> to generate the e-mail for the recipient.</param>
        /// <returns>The <see cref="EmailRecipientCollection{TModel}"/> created and added.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="address"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="displayName"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> argument is <see langword="null"/>.</exception>
        public EmailRecipient<TModel> Add(EmailAddress address, string displayName, TModel model)
        {
            ArgumentNullException.ThrowIfNull(address);
            ArgumentNullException.ThrowIfNull(displayName);
            ArgumentNullException.ThrowIfNull(model);

            var recipient = new EmailRecipient<TModel>(address, displayName, model);

            this.Add(recipient);

            return recipient;
        }
    }
}
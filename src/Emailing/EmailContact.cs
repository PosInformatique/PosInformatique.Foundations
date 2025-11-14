//-----------------------------------------------------------------------
// <copyright file="EmailContact.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using PosInformatique.Foundations.EmailAddresses;

    /// <summary>
    /// Represents an e-mail contact (e-mail address and a display name).
    /// </summary>
    public class EmailContact
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailContact"/> class.
        /// </summary>
        /// <param name="email">The e-mail of the contact.</param>
        /// <param name="displayName">The display name of the contact (can be empty).</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="email"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="displayName"/> argument is <see langword="null"/>.</exception>
        public EmailContact(EmailAddress email, string displayName)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(displayName);

            this.Email = email;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Gets the e-mail of the contact.
        /// </summary>
        public EmailAddress Email { get; }

        /// <summary>
        /// Gets the display name of the contact.
        /// </summary>
        public string DisplayName { get; }
    }
}
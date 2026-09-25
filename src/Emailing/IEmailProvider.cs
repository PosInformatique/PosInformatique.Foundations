//-----------------------------------------------------------------------
// <copyright file="IEmailProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Represents a provider to send an <see cref="EmailMessage"/>.
    /// </summary>
    public interface IEmailProvider
    {
        /// <summary>
        /// Sends the specified e-mail <paramref name="message"/>.
        /// </summary>
        /// <param name="message">E-mail message to send.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> which allows to cancel the send of the e-mail.</param>
        /// <returns>A <see cref="Task"/> instance which represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="EmailProviderException">Thrown when an error occurs at the provider side (configuration errors, unavailable services, etc.) and impacts the sending of e-mails.</exception>
        Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }
}
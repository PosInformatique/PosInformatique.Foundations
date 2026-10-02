//-----------------------------------------------------------------------
// <copyright file="EmailProviderException.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Represents errors that occur at the provider side (configuration errors, unavailable services, etc.)
    /// and impact the sending of e-mails.
    /// </summary>
    public class EmailProviderException : EmailingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailProviderException"/> class.
        /// </summary>
        public EmailProviderException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailProviderException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public EmailProviderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailProviderException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EmailProviderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
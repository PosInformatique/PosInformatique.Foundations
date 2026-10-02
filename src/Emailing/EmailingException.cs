//-----------------------------------------------------------------------
// <copyright file="EmailingException.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Base class for exceptions that occur during the e-mailing process.
    /// </summary>
    public abstract class EmailingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailingException"/> class.
        /// </summary>
        private protected EmailingException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailingException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        private protected EmailingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailingException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        private protected EmailingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
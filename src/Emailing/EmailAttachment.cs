//-----------------------------------------------------------------------
// <copyright file="EmailAttachment.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using PosInformatique.Foundations.MediaTypes;

    /// <summary>
    /// Represents an email attachment of the <see cref="EmailMessage"/>.
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAttachment"/> class.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="contentType">The MIME type of the <paramref name="content"/>.</param>
        /// <param name="content">The content of the attachment. The stream will be read when the <see cref="IEmailManager.SendAsync{TModel}(Email{TModel}, CancellationToken)"/>
        /// is called. The <paramref name="content"/> stream will not be disposed by the e-mailing system it is the responsibility
        /// of the caller to dispose it once the e-mail have been sent.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="fileName"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> argument is empty or contains white spaces.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="contentType"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="content"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="content"/> stream argument is not readable.</exception>
        public EmailAttachment(string fileName, MimeType contentType, Stream content)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
            ArgumentNullException.ThrowIfNull(contentType);
            ArgumentNullException.ThrowIfNull(content);

            if (!content.CanRead)
            {
                throw new ArgumentException("The content stream must be readable.", nameof(content));
            }

            this.FileName = fileName;
            this.ContentType = contentType;
            this.Content = content;
        }

        /// <summary>
        /// Gets the content <see cref="Stream"/> of the attachment.
        /// </summary>
        /// <remarks>
        /// The stream will be read when the <see cref="IEmailManager.SendAsync{TModel}(Email{TModel}, CancellationToken)"/>
        /// is called. The <see cref="Stream"/> will not be disposed by the e-mailing system it is the responsibility
        /// of the caller to dispose it once the e-mail have been sent.
        /// </remarks>
        public Stream Content { get; }

        /// <summary>
        /// Gets the file name of the attachment.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets the content type of the attachment.
        /// </summary>
        public MimeType ContentType { get; }
    }
}
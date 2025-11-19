//-----------------------------------------------------------------------
// <copyright file="MimeTypeExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.MediaTypes
{
    /// <summary>
    /// Provides extension methods for the <see cref="MimeType"/> class.
    /// </summary>
    public static class MimeTypeExtensions
    {
        /// <summary>
        /// Determines whether the specified media type represents an AutoCAD drawing.
        /// </summary>
        /// <param name="mimeType">The media type to check.</param>
        /// <returns><see langword="true" /> if the media type is <c>image/x-dxf</c> or <c>image/x-dwg</c>; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="mimeType"/> argument is <see langword="null"/>.</exception>
        public static bool IsAutoCad(this MimeType mimeType)
        {
            ArgumentNullException.ThrowIfNull(mimeType);

            if (mimeType == MimeTypes.Image.Dxf)
            {
                return true;
            }

            if (mimeType == MimeTypes.Image.Dwg)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified media type represents a PDF document.
        /// </summary>
        /// <param name="mimeType">The media type to check.</param>
        /// <returns><see langword="true" /> if the media type is <c>application/pdf</c>; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="mimeType"/> argument is <see langword="null"/>.</exception>
        public static bool IsPdf(this MimeType mimeType)
        {
            ArgumentNullException.ThrowIfNull(mimeType);

            return mimeType == MimeTypes.Application.Pdf;
        }

        /// <summary>
        /// Determines whether the specified media type represents an image media type (the AutoCAD drawing are excluded).
        /// </summary>
        /// <param name="mimeType">The media type to check.</param>
        /// <returns><see langword="true" /> if the media type is in the image/* family; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="mimeType"/> argument is <see langword="null"/>.</exception>
        public static bool IsImage(this MimeType mimeType)
        {
            ArgumentNullException.ThrowIfNull(mimeType);

            return mimeType.Type == "image" && !IsAutoCad(mimeType);
        }
    }
}
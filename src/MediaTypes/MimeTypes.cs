//-----------------------------------------------------------------------
// <copyright file="MimeTypes.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.MediaTypes
{
    /// <summary>
    /// Provides predefined media types and helper methods for resolving
    /// media types from file extensions and vice versa.
    /// </summary>
    public static class MimeTypes
    {
        private static readonly Dictionary<string, MimeType> FromExtensions = new Dictionary<string, MimeType>()
        {
            { "pdf", Application.Pdf },
            { "docx", Application.Docx },
            { "bmp", Image.Bmp },
            { "dxf", Image.Dxf },
            { "dwg", Image.Dwg },
            { "jpg", Image.Jpeg },
            { "jpeg", Image.Jpeg },
            { "png", Image.Png },
            { "tif", Image.Tiff },
            { "tiff", Image.Tiff },
            { "webp", Image.WebP },
        };

        private static readonly Dictionary<MimeType, string> ToExtensions = new Dictionary<MimeType, string>()
        {
            { Application.Pdf, "pdf" },
            { Application.Docx, "docx" },
            { Image.Bmp, "bmp" },
            { Image.Dxf, "dxf" },
            { Image.Dwg, "dwg" },
            { Image.Jpeg, "jpg" },
            { Image.Png, "png" },
            { Image.Tiff, "tiff" },
            { Image.WebP, "webp" },
        };

        internal static MimeType FromExtension(string extension)
        {
            if (extension.StartsWith(".", StringComparison.InvariantCultureIgnoreCase))
            {
                extension = extension.Substring(1);
            }

            extension = extension.ToLowerInvariant();

            if (FromExtensions.TryGetValue(extension, out var mimeType))
            {
                return mimeType;
            }

            return Application.OctetStream;
        }

        internal static string GetExtension(MimeType mimeType)
        {
            if (ToExtensions.TryGetValue(mimeType, out var extensionFound))
            {
                return "." + extensionFound;
            }

            return string.Empty;
        }

        /// <summary>
        /// Common application/* media types.
        /// </summary>
        public static class Application
        {
            /// <summary>
            /// Gets the media type <c>application/octet-stream</c>.
            /// </summary>
            public static MimeType OctetStream { get; } = MimeType.Parse("application/octet-stream", null);

            /// <summary>
            /// Gets the media type <c>application/pdf</c>.
            /// </summary>
            public static MimeType Pdf { get; } = MimeType.Parse("application/pdf", null);

            /// <summary>
            /// Gets the media type <c>application/vnd.openxmlformats-officedocument.wordprocessingml.document</c>.
            /// </summary>
            public static MimeType Docx { get; } = MimeType.Parse("application/vnd.openxmlformats-officedocument.wordprocessingml.document", null);
        }

        /// <summary>
        /// Common image/* media types.
        /// </summary>
        public static class Image
        {
            /// <summary>
            /// Gets the media type <c>image/bmp</c>.
            /// </summary>
            public static MimeType Bmp { get; } = MimeType.Parse("image/bmp", null);

            /// <summary>
            /// Gets the media type <c>image/x-dxf</c>.
            /// </summary>
            public static MimeType Dxf { get; } = MimeType.Parse("image/x-dxf");

            /// <summary>
            /// Gets the media type <c>image/x-dwg</c>.
            /// </summary>
            public static MimeType Dwg { get; } = MimeType.Parse("image/x-dwg");

            /// <summary>
            /// Gets the media type <c>image/jpeg</c>.
            /// </summary>
            public static MimeType Jpeg { get; } = MimeType.Parse("image/jpeg", null);

            /// <summary>
            /// Gets the media type <c>image/png</c>.
            /// </summary>
            public static MimeType Png { get; } = MimeType.Parse("image/png", null);

            /// <summary>
            /// Gets the media type <c>image/tiff</c>.
            /// </summary>
            public static MimeType Tiff { get; } = MimeType.Parse("image/tiff", null);

            /// <summary>
            /// Gets the media type <c>image/webp</c>.
            /// </summary>
            public static MimeType WebP { get; } = MimeType.Parse("image/webp", null);
        }
    }
}
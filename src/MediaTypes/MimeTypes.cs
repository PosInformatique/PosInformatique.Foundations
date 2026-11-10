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
        private static readonly Dictionary<string, MimeType> FromExtensions = new()
        {
            { "bmp", Image.Bmp },
            { "csv", Text.Csv },
            { "docx", Application.Docx },
            { "dwg", Image.Dwg },
            { "dxf", Image.Dxf },
            { "html", Text.Html },
            { "jpeg", Image.Jpeg },
            { "jpg", Image.Jpeg },
            { "md", Text.Markdown },
            { "pdf", Application.Pdf },
            { "png", Image.Png },
            { "rtf", Text.Rtf },
            { "svg", Image.Svg },
            { "tif", Image.Tiff },
            { "tiff", Image.Tiff },
            { "tsv", Text.TabSeparatedValues },
            { "txt", Text.Plain },
            { "webp", Image.WebP },
        };

        private static readonly Dictionary<MimeType, string> ToExtensions = new()
        {
            { Application.Docx, "docx" },
            { Application.Pdf, "pdf" },
            { Image.Bmp, "bmp" },
            { Image.Dwg, "dwg" },
            { Image.Dxf, "dxf" },
            { Image.Jpeg, "jpg" },
            { Image.Png, "png" },
            { Image.Svg, "svg" },
            { Image.Tiff, "tiff" },
            { Image.WebP, "webp" },
            { Text.Csv, "csv" },
            { Text.Html, "html" },
            { Text.Markdown, "md" },
            { Text.Plain, "txt" },
            { Text.Rtf, "rtf" },
            { Text.TabSeparatedValues, "tsv" },
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
            /// Gets the media type <c>application/vnd.openxmlformats-officedocument.wordprocessingml.document</c>.
            /// </summary>
            public static MimeType Docx { get; } = MimeType.Parse("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            /// <summary>
            /// Gets the media type <c>application/octet-stream</c>.
            /// </summary>
            public static MimeType OctetStream { get; } = MimeType.Parse("application/octet-stream");

            /// <summary>
            /// Gets the media type <c>application/pdf</c>.
            /// </summary>
            public static MimeType Pdf { get; } = MimeType.Parse("application/pdf");
        }

        /// <summary>
        /// Common image/* media types.
        /// </summary>
        public static class Image
        {
            /// <summary>
            /// Gets the media type <c>image/bmp</c>.
            /// </summary>
            public static MimeType Bmp { get; } = MimeType.Parse("image/bmp");

            /// <summary>
            /// Gets the media type <c>image/x-dwg</c>.
            /// </summary>
            public static MimeType Dwg { get; } = MimeType.Parse("image/x-dwg");

            /// <summary>
            /// Gets the media type <c>image/x-dxf</c>.
            /// </summary>
            public static MimeType Dxf { get; } = MimeType.Parse("image/x-dxf");

            /// <summary>
            /// Gets the media type <c>image/jpeg</c>.
            /// </summary>
            public static MimeType Jpeg { get; } = MimeType.Parse("image/jpeg");

            /// <summary>
            /// Gets the media type <c>image/png</c>.
            /// </summary>
            public static MimeType Png { get; } = MimeType.Parse("image/png");

            /// <summary>
            /// Gets the media type <c>image/svg+xml</c>.
            /// </summary>
            public static MimeType Svg { get; } = MimeType.Parse("image/svg+xml");

            /// <summary>
            /// Gets the media type <c>image/tiff</c>.
            /// </summary>
            public static MimeType Tiff { get; } = MimeType.Parse("image/tiff");

            /// <summary>
            /// Gets the media type <c>image/webp</c>.
            /// </summary>
            public static MimeType WebP { get; } = MimeType.Parse("image/webp");
        }

        /// <summary>
        /// Common text/* media types.
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// Gets the media type <c>text/csv</c>.
            /// </summary>
            public static MimeType Csv { get; } = MimeType.Parse("text/csv");

            /// <summary>
            /// Gets the media type <c>text/html</c>.
            /// </summary>
            public static MimeType Html { get; } = MimeType.Parse("text/html");

            /// <summary>
            /// Gets the media type <c>text/markdown</c>.
            /// </summary>
            public static MimeType Markdown { get; } = MimeType.Parse("text/markdown");

            /// <summary>
            /// Gets the media type <c>text/plain</c>.
            /// </summary>
            public static MimeType Plain { get; } = MimeType.Parse("text/plain");

            /// <summary>
            /// Gets the media type <c>text/rtf</c>.
            /// </summary>
            public static MimeType Rtf { get; } = MimeType.Parse("text/rtf");

            /// <summary>
            /// Gets the media type <c>text/tab-separated-values</c>.
            /// </summary>
            public static MimeType TabSeparatedValues { get; } = MimeType.Parse("text/tab-separated-values");
        }
    }
}
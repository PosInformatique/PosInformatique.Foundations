//-----------------------------------------------------------------------
// <copyright file="UriExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System
{
    /// <summary>
    /// Provides extension methods for <see cref="Uri"/>.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Appends path components to an absolute URI.
        /// </summary>
        /// <param name="uri">The absolute base URI.</param>
        /// <param name="paths">The path components to append. Null and empty components are ignored.</param>
        /// <returns>A URI with the appended path and the original query and fragment.</returns>
        /// <remarks>Leading and trailing slashes between components are normalized to a single slash.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="uri"/> or <paramref name="paths"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="uri"/> is relative.</exception>
        public static Uri AppendPath(this Uri uri, params string?[] paths)
        {
            ArgumentNullException.ThrowIfNull(uri);
            ArgumentNullException.ThrowIfNull(paths);

            var builder = new UriBuilder(uri);
            var path = paths.Where(p => !string.IsNullOrEmpty(p))
                .Aggregate(uri.AbsolutePath, (current, part) => $"{current.TrimEnd('/')}/{part!.TrimStart('/')}");

            builder.Path = path;
            return builder.Uri;
        }
    }
}
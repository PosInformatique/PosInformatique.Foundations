//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System
{
    /// <summary>
    /// Provides extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates a string to at most the specified number of UTF-16 code units.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the result.</param>
        /// <returns>The original string if it fits, or its prefix of the specified length.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxLength"/> is negative.</exception>
        public static string Truncate(this string value, int maxLength)
        {
            ArgumentNullException.ThrowIfNull(value);
            ArgumentOutOfRangeException.ThrowIfNegative(maxLength);

            return value[..Math.Min(value.Length, maxLength)];
        }
    }
}
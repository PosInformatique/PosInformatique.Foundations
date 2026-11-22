//-----------------------------------------------------------------------
// <copyright file="TimeProviderExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System
{
    /// <summary>
    /// Provides extension methods for <see cref="TimeProvider"/>.
    /// </summary>
    public static class TimeProviderExtensions
    {
        /// <summary>
        /// Gets the current UTC date from the time provider.
        /// </summary>
        /// <param name="timeProvider">The time provider.</param>
        /// <returns>The UTC date, not the date in the provider's local time zone.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeProvider"/> is <see langword="null"/>.</exception>
        public static DateOnly GetUtcToday(this TimeProvider timeProvider)
        {
            ArgumentNullException.ThrowIfNull(timeProvider);

            return DateOnly.FromDateTime(timeProvider.GetUtcNow().Date);
        }

        /// <summary>
        /// Gets the current date in the time provider's local time zone.
        /// </summary>
        /// <param name="timeProvider">The time provider.</param>
        /// <returns>The date in the time provider's local time zone.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeProvider"/> is <see langword="null"/>.</exception>
        public static DateOnly GetLocalToday(this TimeProvider timeProvider)
        {
            ArgumentNullException.ThrowIfNull(timeProvider);

            return DateOnly.FromDateTime(timeProvider.GetLocalNow().Date);
        }
    }
}
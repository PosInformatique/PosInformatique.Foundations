//-----------------------------------------------------------------------
// <copyright file="DateTimePropertyExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    /// <summary>
    /// Provides extension methods for configuring <see cref="DateTime"/> properties in Entity Framework Core.
    /// </summary>
    public static class DateTimePropertyExtensions
    {
        /// <summary>
        /// Configures the specified <see cref="PropertyBuilder{DateTime}"/> to use UTC for the <see cref="DateTime"/> property.
        /// </summary>
        /// <param name="property">The property builder for the <see cref="DateTime"/> property.</param>
        /// <returns>The <paramref name="property"/> instance to continue the configuration of the property.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="property"/> is <see langword="null"/>.</exception>
        public static PropertyBuilder<DateTime> IsUtc(this PropertyBuilder<DateTime> property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return property
                .HasConversion(DateTimeConverter.Instance);
        }

        /// <summary>
        /// Configures the specified <see cref="PropertyBuilder{DateTime}"/> to use UTC for the nullable <see cref="DateTime"/> property.
        /// </summary>
        /// <param name="property">The property builder for the <see cref="DateTime"/> property.</param>
        /// <returns>The <paramref name="property"/> instance to continue the configuration of the property.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="property"/> is <see langword="null"/>.</exception>
        public static PropertyBuilder<DateTime?> IsUtc(this PropertyBuilder<DateTime?> property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return property
                .HasConversion(DateTimeConverter.Instance);
        }

        private sealed class DateTimeConverter : ValueConverter<DateTime, DateTime>
        {
            private DateTimeConverter()
                : base(dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc))
            {
            }

            public static DateTimeConverter Instance { get; } = new DateTimeConverter();
        }
    }
}
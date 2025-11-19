//-----------------------------------------------------------------------
// <copyright file="MimeTypePropertyExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using PosInformatique.Foundations.MediaTypes;

    /// <summary>
    /// Contains extension methods to map a <see cref="MimeType"/> to a string column.
    /// </summary>
    public static class MimeTypePropertyExtensions
    {
        /// <summary>
        /// Configures the specified <paramref name="property"/> to be mapped on a column with a SQL <c>MimeType</c> type.
        /// The <c>MimeType</c> type must be mapped to a <c>VARCHAR(128)</c>.
        /// </summary>
        /// <typeparam name="T">Type of the property which must be <see cref="MimeType"/>.</typeparam>
        /// <param name="property">Entity property to map in the <see cref="ModelBuilder"/>.</param>
        /// <returns>The <paramref name="property"/> instance to configure the configuration of the property.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="property"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the specified <typeparamref name="T"/> generic type is not a <see cref="MimeType"/>.</exception>
        public static PropertyBuilder<T> IsMimeType<T>(this PropertyBuilder<T> property)
        {
            ArgumentNullException.ThrowIfNull(property);

            if (typeof(T) != typeof(MimeType))
            {
                throw new ArgumentException($"The '{nameof(IsMimeType)}()' method must be called on '{nameof(MimeType)} class.", nameof(property));
            }

            return property
                .IsUnicode(false)
                .HasMaxLength(128)
                .HasColumnType("MimeType")
                .HasConversion(MimeTypeConverter.Instance);
        }

        private sealed class MimeTypeConverter : ValueConverter<MimeType, string>
        {
            private MimeTypeConverter()
                : base(mimeType => mimeType.ToString(), @string => MimeType.Parse(@string))
            {
            }

            public static MimeTypeConverter Instance { get; } = new MimeTypeConverter();
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="PhoneNumberPropertyExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using PosInformatique.Foundations.PhoneNumbers;

    /// <summary>
    /// Contains extension methods to map a <see cref="PhoneNumber"/> to a string column.
    /// </summary>
    public static class PhoneNumberPropertyExtensions
    {
        /// <summary>
        /// Configures the specified <paramref name="property"/> to be mapped on a column with a SQL <c>PhoneNumber</c> type.
        /// The <c>PhoneNumber</c> type must be mapped to a <c>VARCHAR(320)</c>.
        /// </summary>
        /// <typeparam name="T">Type of the property which must be <see cref="PhoneNumber"/>.</typeparam>
        /// <param name="property">Entity property to map in the <see cref="ModelBuilder"/>.</param>
        /// <returns>The <paramref name="property"/> instance to configure the configuration of the property.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="property"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the specified <typeparamref name="T"/> generic type is not a <see cref="PhoneNumber"/>.</exception>
        public static PropertyBuilder<T> IsPhoneNumber<T>(this PropertyBuilder<T> property)
        {
            ArgumentNullException.ThrowIfNull(property);

            if (typeof(T) != typeof(PhoneNumber))
            {
                throw new ArgumentException($"The '{nameof(IsPhoneNumber)}()' method must be called on '{nameof(PhoneNumber)} class.", nameof(property));
            }

            return property
                .IsUnicode(false)
                .HasMaxLength(16)
                .HasColumnType("PhoneNumber")
                .HasConversion(PhoneNumberConverter.Instance);
        }

        private sealed class PhoneNumberConverter : ValueConverter<PhoneNumber, string>
        {
            private PhoneNumberConverter()
                : base(v => v.ToString(), v => PhoneNumber.Parse(v, null))
            {
            }

            public static PhoneNumberConverter Instance { get; } = new PhoneNumberConverter();
        }
    }
}

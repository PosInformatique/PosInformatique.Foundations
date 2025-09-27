//-----------------------------------------------------------------------
// <copyright file="EmailAddressPropertyExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using PosInformatique.Foundations.EmailAddresses;

    /// <summary>
    /// Contains extension method to map a <see cref="EmailAddress"/> to a string column.
    /// </summary>
    public static class EmailAddressPropertyExtensions
    {
        /// <summary>
        /// Configures the specified <paramref name="property"/> to be mapped on a column with a SQL <c>EmailAddress</c> type.
        /// The <c>EmailAddress</c> type must be mapped to a <c>VARCHAR(320)</c>.
        /// </summary>
        /// <typeparam name="T">Type of the property which must be <see cref="EmailAddress"/>.</typeparam>
        /// <param name="property">Entity property to map in the <see cref="ModelBuilder"/>.</param>
        /// <returns>The <paramref name="property"/> instance to configure the configuration of the property.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="property"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the specified <typeparamref name="T"/> generic type is not a <see cref="EmailAddress"/>.</exception>
        public static PropertyBuilder<T> IsEmailAddress<T>(this PropertyBuilder<T> property)
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));

            if (typeof(T) != typeof(EmailAddress))
            {
                throw new ArgumentException($"The '{nameof(IsEmailAddress)}()' method must be called on '{nameof(EmailAddress)} class.", nameof(T));
            }

            return property
                .IsUnicode(false)
                .HasMaxLength(320)
                .HasColumnType("EmailAddress")
                .HasConversion(EmailAddressConverter.Instance);
        }

        private sealed class EmailAddressConverter : ValueConverter<EmailAddress, string>
        {
            private EmailAddressConverter()
                : base(v => v.ToString(), v => EmailAddress.Parse(v))
            {
            }

            public static EmailAddressConverter Instance { get; } = new EmailAddressConverter();
        }
    }
}

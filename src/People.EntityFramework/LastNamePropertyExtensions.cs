//-----------------------------------------------------------------------
// <copyright file="LastNamePropertyExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using PosInformatique.Foundations.People;

    /// <summary>
    /// Contains extension method to map a <see cref="LastName"/> to a string column.
    /// </summary>
    public static class LastNamePropertyExtensions
    {
        /// <summary>
        /// Configures the specified <paramref name="property"/> to be mapped on a <c>NVARCHAR(50)</c> column
        /// to store a <see cref="LastName"/> instance.
        /// </summary>
        /// <param name="property">Entity property to map in the <see cref="ModelBuilder"/>.</param>
        /// <returns>The <paramref name="property"/> instance to configure the configuration of the property.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="property"/> argument is <see langword="null"/>.</exception>
        public static PropertyBuilder<LastName> IsLastName(this PropertyBuilder<LastName> property)
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));

            return property
                .IsUnicode(true)
                .IsFixedLength(false)
                .HasMaxLength(LastName.MaxLength)
                .HasConversion(LastNameConverter.Instance, LastNameComparer.Instance);
        }

        private sealed class LastNameConverter : ValueConverter<LastName, string>
        {
            private LastNameConverter()
                : base(v => v.ToString(), v => v)
            {
            }

            public static LastNameConverter Instance { get; } = new LastNameConverter();
        }

        private sealed class LastNameComparer : ValueComparer<LastName>
        {
            private LastNameComparer()
                : base(true)
            {
            }

            public static LastNameComparer Instance { get; } = new LastNameComparer();
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="FirstNamePropertyExtensions.cs" company="P.O.S Informatique">
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
    /// Contains extension method to map a <see cref="FirstName"/> to a string column.
    /// </summary>
    public static class FirstNamePropertyExtensions
    {
        /// <summary>
        /// Configures the specified <paramref name="property"/> to be mapped on a <c>NVARCHAR(50)</c> column
        /// to store a <see cref="FirstName"/> instance.
        /// </summary>
        /// <param name="property">Entity property to map in the <see cref="ModelBuilder"/>.</param>
        /// <returns>The <paramref name="property"/> instance to configure the configuration of the property.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="property"/> argument is <see langword="null"/>.</exception>
        public static PropertyBuilder<FirstName> IsFirstName(this PropertyBuilder<FirstName> property)
        {
            return property
                .IsUnicode(true)
                .IsFixedLength(false)
                .HasMaxLength(FirstName.MaxLength)
                .HasConversion(FirstNameConverter.Instance, FirstNameComparer.Instance);
        }

        private sealed class FirstNameConverter : ValueConverter<FirstName, string>
        {
            private FirstNameConverter()
                : base(v => v.ToString(), v => v)
            {
            }

            public static FirstNameConverter Instance { get; } = new FirstNameConverter();
        }

        private sealed class FirstNameComparer : ValueComparer<FirstName>
        {
            private FirstNameComparer()
                : base(true)
            {
            }

            public static FirstNameComparer Instance { get; } = new FirstNameComparer();
        }
    }
}

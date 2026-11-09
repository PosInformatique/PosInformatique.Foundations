//-----------------------------------------------------------------------
// <copyright file="EnumToBooleanConverter.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore
{
    using System.Globalization;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    /// <summary>
    /// A value converter that converts an enumeration type to a boolean value and vice versa.
    /// </summary>
    /// <remarks>The <typeparamref name="TEnum"/> type must be an enumeration with values that can be represented as 0 (<see langword="false" />) and 1 (<see langword="true" />).</remarks>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    public sealed class EnumToBooleanConverter<TEnum> : ValueConverter<TEnum, bool>
        where TEnum : struct, Enum
    {
        private EnumToBooleanConverter()
            : base(
                v => Convert.ToBoolean(Convert.ToInt32(v, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                v => (TEnum)Enum.ToObject(typeof(TEnum), Convert.ToInt32(v, CultureInfo.InvariantCulture)))
        {
            EnsureEnumHasOnlyZeroAndOneValues();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="EnumToBooleanConverter{TEnum}"/> class.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the enumeration does not contain exactly two distinct values that can be represented as 0 (<see langword="false" />) and 1 (<see langword="false" />).
        /// This exception is wrapped in a <see cref="TypeInitializationException"/> when accessing the <see cref="Instance"/> property.</exception>
        public static EnumToBooleanConverter<TEnum> Instance { get; } = new EnumToBooleanConverter<TEnum>();

        private static void EnsureEnumHasOnlyZeroAndOneValues()
        {
            var values = Enum.GetValues<TEnum>()
                .Select(v => Convert.ToInt32(v, CultureInfo.InvariantCulture))
                .Distinct()
                .ToArray();

            if (values.Length != 2)
            {
                throw new ArgumentException(
                    $"The enumeration '{typeof(TEnum).FullName}' must only contain two distinct values that can be represented as 0 or 1.",
                    typeof(TEnum).Name);
            }

            if (!values.Contains(0))
            {
                throw new ArgumentException(
                    $"The enumeration '{typeof(TEnum).FullName}' must contain a value that can be represented as 0 (False).",
                    typeof(TEnum).Name);
            }

            if (!values.Contains(1))
            {
                throw new ArgumentException(
                    $"The enumeration '{typeof(TEnum).FullName}' must contain a value that can be represented as 1 (True).",
                    typeof(TEnum).Name);
            }
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="PhoneNumber.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.PhoneNumbers
{
    using System.Diagnostics.CodeAnalysis;
    using global::PhoneNumbers;

    /// <summary>
    /// Represents a valid international phone number in E.164 format. Any attempt to create an invalid phone number is rejected.
    /// </summary>
    /// <remarks>
    /// This class provides several features:
    /// <list type="bullet">
    /// <item><description>
    /// Implements <see cref="IEquatable{T}"/> so that instances can be compared and used seamlessly
    /// in generic scenarios such as collections, equality checks, or dictionaries.
    /// </description></item>
    /// <item><description>
    /// Implements <see cref="IFormattable"/> and <see cref="IParsable{TSelf}"/> to enable generic
    /// conversion to and from string representations, making it easy to integrate with components
    /// that rely on string formatting and parsing.
    /// </description></item>
    /// </list>
    /// </remarks>
    public sealed class PhoneNumber : IEquatable<PhoneNumber>, IFormattable, IParsable<PhoneNumber>
    {
        private const PhoneNumberFormat DefaultPhoneNumberFormat = PhoneNumberFormat.E164;

        private static readonly PhoneNumberUtil PhoneNumbersUtil = PhoneNumberUtil.GetInstance();

        private readonly global::PhoneNumbers.PhoneNumber wrappedInstance;

        private PhoneNumber(string phoneNumber, string? defaultRegion)
        {
            this.wrappedInstance = PhoneNumbersUtil.Parse(phoneNumber, defaultRegion);
        }

        /// <summary>
        /// Implicitly converts a <see cref="PhoneNumber"/> to a <see cref="string"/> in E.164 format.
        /// </summary>
        /// <param name="phoneNumber">The phone number to convert.</param>
        /// <returns>The string representation of the phone number in E.164 format.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="phoneNumber"/> argument is <see langword="null"/>.</exception>
        public static implicit operator string(PhoneNumber phoneNumber)
        {
            ArgumentNullException.ThrowIfNull(phoneNumber);

            return phoneNumber.ToString(DefaultPhoneNumberFormat);
        }

        /// <summary>
        /// Implicitly converts a <see cref="string"/> to a <see cref="PhoneNumber"/>.
        /// </summary>
        /// <param name="phoneNumber">The string to convert to a phone number.</param>
        /// <returns>A <see cref="PhoneNumber"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="phoneNumber"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Thrown when the string is not a valid E.164 phone number.</exception>
        public static implicit operator PhoneNumber(string phoneNumber)
        {
            ArgumentNullException.ThrowIfNull(phoneNumber);

            return Parse(phoneNumber);
        }

        /// <summary>
        /// Determines whether two <see cref="PhoneNumber"/> instances are equal.
        /// </summary>
        /// <param name="left">The first phone number to compare.</param>
        /// <param name="right">The second phone number to compare.</param>
        /// <returns><see langword="true"/> if the phone numbers are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(PhoneNumber? left, PhoneNumber? right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two <see cref="PhoneNumber"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first phone number to compare.</param>
        /// <param name="right">The second phone number to compare.</param>
        /// <returns><see langword="true"/> if the phone numbers are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(PhoneNumber? left, PhoneNumber? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Parses a string representation of a phone number.
        /// </summary>
        /// <param name="s">The phone number string to parse. It can be an E.164 number or a local phone number.</param>
        /// <param name="defaultRegion">
        /// The region of the phone number to parse when <paramref name="s"/> is a local phone number.
        /// This must be specified using an ISO 3166-1 alpha-2 country code (for example "US", "FR", ...).
        /// This parameter is ignored when <paramref name="s"/> is already in E.164 format.
        /// </param>
        /// <returns>A <see cref="PhoneNumber"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="s"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Thrown when the specified value is not a valid phone number in E.164 format.</exception>
        public static PhoneNumber Parse(string s, string? defaultRegion = null)
        {
            ArgumentNullException.ThrowIfNull(s);

            var (result, exception) = ParseInternal(s, defaultRegion);

            if (exception is not null)
            {
                throw exception;
            }

            return result!;
        }

        /// <summary>
        /// Parses a string representation of a phone number.
        /// </summary>
        /// <param name="s">The phone number string to parse. It can be an E.164 number or a local phone number.</param>
        /// <param name="provider">The format provider (not used in this implementation).</param>
        /// <returns>A <see cref="PhoneNumber"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="s"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Thrown when the specified value is not a valid phone number in E.164 format.</exception>
        static PhoneNumber IParsable<PhoneNumber>.Parse(string s, IFormatProvider? provider)
        {
            ArgumentNullException.ThrowIfNull(s);

            return Parse(s, null);
        }

        /// <summary>
        /// Tries to parse a string representation of a phone number.
        /// </summary>
        /// <param name="s">The phone number string to parse. It can be an E.164 number or a local phone number.</param>
        /// <param name="phoneNumber">
        /// When this method returns, contains the parsed <see cref="PhoneNumber"/> if the parsing succeeded,
        /// or <see langword="null"/> if it failed.
        /// </param>
        /// <param name="defaultRegion">
        /// The region of the phone number to parse when <paramref name="s"/> is a local phone number.
        /// This must be specified using an ISO 3166-1 alpha-2 country code (for example "US", "FR", ...).
        /// This parameter is ignored when <paramref name="s"/> is already in E.164 format.
        /// </param>
        /// <returns><see langword="true"/> if the parsing succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)][NotNullWhen(true)] out PhoneNumber? phoneNumber, string? defaultRegion = null)
        {
            phoneNumber = ParseInternal(s, defaultRegion).Number;

            return phoneNumber is not null;
        }

        /// <summary>
        /// Tries to parse a string representation of a phone number.
        /// </summary>
        /// <param name="s">The phone number string to parse. It can be an E.164 number or a local phone number.</param>
        /// <param name="provider">The format provider (not used in this implementation).</param>
        /// <param name="result">
        /// When this method returns, contains the parsed <see cref="PhoneNumber"/> if the parsing succeeded,
        /// or <see langword="null"/> if it failed.
        /// </param>
        /// <returns><see langword="true"/> if the parsing succeeded; otherwise, <see langword="false"/>.</returns>
        static bool IParsable<PhoneNumber>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out PhoneNumber? result)
        {
            return TryParse(s, out result, null);
        }

        /// <summary>
        /// Determines if the specified <paramref name="phoneNumber"/> is a valid phone number in E.164 format.
        /// </summary>
        /// <param name="phoneNumber">The phone number string to test.</param>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="phoneNumber"/> is a valid phone number in E.164 format;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsValid(string phoneNumber)
        {
            return TryParse(phoneNumber, out var _);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is not PhoneNumber number)
            {
                return false;
            }

            return this.Equals(number);
        }

        /// <summary>
        /// Determines whether the current <see cref="PhoneNumber"/> is equal to another <see cref="PhoneNumber"/>.
        /// </summary>
        /// <param name="other">The other phone number to compare with.</param>
        /// <returns><see langword="true"/> if the phone numbers are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(PhoneNumber? other)
        {
            if (other is null)
            {
                return false;
            }

            if (!this.wrappedInstance.Equals(other.wrappedInstance))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.wrappedInstance.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation of the <see cref="PhoneNumber"/> in E.164 format.
        /// </summary>
        /// <returns>The string representation of the <see cref="PhoneNumber"/> in E.164 format.</returns>
        public override string ToString()
        {
            return this.ToString(DefaultPhoneNumberFormat);
        }

        /// <inheritdoc />
        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            return this.ToString();
        }

        /// <summary>
        /// Returns the international representation of the <see cref="PhoneNumber"/>.
        /// </summary>
        /// <returns>The international representation of the phone number.</returns>
        public string ToInternationalString()
        {
            return this.ToString(PhoneNumberFormat.INTERNATIONAL);
        }

        /// <summary>
        /// Returns the national representation of the <see cref="PhoneNumber"/>.
        /// </summary>
        /// <returns>The national representation of the phone number.</returns>
        public string ToNationalString()
        {
            return this.ToString(PhoneNumberFormat.NATIONAL);
        }

        private static (PhoneNumber? Number, Exception? Exception) ParseInternal(string? s, string? defaultRegion)
        {
            if (s is null)
            {
                return (null, null);
            }

            PhoneNumber phoneNumber;

            try
            {
                phoneNumber = new PhoneNumber(s, defaultRegion);
            }
            catch (NumberParseException e)
            {
                return (null, new FormatException($"The specified phone number '{s}' is not a valid E164 phone number.", e));
            }

            if (!PhoneNumbersUtil.IsValidNumber(phoneNumber.wrappedInstance))
            {
                return (null, new FormatException($"The specified phone number '{s}' is not a valid E164 phone number."));
            }

            return (phoneNumber, null);
        }

        private string ToString(PhoneNumberFormat numberFormat)
        {
            return PhoneNumbersUtil.Format(this.wrappedInstance, numberFormat);
        }
    }
}
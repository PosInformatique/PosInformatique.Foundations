//-----------------------------------------------------------------------
// <copyright file="EmailAddress.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.EmailAddresses
{
    using System.Diagnostics.CodeAnalysis;
    using MimeKit;

    /// <summary>
    /// Represents a valid e-mail address. Any attempt to create an invalid e-mail is rejected.
    /// </summary>
    /// <remarks>
    /// This class provides several features:
    /// <list type="bullet">
    /// <item><description>
    /// Implements <see cref="IEquatable{T}"/> and <see cref="IComparable{T}"/> so that instances
    /// can be compared and used seamlessly in generic scenarios such as collections, sorting,
    /// or equality checks.
    /// </description></item>
    /// <item><description>
    /// Implements <see cref="IFormattable"/> and <see cref="IParsable{TSelf}"/> to enable generic
    /// conversion to and from string representations, making it easy to integrate with a wide
    /// range of components that rely on string formatting and parsing.
    /// </description></item>
    /// </list>
    /// </remarks>
    public sealed class EmailAddress : IEquatable<EmailAddress>, IComparable<EmailAddress>, IFormattable, IParsable<EmailAddress>
    {
        private static readonly ParserOptions Options = new()
        {
            AllowAddressesWithoutDomain = false,
            AddressParserComplianceMode = RfcComplianceMode.Strict,
            Rfc2047ComplianceMode = RfcComplianceMode.Strict,
            AllowUnquotedCommasInAddresses = false,
        };

        private readonly string value;

        private EmailAddress(MailboxAddress address)
        {
            this.value = address.Address;

            var parts = this.value.Split('@');
            this.UserName = parts[0];
            this.Domain = parts[1];
        }

        /// <summary>
        /// Gets the user name part of the email address (The part before the <c>@</c> separator).
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets the domain part of the email address (The part after the <c>@</c> separator).
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// Implicitly converts an <see cref="EmailAddress"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="emailAddress">The email address to convert.</param>
        /// <returns>The string representation of the email address.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="emailAddress"/> argument is <see langword="null"/>.</exception>
        public static implicit operator string(EmailAddress emailAddress)
        {
            ArgumentNullException.ThrowIfNull(emailAddress, nameof(emailAddress));

            return emailAddress.value;
        }

        /// <summary>
        /// Implicitly converts a <see cref="string"/> to an <see cref="EmailAddress"/>.
        /// </summary>
        /// <param name="emailAddress">The string to convert to an email address.</param>
        /// <returns>An <see cref="EmailAddress"/> instance.</returns>
        /// <exception cref="FormatException">Thrown when the string is not a valid email address.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="emailAddress"/> argument is <see langword="null"/>.</exception>
        public static implicit operator EmailAddress(string emailAddress)
        {
            ArgumentNullException.ThrowIfNull(emailAddress, nameof(emailAddress));

            return Parse(emailAddress, null);
        }

        /// <summary>
        /// Determines whether two <see cref="EmailAddress"/> instances are equal.
        /// </summary>
        /// <param name="left">The first email address to compare.</param>
        /// <param name="right">The second email address to compare.</param>
        /// <returns><see langword="true"/> if the email addresses are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(EmailAddress? left, EmailAddress? right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two <see cref="EmailAddress"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first email address to compare.</param>
        /// <param name="right">The second email address to compare.</param>
        /// <returns><see langword="true"/> if the email addresses are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(EmailAddress? left, EmailAddress? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="EmailAddress"/> is less than another.
        /// </summary>
        /// <param name="left">The first email address to compare.</param>
        /// <param name="right">The second email address to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <(EmailAddress? left, EmailAddress? right)
        {
            return Comparer<EmailAddress>.Default.Compare(left, right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="EmailAddress"/> is less than or equal to another.
        /// </summary>
        /// <param name="left">The first email address to compare.</param>
        /// <param name="right">The second email address to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(EmailAddress? left, EmailAddress? right)
        {
            return Comparer<EmailAddress>.Default.Compare(left, right) <= 0;
        }

        /// <summary>
        /// Determines whether one <see cref="EmailAddress"/> is greater than another.
        /// </summary>
        /// <param name="left">The first email address to compare.</param>
        /// <param name="right">The second email address to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >(EmailAddress? left, EmailAddress? right)
        {
            return Comparer<EmailAddress>.Default.Compare(left, right) > 0;
        }

        /// <summary>
        /// Determines whether one <see cref="EmailAddress"/> is greater than or equal to another.
        /// </summary>
        /// <param name="left">The first email address to compare.</param>
        /// <param name="right">The second email address to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(EmailAddress? left, EmailAddress? right)
        {
            return Comparer<EmailAddress>.Default.Compare(left, right) >= 0;
        }

        /// <summary>
        /// Parses a string representation of an email address.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>An <see cref="EmailAddress"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="s"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Thrown when the string is not a valid email address.</exception>
        public static EmailAddress Parse(string s)
        {
            ArgumentNullException.ThrowIfNull(s, nameof(s));

            return Parse(s, null);
        }

        /// <summary>
        /// Parses a string representation of an email address using the specified format provider.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="provider">The format provider (not used in this implementation).</param>
        /// <returns>An <see cref="EmailAddress"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="s"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Thrown when the string is not a valid email address.</exception>
        public static EmailAddress Parse(string s, IFormatProvider? provider)
        {
            ArgumentNullException.ThrowIfNull(s, nameof(s));

            if (!TryParse(s, out var result))
            {
                throw new FormatException($"'{s}' is not a valid email address.");
            }

            return result;
        }

        /// <summary>
        /// Tries to parse a string representation of an email address.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="EmailAddress"/> if the parsing succeeded, or <see langword="null"/> if it failed.</param>
        /// <returns><see langword="true"/> if the parsing succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)][NotNullWhen(true)] out EmailAddress? result)
        {
            return TryParse(s, null, out result);
        }

        /// <summary>
        /// Tries to parse a string representation of an email address using the specified format provider.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="provider">The format provider (not used in this implementation).</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="EmailAddress"/> if the parsing succeeded, or <see langword="null"/> if it failed.</param>
        /// <returns><see langword="true"/> if the parsing succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out EmailAddress? result)
        {
            var emailAddress = TryParse(s);

            if (emailAddress is null)
            {
                result = null;
                return false;
            }

            result = new EmailAddress(emailAddress);
            return true;
        }

        /// <summary>
        /// Determines if the specified <paramref name="value"/> is a valid e-mail address.
        /// </summary>
        /// <param name="value">E-mail address value to test.</param>
        /// <returns><see langword="true"/> if the <paramref name="value"/> is valid e-mail address, <see langword="false"/> otherwise.</returns>
        public static bool IsValid(string value)
        {
            return TryParse(value, out var _);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is not EmailAddress emailAddress)
            {
                return false;
            }

            return this.Equals(emailAddress);
        }

        /// <summary>
        /// Determines whether the current <see cref="EmailAddress"/> is equal to another <see cref="EmailAddress"/>.
        /// </summary>
        /// <param name="other">The other email address to compare with.</param>
        /// <returns><see langword="true"/> if the email addresses are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(EmailAddress? other)
        {
            if (other is null)
            {
                return false;
            }

            if (!this.value.Equals(other.value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.value.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the string representation of the <see cref="EmailAddress"/>.
        /// </summary>
        /// <returns>The string representation of the <see cref="EmailAddress"/>.</returns>
        public override string ToString()
        {
            return this.value;
        }

        /// <inheritdoc />
        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            return this.ToString();
        }

        /// <summary>
        /// Compare the current <see cref="EmailAddress"/> instance with the <paramref name="other"/> one.
        /// </summary>
        /// <param name="other">Other e-mail address to compare.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared:
        /// <list type="bullet">
        /// <item><description>0 if the current <see cref="EmailAddress"/> is equal to the <paramref name="other"/>.</description></item>
        /// <item><description>A value less than 0 if the current <see cref="EmailAddress"/> is before the <paramref name="other"/> in alphabetic order.</description></item>
        /// <item><description>A value greater than 0 if the current <see cref="EmailAddress"/> is after the <paramref name="other"/> in alphabetic order.</description></item>
        /// </list>
        /// </returns>
        public int CompareTo(EmailAddress? other)
        {
            if (other is null)
            {
                return string.Compare(this.value, null, StringComparison.OrdinalIgnoreCase);
            }

            return string.Compare(this.value, other.value, StringComparison.OrdinalIgnoreCase);
        }

        private static MailboxAddress? TryParse(string? input)
        {
            if (input is null)
            {
                return null;
            }

            input = input.ToLowerInvariant();

            if (!MailboxAddress.TryParse(Options, input, out var address))
            {
                return null;
            }

            if (address.IsInternational)
            {
                return null;
            }

            return address;
        }
    }
}

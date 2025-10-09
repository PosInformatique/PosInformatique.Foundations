//-----------------------------------------------------------------------
// <copyright file="LastName.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Represents a normalized last name with the following rules:
    /// <list type="bullet">
    /// <item>The maximum length is <c>50</c> characters (see <see cref="MaxLength"/>).</item>
    /// <item>Only letters are allowed, with separators limited to space and <c>-</c> (list of allowed characters are accessible from the <see cref="AllowedSeparators"/> property).</item>
    /// <item>All letters are uppercased (e.g., <c>SMITH</c>, <c>SMITH-JOHNSON</c>).</item>
    /// <item>No consecutive or trailing separators are allowed.</item>
    /// <item>Implicit conversions from/to <see cref="string"/> are provided.</item>
    /// <item>Implements <see cref="IFormattable"/> and <see cref="IParsable{TSelf}"/> for standard .NET conversions.</item>
    /// <item>Acts like a read-only array of <see cref="char"/> via <see cref="IReadOnlyList{T}"/>.</item>
    /// </list>
    /// Using this type standardizes last name representation across your domain (users, people, customers, etc.).
    /// </summary>
    public sealed class LastName : IReadOnlyList<char>, IEquatable<LastName>, IComparable<LastName>, IFormattable, IParsable<LastName>
    {
        /// <summary>
        /// Maximum allowed length of a <see cref="LastName"/> (50).
        /// </summary>
        public const int MaxLength = 50;

        private static readonly CultureInfo DefaultCulture = new CultureInfo("fr-FR");

        private readonly string value;

        private LastName(string value)
        {
            this.value = value;
        }

        private enum InvalidReason
        {
            None,
            Null,
            InvalidCharacter,
            TooLong,
            Empty,
        }

        /// <summary>
        /// Gets the separators allowed in a last name.
        /// </summary>
        public static IReadOnlyList<char> AllowedSeparators { get; } = [' ', '-'];

        /// <summary>
        /// Gets the number of characters in the last name.
        /// </summary>
        int IReadOnlyCollection<char>.Count => this.value.Length;

        /// <summary>
        /// Gets the number of characters in the last name.
        /// </summary>
        public int Length => this.value.Length;

        /// <summary>
        /// Gets the character at the specified zero-based index.
        /// </summary>
        /// <param name="index">The zero-based position of the character.</param>
        /// <returns>The character at the specified <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than or equal to <see cref="Length"/>.</exception>
        public char this[int index] => this.value[index];

        /// <summary>
        /// Implicitly converts a <see cref="LastName"/> to its <see cref="string"/> representation.
        /// </summary>
        /// <param name="lastName">The instance to convert.</param>
        /// <returns>The normalized string value.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="lastName"/> is <see langword="null"/>.</exception>
        public static implicit operator string(LastName lastName)
        {
            ArgumentNullException.ThrowIfNull(lastName, nameof(lastName));

            return lastName.ToString();
        }

        /// <summary>
        /// Implicitly converts a <see cref="string"/> to a <see cref="LastName"/>.
        /// </summary>
        /// <param name="lastName">The string value to convert.</param>
        /// <returns>The created <see cref="LastName"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="lastName"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the value is empty, exceeds <see cref="MaxLength"/>, or contains invalid characters.</exception>
        public static implicit operator LastName(string lastName)
        {
            return Create(lastName);
        }

        /// <summary>
        /// Determines whether two <see cref="LastName"/> values have the same content.
        /// </summary>
        /// <param name="left">The first <see cref="LastName"/> to compare.</param>
        /// <param name="right">The second <see cref="LastName"/> to compare.</param>
        /// <returns><see langword="true"/> if the values are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(LastName? left, LastName? right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two <see cref="LastName"/> values have different content.
        /// </summary>
        /// <param name="left">The first <see cref="LastName"/> to compare.</param>
        /// <param name="right">The second <see cref="LastName"/> to compare.</param>
        /// <returns><see langword="true"/> if the values are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(LastName? left, LastName? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the <paramref name="left"/> value is lexicographically less than the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The first <see cref="LastName"/> to compare.</param>
        /// <param name="right">The second <see cref="LastName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <(LastName? left, LastName? right)
        {
            return Comparer<LastName>.Default.Compare(left, right) < 0;
        }

        /// <summary>
        /// Determines whether the <paramref name="left"/> value is lexicographically less than or equal to the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The first <see cref="LastName"/> to compare.</param>
        /// <param name="right">The second <see cref="LastName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(LastName? left, LastName? right)
        {
            return Comparer<LastName>.Default.Compare(left, right) <= 0;
        }

        /// <summary>
        /// Determines whether the <paramref name="left"/> value is lexicographically greater than the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The first <see cref="LastName"/> to compare.</param>
        /// <param name="right">The second <see cref="LastName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >(LastName? left, LastName? right)
        {
            return Comparer<LastName>.Default.Compare(left, right) > 0;
        }

        /// <summary>
        /// Determines whether the <paramref name="left"/> value is lexicographically greater than or equal to the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The first <see cref="LastName"/> to compare.</param>
        /// <param name="right">The second <see cref="LastName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(LastName? left, LastName? right)
        {
            return Comparer<LastName>.Default.Compare(left, right) >= 0;
        }

        /// <summary>
        /// Creates a <see cref="LastName"/> from the provided string, enforcing normalization and validation rules.
        /// </summary>
        /// <param name="lastName">The input value.</param>
        /// <returns>A valid <see cref="LastName"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="lastName"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the value is empty, exceeds <see cref="MaxLength"/>, or contains invalid characters.</exception>
        public static LastName Create(string lastName)
        {
            var result = TryCreateCore(lastName);

            if (result.LastName is null)
            {
                if (result.InvalidReason == InvalidReason.Null)
                {
                    throw new ArgumentNullException(nameof(lastName));
                }

                if (result.InvalidReason == InvalidReason.TooLong)
                {
                    throw new ArgumentException($"The last name cannot exceed more than {MaxLength} characters.", nameof(lastName));
                }

                if (result.InvalidReason == InvalidReason.Empty)
                {
                    throw new ArgumentException($"The last name cannot be empty.", nameof(lastName));
                }

                throw new ArgumentException($"'{lastName}' is not a valid last name.", nameof(lastName));
            }

            return result.LastName;
        }

        /// <summary>
        /// Tries to create a <see cref="LastName"/> from the provided value.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="lastName">When this method returns, contains the created <see cref="LastName"/> if successful; otherwise <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if creation succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool TryCreate([NotNullWhen(true)] string? value, [MaybeNullWhen(false)][NotNullWhen(true)] out LastName? lastName)
        {
            var result = TryCreateCore(value);

            if (result.LastName is null)
            {
                lastName = null;
                return false;
            }

            lastName = result.LastName;
            return true;
        }

        /// <summary>
        /// Determines whether the specified value is a valid last name according to the rules.
        /// </summary>
        /// <param name="lastName">The value to validate.</param>
        /// <returns><see langword="true"/> if valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(string lastName)
        {
            return TryCreate(lastName, out var _);
        }

        /// <summary>
        /// Parses a <see cref="string"/> into a <see cref="LastName"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to parse.</param>
        /// <param name="provider">A format provider (ignored).</param>
        /// <returns>The parsed <see cref="LastName"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="s"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// If the value is empty, exceeds <see cref="MaxLength"/>, or contains invalid characters.
        /// </exception>
        static LastName IParsable<LastName>.Parse(string s, IFormatProvider? provider)
        {
            return Create(s);
        }

        /// <summary>
        /// Tries to parse a <see cref="string"/> into a <see cref="LastName"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to parse.</param>
        /// <param name="provider">A format provider (ignored).</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="LastName"/> if successful; otherwise <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
        static bool IParsable<LastName>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out LastName result)
        {
            return TryCreate(s, out result);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="LastName"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not LastName lastName)
            {
                return false;
            }

            return this.Equals(lastName);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another <see cref="LastName"/>.
        /// </summary>
        /// <param name="other">A <see cref="LastName"/> to compare with this instance.</param>
        /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(LastName? other)
        {
            if (other is null)
            {
                return false;
            }

            return this.value.Equals(other.value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.value.GetHashCode(StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns the normalized string representation of the last name.
        /// </summary>
        /// <returns>The normalized last name.</returns>
        public override string ToString()
        {
            return this.value;
        }

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">A format string (ignored).</param>
        /// <param name="formatProvider">A format provider (ignored).</param>
        /// <returns>The normalized last name.</returns>
        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            return this.ToString();
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> that iterates through the characters of the last name.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> over the characters.</returns>
        public IEnumerator<char> GetEnumerator()
        {
            return this.value.GetEnumerator();
        }

        /// <summary>
        /// Compares the current instance with another <see cref="LastName"/> and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same position
        /// in the sort order as the other object.
        /// </summary>
        /// <param name="other">The other <see cref="LastName"/> to compare.</param>
        /// <returns>
        /// A value less than zero if this instance precedes <paramref name="other"/>; zero if they are equal;
        /// greater than zero if this instance follows <paramref name="other"/>.
        /// </returns>
        public int CompareTo(LastName? other)
        {
            if (other is null)
            {
                return string.Compare(this.value, null, StringComparison.Ordinal);
            }

            return string.Compare(this.value, other.value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> that iterates through the characters of the last name.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> over the characters.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static ParseResult TryCreateCore(string? value)
        {
            if (value is null)
            {
                return ParseResult.Invalid(InvalidReason.Null);
            }

            var lastNameBuilder = new StringBuilder(value.Length);

            var alreadyHaveSeparator = true;

            for (var i = 0; i < value.Length; i++)
            {
                var letter = value[i];

                if (char.IsLetter(letter))
                {
                    // It is a letter, add it as upper case.
                    lastNameBuilder.Append(char.ToUpper(letter, DefaultCulture));
                    alreadyHaveSeparator = false;
                }
                else if (AllowedSeparators.Contains(letter))
                {
                    // Allowed character
                    if (!alreadyHaveSeparator)
                    {
                        // Add the separator and define the next letter to uppercase.
                        lastNameBuilder.Append(letter);
                        alreadyHaveSeparator = true;
                    }
                    else
                    {
                        // Ignore the separator (already have more than one).
                    }
                }
                else
                {
                    // Invalid character
                    return ParseResult.Invalid(InvalidReason.InvalidCharacter);
                }
            }

            // If at the end we have a separator, remove it.
            if (lastNameBuilder.Length > 0 && AllowedSeparators.Contains(lastNameBuilder[^1]))
            {
                lastNameBuilder.Remove(lastNameBuilder.Length - 1, 1);
            }

            if (lastNameBuilder.Length == 0)
            {
                return ParseResult.Invalid(InvalidReason.Empty);
            }

            if (lastNameBuilder.Length > MaxLength)
            {
                return ParseResult.Invalid(InvalidReason.TooLong);
            }

            return ParseResult.Valid(new LastName(lastNameBuilder.ToString()));
        }

        private readonly struct ParseResult
        {
            private ParseResult(LastName? lastName, InvalidReason? invalidReason)
            {
                this.LastName = lastName;
                this.InvalidReason = invalidReason;
            }

            public LastName? LastName { get; }

            public InvalidReason? InvalidReason { get; }

            public static ParseResult Invalid(InvalidReason invalidReason)
            {
                return new ParseResult(null, invalidReason);
            }

            public static ParseResult Valid(LastName lastName)
            {
                return new ParseResult(lastName, null);
            }
        }
    }
}

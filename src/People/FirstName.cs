//-----------------------------------------------------------------------
// <copyright file="FirstName.cs" company="P.O.S Informatique">
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
    /// Represents a normalized first name with the following rules:
    /// <list type="bullet">
    /// <item>The maximum length is <c>50</c> characters (see <see cref="MaxLength"/>).</item>
    /// <item>Only letters are allowed, with separators limited to space and <c>-</c> (list of allowed characters are accessible from the <see cref="AllowedSeparators"/> property).</item>
    /// <item>Each word starts with an uppercase letter (e.g., <c>John</c>, <c>John Henri-Smith</c>).</item>
    /// <item>No consecutive or trailing separators are allowed.</item>
    /// <item>Implicit conversions from/to <see cref="string"/> are provided.</item>
    /// <item>Implements <see cref="IFormattable"/> and <see cref="IParsable{TSelf}"/> for standard .NET conversions.</item>
    /// <item>Acts like a read-only array of <see cref="char"/> via <see cref="IReadOnlyList{T}"/>.</item>
    /// </list>
    /// Using this type standardizes first name representation across your domain (users, people, customers, etc.).
    /// </summary>
    public sealed class FirstName : IReadOnlyList<char>, IEquatable<FirstName>, IComparable<FirstName>, IFormattable, IParsable<FirstName>
    {
        /// <summary>
        /// Maximum allowed length of a <see cref="FirstName"/> (50).
        /// </summary>
        public const int MaxLength = 50;

        private static readonly CultureInfo DefaultCulture = new CultureInfo("fr-FR");

        private readonly string value;

        private FirstName(string value)
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
        /// Gets the separators allowed in a first name.
        /// </summary>
        public static IReadOnlyList<char> AllowedSeparators { get; } = [' ', '-'];

        /// <summary>
        /// Gets the number of characters in the first name.
        /// </summary>
        int IReadOnlyCollection<char>.Count => this.value.Length;

        /// <summary>
        /// Gets the number of characters in the first name.
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
        /// Implicitly converts a <see cref="FirstName"/> to its <see cref="string"/> representation.
        /// </summary>
        /// <param name="firstName">The instance to convert.</param>
        /// <returns>The normalized string value.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="firstName"/> is <see langword="null"/>.</exception>
        public static implicit operator string(FirstName firstName)
        {
            ArgumentNullException.ThrowIfNull(firstName, nameof(firstName));

            return firstName.ToString();
        }

        /// <summary>
        /// Implicitly converts a <see cref="string"/> to a <see cref="FirstName"/>.
        /// </summary>
        /// <param name="firstName">The string value to convert.</param>
        /// <returns>The created <see cref="FirstName"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="firstName"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the value is empty, exceeds <see cref="MaxLength"/>, or contains invalid characters.</exception>
        public static implicit operator FirstName(string firstName)
        {
            return Create(firstName);
        }

        /// <summary>
        /// Determines whether two <see cref="FirstName"/> values have the same content.
        /// </summary>
        /// <param name="left">The first <see cref="FirstName"/> to compare.</param>
        /// <param name="right">The second <see cref="FirstName"/> to compare.</param>
        /// <returns><see langword="true"/> if the values are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(FirstName? left, FirstName? right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two <see cref="FirstName"/> values have different content.
        /// </summary>
        /// <param name="left">The first <see cref="FirstName"/> to compare.</param>
        /// <param name="right">The second <see cref="FirstName"/> to compare.</param>
        /// <returns><see langword="true"/> if the values are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(FirstName? left, FirstName? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the <paramref name="left"/> value is lexicographically less than the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The first <see cref="FirstName"/> to compare.</param>
        /// <param name="right">The second <see cref="FirstName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <(FirstName? left, FirstName? right)
        {
            return Comparer<FirstName>.Default.Compare(left, right) < 0;
        }

        /// <summary>
        /// Determines whether the <paramref name="left"/> value is lexicographically less than or equal to the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The first <see cref="FirstName"/> to compare.</param>
        /// <param name="right">The second <see cref="FirstName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(FirstName? left, FirstName? right)
        {
            return Comparer<FirstName>.Default.Compare(left, right) <= 0;
        }

        /// <summary>
        /// Determines whether the <paramref name="left"/> value is lexicographically greater than the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The first <see cref="FirstName"/> to compare.</param>
        /// <param name="right">The second <see cref="FirstName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >(FirstName? left, FirstName? right)
        {
            return Comparer<FirstName>.Default.Compare(left, right) > 0;
        }

        /// <summary>
        /// Determines whether the <paramref name="left"/> value is lexicographically greater than or equal to the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The first <see cref="FirstName"/> to compare.</param>
        /// <param name="right">The second <see cref="FirstName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(FirstName? left, FirstName? right)
        {
            return Comparer<FirstName>.Default.Compare(left, right) >= 0;
        }

        /// <summary>
        /// Creates a <see cref="FirstName"/> from the provided string, enforcing normalization and validation rules.
        /// </summary>
        /// <param name="firstName">The input value.</param>
        /// <returns>A valid <see cref="FirstName"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="firstName"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the value is empty, exceeds <see cref="MaxLength"/>, or contains invalid characters.</exception>
        public static FirstName Create(string firstName)
        {
            var result = TryCreateCore(firstName);

            if (result.FirstName is null)
            {
                if (result.InvalidReason == InvalidReason.Null)
                {
                    throw new ArgumentNullException(nameof(firstName));
                }

                if (result.InvalidReason == InvalidReason.TooLong)
                {
                    throw new ArgumentException($"The first name cannot exceed more than {MaxLength} characters.", nameof(firstName));
                }

                if (result.InvalidReason == InvalidReason.Empty)
                {
                    throw new ArgumentException($"The first name cannot be empty.", nameof(firstName));
                }

                throw new ArgumentException($"'{firstName}' is not a valid first name.", nameof(firstName));
            }

            return result.FirstName;
        }

        /// <summary>
        /// Tries to create a <see cref="FirstName"/> from the provided value.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="firstName">When this method returns, contains the created <see cref="FirstName"/> if successful; otherwise <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if creation succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool TryCreate([NotNullWhen(true)] string? value, [MaybeNullWhen(false)][NotNullWhen(true)] out FirstName? firstName)
        {
            var result = TryCreateCore(value);

            if (result.FirstName is null)
            {
                firstName = null;
                return false;
            }

            firstName = result.FirstName;
            return true;
        }

        /// <summary>
        /// Determines whether the specified value is a valid first name according to the rules.
        /// </summary>
        /// <param name="firstName">The value to validate.</param>
        /// <returns><see langword="true"/> if valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(string firstName)
        {
            return TryCreate(firstName, out var _);
        }

        /// <summary>
        /// Parses a <see cref="string"/> into a <see cref="FirstName"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to parse.</param>
        /// <param name="provider">A format provider (ignored).</param>
        /// <returns>The parsed <see cref="FirstName"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="s"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// If the value is empty, exceeds <see cref="MaxLength"/>, or contains invalid characters.
        /// </exception>
        static FirstName IParsable<FirstName>.Parse(string s, IFormatProvider? provider)
        {
            return Create(s);
        }

        /// <summary>
        /// Tries to parse a <see cref="string"/> into a <see cref="FirstName"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to parse.</param>
        /// <param name="provider">A format provider (ignored).</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="FirstName"/> if successful; otherwise <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
        static bool IParsable<FirstName>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out FirstName? result)
        {
            return TryCreate(s, out result);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="FirstName"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not FirstName firstName)
            {
                return false;
            }

            return this.Equals(firstName);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another <see cref="FirstName"/>.
        /// </summary>
        /// <param name="other">A <see cref="FirstName"/> to compare with this instance.</param>
        /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(FirstName? other)
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
        /// Returns the normalized string representation of the first name.
        /// </summary>
        /// <returns>The normalized first name.</returns>
        public override string ToString()
        {
            return this.value;
        }

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">A format string (ignored).</param>
        /// <param name="formatProvider">A format provider (ignored).</param>
        /// <returns>The normalized first name.</returns>
        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            return this.ToString();
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> that iterates through the characters of the first name.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> over the characters.</returns>
        public IEnumerator<char> GetEnumerator()
        {
            return this.value.GetEnumerator();
        }

        /// <summary>
        /// Compares the current instance with another <see cref="FirstName"/> and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same position
        /// in the sort order as the other object.
        /// </summary>
        /// <param name="other">The other <see cref="FirstName"/> to compare.</param>
        /// <returns>
        /// A value less than zero if this instance precedes <paramref name="other"/>; zero if they are equal;
        /// greater than zero if this instance follows <paramref name="other"/>.
        /// </returns>
        public int CompareTo(FirstName? other)
        {
            if (other is null)
            {
                return string.Compare(this.value, null, StringComparison.Ordinal);
            }

            return string.Compare(this.value, other.value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> that iterates through the characters of the first name.
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

            var firstNameBuilder = new StringBuilder(value.Length);

            var upperCase = true;

            for (var i = 0; i < value.Length; i++)
            {
                var letter = value[i];

                if (char.IsLetter(letter))
                {
                    // It is a letter, make it in upper or lower case depending of the current context.
                    if (upperCase)
                    {
                        firstNameBuilder.Append(char.ToUpper(letter, DefaultCulture));
                        upperCase = false;
                    }
                    else
                    {
                        firstNameBuilder.Append(char.ToLower(letter, DefaultCulture));
                    }
                }
                else if (AllowedSeparators.Contains(letter))
                {
                    // Allowed character
                    if (!upperCase)
                    {
                        // Add the separator and define the next letter to uppercase.
                        firstNameBuilder.Append(letter);
                        upperCase = true;
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
            if (firstNameBuilder.Length > 0 && AllowedSeparators.Contains(firstNameBuilder[^1]))
            {
                firstNameBuilder.Remove(firstNameBuilder.Length - 1, 1);
            }

            if (firstNameBuilder.Length == 0)
            {
                return ParseResult.Invalid(InvalidReason.Empty);
            }

            if (firstNameBuilder.Length > MaxLength)
            {
                return ParseResult.Invalid(InvalidReason.TooLong);
            }

            return ParseResult.Valid(new FirstName(firstNameBuilder.ToString()));
        }

        private readonly struct ParseResult
        {
            private ParseResult(FirstName? firstName, InvalidReason? invalidReason)
            {
                this.FirstName = firstName;
                this.InvalidReason = invalidReason;
            }

            public FirstName? FirstName { get; }

            public InvalidReason? InvalidReason { get; }

            public static ParseResult Invalid(InvalidReason invalidReason)
            {
                return new ParseResult(null, invalidReason);
            }

            public static ParseResult Valid(FirstName firstName)
            {
                return new ParseResult(firstName, null);
            }
        }
    }
}

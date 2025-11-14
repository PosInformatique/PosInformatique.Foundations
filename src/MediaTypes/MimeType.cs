//-----------------------------------------------------------------------
// <copyright file="MimeType.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.MediaTypes
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents an immutable media type (formerly known as MIME type),
    /// composed of a type and a subtype, such as <c>application/json</c> or <c>image/png</c>.
    /// </summary>
    public sealed class MimeType : IEquatable<MimeType>, IParsable<MimeType>
    {
        private MimeType(string type, string subtype)
        {
            this.Type = type;
            this.Subtype = subtype;
        }

        /// <summary>
        /// Gets the main type of the media type, for example <c>application</c> or <c>image</c>.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the subtype of the media type, for example <c>json</c> or <c>png</c>.
        /// </summary>
        public string Subtype { get; }

        /// <summary>
        /// Determines whether two <see cref="MimeType"/> instances are equal.
        /// </summary>
        /// <param name="mimeType1">The first media type to compare.</param>
        /// <param name="mimeType2">The second media type to compare.</param>
        /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(MimeType? mimeType1, MimeType? mimeType2)
        {
            if (mimeType1 is null)
            {
                return mimeType2 is null;
            }

            return mimeType1.Equals(mimeType2);
        }

        /// <summary>
        /// Determines whether two <see cref="MimeType"/> instances are not equal.
        /// </summary>
        /// <param name="mimeType1">The first media type to compare.</param>
        /// <param name="mimeType2">The second media type to compare.</param>
        /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(MimeType? mimeType1, MimeType? mimeType2)
        {
            return !(mimeType1 == mimeType2);
        }

        /// <summary>
        /// Parses the specified string to create a new <see cref="MimeType"/> instance.
        /// </summary>
        /// <param name="s">The string that contains the media type, for example "application/json".</param>
        /// <returns>A new <see cref="MimeType"/> instance representing the specified media type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="s"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Thrown when the string is not a valid media type.</exception>
        public static MimeType Parse(string s)
        {
            ArgumentNullException.ThrowIfNull(s);

            return Parse(s, null);
        }

        /// <summary>
        /// Parses the specified string to create a new <see cref="MimeType"/> instance using the given format provider.
        /// </summary>
        /// <param name="s">The string that contains the media type, for example "application/json".</param>
        /// <param name="provider">An optional format provider. This parameter is not used.</param>
        /// <returns>A new <see cref="MimeType"/> instance representing the specified media type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="s"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Thrown when the string is not a valid media type.</exception>
        public static MimeType Parse(string s, IFormatProvider? provider)
        {
            ArgumentNullException.ThrowIfNull(s);

            if (TryParse(s, out var result))
            {
                return result;
            }

            throw new FormatException("Invalid MIME type format.");
        }

        /// <summary>
        /// Tries to parse the specified string into a <see cref="MimeType"/> instance.
        /// </summary>
        /// <param name="s">The string that contains the media type, for example "application/json".</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="MimeType"/> if the operation succeeded; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
        public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)][NotNullWhen(true)] out MimeType? result)
        {
            return TryParse(s, null, out result);
        }

        /// <summary>
        /// Tries to parse the specified string into a <see cref="MimeType"/> instance using the given format provider.
        /// </summary>
        /// <param name="s">The string that contains the media type, for example "application/json".</param>
        /// <param name="provider">An optional format provider. This parameter is not used.</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="MimeType"/> if the operation succeeded; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out MimeType? result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            var parts = s.Split('/');
            if (parts.Length != 2)
            {
                return false;
            }

            var type = parts[0];
            var subtype = parts[1];

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(subtype))
            {
                return false;
            }

            result = new MimeType(type, subtype);
            return true;
        }

        /// <summary>
        /// Gets the <see cref="MimeType"/> associated with the specified file extension.
        /// </summary>
        /// <param name="extension">The file extension, with or without a leading dot (for example <c>.json</c> or <c>json</c>).</param>
        /// <returns>The <see cref="MimeType"/> associated with the specified extension.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="extension"/> argument is <see langword="null"/>.</exception>
        public static MimeType FromExtension(string extension)
        {
            ArgumentNullException.ThrowIfNull(extension);

            return MimeTypes.FromExtension(extension);
        }

        /// <summary>
        /// Determines whether the current <see cref="MimeType"/> is equal to another <see cref="MimeType"/>.
        /// </summary>
        /// <param name="other">The other media type to compare with.</param>
        /// <returns><see langword="true"/> if the media type are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(MimeType? other)
        {
            if (other is null)
            {
                return false;
            }

            return this.Type == other.Type && this.Subtype == other.Subtype;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is MimeType mimeType)
            {
                return this.Equals(mimeType);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Type, this.Subtype);
        }

        /// <summary>
        /// Returns the string representation of this media type in the <c>type/subtype</c> format.
        /// </summary>
        /// <returns>A string that represents this media type.</returns>
        public override string ToString()
        {
            return $"{this.Type}/{this.Subtype}";
        }

        /// <summary>
        /// Gets the default file extension associated with this media type.
        /// </summary>
        /// <returns>The file extension associated with this media type. An empty string if no extension is associated to the media type.</returns>
        public string GetExtension()
        {
            return MimeTypes.GetExtension(this);
        }
    }
}
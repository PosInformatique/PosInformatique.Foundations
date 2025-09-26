//-----------------------------------------------------------------------
// <copyright file="EmailAddressJsonConverter.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.EmailAddresses.Json
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// <see cref="JsonConverter{T}"/> which allows to serialize and deserialize an <see cref="EmailAddress"/>
    /// as a JSON string.
    /// </summary>
    public sealed class EmailAddressJsonConverter : JsonConverter<EmailAddress>
    {
        /// <inheritdoc />
        public override bool HandleNull => true;

        /// <inheritdoc />
        public override EmailAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var input = reader.GetString();

            if (input is null)
            {
                return null;
            }

            if (!EmailAddress.TryParse(input, out var emailAddress))
            {
                throw new JsonException($"'{input}' is not a valid email address.");
            }

            return emailAddress;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, EmailAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}

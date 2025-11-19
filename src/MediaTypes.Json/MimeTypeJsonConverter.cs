//-----------------------------------------------------------------------
// <copyright file="MimeTypeJsonConverter.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.MediaTypes.Json
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// <see cref="JsonConverter{T}"/> which allows to serialize and deserialize an <see cref="MimeType"/>
    /// as a JSON string.
    /// </summary>
    public sealed class MimeTypeJsonConverter : JsonConverter<MimeType>
    {
        /// <inheritdoc />
        public override bool HandleNull => true;

        /// <inheritdoc />
        public override MimeType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var input = reader.GetString();

            if (input is null)
            {
                return null;
            }

            if (!MimeType.TryParse(input, out var mimeType))
            {
                throw new JsonException($"'{input}' is not a valid MIME type.");
            }

            return mimeType;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, MimeType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="PhoneNumberJsonConverter.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.PhoneNumbers.Json
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// <see cref="JsonConverter{T}"/> which allows to serialize and deserialize an <see cref="PhoneNumber"/>
    /// as a JSON string.
    /// </summary>
    public sealed class PhoneNumberJsonConverter : JsonConverter<PhoneNumber>
    {
        /// <inheritdoc />
        public override bool HandleNull => true;

        /// <inheritdoc />
        public override PhoneNumber? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var input = reader.GetString();

            if (input is null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            return PhoneNumber.Parse(input!);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, PhoneNumber value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="FirstNameJsonConverter.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.Json
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// <see cref="JsonConverter{T}"/> which allows to serialize and deserialize an <see cref="FirstName"/>
    /// as a JSON string.
    /// </summary>
    public sealed class FirstNameJsonConverter : JsonConverter<FirstName>
    {
        /// <inheritdoc />
        public override bool HandleNull => true;

        /// <inheritdoc />
        public override FirstName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var input = reader.GetString();

            if (input is null)
            {
                return null;
            }

            if (!FirstName.TryCreate(input, out var firstName))
            {
                throw new JsonException($"'{input}' is not a valid first name.");
            }

            return firstName;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, FirstName value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}

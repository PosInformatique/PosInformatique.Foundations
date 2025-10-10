//-----------------------------------------------------------------------
// <copyright file="LastNameJsonConverter.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.Json
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// <see cref="JsonConverter{T}"/> which allows to serialize and deserialize an <see cref="LastName"/>
    /// as a JSON string.
    /// </summary>
    public sealed class LastNameJsonConverter : JsonConverter<LastName>
    {
        /// <inheritdoc />
        public override bool HandleNull => true;

        /// <inheritdoc />
        public override LastName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var input = reader.GetString();

            if (input is null)
            {
                return null;
            }

            if (!LastName.TryCreate(input, out var lastName))
            {
                throw new JsonException($"'{input}' is not a valid last name.");
            }

            return lastName;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, LastName value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}

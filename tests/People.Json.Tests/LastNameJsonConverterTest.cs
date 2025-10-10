//-----------------------------------------------------------------------
// <copyright file="LastNameJsonConverterTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.Json.Tests
{
    using System.Text.Json;

    public class LastNameJsonConverterTest
    {
        [Fact]
        public void Serialization()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new LastNameJsonConverter(),
                },
            };

            var @object = new JsonClass
            {
                StringValue = "The string value",
                LastName = "The last name",
            };

            @object.Should().BeJsonSerializableInto(
                new
                {
                    StringValue = "The string value",
                    LastName = "THE LAST NAME",
                },
                options);
        }

        [Fact]
        public void Deserialization()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new LastNameJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                LastName = "The last name",
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    LastName = @"The last name",
                },
                options);
        }

        [Fact]
        public void Deserialization_WithNullValue()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new LastNameJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                LastName = (string)null,
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    LastName = null,
                },
                options);
        }

        [Fact]
        public void Deserialization_WithInvalidEmail()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new LastNameJsonConverter(),
                },
            };

            var act = () =>
            {
                JsonSerializer.Deserialize<JsonClass>("{\"StringValue\":\"\",\"LastName\":\"The $$ last name\"}", options);
            };

            act.Should().ThrowExactly<JsonException>()
                .WithMessage("'The $$ last name' is not a valid last name.");
        }

        private class JsonClass
        {
            public string StringValue { get; set; }

            public LastName LastName { get; set; }
        }
    }
}

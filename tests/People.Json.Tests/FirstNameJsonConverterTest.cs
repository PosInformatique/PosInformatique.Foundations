//-----------------------------------------------------------------------
// <copyright file="FirstNameJsonConverterTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.Json.Tests
{
    using System.Text.Json;

    public class FirstNameJsonConverterTest
    {
        [Fact]
        public void Serialization()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new FirstNameJsonConverter(),
                },
            };

            var @object = new JsonClass
            {
                StringValue = "The string value",
                FirstName = "The first name",
            };

            @object.Should().BeJsonSerializableInto(
                new
                {
                    StringValue = "The string value",
                    FirstName = "The First Name",
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
                    new FirstNameJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                FirstName = "The first name",
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    FirstName = @"The first name",
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
                    new FirstNameJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                FirstName = (string)null,
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    FirstName = null,
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
                    new FirstNameJsonConverter(),
                },
            };

            var act = () =>
            {
                JsonSerializer.Deserialize<JsonClass>("{\"StringValue\":\"\",\"FirstName\":\"The $$ first name\"}", options);
            };

            act.Should().ThrowExactly<JsonException>()
                .WithMessage("'The $$ first name' is not a valid first name.");
        }

        private class JsonClass
        {
            public string StringValue { get; set; }

            public FirstName FirstName { get; set; }
        }
    }
}

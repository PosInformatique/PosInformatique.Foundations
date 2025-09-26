//-----------------------------------------------------------------------
// <copyright file="EmailAddressJsonConverterTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.EmailAddresses.Json.Tests
{
    using System.Text.Json;

    public class EmailAddressJsonConverterTest
    {
        [Fact]
        public void Serialization()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new EmailAddressJsonConverter(),
                },
            };

            var @object = new JsonClass
            {
                StringValue = "The string value",
                EmailAddress = EmailAddress.Parse(@"""Test"" <test@test.com>"),
            };

            @object.Should().BeJsonSerializableInto(
                new
                {
                    StringValue = "The string value",
                    EmailAddress = "test@test.com",
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
                    new EmailAddressJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                EmailAddress = "\"Test\" <test@test.com>",
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    EmailAddress = EmailAddress.Parse("test@test.com"),
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
                    new EmailAddressJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                EmailAddress = (string)null,
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    EmailAddress = null,
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
                    new EmailAddressJsonConverter(),
                },
            };

            var act = () =>
            {
                JsonSerializer.Deserialize<JsonClass>("{\"StringValue\":\"\",\"EmailAddress\":\"@invalidEmail.com\"}", options);
            };

            act.Should().ThrowExactly<JsonException>()
                .WithMessage("'@invalidEmail.com' is not a valid email address.");
        }

        private class JsonClass
        {
            public string StringValue { get; set; }

            public EmailAddress EmailAddress { get; set; }
        }
    }
}

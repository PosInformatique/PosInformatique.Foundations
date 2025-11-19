//-----------------------------------------------------------------------
// <copyright file="PhoneNumberJsonConverterTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.PhoneNumbers.Json.Tests
{
    using System.Text.Json;

    public class PhoneNumberJsonConverterTest
    {
        [Fact]
        public void Serialization()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new PhoneNumberJsonConverter(),
                },
            };

            var @object = new JsonClass
            {
                StringValue = "The string value",
                PhoneNumber = PhoneNumber.Parse("+33111111111"),
            };

            @object.Should().BeJsonSerializableInto(
                new
                {
                    StringValue = "The string value",
                    PhoneNumber = "+33111111111",
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
                    new PhoneNumberJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                PhoneNumber = "+33111111111",
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    PhoneNumber = PhoneNumber.Parse("+33111111111"),
                },
                options);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Deserialization_WithNullOrWhiteSpaceValue(string value)
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new PhoneNumberJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                PhoneNumber = value,
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    PhoneNumber = null,
                },
                options);
        }

        private class JsonClass
        {
            public string StringValue { get; set; }

            public PhoneNumber PhoneNumber { get; set; }
        }
    }
}
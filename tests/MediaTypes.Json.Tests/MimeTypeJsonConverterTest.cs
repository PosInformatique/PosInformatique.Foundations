//-----------------------------------------------------------------------
// <copyright file="MimeTypeJsonConverterTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.MediaTypes.Json
{
    using System.Text.Json;

    public class MimeTypeJsonConverterTest
    {
        [Fact]
        public void Serialization()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new MimeTypeJsonConverter(),
                },
            };

            var @object = new JsonClass
            {
                StringValue = "The string value",
                MimeType = MimeType.Parse("image/jpeg"),
            };

            @object.Should().BeJsonSerializableInto(
                new
                {
                    StringValue = "The string value",
                    MimeType = "image/jpeg",
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
                    new MimeTypeJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                MimeType = "image/jpeg",
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    MimeType = MimeType.Parse("image/jpeg"),
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
                    new MimeTypeJsonConverter(),
                },
            };

            var json = new
            {
                StringValue = "The string value",
                MimeType = (string)null,
            };

            json.Should().BeJsonDeserializableInto(
                new JsonClass
                {
                    StringValue = "The string value",
                    MimeType = null,
                },
                options);
        }

        [Fact]
        public void Deserialization_WithInvalidMimeType()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new MimeTypeJsonConverter(),
                },
            };

            var act = () =>
            {
                JsonSerializer.Deserialize<JsonClass>("{\"StringValue\":\"\",\"MimeType\":\"invalid-mime-type\"}", options);
            };

            act.Should().ThrowExactly<JsonException>()
                .WithMessage("'invalid-mime-type' is not a valid MIME type.");
        }

        private class JsonClass
        {
            public string StringValue { get; set; }

            public MimeType MimeType { get; set; }
        }
    }
}
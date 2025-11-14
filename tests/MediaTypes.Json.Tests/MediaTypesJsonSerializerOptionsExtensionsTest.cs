//-----------------------------------------------------------------------
// <copyright file="MediaTypesJsonSerializerOptionsExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Text.Json.Tests
{
    using PosInformatique.Foundations.MediaTypes.Json;

    public class MediaTypesJsonSerializerOptionsExtensionsTest
    {
        [Fact]
        public void AddMediaTypesConverters()
        {
            var options = new JsonSerializerOptions();

            options.AddMediaTypesConverters();

            options.Converters.Should().HaveCount(1);
            options.Converters[0].Should().BeOfType<MimeTypeJsonConverter>();

            // Call again to check nothing has been changed.
            options.AddMediaTypesConverters();

            options.Converters.Should().HaveCount(1);
            options.Converters[0].Should().BeOfType<MimeTypeJsonConverter>();
        }

        [Fact]
        public void AddMediaTypesConverters_WithNullArgument()
        {
            var act = () =>
            {
                MediaTypesJsonSerializerOptionsExtensions.AddMediaTypesConverters(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }
    }
}
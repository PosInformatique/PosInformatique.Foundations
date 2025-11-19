//-----------------------------------------------------------------------
// <copyright file="PhoneNumbersJsonSerializerOptionsExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Text.Json.Tests
{
    using PosInformatique.Foundations.PhoneNumbers.Json;

    public class PhoneNumbersJsonSerializerOptionsExtensionsTest
    {
        [Fact]
        public void AddPhoneNumbersConverters()
        {
            var options = new JsonSerializerOptions();

            options.AddPhoneNumbersConverters();

            options.Converters.Should().HaveCount(1);
            options.Converters[0].Should().BeOfType<PhoneNumberJsonConverter>();

            // Call again to check nothing has been changed.
            options.AddPhoneNumbersConverters();

            options.Converters.Should().HaveCount(1);
            options.Converters[0].Should().BeOfType<PhoneNumberJsonConverter>();
        }

        [Fact]
        public void AddPhoneNumbersConverters_WithNullArgument()
        {
            var act = () =>
            {
                PhoneNumbersJsonSerializerOptionsExtensions.AddPhoneNumbersConverters(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }
    }
}
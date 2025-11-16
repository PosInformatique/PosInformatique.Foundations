//-----------------------------------------------------------------------
// <copyright file="EmailAddressesJsonSerializerOptionsExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Text.Json.Tests
{
    using PosInformatique.Foundations.EmailAddresses.Json;

    public class EmailAddressesJsonSerializerOptionsExtensionsTest
    {
        [Fact]
        public void AddEmailAddressesConverters()
        {
            var options = new JsonSerializerOptions();

            options.AddEmailAddressesConverters();

            options.Converters.Should().HaveCount(1);
            options.Converters[0].Should().BeOfType<EmailAddressJsonConverter>();

            // Call again to check nothing has been changed.
            options.AddEmailAddressesConverters();

            options.Converters.Should().HaveCount(1);
            options.Converters[0].Should().BeOfType<EmailAddressJsonConverter>();
        }

        [Fact]
        public void AddEmailAddressesConverters_WithNullArgument()
        {
            var act = () =>
            {
                EmailAddressesJsonSerializerOptionsExtensions.AddEmailAddressesConverters(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }
    }
}
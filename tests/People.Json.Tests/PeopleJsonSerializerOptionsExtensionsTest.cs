//-----------------------------------------------------------------------
// <copyright file="PeopleJsonSerializerOptionsExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Text.Json.Tests
{
    using PosInformatique.Foundations.People.Json;

    public class PeopleJsonSerializerOptionsExtensionsTest
    {
        [Fact]
        public void AddEmailAddressesConverters()
        {
            var options = new JsonSerializerOptions();

            options.AddPeopleConverters();

            options.Converters.Should().HaveCount(2);
            options.Converters[0].Should().BeOfType<FirstNameJsonConverter>();
            options.Converters[1].Should().BeOfType<LastNameJsonConverter>();

            // Call again to check nothing has been changed.
            options.AddPeopleConverters();

            options.Converters.Should().HaveCount(2);
            options.Converters[0].Should().BeOfType<FirstNameJsonConverter>();
            options.Converters[1].Should().BeOfType<LastNameJsonConverter>();
        }

        [Fact]
        public void AddEmailAddressesConverters_WithNullArgument()
        {
            var act = () =>
            {
                PeopleJsonSerializerOptionsExtensions.AddPeopleConverters(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }
    }
}

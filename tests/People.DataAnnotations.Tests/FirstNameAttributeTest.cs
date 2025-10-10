//-----------------------------------------------------------------------
// <copyright file="FirstNameAttributeTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.DataAnnotations.Tests
{
    using System.Globalization;

    public class FirstNameAttributeTest
    {
        [Theory]
        [InlineData("any", "First name must contain only alphabetic characters or the separators [' ', '-'].")]
        [InlineData("fr", "Le prénom doit contenir uniquement des caractères alphabétiques ou les séparateurs [' ', '-'].")]
        public void FormatErrorMessage(string culture, string expectedErrorMessage)
        {
            PeopleDataAnnotationsResources.Culture = new CultureInfo(culture);

            var attribute = new FirstNameAttribute();

            attribute.FormatErrorMessage(default).Should().Be(expectedErrorMessage);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("The first name")]
        [InlineData(1234)]
        public void IsValid_True(object value)
        {
            var attribute = new FirstNameAttribute();

            attribute.IsValid(value).Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("The first name $$")]
        public void IsValid_False(object value)
        {
            var attribute = new FirstNameAttribute();

            attribute.IsValid(value).Should().BeFalse();
        }
    }
}

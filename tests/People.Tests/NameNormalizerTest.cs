//-----------------------------------------------------------------------
// <copyright file="NameNormalizerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.Tests
{
    public class NameNormalizerTest
    {
        [Fact]
        public void GetFullNameForDisplay()
        {
            NameNormalizer.GetFullNameForDisplay("The first name", "The last name").Should().Be("The First Name THE LAST NAME");
        }

        [Fact]
        public void GetFullNameForDisplay_WithFirstNameNullArgument()
        {
            var act = () =>
            {
                NameNormalizer.GetFullNameForDisplay(null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("firstName");
        }

        [Fact]
        public void GetFullNameForDisplay_WithLastNameNullArgument()
        {
            var act = () =>
            {
                NameNormalizer.GetFullNameForDisplay("The first name", null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("lastName");
        }

        [Fact]
        public void GetFullNameForOrder()
        {
            NameNormalizer.GetFullNameForOrder("The first name", "The last name").Should().Be("THE LAST NAME The First Name");
        }

        [Fact]
        public void GetFullNameForOrder_WithFirstNameNullArgument()
        {
            var act = () =>
            {
                NameNormalizer.GetFullNameForOrder(null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("firstName");
        }

        [Fact]
        public void GetFullNameForOrder_WithLastNameNullArgument()
        {
            var act = () =>
            {
                NameNormalizer.GetFullNameForOrder("The first name", null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("lastName");
        }
    }
}

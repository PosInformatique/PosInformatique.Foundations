//-----------------------------------------------------------------------
// <copyright file="PersonExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.Tests
{
    public class PersonExtensionsTest
    {
        [Fact]
        public void GetFullNameForDisplay()
        {
            var person = new Mock<IPerson>(MockBehavior.Strict);
            person.Setup(p => p.FirstName)
                .Returns("The first name");
            person.Setup(p => p.LastName)
                .Returns("The last name");

            PersonExtensions.GetFullNameForDisplay(person.Object).Should().Be("The First Name THE LAST NAME");

            person.VerifyAll();
        }

        [Fact]
        public void GetFullNameForDisplay_WithFirstNameNullArgument()
        {
            var act = () =>
            {
                PersonExtensions.GetFullNameForDisplay(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("person");
        }

        [Fact]
        public void GetFullNameForOrder()
        {
            var person = new Mock<IPerson>(MockBehavior.Strict);
            person.Setup(p => p.FirstName)
                .Returns("The first name");
            person.Setup(p => p.LastName)
                .Returns("The last name");

            PersonExtensions.GetFullNameForOrder(person.Object).Should().Be("THE LAST NAME The First Name");

            person.VerifyAll();
        }

        [Fact]
        public void GetFullNameForOrder_WithFirstNameNullArgument()
        {
            var act = () =>
            {
                PersonExtensions.GetFullNameForOrder(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("person");
        }

        [Fact]
        public void GetInitials()
        {
            var person = new Mock<IPerson>(MockBehavior.Strict);
            person.Setup(p => p.FirstName)
                .Returns("First name");
            person.Setup(p => p.LastName)
                .Returns("Last name");

            PersonExtensions.GetInitials(person.Object).Should().Be("FL");

            person.VerifyAll();
        }

        [Fact]
        public void GetInitials_WithFirstNameNullArgument()
        {
            var act = () =>
            {
                PersonExtensions.GetInitials(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("person");
        }
    }
}

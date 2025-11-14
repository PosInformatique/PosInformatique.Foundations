//-----------------------------------------------------------------------
// <copyright file="EmailContactTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.EmailAddresses;

    public class EmailContactTest
    {
        [Fact]
        public void Constructor()
        {
            var email = EmailAddress.Parse("user@domain.com");

            var contact = new EmailContact(email, "The display name");

            contact.Email.Should().BeSameAs(email);
            contact.DisplayName.Should().Be("The display name");
        }

        [Fact]
        public void Constructor_WithNullEmail()
        {
            var act = () =>
            {
                new EmailContact(null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("email");
        }

        [Fact]
        public void Constructor_WithNullDisplayName()
        {
            var email = EmailAddress.Parse("user@domain.com");

            var act = () =>
            {
                new EmailContact(email, null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("displayName");
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="EmailRecipientTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.EmailAddresses;

    public class EmailRecipientTest
    {
        [Fact]
        public void Constructor()
        {
            var addressEmail = EmailAddress.Parse("email@domain.com");
            var model = new Model();

            var emailRecipient = new EmailRecipient<Model>(addressEmail, "The display name", model);

            emailRecipient.Address.Should().BeSameAs(addressEmail);
            emailRecipient.Model.Should().BeSameAs(model);
            emailRecipient.DisplayName.Should().Be("The display name");
        }

        [Fact]
        public void Constructor_WithNullAddress()
        {
            var act = () =>
            {
                new EmailRecipient<Model>(null, default, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("address");
        }

        [Fact]
        public void Constructor_WithNullDisplayName()
        {
            var address = EmailAddress.Parse("email@domain.com");

            var act = () =>
            {
                new EmailRecipient<Model>(address, null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("displayName");
        }

        [Fact]
        public void Constructor_WithNullModel()
        {
            var address = EmailAddress.Parse("email@domain.com");

            var act = () =>
            {
                new EmailRecipient<Model>(address, "The display name", null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("model");
        }

        private class Model : EmailModel
        {
        }
    }
}
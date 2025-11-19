//-----------------------------------------------------------------------
// <copyright file="EmailRecipientCollectionTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.EmailAddresses;

    public class EmailRecipientCollectionTest
    {
        [Fact]
        public void Constructor()
        {
            var collection = new EmailRecipientCollection<Model>();

            collection.Should().BeEmpty();
        }

        [Fact]
        public void Add()
        {
            var model = new Model();

            var collection = new EmailRecipientCollection<Model>();

            var result = collection.Add(EmailAddress.Parse("name@domain.com"), "The display name", model);

            collection.Should().Equal(result);

            result.Address.Should().Be(EmailAddress.Parse("name@domain.com"));
            result.DisplayName.Should().Be("The display name");
            result.Model.Should().BeSameAs(model);
        }

        [Fact]
        public void Add_WithNullAddress()
        {
            var collection = new EmailRecipientCollection<Model>();

            collection.Invoking(c => c.Add(null, default, default))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("address");
        }

        [Fact]
        public void Add_WithNullDisplayName()
        {
            var address = EmailAddress.Parse("email@domain.com");

            var collection = new EmailRecipientCollection<Model>();

            collection.Invoking(c => c.Add(address, null, default))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("displayName");
        }

        [Fact]
        public void Add_WithNullModel()
        {
            var address = EmailAddress.Parse("email@domain.com");

            var collection = new EmailRecipientCollection<Model>();

            collection.Invoking(c => c.Add(address, "The display name", null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("model");
        }

        private sealed class Model
        {
        }
    }
}